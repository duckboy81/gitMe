'Created by Jean-Luc Duckworth 2013-2014

Option Explicit On

Public Class MainScreen
    'External Windows libraries used to find MMTTY window on startup
    Private Declare Function FindWindow Lib "user32.dll" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Private Declare Function FindWindowEx Lib "user32.dll" Alias "FindWindowExA" (ByVal hWndParent As IntPtr, ByVal hWndChildAfter As Integer, ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Private Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (ByVal hWnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As IntPtr
    Private Declare Function PostMessage Lib "user32.dll" Alias "PostMessageA" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As IntPtr
    Private Declare Function RegisterWindowMessage Lib "user32.dll" Alias "RegisterWindowMessageA" (ByVal lpString As String) As UInteger

    '******CONFIGURATION VARIABLES******'
    Dim DEBUG_BYPASS_CHECKSUM As Boolean = False
    Dim numChar2ShowUpdate As Integer = 14
    Dim configBaudRate As Integer = 7500 '(Actual Baud rate * 100) = this configuration
    Dim configMarkFreq As Integer = 2200 'Hz
    Dim configSpaceFreq As Integer = 2540 'Hz

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

    Dim rock_list As List(Of talon_node) = New List(Of talon_node)
    Dim event_list As List(Of talon_event) = New List(Of talon_event)

    'Multiple Enum declarations
    Enum message_types
        GPS_MSG
        DETECT_MSG
        EXFIL_NODE_IDENT_MSG
    End Enum

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


    Private Structure talon_node
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

    Private Structure talon_event
        Public time As Date
        Public new_flag As Boolean
        Public ignore_flag As Boolean
        Public node_name As String
        Public message_type As message_types

    End Structure

    Private Sub MainScreen_load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Hide MMTTY
        'MMTTY_process = Process.Start("MMTTY.EXE", "-r -m -Z")
        'MMTTY_process = Process.Start("MMTTY.EXE", "-r -n -Z") ''NOT SURE WHAT THE -n DOES IN THIS CASE!

        'Load Map
        WebBrowser1.Navigate(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GMaps.html"))
    End Sub

    Private Sub AddEvent(thisEvent As talon_event)
        ListBox1.Items.Add(thisEvent)
    End Sub

    Private Sub RemoveEvent(thisEvent As talon_event)
        ListBox1.Items.Remove(thisEvent)
    End Sub

    Private Sub UpdateTimeShown()
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

    Private Function is_GPS_Override_Set() As Boolean
        Return (gps_override_button.Text = "Disable GPS Override")
    End Function

    Private Sub Set_GPS_Override()
        gps_override_button.Text = "Disable GPS Override"
    End Sub

    Private Sub Reset_GPS_Override()
        gps_override_button.Text = "Manually Input GPS"
    End Sub

    Private Function is_Ignore_Set() As Boolean
        Return (ignore_node_button.Text = "Allow Node")
    End Function

    Private Sub Set_Ignore()
        ignore_node_button.Text = "Allow Node"
    End Sub

    Private Sub Reset_Ignore()
        ignore_node_button.Text = "Ignore Node"
    End Sub

    Private Sub ComboBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedValueChanged, ComboBox1.SelectedIndexChanged
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

                gps_lat_label.Text = thisRock.gps_lat_deg & "° " & _
                    thisRock.gps_lat_min & "' " & _
                    thisRock.gps_lat_sec & Chr(34)

                If (thisRock.gps_lat_deg > 0) Then
                    gps_lat_label.Text += " N"
                ElseIf (thisRock.gps_lat_deg < 0) Then
                    gps_lat_label.Text += " S"
                End If

                gps_long_label.Text = thisRock.gps_long_deg & "° " & _
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
                InvokeWebScript("highlightMarker", New String() {thisRock.node_name})

                Exit For
            End If
        Next
    End Sub

    Private Sub InvokeWebScript(functionName As String, params As Object())
        If (IsNothing(WebBrowser1) = False And IsNothing(WebBrowser1.Document) = False) Then
            WebBrowser1.Document.InvokeScript(functionName, params)
        End If
    End Sub

    Private Sub center_map_button_Click(sender As Object, e As EventArgs) Handles center_map_button.Click
        If (IsNothing(ComboBox1.Tag) = False) Then
            InvokeWebScript("centerMap", New String() {ComboBox1.Tag})
        End If
    End Sub

    Private Sub gps_override_button_Click(sender As Object, e As EventArgs) Handles gps_override_button.Click
        If (IsNothing(ComboBox1.Tag) = False) Then
            If (is_GPS_Override_Set()) Then
                'Disable the override, find the most recent coordinates from the batch that we haven't ignored.  If none, set 0's.

                'Update webpage map coordinates

                'Change the button
                Reset_GPS_Override()
            Else
                'Show the override form
                GPSOverride.ShowDialog()

                ''Update webpage map coordinate && change the push button as needed in the other form
            End If
        End If
    End Sub

    Private Sub ignore_node_button_Click(sender As Object, e As EventArgs) Handles ignore_node_button.Click
        If (IsNothing(ComboBox1.Tag) = False) Then

            'Find the node
            For i As Integer = 0 To rock_list.Count - 1
                Dim thisRock As talon_node = rock_list(i)

                If (ComboBox1.Tag.ToString.Contains(thisRock.node_name)) Then

                    If (is_Ignore_Set()) Then

                        'Reset Flag
                        thisRock.ignore_flag = False

                        'Update list
                        rock_list(i) = thisRock

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
                        rock_list(i) = thisRock

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

    Private Sub delete_node_button_Click(sender As Object, e As EventArgs) Handles delete_node_button.Click
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

    Private Sub reset_detections_button_Click(sender As Object, e As EventArgs) Handles reset_detections_button.Click
        If (IsNothing(ComboBox1.Tag) = False) Then
            'Find the node
            For i As Integer = 0 To rock_list.Count - 1
                Dim thisRock As talon_node = rock_list(i)
                If (ComboBox1.Tag.ToString.Contains(thisRock.node_name)) Then

                    'Reset the info
                    thisRock.number_of_detections = 0

                    'Update text
                    number_detections_label.Text = thisRock.number_of_detections

                    'Update list
                    rock_list(i) = thisRock
                End If
            Next

        End If
    End Sub

    Private Sub mark_unread_button_Click(sender As Object, e As EventArgs) Handles mark_unread_button.Click
        'TODO: Remove this manual addition
        ComboBox1.Enabled = True
        rock_list.Add(New talon_node With {.node_name = "Node 0001", _
                                           .gps_lat_deg = 32, _
                                           .gps_lat_min = 45, _
                                           .gps_lat_sec = 134.0, _
                                           .gps_long_deg = 151, _
                                           .gps_long_min = 21, _
                                           .gps_long_sec = 10.134, _
                                           .number_of_detections = 10, _
                                           .last_comm = New Date(1, 1, 1, 4, 1, 39, 0)
                                           })
        ComboBox1.Items.Add(rock_list(0).node_name)

        rock_list.Add(New talon_node With {.node_name = "Node 0002", _
                                           .gps_lat_deg = 42, _
                                           .gps_lat_min = 43, _
                                           .gps_lat_sec = 124.0, _
                                           .gps_long_deg = 131, _
                                           .gps_long_min = 25, _
                                           .gps_long_sec = 11.134, _
                                           .number_of_detections = 4, _
                                           .last_comm = New Date(1, 1, 2, 4, 4, 39, 0)
                                           })
        ComboBox1.Items.Add(rock_list(1).node_name)

        rock_list.Add(New talon_node With {.node_name = "Node 0003", _
                                           .gps_lat_deg = 12, _
                                           .gps_lat_min = 35, _
                                           .gps_lat_sec = 114.0, _
                                           .gps_long_deg = 141, _
                                           .gps_long_min = 12, _
                                           .gps_long_sec = 10.121, _
                                           .number_of_detections = 3, _
                                           .last_comm = New Date(1, 1, 1, 1, 45, 39, 0)
                                           })
        ComboBox1.Items.Add(rock_list(2).node_name)
    End Sub
End Class