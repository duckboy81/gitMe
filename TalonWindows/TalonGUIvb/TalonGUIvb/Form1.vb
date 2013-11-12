Option Explicit On
Public Class Form1
    Private Declare Function FindWindow Lib "user32.dll" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Private Declare Function FindWindowEx Lib "user32.dll" Alias "FindWindowExA" (ByVal hWndParent As IntPtr, ByVal hWndChildAfter As Integer, ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Private Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (ByVal hWnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As IntPtr
    Private Declare Function PostMessage Lib "user32.dll" Alias "PostMessageA" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As IntPtr
    'Private Declare Function RegisterWindowMessage Lib "user32" Alias "RegisterWindowMessageA" (ByVal lpString As String) As Long
    Private Declare Function RegisterWindowMessage Lib "user32.dll" Alias "RegisterWindowMessageA" (ByVal lpString As String) As UInteger

    Private Class rock
        Public rockName As String
        Public numDetections As Integer
        Public posLatitude As Double
        Public posLongitude As Double
        Public dataIgnored As Boolean
        Public status As String
        Public batteryLevel As Integer
        Public updatedAnimationCount As Integer
        Public updatedString As String
    End Class

    'MMTTY -> APP 
    Enum FROM_MMTTY
        TXM_HANDLE = &H8000
        TXM_REQHANDLE
        TXM_START
        TXM_CHAR
        TXM_PTTEVENT
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
    Dim numChar2ShowUpdate As Integer = 8

    Private Sub PaintMap(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles GroupBox2.Paint
        'Handles Me.Paint

        Dim blueBrush As New Drawing.SolidBrush(Color.Blue)
        Dim whiteBrush As New Drawing.SolidBrush(Color.White)
        Dim redBrush As New Drawing.SolidBrush(Color.Red)
        Dim blackBrush As New Drawing.SolidBrush(Color.Black)
        Dim greenBrush As New Drawing.SolidBrush(Color.Green)

        e.Graphics.FillRectangle(blackBrush, 20 - 1, 30 - 1, GroupBox2.Width - 40 + 2, GroupBox2.Height - 50 + 2)
        e.Graphics.FillRectangle(blueBrush, 20, 30, GroupBox2.Width - 40, GroupBox2.Height - 50)

    End Sub


    Private Sub AddRockToList(thisRock As rock)
        ListBox1.Items.Add(thisRock.rockName)
    End Sub

    Private Sub ListBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedValueChanged, ListBox1.SelectedIndexChanged

        For Each thisRock As rock In rockList
            IgnoreDataButton.Enabled = False
            ResetButton.Enabled = False

            If (ListBox1.SelectedItem.ToString.Contains(thisRock.rockName)) Then
                statusLabel.Text = thisRock.status
                detectionsLabel.Text = thisRock.numDetections
                latitudeLabel.Text = thisRock.posLatitude
                longitudeLabel.Text = thisRock.posLongitude
                batteryLabel.Text = thisRock.batteryLevel.ToString + "%"
                BatteryBar.Value = thisRock.batteryLevel

                IgnoreDataButton.Text = "Ignore Data"
                If (thisRock.dataIgnored = True) Then
                    IgnoreDataButton.Text = "Allow Data"
                End If

                IgnoreDataButton.Enabled = True
                ResetButton.Enabled = True
                Exit For
            End If
        Next

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim tempRock As rock

        tempRock = New rock
        tempRock.rockName = "Rock " + rockCount.ToString
        rockCount = rockCount + 1
        tempRock.numDetections = 4
        tempRock.posLatitude = 32.40239
        tempRock.posLongitude = -103.123112
        tempRock.dataIgnored = False
        tempRock.status = "Listening"
        tempRock.batteryLevel = 46
        tempRock.updatedString = "[-UPDATED--UPDATED-]"
        tempRock.updatedAnimationCount = tempRock.updatedString.Length + 6
        rockList.Add(tempRock)
        AddRockToList(tempRock)

        tempRock = New rock
        tempRock.rockName = "Rock " + rockCount.ToString
        rockCount = rockCount + 1
        tempRock.numDetections = 14
        tempRock.posLatitude = 33.40239
        tempRock.posLongitude = -101.123112
        tempRock.dataIgnored = False
        tempRock.status = "Obtaining GPS Data"
        tempRock.batteryLevel = 83
        rockList.Add(tempRock)
        AddRockToList(tempRock)

        tempRock = New rock
        tempRock.rockName = "Rock " + rockCount.ToString
        rockCount = rockCount + 1
        tempRock.numDetections = 0
        tempRock.posLatitude = 31.40239
        tempRock.posLongitude = -102.123112
        tempRock.dataIgnored = False
        tempRock.status = "Listening"
        tempRock.batteryLevel = 49
        rockList.Add(tempRock)
        AddRockToList(tempRock)
    End Sub

    Private Sub IgnoreDataButton_Click(sender As Object, e As EventArgs) Handles IgnoreDataButton.Click
        For Each thisRock As rock In rockList
            If (ListBox1.SelectedItem = thisRock.rockName) Then
                thisRock.dataIgnored = Not (thisRock.dataIgnored)

                Exit For
            End If
        Next

        'Update the current information
        Call ListBox1_SelectedValueChanged(Nothing, Nothing)
    End Sub

    Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click
        For Each thisRock As rock In rockList
            If (ListBox1.SelectedItem = thisRock.rockName) Then
                thisRock.batteryLevel = 0
                thisRock.dataIgnored = False
                thisRock.numDetections = 0
                thisRock.posLatitude = 0
                thisRock.posLongitude = 0
                thisRock.status = "Awaiting Next Data Pulse"
                Exit For
            End If
        Next

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
                        Case FROM_MMTTY.TXM_HANDLE
                            MMTTY_Handle = lParam
                            PostMessage(MMTTY_Handle, MSG_MMTTY, TO_MMTTY.RXM_HANDLE, hWnd)
                        Case FROM_MMTTY.TXM_CHAR
                            MMTTY_Text.Text = MMTTY_Text.Text + Chr(lParam.ToString)
                    End Select
                End If
        End Select

        MyBase.WndProc(m)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        MMTTY_process = Process.Start("C:\Users\C14JeanLuc.Duckworth\Downloads\MMTTY.EXE", "-r")

        'PostMessage(MMTTY_Handle, MSG_MMTTY, TO_MMTTY.RXM_PTT, 2)

        Dim hwnd As IntPtr = FindWindow(vbNullString, "Virtual CDRom Control Panel")
        Dim x As IntPtr = FindWindowEx(hwnd, 0, vbNullString, "Driver Control ...")

        'PostMessage(x, BM_CLICK, 0&, 0&)
        'Thread.Sleep(200)
        hwnd = FindWindow(vbNullString, "Virtual CD-ROM Driver Control")
        Debug.Print(hwnd)
    End Sub
End Class

