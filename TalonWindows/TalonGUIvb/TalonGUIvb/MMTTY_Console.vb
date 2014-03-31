Public Class MMTTY_Console
    Dim blueBrush As New Drawing.SolidBrush(Color.Blue)
    Dim whiteBrush As New Drawing.SolidBrush(Color.White)
    Dim redBrush As New Drawing.SolidBrush(Color.Red)
    Dim blackBrush As New Drawing.SolidBrush(Color.Black)
    Dim greenBrush As New Drawing.SolidBrush(Color.Green)

    Private Sub RTTYbox_Paint(sender As Object, e As PaintEventArgs) Handles RTTYbox.Paint
        'Draw single line
        Dim xPos As Integer = monitorBar.Location.X + monitorBar.Width * squelchBar.Value / (monitorBar.Maximum - monitorBar.Minimum)
        e.Graphics.FillRectangle(blackBrush, xPos, monitorBar.Location.Y + 1, 5, monitorBar.Size.Height - 2)

    End Sub

    Private Sub MMTTYBox_TextChanged(sender As Object, e As EventArgs) Handles MMTTYBox.TextChanged
        MMTTYBox.Text = MMTTYBox.Text.Trim

        If (MMTTYBox.TextLength > 69) Then
            MMTTYBox.Text = MMTTYBox.Text.Substring(1, MMTTYBox.TextLength - 1)
        End If
    End Sub

    Private Sub squelchBar_Scroll(sender As Object, e As EventArgs) Handles squelchBar.Scroll
        MainScreen.squelchBarScroll_event_executer()
        monitorBar.Refresh()
    End Sub
End Class