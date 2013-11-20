'Created by Jean-Luc Duckworth
'TODO: Just added the GMaps feature into the form -- working on how to add a point to the map dynamically --
'next problem is to control the javascript from VB.net
'http://stackoverflow.com/questions/3514152/adding-points-to-google-map-api

Option Explicit On

Public Class Form1
    Private Declare Function FindWindow Lib "user32.dll" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Private Declare Function FindWindowEx Lib "user32.dll" Alias "FindWindowExA" (ByVal hWndParent As IntPtr, ByVal hWndChildAfter As Integer, ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Private Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (ByVal hWnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As IntPtr
    Private Declare Function PostMessage Lib "user32.dll" Alias "PostMessageA" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As IntPtr
    Private Declare Function RegisterWindowMessage Lib "user32.dll" Alias "RegisterWindowMessageA" (ByVal lpString As String) As UInteger

    '******CONFIGURATION VARIABLES******'
    Dim numChar2ShowUpdate As Integer = 14
    'Dim configBaudRate As Integer = 5600 '(Baud rate * 100) = configuration
    Dim configBaudRate As Integer = 7500 '(Baud rate * 100) = configuration
    Dim configMarkFreq As Integer = 2100 'Hz
    Dim configSpaceFreq As Integer = 2450 'Hz

    Dim configSquelch As Integer = 0 'TODO

    Dim configSwitch_DEM_TYPE As Short = 0      'b0-b1      Demodulator type        0-IIR, 1-FIR, 2-PLL
    Dim configSwitch_AFC As Short = 0           'b2
    Dim configSwitch_NET As Short = 0           'b3
    Dim configSwitch_ATC As Short = 0           'b4
    Dim configSwitch_BPF As Short = 0           'b5
    Dim configSwitch_LMS_NOTCH As Short = 0     'b6
    Dim configSwitch_SQ As Short = 1            'b7         Squelch
    Dim configSwitch_Rev As Short = 0           'b8
    Dim configSwitch_UOS As Short = 0           'b9
    Dim configSwitch_AFC_Alg As Short = 1       'b10-b11    AFC shift algorithm (should not be used)
    Dim configSwitch_Integrator As Short = 0    'b12        Integrator type         0-FIR, 1-IIR
    Dim configSwitch_LMS_OR_NOTCH As Short = 0  'b13        LMS or Notch		    0-LMS, 1-Notch
    Dim configSwitch_NUM_NOTCH As Short = 0     'b14                                0-single notch, 1-two notches
    Dim configSwitch_KEY_BUFFER As Short = 0    'b15        RXM_CHAR method	        0-Key buffer OFF, 1-Key buffer ON
    Dim configSwitch_WORD_WRAP As Short = 0     'b16        Word wrap
    Dim configSwitch_WAY_TO_SENT As Short = 0   'b17-b18    Way to send             0-Char, 1-Word, 2-Line
    Dim configSwitch As Short = 0


    Private Class rock
        Public rockName As String
        Public numDetections As Integer
        Public posLatitudeDeg As Integer
        Public posLatitudeMinute As Integer
        Public posLatitudeSecond As Integer
        Public posLongitudeDeg As Integer
        Public posLongitudeMinute As Integer
        Public posLongitudeSecond As Integer
        Public dataIgnored As Boolean
        Public status As String
        Public batteryLevel As Integer
        Public updatedAnimationCount As Integer
        Public updatedString As String
    End Class

    Dim blueBrush As New Drawing.SolidBrush(Color.Blue)
    Dim whiteBrush As New Drawing.SolidBrush(Color.White)
    Dim redBrush As New Drawing.SolidBrush(Color.Red)
    Dim blackBrush As New Drawing.SolidBrush(Color.Black)
    Dim greenBrush As New Drawing.SolidBrush(Color.Green)
    Dim viewport As Point = New Point(0, 0)
    Dim viewportScale As Single = 1
    Dim xMouseDrag As Integer
    Dim yMouseDrag As Integer

    Dim incomingDataStart As Boolean = False
    Dim incomingDataBuffer As Queue(Of Char) = New Queue(Of Char)()
    Dim incomingDataNumUntilEnd As Integer = -1

    'MMTTY -> APP 
    Enum FROM_MMTTY
        TXM_HANDLE = &H8000
        TXM_REQHANDLE
        TXM_START
        TXM_CHAR
        TXM_PTTEVENT

        TXM_WIDTH
        TXM_BAUD
        TXM_MARK
        TXM_SPACE
        TXM_SWITCH

        TXM_VIEW
        TXM_LEVEL
        TXM_FIGEVENT
        TXM_RESO
        TXM_LPF

        TXM_THREAD
        TXM_PROFILE
        TXM_NOTCH
        TXM_DEFSHIFT
        TXM_RADIOFREQ

        TXM_SHOWSETUP
        TXM_SHOWPROFILE
    End Enum

    'APP -> MMTTY
    Enum TO_MMTTY
        RXM_HANDLE = &H0
        RXM_REQHANDLE
        RXM_EXIT
        RXM_PTT
        RXM_CHAR

        RXM_WINPOS
        RXM_WIDTH
        RXM_REQPARA
        RXM_SETBAUD
        RXM_SETMARK

        RXM_SETSPACE
        RXM_SETSWITCH
        RXM_SETHAM
        RXM_SHOWSETUP
        RXM_SETVIEW

        RXM_SETSQLVL
        RXM_SHOW
        RXM_SETFIG
        RXM_SETRESO
        RXM_SETLPF

        RXM_SETTXDELAY
        RXM_UPDATECOM
        RXM_SUSPEND
        RXM_NOTCH
        RXM_PROFILE

        RXM_TIMER
        RXM_ENBFOCUS
        RXM_SETDEFFREQ
        RXM_SETLENGTH
    End Enum

    Dim MSG_MMTTY As Long = RegisterWindowMessage("MMTTY")
    Dim MMTTY_Handle As Long = vbNull
    Dim MMTTY_ThreadId As Long = vbNull
    Dim MMTTY_process As System.Diagnostics.Process

    Dim rockList As ArrayList = New ArrayList
    Dim rockCount As Integer = 1

    'Private Sub PaintMap(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles GroupBox2.Paint

    '    e.Graphics.TranslateTransform(20, 30)
    '    e.Graphics.Clip = New Region(New Rectangle(-1, -1, GroupBox2.Width - 40 + 3, GroupBox2.Height - 50 + 3))

    '    e.Graphics.FillRectangle(whiteBrush, 0, 0, GroupBox2.Width - 40, GroupBox2.Height - 50)

    '    Dim ugsLocations As New List(Of Point)

    '    ugsLocations.Add(New Point(34.555, -104.232))
    '    ugsLocations.Add(New Point(37.555, -107.232))
    '    ugsLocations.Add(New Point(38.555, -103.232))
    '    ugsLocations.Add(New Point(32.555, -108.232))

    '    'Set both viewport initial properties
    '    If (viewport.IsEmpty) Then
    '        Dim avgX As Double
    '        Dim avgY As Double

    '        viewportScale = 1

    '        For Each thisUgs In ugsLocations
    '            avgX += thisUgs.X
    '            avgY += thisUgs.Y
    '        Next

    '        avgX = avgX / ugsLocations.Count
    '        avgY = avgY / ugsLocations.Count

    '        viewport = New Point(e.Graphics.VisibleClipBounds.Width / 2 + avgX, e.Graphics.VisibleClipBounds.Height / 2 + avgY)
    '    End If

    '    For Each thisUGS In ugsLocations
    '        Dim xPos As Integer = viewport.X - thisUGS.X
    '        Dim yPos As Integer = viewport.Y - thisUGS.Y

    '        'Offset x direction
    '        xPos = xPos + (xPos - e.Graphics.VisibleClipBounds.Width) * viewportScale

    '        'Offset y direction
    '        yPos = yPos + (yPos - e.Graphics.VisibleClipBounds.Height) * viewportScale

    '        e.Graphics.FillPie(Brushes.Red, xPos, yPos, 15, 15, 0, 360)
    '    Next

    '    ''Define a geographic coordinate, in this case a GPS location
    '    'Dim myLocation As PointF = New PointF()
    '    'myLocation.Y = 39.2       '39 North
    '    'myLocation.X = -105.4     '105  West

    '    ''Now use our projection class to flatten this coordinate
    '    'Dim projection As PlateCaree = New PlateCaree()
    '    'Dim myProjectedLocation As PointF = projection.Project(myLocation)
    '    'myLocation = projection.Deproject(myProjectedLocation)

    '    ''Use a matrix for translation and scaling
    '    'Dim transform As Drawing2D.Matrix = New Drawing2D.Matrix()

    '    ''Define the viewport
    '    'Dim viewport As PointF = New PointF(39, -105)

    '    ''First, translate all projected points so that they match up with pixel 0,0
    '    'transform.Translate(-viewport.X, viewport.Y, Drawing2D.MatrixOrder.Append)

    '    ''Next, scale all points so that the viewport fits inside the form.
    '    ''transform.Scale(RTTYbox.Width / viewport.Width, RTTYbox.Height / -viewport.Height, MatrixOrder.Append)

    '    ''Apply this transform to all graphics operations
    '    'e.Graphics.Transform = transform

    '    ''Draw coordinates
    '    'e.Graphics.FillPolygon(Brushes.Red, )

    '    e.Graphics.DrawRectangle(Pens.Black, -1, -1, GroupBox2.Width - 40 + 2, GroupBox2.Height - 50 + 2)
    'End Sub

    Private Sub testBrowser()

    End Sub

    Private Sub AddRockToList(thisRock As rock)
        ListBox1.Items.Add(thisRock.rockName)
    End Sub

    Private Sub ListBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedValueChanged, ListBox1.SelectedIndexChanged
        Dim buttonEnable As Boolean = False

        For Each thisRock As rock In rockList

            If (IsNothing(ListBox1.SelectedItem) = False) Then

                If (ListBox1.SelectedItem.ToString.Contains(thisRock.rockName) And ListBox1.Tag <> thisRock.rockName) Then
                    ListBox1.Tag = thisRock.rockName
                    statusLabel.Text = thisRock.status
                    detectionsLabel.Text = thisRock.numDetections
                    batteryLabel.Text = thisRock.batteryLevel.ToString + "%"
                    BatteryBar.Value = thisRock.batteryLevel

                    latitudeLabel.Text = thisRock.posLatitudeDeg & "° " & _
                        thisRock.posLatitudeMinute & "' " & _
                        thisRock.posLatitudeSecond & Chr(34)

                    If (thisRock.posLatitudeDeg > 0) Then
                        latitudeLabel.Text += " N"
                    End If

                    If (thisRock.posLatitudeDeg < 0) Then
                        latitudeLabel.Text += " S"
                    End If


                    longitudeLabel.Text = thisRock.posLongitudeDeg & "° " & _
                        thisRock.posLongitudeMinute & "' " & _
                        thisRock.posLongitudeSecond & Chr(34)

                    If (thisRock.posLongitudeDeg > 0) Then
                        longitudeLabel.Text += " E"
                    End If

                    If (thisRock.posLongitudeDeg < 0) Then
                        longitudeLabel.Text += " W"
                    End If

                    IgnoreDataButton.Text = "Ignore Data"
                    If (thisRock.dataIgnored = True) Then
                        IgnoreDataButton.Text = "Allow Data"
                    End If

                    Exit For
                End If

                buttonEnable = True
            End If
        Next

        IgnoreDataButton.Enabled = buttonEnable
        ResetButton.Enabled = buttonEnable
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim tempRock As rock

        'ROCK 1
        tempRock = New rock
        tempRock.rockName = "Rock " + rockCount.ToString
        rockCount = rockCount + 1
        tempRock.numDetections = 4

        tempRock.posLatitudeDeg = 34
        tempRock.posLatitudeMinute = 14
        tempRock.posLatitudeSecond = 12.34
        tempRock.posLongitudeDeg = -101
        tempRock.posLongitudeMinute = 13
        tempRock.posLongitudeSecond = 45

        tempRock.dataIgnored = False
        tempRock.status = "Listening"
        tempRock.batteryLevel = 46
        tempRock.updatedString = "[-UPDATED--UPDATED-]"
        tempRock.updatedAnimationCount = tempRock.updatedString.Length + 6
        rockList.Add(tempRock)
        AddRockToList(tempRock)

        'ROCK 2
        tempRock = New rock
        tempRock.rockName = "Rock " + rockCount.ToString
        rockCount = rockCount + 1
        tempRock.numDetections = 14

        tempRock.posLatitudeDeg = 34
        tempRock.posLatitudeMinute = 14
        tempRock.posLatitudeSecond = 12.34
        tempRock.posLongitudeDeg = -101
        tempRock.posLongitudeMinute = 13
        tempRock.posLongitudeSecond = 45

        tempRock.dataIgnored = False
        tempRock.status = "Obtaining GPS Data"
        tempRock.batteryLevel = 83
        rockList.Add(tempRock)
        AddRockToList(tempRock)

        'ROCK 3
        tempRock = New rock
        tempRock.rockName = "Rock " + rockCount.ToString
        rockCount = rockCount + 1
        tempRock.numDetections = 0

        tempRock.posLatitudeDeg = 34
        tempRock.posLatitudeMinute = 14
        tempRock.posLatitudeSecond = 12.34
        tempRock.posLongitudeDeg = -101
        tempRock.posLongitudeMinute = 13
        tempRock.posLongitudeSecond = 45

        tempRock.dataIgnored = False
        tempRock.status = "Listening"
        tempRock.batteryLevel = 49
        rockList.Add(tempRock)
        AddRockToList(tempRock)
    End Sub

    Private Sub IgnoreDataButton_Click(sender As Object, e As EventArgs) Handles IgnoreDataButton.Click
        For Each thisRock As rock In rockList
            If (ListBox1.Tag = thisRock.rockName) Then
                thisRock.dataIgnored = Not (thisRock.dataIgnored)

                Exit For
            End If
        Next

        'Ensure next method updates
        ListBox1.Tag = ""

        'Update the current information
        Call ListBox1_SelectedValueChanged(Nothing, Nothing)
    End Sub

    Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click
        For Each thisRock As rock In rockList
            If (ListBox1.Tag = thisRock.rockName) Then
                thisRock.batteryLevel = 0
                thisRock.dataIgnored = False
                thisRock.numDetections = 0
                thisRock.posLatitudeDeg = 0
                thisRock.posLatitudeMinute = 0
                thisRock.posLatitudeSecond = 0
                thisRock.posLongitudeDeg = 0
                thisRock.posLongitudeMinute = 0
                thisRock.posLongitudeSecond = 0
                thisRock.status = "Awaiting Next Data Pulse"
                Exit For
            End If
        Next

        'Ensure next method updates
        ListBox1.Tag = ""

        'Update the current information
        Call ListBox1_SelectedValueChanged(Nothing, Nothing)
    End Sub

    Private Sub RockListAnimationTimer_Tick(sender As Object, e As EventArgs) Handles RockListAnimationTimer.Tick
        Dim startSubString As Integer
        Dim currStringLength As Integer
        Dim currUpdateString As String = ""
        Dim currIndex As Integer = ListBox1.TopIndex()

        For Each thisRock As rock In rockList
            If (thisRock.updatedAnimationCount > 0) Then

                'Change the below spaces to increase or decrease amount shown
                currUpdateString = MakeSpaces(numChar2ShowUpdate) + thisRock.updatedString + MakeSpaces(numChar2ShowUpdate)
                currStringLength = currUpdateString.Length

                startSubString = currStringLength - thisRock.updatedAnimationCount - numChar2ShowUpdate + 1

                If (startSubString >= 0) Then

                    currUpdateString = currUpdateString.Substring(startSubString, numChar2ShowUpdate)

                    For i As Integer = 0 To ListBox1.Items.Count
                        'For Each rockName As String In ListBox1.Items
                        If (ListBox1.Items(i).ToString.Contains(thisRock.rockName)) Then
                            ListBox1.Items.Item(i) = thisRock.rockName + "    " + currUpdateString
                            Exit For
                        End If
                    Next

                    thisRock.updatedAnimationCount = thisRock.updatedAnimationCount - 1
                End If
            End If
        Next

        ListBox1.TopIndex = currIndex
    End Sub

    Private Function MakeSpaces(numberSpaces As Integer) As String
        Dim newString As String = ""

        For i As Integer = 1 To numberSpaces
            newString = newString + " "
        Next

        Return newString
    End Function

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Dim hWnd As IntPtr = m.HWnd
        Dim message As Integer = m.Msg
        Dim wParam As IntPtr = m.WParam
        Dim lParam As IntPtr = m.LParam

        'Show the Message "OK" if someone sends a message with MSG_MMTTY:
        Select Case message
            Case WindowMessages.WM_DESTROY
                PostMessage(MMTTY_Handle, MSG_MMTTY, TO_MMTTY.RXM_EXIT, vbNull)
            Case Else
                If (message = MSG_MMTTY) Then
                    Select Case wParam
                        Case FROM_MMTTY.TXM_LEVEL
                            Dim signalLevel As Short = lParam.ToInt32 And &HFFFF
                            Dim squelchLevel As Short = (lParam.ToInt32 And &HFFFF0000) >> 16


                            If (signalLevel > 1250) Then
                                signalLevel = 1250
                            End If

                            If (signalLevel < 0) Then
                                signalLevel = 0
                            End If

                            monitorBar.Value = signalLevel

                            If (squelchBar.Value = 1) Then
                                'Enable squelch slider
                                squelchBar.Enabled = True

                                'Set initial squelch
                                squelchBar.Value = squelchLevel
                            End If

                            Call RTTYprogressBar()
                        Case FROM_MMTTY.TXM_HANDLE
                            MMTTY_Handle = lParam
                            PostMessage(MMTTY_Handle, MSG_MMTTY, TO_MMTTY.RXM_HANDLE, hWnd)
                            'Send configuration messages
                            PostMessage(MMTTY_Handle, MSG_MMTTY, TO_MMTTY.RXM_SETBAUD, configBaudRate)
                            PostMessage(MMTTY_Handle, MSG_MMTTY, TO_MMTTY.RXM_SETMARK, configMarkFreq)
                            PostMessage(MMTTY_Handle, MSG_MMTTY, TO_MMTTY.RXM_SETSPACE, configSpaceFreq)

                            configSwitch = configSwitch_DEM_TYPE _
                                Or configSwitch_AFC << 2 _
                                Or configSwitch_NET << 3 _
                                Or configSwitch_ATC << 4 _
                                Or configSwitch_BPF << 5 _
                                Or configSwitch_LMS_NOTCH << 6 _
                                Or configSwitch_SQ << 7 _
                                Or configSwitch_Rev << 8 _
                                Or configSwitch_UOS << 9 _
                                Or configSwitch_AFC_Alg << 10 _
                                Or configSwitch_Integrator << 12 _
                                Or configSwitch_LMS_OR_NOTCH << 13 _
                                Or configSwitch_NUM_NOTCH << 14 _
                                Or configSwitch_KEY_BUFFER << 15 _
                                Or configSwitch_WORD_WRAP << 16 _
                                Or configSwitch_WAY_TO_SENT << 17

                            PostMessage(MMTTY_Handle, MSG_MMTTY, TO_MMTTY.RXM_SETSWITCH, configSwitch)

                            RTTYbox.Text = "RTTY Status - Running"

                        Case FROM_MMTTY.TXM_CHAR
                            'Dim incomingDataStart As Boolean = False
                            'Dim incomingDataBuffer As Queue(Of Char) = New Queue(Of Char)()
                            'Dim incomingDataNumUntilEnd As Integer = -1
                            Dim incomingChar As Char = Chr(lParam.ToString)

                            'Add to debug box
                            MMTTYBox.Text = MMTTYBox.Text + incomingChar

                            'Beginning of string
                            If (incomingChar = "$") Then
                                incomingDataStart = True

                                If (incomingDataNumUntilEnd < -1) Then
                                    Call synthesizeIncomingData()
                                End If

                                incomingDataNumUntilEnd = -1
                            End If

                            If (incomingDataStart = True) Then
                                'Checksum of string
                                If (incomingChar = "&") Then
                                    incomingDataNumUntilEnd = 6
                                End If

                                'Add to queue
                                incomingDataBuffer.Enqueue(incomingChar)

                                'Decrement handle
                                incomingDataNumUntilEnd = incomingDataNumUntilEnd - 1
                            End If

                            'End of string
                            If (incomingDataNumUntilEnd = 0 And incomingDataStart = True) Then
                                incomingDataStart = False
                                Call synthesizeIncomingData()
                            End If

                        Case Else
                            If (1 = 1) Then

                            End If
                    End Select
                End If
        End Select

        MyBase.WndProc(m)
    End Sub

    Private Sub synthesizeIncomingData()
        Dim startStringIdent As Boolean = False
        Dim compiledString As String = ""

        While (incomingDataBuffer.Count > 0)
            If (incomingDataBuffer.Peek = "$" And startStringIdent = True) Then
                Exit While
            End If

            startStringIdent = True

            compiledString = compiledString + incomingDataBuffer.Dequeue

        End While

        Console.WriteLine(compiledString)
        handleIncomingData(compiledString)
    End Sub

    Private Function handleRockData(rockName As String) As Integer
        Dim i As Integer = 0
        Dim tempRock As rock

        For Each thisRock As rock In rockList
            If (InStr(thisRock.rockName, rockName)) Then
                thisRock.updatedString = "[-UPDATED--UPDATED-]"
                thisRock.updatedAnimationCount = thisRock.updatedString.Length + 6
                Return i
            End If

            i = i + 1
        Next

        tempRock = New rock
        tempRock.rockName = "Rock " + rockName
        rockCount = rockCount + 1
        tempRock.numDetections = 0
        tempRock.posLatitudeDeg = 0
        tempRock.posLatitudeMinute = 0
        tempRock.posLatitudeSecond = 0
        tempRock.posLongitudeDeg = 0
        tempRock.posLongitudeMinute = 0
        tempRock.posLongitudeSecond = 0
        tempRock.dataIgnored = False
        tempRock.status = "Initial Incoming Data"
        tempRock.batteryLevel = 0
        tempRock.updatedString = "[-UPDATED--UPDATED-]"
        tempRock.updatedAnimationCount = tempRock.updatedString.Length + 6
        rockList.Add(tempRock)
        AddRockToList(tempRock)

        Return i
    End Function

    Private Sub handleIncomingData(incomingString As String)
        Dim stringPortionPos As Integer = 0
        Dim message_id As String = ""
        Dim module_id As String = ""
        Dim ham_message As String = ""
        Dim checksum As UInteger = 0
        Dim thisRockID As Integer
        Dim thisRock As rock

        'Needs to contain checksum identifier
        If (Not incomingString.Contains("&")) Then
            Exit Sub
        End If

        Try
            'Remove leading $ sign
            incomingString = incomingString.Substring(1)

            'Determine checksum
            checksum = CInt(incomingString.Substring(incomingString.LastIndexOf("&") + 1, 5))
            incomingString = incomingString.Substring(0, incomingString.LastIndexOf("&"))

            'Determine message id
            message_id = incomingString.Split(";")(0)

            'Determine module id
            module_id = incomingString.Split(";")(1)

            'Determine message
            ham_message = incomingString.Split(";")(2)

            thisRockID = handleRockData(module_id)
            thisRock = rockList(thisRockID)

            Select Case message_id
                Case "G"
                    thisRock.posLatitudeDeg = ham_message.Split(",")(1).Substring(0, 2)
                    thisRock.posLatitudeMinute = ham_message.Split(",")(1).Substring(2, 2)
                    thisRock.posLatitudeSecond = ham_message.Split(",")(1).Substring(5)

                    thisRock.posLongitudeDeg = ham_message.Split(",")(3).Substring(0, 3)
                    thisRock.posLongitudeMinute = ham_message.Split(",")(3).Substring(3, 2)
                    thisRock.posLongitudeSecond = ham_message.Split(",")(3).Substring(6)

                Case "S"

                Case "D"

            End Select


        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub squelchBar_Scroll(sender As Object, e As EventArgs) Handles squelchBar.Scroll
        PostMessage(MMTTY_Handle, MSG_MMTTY, TO_MMTTY.RXM_SETSQLVL, squelchBar.Value)
        monitorBar.Refresh()
    End Sub

    Private Sub RTTYprogressBar()
        Dim boxGraphics As Graphics = RTTYbox.CreateGraphics()

        'Draw sq open box
        Dim xPos2 As Integer = monitorBar.Location.X + monitorBar.Width + 5
        boxGraphics.FillRectangle(blackBrush, xPos2 - 1, monitorBar.Location.Y + 5 - 1, monitorBar.Size.Height - 10 + 2, monitorBar.Size.Height - 10 + 2)
        boxGraphics.FillRectangle(redBrush, xPos2, monitorBar.Location.Y + 5, monitorBar.Size.Height - 10, monitorBar.Size.Height - 10)

        If (squelchBar.Value >= monitorBar.Value) Then
            boxGraphics.FillRectangle(blackBrush, xPos2, monitorBar.Location.Y + 5, monitorBar.Size.Height - 10, monitorBar.Size.Height - 10)
        End If
    End Sub

    Private Sub RTTYbox_Paint(sender As Object, e As PaintEventArgs) Handles RTTYbox.Paint
        'Draw single line
        Dim xPos As Integer = monitorBar.Location.X + monitorBar.Width * squelchBar.Value / (monitorBar.Maximum - monitorBar.Minimum)
        e.Graphics.FillRectangle(blackBrush, xPos, monitorBar.Location.Y + 1, 5, monitorBar.Size.Height - 2)

    End Sub

    Private Sub MMTTYBox_TextChanged(sender As Object, e As EventArgs) Handles MMTTYBox.TextChanged
        If (MMTTYBox.TextLength > 56) Then
            MMTTYBox.Text = MMTTYBox.Text.Substring(MMTTYBox.TextLength - 56 - 1)
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Hide MMTTY
        MMTTY_process = Process.Start("MMTTY.EXE", "-r -m -Z")
    End Sub

    Private Sub buttonZoomIn_Click(sender As Object, e As EventArgs) Handles buttonZoomIn.Click
        'viewportScale += 1
        'GroupBox2.Invalidate()
 

        WebBrowser1.Navigate(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GMaps.html"))
    End Sub

    Private Sub buttonZoomOut_Click(sender As Object, e As EventArgs) Handles buttonZoomOut.Click
        viewportScale -= 1
        GroupBox2.Invalidate()
    End Sub

    Private Sub buttonReset_Click(sender As Object, e As EventArgs) Handles buttonReset.Click
        viewport = Nothing
        GroupBox2.Invalidate()
    End Sub

    Private Sub GroupBox2_clickDown(sender As Object, e As MouseEventArgs) Handles GroupBox2.MouseDown
        'Record the initial click location
        xMouseDrag = e.X
        yMouseDrag = e.Y
    End Sub

    Private Sub GroupBox2_mouseMove(sender As Object, e As MouseEventArgs) Handles GroupBox2.MouseMove
        If (e.Button = Windows.Forms.MouseButtons.Left) Then
            viewport.X = viewport.X + (e.X - xMouseDrag)
            viewport.Y = viewport.Y + (e.Y - yMouseDrag)
            GroupBox2.Invalidate()

            xMouseDrag = e.X
            yMouseDrag = e.Y
        End If
    End Sub

End Class

