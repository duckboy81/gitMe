Public Class GPSOverride

    Friend Sub override_win_confirm_Click(sender As Object, e As EventArgs) Handles override_win_confirm.Click
        'Find the node
        For i As Integer = 0 To MainScreen.rock_list.Count - 1
            Dim thisRock As MainScreen.talon_node = MainScreen.rock_list(i)

            If (MainScreen.ComboBox1.Tag.ToString.Contains(thisRock.node_name)) Then
                'Move the info
                thisRock.gps_lat_deg = lat_deg.Value
                thisRock.gps_lat_min = lat_min.Value
                thisRock.gps_lat_sec = lat_sec.Value
                thisRock.gps_long_deg = lng_deg.Value
                thisRock.gps_long_min = lng_min.Value
                thisRock.gps_long_sec = lng_sec.Value

                'Validate input
                If (thisRock.gps_lat_deg = 90) Then
                    thisRock.gps_lat_min = 0
                    lat_min.Value = 0

                    thisRock.gps_lat_sec = 0
                    lat_sec.Value = 0
                End If

                If (thisRock.gps_long_deg = 180) Then
                    thisRock.gps_long_min = 0
                    lng_min.Value = 0

                    thisRock.gps_long_sec = 0
                    lng_sec.Value = 0
                End If

                'Check for direction change (N/S)
                If (lat_dir.Text = "S") Then
                    thisRock.gps_lat_deg = thisRock.gps_lat_deg * -1
                End If

                'Check for direction change (E/W)
                If (lng_dir.Text = "W") Then
                    thisRock.gps_long_deg = thisRock.gps_long_deg * -1
                End If

                'Update list
                thisRock.last_comm = MainScreen.rock_list(i).last_comm
                thisRock.manual_gps = True
                MainScreen.rock_list(i) = thisRock

                'Update UI
                MainScreen.ComboBox1_SelectedValueChanged(Nothing, Nothing)

                'Update button
                MainScreen.Set_GPS_Override()

                'TODO: Update GMap list

                'Close window
                Me.Close()
            End If
        Next

    End Sub

    Friend Sub override_win_cancel_Click(sender As Object, e As EventArgs) Handles override_win_cancel.Click
        Me.Close()
    End Sub

    Friend Sub GPSOverride_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Set N & E in the direction boxes for easier user understanding/interaction
        lat_dir.SelectedIndex = 0
        lng_dir.SelectedIndex = 0
    End Sub

    Private Sub lat_deg_ValueChanged(sender As Object, e As EventArgs) Handles lat_deg.ValueChanged
        If (lat_deg.Value = 90) Then
            lat_min.Value = 0
            lat_sec.Value = 0
        End If
    End Sub

    Private Sub lng_deg_ValueChanged(sender As Object, e As EventArgs) Handles lng_deg.ValueChanged
        If (lng_deg.Value = 180) Then
            lng_min.Value = 0
            lng_sec.Value = 0
        End If
    End Sub
End Class