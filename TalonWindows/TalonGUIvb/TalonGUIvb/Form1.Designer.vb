<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CenterMapButton = New System.Windows.Forms.Button()
        Me.timeSinceLabel = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.BatteryBar = New System.Windows.Forms.ProgressBar()
        Me.buttonZoomIn = New System.Windows.Forms.Button()
        Me.buttonZoomOut = New System.Windows.Forms.Button()
        Me.ResetDetect = New System.Windows.Forms.Button()
        Me.ResetGPS = New System.Windows.Forms.Button()
        Me.IgnoreDataButton = New System.Windows.Forms.Button()
        Me.detectionsLabel = New System.Windows.Forms.Label()
        Me.batteryLabel = New System.Windows.Forms.Label()
        Me.statusLabel = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.longitudeLabel = New System.Windows.Forms.Label()
        Me.latitudeLabel = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.RockListAnimationTimer = New System.Windows.Forms.Timer(Me.components)
        Me.RTTYbox = New System.Windows.Forms.GroupBox()
        Me.MMTTYBox = New System.Windows.Forms.TextBox()
        Me.monitorBar = New System.Windows.Forms.ProgressBar()
        Me.squelchBar = New System.Windows.Forms.TrackBar()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RockStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.RTTYbox.SuspendLayout()
        CType(Me.squelchBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CenterMapButton)
        Me.GroupBox1.Controls.Add(Me.timeSinceLabel)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.BatteryBar)
        Me.GroupBox1.Controls.Add(Me.buttonZoomIn)
        Me.GroupBox1.Controls.Add(Me.buttonZoomOut)
        Me.GroupBox1.Controls.Add(Me.ResetDetect)
        Me.GroupBox1.Controls.Add(Me.ResetGPS)
        Me.GroupBox1.Controls.Add(Me.IgnoreDataButton)
        Me.GroupBox1.Controls.Add(Me.detectionsLabel)
        Me.GroupBox1.Controls.Add(Me.batteryLabel)
        Me.GroupBox1.Controls.Add(Me.statusLabel)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.ListBox1)
        Me.GroupBox1.Controls.Add(Me.longitudeLabel)
        Me.GroupBox1.Controls.Add(Me.latitudeLabel)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!)
        Me.GroupBox1.Location = New System.Drawing.Point(14, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(738, 140)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Rock Status"
        '
        'CenterMapButton
        '
        Me.CenterMapButton.Enabled = False
        Me.CenterMapButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CenterMapButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CenterMapButton.Location = New System.Drawing.Point(623, 18)
        Me.CenterMapButton.Name = "CenterMapButton"
        Me.CenterMapButton.Size = New System.Drawing.Size(90, 25)
        Me.CenterMapButton.TabIndex = 6
        Me.CenterMapButton.Text = "Center Map"
        Me.CenterMapButton.UseVisualStyleBackColor = True
        '
        'timeSinceLabel
        '
        Me.timeSinceLabel.AutoSize = True
        Me.timeSinceLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.timeSinceLabel.Location = New System.Drawing.Point(531, 111)
        Me.timeSinceLabel.MaximumSize = New System.Drawing.Size(70, 0)
        Me.timeSinceLabel.MinimumSize = New System.Drawing.Size(70, 0)
        Me.timeSinceLabel.Name = "timeSinceLabel"
        Me.timeSinceLabel.Size = New System.Drawing.Size(70, 16)
        Me.timeSinceLabel.TabIndex = 5
        Me.timeSinceLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(417, 111)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(79, 16)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Time Since:"
        '
        'BatteryBar
        '
        Me.BatteryBar.Location = New System.Drawing.Point(262, 105)
        Me.BatteryBar.Name = "BatteryBar"
        Me.BatteryBar.Size = New System.Drawing.Size(127, 23)
        Me.BatteryBar.TabIndex = 0
        '
        'buttonZoomIn
        '
        Me.buttonZoomIn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.buttonZoomIn.Location = New System.Drawing.Point(317, -5)
        Me.buttonZoomIn.Name = "buttonZoomIn"
        Me.buttonZoomIn.Size = New System.Drawing.Size(91, 15)
        Me.buttonZoomIn.TabIndex = 0
        Me.buttonZoomIn.Text = "Zoom In"
        Me.buttonZoomIn.UseVisualStyleBackColor = True
        '
        'buttonZoomOut
        '
        Me.buttonZoomOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.buttonZoomOut.Location = New System.Drawing.Point(209, -5)
        Me.buttonZoomOut.Name = "buttonZoomOut"
        Me.buttonZoomOut.Size = New System.Drawing.Size(61, 15)
        Me.buttonZoomOut.TabIndex = 0
        Me.buttonZoomOut.Text = "Zoom Out"
        Me.buttonZoomOut.UseVisualStyleBackColor = True
        '
        'ResetDetect
        '
        Me.ResetDetect.Enabled = False
        Me.ResetDetect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ResetDetect.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ResetDetect.Location = New System.Drawing.Point(623, 109)
        Me.ResetDetect.Name = "ResetDetect"
        Me.ResetDetect.Size = New System.Drawing.Size(90, 25)
        Me.ResetDetect.TabIndex = 2
        Me.ResetDetect.Text = "Reset Detect"
        Me.ResetDetect.UseVisualStyleBackColor = True
        '
        'ResetGPS
        '
        Me.ResetGPS.Enabled = False
        Me.ResetGPS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ResetGPS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ResetGPS.Location = New System.Drawing.Point(623, 78)
        Me.ResetGPS.Name = "ResetGPS"
        Me.ResetGPS.Size = New System.Drawing.Size(90, 25)
        Me.ResetGPS.TabIndex = 2
        Me.ResetGPS.Text = "Reset GPS"
        Me.ResetGPS.UseVisualStyleBackColor = True
        '
        'IgnoreDataButton
        '
        Me.IgnoreDataButton.Enabled = False
        Me.IgnoreDataButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.IgnoreDataButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IgnoreDataButton.Location = New System.Drawing.Point(623, 48)
        Me.IgnoreDataButton.Name = "IgnoreDataButton"
        Me.IgnoreDataButton.Size = New System.Drawing.Size(90, 25)
        Me.IgnoreDataButton.TabIndex = 1
        Me.IgnoreDataButton.Text = "Ignore Data"
        Me.IgnoreDataButton.UseVisualStyleBackColor = True
        '
        'detectionsLabel
        '
        Me.detectionsLabel.AutoSize = True
        Me.detectionsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.detectionsLabel.Location = New System.Drawing.Point(359, 53)
        Me.detectionsLabel.MinimumSize = New System.Drawing.Size(30, 0)
        Me.detectionsLabel.Name = "detectionsLabel"
        Me.detectionsLabel.Size = New System.Drawing.Size(30, 16)
        Me.detectionsLabel.TabIndex = 2
        Me.detectionsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'batteryLabel
        '
        Me.batteryLabel.AutoSize = True
        Me.batteryLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.batteryLabel.Location = New System.Drawing.Point(349, 82)
        Me.batteryLabel.MinimumSize = New System.Drawing.Size(40, 0)
        Me.batteryLabel.Name = "batteryLabel"
        Me.batteryLabel.Size = New System.Drawing.Size(40, 16)
        Me.batteryLabel.TabIndex = 2
        Me.batteryLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'statusLabel
        '
        Me.statusLabel.AutoSize = True
        Me.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.statusLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.statusLabel.Location = New System.Drawing.Point(259, 24)
        Me.statusLabel.MaximumSize = New System.Drawing.Size(330, 0)
        Me.statusLabel.MinimumSize = New System.Drawing.Size(330, 0)
        Me.statusLabel.Name = "statusLabel"
        Me.statusLabel.Size = New System.Drawing.Size(330, 22)
        Me.statusLabel.TabIndex = 2
        Me.statusLabel.Text = "No Rock Selected"
        Me.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(259, 53)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(75, 16)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Detections:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(417, 82)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 16)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Longitude:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(259, 82)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Battery:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(416, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 16)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Latitude:"
        '
        'ListBox1
        '
        Me.ListBox1.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 18
        Me.ListBox1.Location = New System.Drawing.Point(11, 24)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(233, 94)
        Me.ListBox1.TabIndex = 0
        '
        'longitudeLabel
        '
        Me.longitudeLabel.AutoSize = True
        Me.longitudeLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.longitudeLabel.Location = New System.Drawing.Point(476, 82)
        Me.longitudeLabel.MaximumSize = New System.Drawing.Size(125, 0)
        Me.longitudeLabel.MinimumSize = New System.Drawing.Size(125, 0)
        Me.longitudeLabel.Name = "longitudeLabel"
        Me.longitudeLabel.Size = New System.Drawing.Size(125, 16)
        Me.longitudeLabel.TabIndex = 2
        Me.longitudeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'latitudeLabel
        '
        Me.latitudeLabel.AutoSize = True
        Me.latitudeLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.latitudeLabel.Location = New System.Drawing.Point(476, 53)
        Me.latitudeLabel.MaximumSize = New System.Drawing.Size(125, 0)
        Me.latitudeLabel.MinimumSize = New System.Drawing.Size(125, 0)
        Me.latitudeLabel.Name = "latitudeLabel"
        Me.latitudeLabel.Size = New System.Drawing.Size(125, 16)
        Me.latitudeLabel.TabIndex = 2
        Me.latitudeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.WebBrowser1)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(14, 158)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(738, 340)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Rock Mapping"
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Location = New System.Drawing.Point(5, 29)
        Me.WebBrowser1.Margin = New System.Windows.Forms.Padding(0)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.ScrollBarsEnabled = False
        Me.WebBrowser1.Size = New System.Drawing.Size(727, 291)
        Me.WebBrowser1.TabIndex = 1
        Me.WebBrowser1.TabStop = False
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(487, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(121, 18)
        Me.Button1.TabIndex = 0
        Me.Button1.TabStop = False
        Me.Button1.Text = "Populate Rock List"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'RockListAnimationTimer
        '
        Me.RockListAnimationTimer.Enabled = True
        '
        'RTTYbox
        '
        Me.RTTYbox.Controls.Add(Me.MMTTYBox)
        Me.RTTYbox.Controls.Add(Me.monitorBar)
        Me.RTTYbox.Controls.Add(Me.squelchBar)
        Me.RTTYbox.Controls.Add(Me.Label4)
        Me.RTTYbox.Controls.Add(Me.Label1)
        Me.RTTYbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!)
        Me.RTTYbox.Location = New System.Drawing.Point(14, 504)
        Me.RTTYbox.Name = "RTTYbox"
        Me.RTTYbox.Size = New System.Drawing.Size(738, 126)
        Me.RTTYbox.TabIndex = 0
        Me.RTTYbox.TabStop = False
        Me.RTTYbox.Text = "RTTY Status - Offline"
        '
        'MMTTYBox
        '
        Me.MMTTYBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.MMTTYBox.Enabled = False
        Me.MMTTYBox.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MMTTYBox.Location = New System.Drawing.Point(17, 77)
        Me.MMTTYBox.Name = "MMTTYBox"
        Me.MMTTYBox.Size = New System.Drawing.Size(706, 26)
        Me.MMTTYBox.TabIndex = 0
        Me.MMTTYBox.TabStop = False
        '
        'monitorBar
        '
        Me.monitorBar.Location = New System.Drawing.Point(456, 35)
        Me.monitorBar.MarqueeAnimationSpeed = 1
        Me.monitorBar.Maximum = 1250
        Me.monitorBar.Name = "monitorBar"
        Me.monitorBar.Size = New System.Drawing.Size(247, 23)
        Me.monitorBar.Step = 25
        Me.monitorBar.TabIndex = 0
        '
        'squelchBar
        '
        Me.squelchBar.Enabled = False
        Me.squelchBar.LargeChange = 100
        Me.squelchBar.Location = New System.Drawing.Point(103, 35)
        Me.squelchBar.Maximum = 1250
        Me.squelchBar.Name = "squelchBar"
        Me.squelchBar.Size = New System.Drawing.Size(242, 45)
        Me.squelchBar.SmallChange = 50
        Me.squelchBar.TabIndex = 3
        Me.squelchBar.TickFrequency = 50
        Me.squelchBar.Value = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label4.Location = New System.Drawing.Point(382, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 20)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Signal"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label1.Location = New System.Drawing.Point(30, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Squelch"
        '
        'RockStatusTimer
        '
        Me.RockStatusTimer.Enabled = True
        Me.RockStatusTimer.Interval = 1000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(764, 642)
        Me.Controls.Add(Me.RTTYbox)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(780, 680)
        Me.MinimumSize = New System.Drawing.Size(640, 640)
        Me.Name = "Form1"
        Me.Text = "Talon Rock Monitor"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.RTTYbox.ResumeLayout(False)
        Me.RTTYbox.PerformLayout()
        CType(Me.squelchBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents statusLabel As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents longitudeLabel As System.Windows.Forms.Label
    Friend WithEvents latitudeLabel As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents detectionsLabel As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ResetGPS As System.Windows.Forms.Button
    Friend WithEvents BatteryBar As System.Windows.Forms.ProgressBar
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents batteryLabel As System.Windows.Forms.Label
    Friend WithEvents IgnoreDataButton As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents RockListAnimationTimer As System.Windows.Forms.Timer
    Friend WithEvents RTTYbox As System.Windows.Forms.GroupBox
    Friend WithEvents squelchBar As System.Windows.Forms.TrackBar
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents monitorBar As System.Windows.Forms.ProgressBar
    Friend WithEvents MMTTYBox As System.Windows.Forms.TextBox
    Friend WithEvents buttonZoomOut As System.Windows.Forms.Button
    Friend WithEvents buttonZoomIn As System.Windows.Forms.Button
    Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
    Friend WithEvents timeSinceLabel As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents RockStatusTimer As System.Windows.Forms.Timer
    Friend WithEvents CenterMapButton As System.Windows.Forms.Button
    Friend WithEvents ResetDetect As System.Windows.Forms.Button

End Class
