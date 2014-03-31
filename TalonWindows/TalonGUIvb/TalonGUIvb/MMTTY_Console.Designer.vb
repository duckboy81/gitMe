<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MMTTY_Console
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MMTTY_Console))
        Me.RTTYbox = New System.Windows.Forms.GroupBox()
        Me.mmtty_show_button = New System.Windows.Forms.Button()
        Me.MMTTYBox = New System.Windows.Forms.TextBox()
        Me.monitorBar = New System.Windows.Forms.ProgressBar()
        Me.squelchBar = New System.Windows.Forms.TrackBar()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RTTYbox.SuspendLayout()
        CType(Me.squelchBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RTTYbox
        '
        Me.RTTYbox.Controls.Add(Me.mmtty_show_button)
        Me.RTTYbox.Controls.Add(Me.MMTTYBox)
        Me.RTTYbox.Controls.Add(Me.monitorBar)
        Me.RTTYbox.Controls.Add(Me.squelchBar)
        Me.RTTYbox.Controls.Add(Me.Label4)
        Me.RTTYbox.Controls.Add(Me.Label1)
        Me.RTTYbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!)
        Me.RTTYbox.Location = New System.Drawing.Point(5, 6)
        Me.RTTYbox.Name = "RTTYbox"
        Me.RTTYbox.Size = New System.Drawing.Size(738, 147)
        Me.RTTYbox.TabIndex = 1
        Me.RTTYbox.TabStop = False
        Me.RTTYbox.Text = "RTTY Status - Offline"
        '
        'mmtty_show_button
        '
        Me.mmtty_show_button.Enabled = False
        Me.mmtty_show_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mmtty_show_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mmtty_show_button.Location = New System.Drawing.Point(17, 115)
        Me.mmtty_show_button.Name = "mmtty_show_button"
        Me.mmtty_show_button.Size = New System.Drawing.Size(111, 26)
        Me.mmtty_show_button.TabIndex = 20
        Me.mmtty_show_button.Text = "Show MMTTY"
        Me.mmtty_show_button.UseVisualStyleBackColor = True
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
        'MMTTY_Console
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(749, 159)
        Me.Controls.Add(Me.RTTYbox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MMTTY_Console"
        Me.Text = "MMTTY Console"
        Me.RTTYbox.ResumeLayout(False)
        Me.RTTYbox.PerformLayout()
        CType(Me.squelchBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RTTYbox As System.Windows.Forms.GroupBox
    Friend WithEvents MMTTYBox As System.Windows.Forms.TextBox
    Friend WithEvents monitorBar As System.Windows.Forms.ProgressBar
    Friend WithEvents squelchBar As System.Windows.Forms.TrackBar
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents mmtty_show_button As System.Windows.Forms.Button
End Class
