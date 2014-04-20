'Created by Jean-Luc Duckworth 2013-2014

Option Explicit On

Public Class MainScreen
    'External Windows libraries used to find MMTTY window on startup
    Friend Declare Function FindWindow Lib "user32.dll" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Friend Declare Function FindWindowEx Lib "user32.dll" Alias "FindWindowExA" (ByVal hWndParent As IntPtr, ByVal hWndChildAfter As Integer, ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Friend Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (ByVal hWnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As IntPtr
    Friend Declare Function PostMessage Lib "user32.dll" Alias "PostMessageA" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As IntPtr
    Friend Declare Function RegisterWindowMessage Lib "user32.dll" Alias "RegisterWindowMessageA" (ByVal lpString As String) As UInteger

    '******CONFIGURATION VARIABLES******'
    'TODO REPLACE THE CHECKSUM WITH FALSE
    Dim DEBUG_BYPASS_CHECKSUM As Boolean = False
    Dim numChar2ShowUpdate As Integer = 14
    Dim configBaudRate As Integer = 7500 '(Actual Baud rate * 100) = this configuration
    Dim configMarkFreq As Integer = 2195 'Hz
    Dim configSpaceFreq As Integer = 2533 'Hz

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

    'Other global variables
    Dim incomingDataStart As Boolean = False
    Dim incomingDataBuffer As Queue(Of Char) = New Queue(Of Char)()
    Dim incomingDataNumUntilEnd As Integer = -1

    Dim MSG_MMTTY As Long = RegisterWindowMessage("MMTTY")
    Dim MMTTY_Handle As Long = vbNull
    Dim MMTTY_ThreadId As Long = vbNull
    Dim MMTTY_process As System.Diagnostics.Process

    Friend rock_list As List(Of talon_node) = New List(Of talon_node)
    Friend event_list As List(Of talon_event) = New List(Of talon_event)

    Dim blueBrush As New Drawing.SolidBrush(Color.Blue)
    Dim whiteBrush As New Drawing.SolidBrush(Color.White)
    Dim redBrush As New Drawing.SolidBrush(Color.Red)
    Dim blackBrush As New Drawing.SolidBrush(Color.Black)
    Dim greenBrush As New Drawing.SolidBrush(Color.Green)

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


    Friend Structure talon_node
        Public node_name As String
        Public is_exfil_node As Boolean
        Public ignore_flag As Boolean
        Public manual_gps As Boolean
        Public last_comm As Date
        Public number_of_detections As Integer

        'GPS Related Vars
        Public gps_lat_deg As Integer
        Public gps_lat_min As Integer
        Public gps_lat_sec As Integer
        Public gps_long_deg As Integer
        Public gps_long_min As Integer
        Public gps_long_sec As Integer

    End Structure

    Friend Structure talon_event
        Public time As Date
        Public new_flag As Boolean
        Public ignore_flag As Boolean
        Public node_name As String
        Public message_type As String
        Public event_name As String
        Public speed As String

    End Structure

    Friend Sub MainScreen_load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Hide MMTTY
        MMTTY_process = Process.Start("MMTTY.EXE", "-r -m -Z")
        'MMTTY_process = Process.Start("MMTTY.EXE", "-r -n -Z") ''NOT SURE WHAT THE -n DOES IN THIS CASE!

        'Load Map
        WebBrowser1.Navigate(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GMaps.html"))
    End Sub

    Friend Sub AddEvent(thisEvent As talon_event)
        ListBox1.Items.Add(thisEvent)
    End Sub

    Friend Sub RemoveEvent(thisEvent As talon_event)
        ListBox1.Items.Remove(thisEvent)
    End Sub

    Friend Sub UpdateTimeShown()
        For Each thisRock As talon_node In rock_list
            If (ComboBox1.Tag = thisRock.node_name) Then
                last_comm_label.Text = FormatTime(thisRock.last_comm)
            End If
        Next
    End Sub

    Function FormatTime(thisDate As Date) As String
        Dim hourSince As String = thisDate.Hour.ToString
        Dim minuteSince As String = thisDate.Minute.ToString
        Dim secondSince As String = thisDate.Second.ToString

        'Format numbers less than values of 10 (to ensure at least two digits appear on screen)
        If (thisDate.Hour < 10) Then
            hourSince = "0" + hourSince
        End If

        If (thisDate.Minute < 10) Then
            minuteSince = "0" + minuteSince
        End If

        If (thisDate.Second < 10) Then
            secondSince = "0" + secondSince
        End If

        Return hourSince + ":" + minuteSince + ":" + secondSince

    End Function

    Friend Function is_GPS_Override_Set() As Boolean
        Return (gps_override_button.Text = "Disable GPS Override")
    End Function

    Friend Sub Set_GPS_Override()
        gps_override_button.Text = "Disable GPS Override"
    End Sub

    Friend Sub Reset_GPS_Override()
        gps_override_button.Text = "Manually Input GPS"
    End Sub

    Friend Function is_Ignore_Set() As Boolean
        Return (ignore_node_button.Text = "Allow Node")
    End Function

    Friend Sub Set_Ignore()
        ignore_node_button.Text = "Allow Node"
    End Sub

    Friend Sub Reset_Ignore()
        ignore_node_button.Text = "Ignore Node"
    End Sub

    Friend Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged

        'Update buttons
        mark_unread_button.Enabled = False
        ignore_msg_button.Enabled = False

        If (ListBox1.Items.Count = 0) Then
            ListBox1.Enabled = False
        Else
            ListBox1.Enabled = True
        End If

        If (IsNothing(ListBox1.SelectedItem)) Then
            Exit Sub
        End If

        For Each thisEvent As talon_event In event_list
            If (ListBox1.SelectedItem.ToString.Contains(thisEvent.event_name)) Then
                'Save event name
                ListBox1.Tag = thisEvent.event_name

                'Update labels
                event_msg_type.Text = thisEvent.message_type
                event_node_name_label.Text = thisEvent.node_name
                event_speed_label.Text = thisEvent.speed + " mph"

                'Update buttons
                mark_unread_button.Enabled = True
                ignore_msg_button.Enabled = True
                Exit For
            End If
        Next

        Update_ListBox1(False)
    End Sub


    Friend Sub Update_ListBox1(recentlyNewed As Boolean)
        'Save the currently selected one
        Dim currEventName
        Dim currEventTag = ListBox1.Tag
        Dim perEventUpdateName As String


        RemoveHandler ListBox1.SelectedIndexChanged, AddressOf ListBox1_SelectedIndexChanged

        If (IsNothing(ListBox1.SelectedItem) = False) Then
            currEventName = ListBox1.SelectedItem.ToString
        End If

        'Clear listbox
        'ListBox1.Items.Clear()

        'TODO: Do not clear the entire list, just update the one event we need to update

        'Lower new flag if something is selected
        If (IsNothing(currEventName) = False And recentlyNewed = False) Then
            For i As Integer = 0 To event_list.Count - 1
                Dim thisEvent As talon_event = event_list(i)

                If (currEventName.Contains(thisEvent.event_name)) Then
                    thisEvent.new_flag = False
                    event_list(i) = thisEvent
                    Exit For
                End If
            Next
        End If

        For i As Integer = 0 To event_list.Count - 1
            Dim thisEvent As talon_event = event_list(i)

            'Add to list
            perEventUpdateName = thisEvent.event_name

            'Add the ignore message to names
            If (thisEvent.ignore_flag = True) Then
                perEventUpdateName = "(i) " + perEventUpdateName
                If (IsNothing(currEventTag) = False) Then
                    If (thisEvent.event_name.Contains(currEventTag)) Then
                        ignore_msg_button.Text = "Allow Event"
                    End If
                End If
            ElseIf (IsNothing(currEventTag) = False) Then
                If (thisEvent.event_name.Contains(currEventTag)) Then
                    ignore_msg_button.Text = "Ignore Event"
                End If
            End If


            'Add the new message to names
            If (thisEvent.new_flag = True) Then
                perEventUpdateName = perEventUpdateName + " (new)"
            End If

            'Add the final name to the list
            If (ListBox1.Items.Count = i) Then
                ListBox1.Items.Add(perEventUpdateName)
            Else
                ListBox1.Items(i) = perEventUpdateName
            End If
        Next

        'Highlight previously selected item
        If (IsNothing(currEventTag) = False) Then
            For Each thisItem In ListBox1.Items
                If (thisItem.ToString.Contains(currEventTag)) Then
                    ListBox1.SelectedItem = thisItem.ToString
                    Exit For
                End If
            Next
        Else
            ListBox1.Tag = Nothing
        End If

        'Check to see if the list is empty
        If (ListBox1.Items.Count = 0) Then
            mark_unread_button.Enabled = False
            ignore_msg_button.Enabled = False
        Else
            mark_unread_button.Enabled = True
            ignore_msg_button.Enabled = True
        End If

        AddHandler ListBox1.SelectedIndexChanged, AddressOf ListBox1_SelectedIndexChanged
    End Sub

    Friend Sub ComboBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedValueChanged, ComboBox1.SelectedIndexChanged
        'Dim buttonEnable As Boolean = False

        If (IsNothing(ComboBox1.SelectedItem) = True) Then
            Exit Sub
        End If

        For Each thisRock As talon_node In rock_list

            If (ComboBox1.SelectedItem.ToString.Contains(thisRock.node_name)) Then
                'Save node name
                ComboBox1.Tag = thisRock.node_name

                number_detections_label.Text = thisRock.number_of_detections

                'Build time
                last_comm_label.Text = FormatTime(thisRock.last_comm)

                gps_lat_label.Text = Math.Abs(thisRock.gps_lat_deg) & "° " & _
                    thisRock.gps_lat_min & "' " & _
                    thisRock.gps_lat_sec & Chr(34)

                If (thisRock.gps_lat_deg > 0) Then
                    gps_lat_label.Text += " N"
                ElseIf (thisRock.gps_lat_deg < 0) Then
                    gps_lat_label.Text += " S"
                End If

                gps_long_label.Text = Math.Abs(thisRock.gps_long_deg) & "° " & _
                    thisRock.gps_long_min & "' " & _
                    thisRock.gps_long_sec & Chr(34)

                If (thisRock.gps_long_deg > 0) Then
                    gps_long_label.Text += " E"
                ElseIf (thisRock.gps_long_deg < 0) Then
                    gps_long_label.Text += " W"
                End If

                'GPS Override Button
                If (thisRock.manual_gps = True) Then
                    Set_GPS_Override()
                Else
                    Reset_GPS_Override()
                End If

                'Ignore Node Button
                If (thisRock.ignore_flag = True) Then
                    Set_Ignore()
                Else
                    Reset_Ignore()
                End If

                'Center Map Button
                If (thisRock.gps_lat_deg + thisRock.gps_lat_min + thisRock.gps_lat_sec + _
                    thisRock.gps_long_deg + thisRock.gps_long_min + thisRock.gps_long_sec = 0) Then
                    center_map_button.Enabled = False
                Else
                    center_map_button.Enabled = True
                End If

                'Other buttons
                reset_detections_button.Enabled = True
                delete_node_button.Enabled = True
                gps_override_button.Enabled = True
                ignore_node_button.Enabled = True

                'Highlight marker
                If (thisRock.ignore_flag = False) Then
                    InvokeWebScript("highlightMarker", New String() {thisRock.node_name, _
                                                                     RockToLatitude(thisRock), _
                                                                     RockToLongitude(thisRock)})

                    Exit For
                End If
            End If
        Next
    End Sub

    Friend Sub InvokeWebScript(functionName As String, params As Object())
        If (IsNothing(WebBrowser1) = False And IsNothing(WebBrowser1.Document) = False) Then
            WebBrowser1.Document.InvokeScript(functionName, params)
        End If
    End Sub

    Friend Sub center_map_button_Click(sender As Object, e As EventArgs) Handles center_map_button.Click
        If (IsNothing(ComboBox1.Tag) = False) Then
            InvokeWebScript("centerMap", New String() {ComboBox1.Tag})
        End If
    End Sub

    Friend Sub gps_override_button_Click(sender As Object, e As EventArgs) Handles gps_override_button.Click
        If (IsNothing(ComboBox1.Tag) = False) Then
            If (is_GPS_Override_Set()) Then
                'Find the node
                For i As Integer = 0 To rock_list.Count - 1
                    Dim thisRock As talon_node = rock_list(i)

                    If (ComboBox1.Tag.ToString.Contains(thisRock.node_name)) Then

                        'Disable the override, reset coordinates to 0
                        thisRock.gps_lat_deg = 0
                        thisRock.gps_lat_min = 0
                        thisRock.gps_lat_sec = 0
                        thisRock.gps_long_deg = 0
                        thisRock.gps_long_min = 0
                        thisRock.gps_long_sec = 0

                        'Update list
                        thisRock.last_comm = rock_list(i).last_comm
                        thisRock.manual_gps = False
                        rock_list(i) = thisRock

                        'Update UI
                        ComboBox1_SelectedValueChanged(Nothing, Nothing)

                        'Update webpage map coordinates
                        If (thisRock.ignore_flag = False) Then
                            InvokeWebScript("highlightMarker", _
                                            New String() {thisRock.node_name, _
                                                          RockToLatitude(thisRock), _
                                                          RockToLongitude(thisRock)})
                        End If

                        'Change the button
                        Reset_GPS_Override()

                    End If
                Next
            Else
                'Show the override form
                GPSOverride.ShowDialog()

            End If
        End If
    End Sub

    Friend Sub ignore_node_button_Click(sender As Object, e As EventArgs) Handles ignore_node_button.Click
        If (IsNothing(ComboBox1.Tag) = False) Then

            'Find the node
            For i As Integer = 0 To rock_list.Count - 1
                Dim thisRock As talon_node = rock_list(i)

                If (ComboBox1.Tag.ToString.Contains(thisRock.node_name)) Then

                    If (is_Ignore_Set()) Then

                        'Reset Flag
                        thisRock.ignore_flag = False

                        'Update list
                        thisRock.last_comm = rock_list(i).last_comm
                        rock_list(i) = thisRock

                        'Update GMaps
                        InvokeWebScript("showMarker", New String() {thisRock.node_name})

                        'Change the button
                        Reset_Ignore()

                        'Update entry
                        ComboBox1.Items.Remove(thisRock.node_name + " (ignored)")
                        ComboBox1.Items.Add(thisRock.node_name)
                        ComboBox1.SelectedItem = thisRock.node_name
                    Else

                        'Set Flag
                        thisRock.ignore_flag = True

                        'Update list
                        thisRock.last_comm = rock_list(i).last_comm
                        rock_list(i) = thisRock

                        'Update GMaps
                        InvokeWebScript("hideMarker", New String() {thisRock.node_name})

                        'Change the button
                        Set_Ignore()

                        'Update entry
                        ComboBox1.Items.Remove(thisRock.node_name)
                        ComboBox1.Items.Add(thisRock.node_name + " (ignored)")
                        ComboBox1.SelectedItem = thisRock.node_name + " (ignored)"

                    End If

                End If
            Next
        End If
    End Sub

    Friend Sub delete_node_button_Click(sender As Object, e As EventArgs) Handles delete_node_button.Click
        If (IsNothing(ComboBox1.Tag) = False) Then
            Dim response
            response = MsgBox("Are you sure you want to delete this node?", vbYesNoCancel, "Confirm Action")

            If response = vbYes Then
                'Find the node
                For i As Integer = 0 To rock_list.Count - 1
                    Dim thisRock As talon_node = rock_list(i)
                    If (ComboBox1.Tag.ToString.Contains(thisRock.node_name)) Then

                        'Delete from array list
                        rock_list.RemoveAt(i)

                        'Delete from webpage
                        InvokeWebScript("removeMarker", New String() {thisRock.node_name})

                        'Disable all the buttons
                        center_map_button.Enabled = False
                        gps_override_button.Enabled = False
                        ignore_node_button.Enabled = False
                        reset_detections_button.Enabled = False
                        delete_node_button.Enabled = False

                        'Reset labels
                        gps_lat_label.Text = ""
                        gps_long_label.Text = ""
                        last_comm_label.Text = ""
                        number_detections_label.Text = ""

                        'Reset tag
                        ComboBox1.Tag = Nothing

                        'Remove from ComboBox
                        ComboBox1.Items.Remove(thisRock.node_name)
                        ComboBox1.Items.Remove(thisRock.node_name + " (ignored)")

                        'Break from the sub
                        Exit Sub
                    End If
                Next
            End If
        End If
    End Sub

    Friend Sub reset_detections_button_Click(sender As Object, e As EventArgs) Handles reset_detections_button.Click
        If (IsNothing(ComboBox1.Tag) = False) Then
            Dim response
            response = MsgBox("Are you sure you want to reset this node's detection counter? (Does not effect event history)", vbYesNoCancel, "Confirm Action")

            If response = vbYes Then

                'Find the node
                For i As Integer = 0 To rock_list.Count - 1
                    Dim thisRock As talon_node = rock_list(i)
                    If (ComboBox1.Tag.ToString.Contains(thisRock.node_name)) Then

                        'Reset the info
                        thisRock.number_of_detections = 0

                        'Update text
                        number_detections_label.Text = thisRock.number_of_detections

                        'Update list
                        thisRock.last_comm = rock_list(i).last_comm
                        rock_list(i) = thisRock
                    End If
                Next

            End If
        End If
    End Sub

    Friend Sub ignore_msg_button_Click(sender As Object, e As EventArgs) Handles ignore_msg_button.Click
        If (IsNothing(ListBox1.Tag) = False) Then
            'Find the node
            For i As Integer = 0 To event_list.Count - 1
                Dim thisEvent As talon_event = event_list(i)

                If (ListBox1.Tag.ToString.Contains(thisEvent.event_name)) Then

                    If (thisEvent.ignore_flag = True) Then

                        'Reset Flag
                        thisEvent.ignore_flag = False

                        'Update list
                        event_list(i) = thisEvent

                        'Change the button
                        ignore_msg_button.Text = "Ignore Event"

                        'Update entry
                        Update_ListBox1(False)
                    Else

                        'Set Flag
                        thisEvent.ignore_flag = True

                        'Update list
                        event_list(i) = thisEvent

                        'Change the button
                        ignore_msg_button.Text = "Allow Event"

                        'Update entry
                        Update_ListBox1(False)

                    End If

                End If
            Next
        End If
    End Sub

    Friend Sub mark_unread_button_Click(sender As Object, e As EventArgs) Handles mark_unread_button.Click
        If (IsNothing(ListBox1.Tag) = False) Then
            'Find the node
            For i As Integer = 0 To event_list.Count - 1
                Dim thisEvent As talon_event = event_list(i)

                If (ListBox1.Tag.ToString.Contains(thisEvent.event_name)) Then

                    'Raise flag
                    thisEvent.new_flag = True

                    'Update list
                    event_list(i) = thisEvent

                    'Update entry
                    Update_ListBox1(True)

                End If
            Next
        End If
    End Sub

    Friend Sub refresh_page_button_Click(sender As Object, e As EventArgs) Handles refresh_page_button.Click
        WebBrowser1.Navigate(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GMaps.html"))
    End Sub

    Private Sub repopulate_map_button_Click(sender As Object, e As EventArgs) Handles repopulate_map_button.Click
        For i As Integer = 0 To rock_list.Count - 1
            Dim thisRock As talon_node = rock_list(i)
            Dim thisRockLat = RockToLatitude(thisRock)
            Dim thisRockLong = RockToLongitude(thisRock)

            If ((thisRockLat <> 0 Or thisRockLong <> 0) And thisRock.ignore_flag = False) Then
                If (IsNothing(ComboBox1.Tag) = False) Then
                    If (ComboBox1.Tag.ToString.Contains(thisRock.node_name)) Then
                        InvokeWebScript("highlightMarker", _
                                        New String() {thisRock.node_name, thisRockLat, thisRockLong})
                    Else
                        InvokeWebScript("updateMarker", _
                                        New String() {thisRock.node_name, thisRockLat, thisRockLong})
                    End If
                Else
                    InvokeWebScript("updateMarker", _
                                    New String() {thisRock.node_name, thisRockLat, thisRockLong})
                End If
            End If
        Next
    End Sub

    Friend Sub RockStatusTimer_Tick(sender As Object, e As EventArgs) Handles RockStatusTimer.Tick
        For i As Integer = 0 To rock_list.Count - 1
            Dim thisRock As talon_node = rock_list(i)
            If (thisRock.last_comm > New Date(0)) Then
                thisRock.last_comm = thisRock.last_comm.AddSeconds(1)

                'Update list
                rock_list(i) = thisRock
            End If
        Next

        UpdateTimeShown()
    End Sub

    Friend Function RockToLatitude(thisRock As talon_node) As Double
        Dim latitudeDec As Double = Math.Abs(thisRock.gps_lat_deg) + _
            Math.Abs(thisRock.gps_lat_min / 60) + _
            Math.Abs(thisRock.gps_lat_sec / 3600)


        If (thisRock.gps_lat_deg < 0) Then
            Return -1 * latitudeDec
        Else
            Return latitudeDec
        End If
    End Function

    Friend Function RockToLongitude(thisRock As talon_node) As Double
        Dim longitudeDec As Double = Math.Abs(thisRock.gps_long_deg) + _
            Math.Abs(thisRock.gps_long_min / 60) + _
            Math.Abs(thisRock.gps_long_sec / 3600)

        If (thisRock.gps_long_deg < 0) Then
            Return -1 * longitudeDec
        Else
            Return longitudeDec
        End If
    End Function

    Private Sub show_MTTY_console_button_Click(sender As Object, e As EventArgs) Handles show_MTTY_console_button.Click
        MMTTY_Console.Show()
        MMTTY_Console.Focus()
    End Sub

    '**************** Time to handle all MMTTY window events ********************
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

                            MMTTY_Console.monitorBar.Value = signalLevel

                            If (MMTTY_Console.squelchBar.Value = 1) Then
                                'Enable squelch slider
                                MMTTY_Console.squelchBar.Enabled = True

                                'Set initial squelch
                                MMTTY_Console.squelchBar.Value = squelchLevel
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

                            MMTTY_Console.RTTYbox.Text = "RTTY Status - Running"

                        Case FROM_MMTTY.TXM_CHAR
                            'Dim incomingDataStart As Boolean = False
                            'Dim incomingDataBuffer As Queue(Of Char) = New Queue(Of Char)()
                            'Dim incomingDataNumUntilEnd As Integer = -1
                            Dim incomingChar As Char = Chr(lParam.ToString)

                            'Add to debug box
                            MMTTY_Console.MMTTYBox.Text = MMTTY_Console.MMTTYBox.Text + incomingChar

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
                                    incomingDataNumUntilEnd = 3
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

    Private Sub handleRockData(rockName As String)
        Dim tempRock As talon_node

        'Check to see if the node already exists
        For i As Integer = 0 To rock_list.Count - 1
            Dim thisRock As talon_node = rock_list(i)

            If (InStr(thisRock.node_name, rockName)) Then
                Exit Sub
            End If
        Next

        tempRock = New talon_node With {
            .node_name = "Node xxxx", _
            .gps_lat_deg = 0, _
            .gps_lat_min = 0, _
            .gps_lat_sec = 0, _
            .gps_long_deg = 0, _
            .gps_long_min = 0, _
            .gps_long_sec = 0, _
            .number_of_detections = 0, _
            .last_comm = New Date(1)
        }

        rock_list.Add(tempRock)
        ComboBox1.Items.Add(tempRock.node_name)
    End Sub

    Private Sub handleIncomingData(incomingString As String)
        Dim stringPortionPos As Integer = 0
        Dim message_id As String = ""
        Dim module_id As String = ""
        Dim ham_message As String = ""
        Dim checksum As UInteger = 0
        Dim checksumCalculated As UInteger = 0
        Dim thisRock As talon_node
        Dim found_rock As Integer
        Dim newEvent As talon_event = New talon_event

        'Needs to contain checksum identifier
        If (Not incomingString.Contains("&")) Then
            Exit Sub
        End If

        Try
            'Remove leading $ sign
            incomingString = incomingString.Substring(1)

            'Determine checksum
            checksum = Convert.ToInt32(incomingString.Substring(incomingString.LastIndexOf("&") + 1, 2), 16)
            incomingString = incomingString.Substring(0, incomingString.LastIndexOf("&"))

            'Determine module id
            module_id = incomingString.Split(";")(0).Trim

            'Determine message id
            message_id = incomingString.Split(";")(1).Trim

            'Determine message
            ham_message = incomingString.Split(";")(2).Trim

            'Determine if the message is valid
            checksumCalculated = 0

            'Temp:
            '   start with uint 0x0000
            '   Add up all the individual character's values
            '   Flip bits
            '   Add moduleID
            '   converts checksum to ascii
            '   converts module

            For Each currChar As Char In message_id & ";" & ham_message
                checksumCalculated += Asc(currChar)
            Next

            checksumCalculated = Not (checksumCalculated)
            checksumCalculated = &HFFFF And checksumCalculated 'This will prevent overflow exception (yes we want an overflow)
            'checksumCalculated += Asc(message_id)
            checksumCalculated += module_id

            checksumCalculated = &HFF And checksumCalculated

            'Compare both checksums
            If (checksum <> checksumCalculated And Not (DEBUG_BYPASS_CHECKSUM)) Then
                Exit Sub
            End If

            'Check to see if the node already exists
            For found_rock = 0 To rock_list.Count - 1
                Dim tempRock As talon_node = rock_list(found_rock)

                If (tempRock.node_name.Contains(module_id)) Then
                    thisRock = rock_list(found_rock)
                    Exit For
                End If
            Next

            If (found_rock = rock_list.Count) Then
                thisRock = New talon_node With {
                .node_name = "Node " & module_id, _
                .gps_lat_deg = 0, _
                .gps_lat_min = 0, _
                .gps_lat_sec = 0, _
                .gps_long_deg = 0, _
                .gps_long_min = 0, _
                .gps_long_sec = 0, _
                .number_of_detections = 0, _
                .last_comm = New Date(1)
            }

                found_rock = -1
                ComboBox1.Items.Add(thisRock.node_name)
            End If

            Select Case message_id
                Case "G"
                    thisRock.gps_lat_deg = ham_message.Split(",")(1).Substring(0, 2)
                    thisRock.gps_lat_min = ham_message.Split(",")(1).Substring(2, 2)
                    thisRock.gps_lat_sec = ham_message.Split(",")(1).Substring(4) * 60

                    thisRock.gps_long_deg = ham_message.Split(",")(3).Substring(0, 3)
                    thisRock.gps_long_min = ham_message.Split(",")(3).Substring(3, 2)
                    thisRock.gps_long_sec = ham_message.Split(",")(3).Substring(5) * 60

                    'Check for negativity
                    If (ham_message.Split(",")(2).Contains("S")) Then
                        thisRock.gps_lat_deg *= -1
                    End If

                    If (ham_message.Split(",")(4).Contains("W")) Then
                        thisRock.gps_long_deg *= -1
                    End If

                    'Add/Move marker on map
                    InvokeWebScript("updateMarker", _
                        New String() {thisRock.node_name, RockToLatitude(thisRock), RockToLongitude(thisRock)})

                    'Add information to event list
                    newEvent.message_type = "GPS Update"
                Case "S"
                    'Add information to event list
                    newEvent.message_type = "Awake Beacon"

                Case "D"
                    'Add information to event list
                    newEvent.message_type = "Detection Alarm"

                    thisRock.number_of_detections = thisRock.number_of_detections + 1

            End Select

            'Event list update
            newEvent.time = DateTime.UtcNow
            newEvent.speed = 0
            newEvent.node_name = thisRock.node_name
            newEvent.new_flag = True
            newEvent.ignore_flag = False
            newEvent.event_name = newEvent.time.ToString("dd MMM yyyy - HH:mm:ss.ff")

            'Add event to event list
            event_list.Add(newEvent)

            'Refresh listbox
            Update_ListBox1(False)

            'Update last communication timer
            thisRock.last_comm = New Date(1)

            If (found_rock = -1) Then
                rock_list.Add(thisRock)
            Else
                rock_list(found_rock) = thisRock
            End If

            'Update selected info
            If (ComboBox1.Tag = thisRock.node_name) Then
                ComboBox1_SelectedValueChanged(Nothing, Nothing)
            End If

            'Enable the combobox if needed
            ComboBox1.Enabled = 1

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Friend Sub squelchBarScroll_event_executer()
        PostMessage(MMTTY_Handle, MSG_MMTTY, TO_MMTTY.RXM_SETSQLVL, MMTTY_Console.squelchBar.Value)
    End Sub

    Private Sub RTTYprogressBar()
        Dim boxGraphics As Graphics = MMTTY_Console.RTTYbox.CreateGraphics()

        'Draw sq open box
        Dim xPos2 As Integer = MMTTY_Console.monitorBar.Location.X + MMTTY_Console.monitorBar.Width + 5
        boxGraphics.FillRectangle(blackBrush, xPos2 - 1, MMTTY_Console.monitorBar.Location.Y + 5 - 1, MMTTY_Console.monitorBar.Size.Height - 10 + 2, MMTTY_Console.monitorBar.Size.Height - 10 + 2)
        boxGraphics.FillRectangle(redBrush, xPos2, MMTTY_Console.monitorBar.Location.Y + 5, MMTTY_Console.monitorBar.Size.Height - 10, MMTTY_Console.monitorBar.Size.Height - 10)

        If (MMTTY_Console.squelchBar.Value >= MMTTY_Console.monitorBar.Value) Then
            boxGraphics.FillRectangle(blackBrush, xPos2, MMTTY_Console.monitorBar.Location.Y + 5, MMTTY_Console.monitorBar.Size.Height - 10, MMTTY_Console.monitorBar.Size.Height - 10)
        End If
    End Sub

    
End Class