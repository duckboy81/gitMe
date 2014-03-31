<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GPSOverride
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GPSOverride))
        Me.override_win_confirm = New System.Windows.Forms.Button()
        Me.override_win_cancel = New System.Windows.Forms.Button()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lat_deg = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lng_deg = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lat_min = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lng_min = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lat_sec = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lng_sec = New System.Windows.Forms.NumericUpDown()
        Me.lat_dir = New System.Windows.Forms.ComboBox()
        Me.lng_dir = New System.Windows.Forms.ComboBox()
        CType(Me.lat_deg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lng_deg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lat_min, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lng_min, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lat_sec, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lng_sec, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'override_win_confirm
        '
        Me.override_win_confirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.override_win_confirm.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.override_win_confirm.Location = New System.Drawing.Point(142, 112)
        Me.override_win_confirm.Name = "override_win_confirm"
        Me.override_win_confirm.Size = New System.Drawing.Size(111, 26)
        Me.override_win_confirm.TabIndex = 9
        Me.override_win_confirm.Text = "Override"
        Me.override_win_confirm.UseVisualStyleBackColor = True
        '
        'override_win_cancel
        '
        Me.override_win_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.override_win_cancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.override_win_cancel.Location = New System.Drawing.Point(281, 112)
        Me.override_win_cancel.Name = "override_win_cancel"
        Me.override_win_cancel.Size = New System.Drawing.Size(111, 26)
        Me.override_win_cancel.TabIndex = 10
        Me.override_win_cancel.Text = "Cancel"
        Me.override_win_cancel.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(12, 32)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(58, 16)
        Me.Label15.TabIndex = 9
        Me.Label15.Text = "Latitude:"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(12, 73)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(70, 16)
        Me.Label17.TabIndex = 10
        Me.Label17.Text = "Longitude:"
        '
        'lat_deg
        '
        Me.lat_deg.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lat_deg.Location = New System.Drawing.Point(91, 30)
        Me.lat_deg.Maximum = New Decimal(New Integer() {90, 0, 0, 0})
        Me.lat_deg.Name = "lat_deg"
        Me.lat_deg.Size = New System.Drawing.Size(60, 21)
        Me.lat_deg.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(157, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 16)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "deg"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(157, 73)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(32, 16)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "deg"
        '
        'lng_deg
        '
        Me.lng_deg.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lng_deg.Location = New System.Drawing.Point(91, 71)
        Me.lng_deg.Maximum = New Decimal(New Integer() {180, 0, 0, 0})
        Me.lng_deg.Name = "lng_deg"
        Me.lng_deg.Size = New System.Drawing.Size(60, 21)
        Me.lng_deg.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(278, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 16)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "min"
        '
        'lat_min
        '
        Me.lat_min.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lat_min.Location = New System.Drawing.Point(212, 30)
        Me.lat_min.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.lat_min.Name = "lat_min"
        Me.lat_min.Size = New System.Drawing.Size(60, 21)
        Me.lat_min.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(278, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(29, 16)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "min"
        '
        'lng_min
        '
        Me.lng_min.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lng_min.Location = New System.Drawing.Point(212, 71)
        Me.lng_min.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.lng_min.Name = "lng_min"
        Me.lng_min.Size = New System.Drawing.Size(60, 21)
        Me.lng_min.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(400, 32)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(30, 16)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "sec"
        '
        'lat_sec
        '
        Me.lat_sec.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lat_sec.Location = New System.Drawing.Point(334, 30)
        Me.lat_sec.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.lat_sec.Name = "lat_sec"
        Me.lat_sec.Size = New System.Drawing.Size(60, 21)
        Me.lat_sec.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(400, 73)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(30, 16)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "sec"
        '
        'lng_sec
        '
        Me.lng_sec.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lng_sec.Location = New System.Drawing.Point(334, 71)
        Me.lng_sec.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.lng_sec.Name = "lng_sec"
        Me.lng_sec.Size = New System.Drawing.Size(60, 21)
        Me.lng_sec.TabIndex = 7
        '
        'lat_dir
        '
        Me.lat_dir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lat_dir.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lat_dir.FormattingEnabled = True
        Me.lat_dir.Items.AddRange(New Object() {"N", "S"})
        Me.lat_dir.Location = New System.Drawing.Point(458, 30)
        Me.lat_dir.MaxDropDownItems = 2
        Me.lat_dir.Name = "lat_dir"
        Me.lat_dir.Size = New System.Drawing.Size(36, 21)
        Me.lat_dir.TabIndex = 4
        '
        'lng_dir
        '
        Me.lng_dir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lng_dir.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lng_dir.FormattingEnabled = True
        Me.lng_dir.Items.AddRange(New Object() {"E", "W"})
        Me.lng_dir.Location = New System.Drawing.Point(458, 71)
        Me.lng_dir.MaxDropDownItems = 2
        Me.lng_dir.Name = "lng_dir"
        Me.lng_dir.Size = New System.Drawing.Size(36, 21)
        Me.lng_dir.TabIndex = 8
        '
        'GPSOverride
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(524, 150)
        Me.ControlBox = False
        Me.Controls.Add(Me.lng_dir)
        Me.Controls.Add(Me.lat_dir)
        Me.Controls.Add(Me.lng_sec)
        Me.Controls.Add(Me.lng_min)
        Me.Controls.Add(Me.lng_deg)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lat_sec)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lat_min)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lat_deg)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.override_win_cancel)
        Me.Controls.Add(Me.override_win_confirm)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(540, 188)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(540, 188)
        Me.Name = "GPSOverride"
        Me.Text = "GPS Override"
        Me.TopMost = True
        CType(Me.lat_deg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lng_deg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lat_min, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lng_min, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lat_sec, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lng_sec, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents override_win_confirm As System.Windows.Forms.Button
    Friend WithEvents override_win_cancel As System.Windows.Forms.Button
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lat_deg As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lng_deg As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lat_min As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lng_min As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lat_sec As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lng_sec As System.Windows.Forms.NumericUpDown
    Friend WithEvents lat_dir As System.Windows.Forms.ComboBox
    Friend WithEvents lng_dir As System.Windows.Forms.ComboBox
End Class
