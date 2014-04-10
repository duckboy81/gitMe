<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainScreen
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainScreen))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.mark_unread_button = New System.Windows.Forms.Button()
        Me.nodeSelection = New System.Windows.Forms.ComboBox()
        Me.ignore_msg_button = New System.Windows.Forms.Button()
        Me.event_msg_type = New System.Windows.Forms.Label()
        Me.typeSelection = New System.Windows.Forms.ComboBox()
        Me.event_speed_label = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.event_node_name_label = New System.Windows.Forms.Label()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.delete_node_button = New System.Windows.Forms.Button()
        Me.reset_detections_button = New System.Windows.Forms.Button()
        Me.ignore_node_button = New System.Windows.Forms.Button()
        Me.gps_override_button = New System.Windows.Forms.Button()
        Me.number_detections_label = New System.Windows.Forms.Label()
        Me.center_map_button = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.gps_long_label = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.last_comm_label = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.gps_lat_label = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.show_MTTY_console_button = New System.Windows.Forms.Button()
        Me.repopulate_map_button = New System.Windows.Forms.Button()
        Me.refresh_page_button = New System.Windows.Forms.Button()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.RockStatusTimer = New System.Windows.Forms.Timer(Me.components)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(12, 30)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.mark_unread_button)
        Me.SplitContainer1.Panel1.Controls.Add(Me.nodeSelection)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ignore_msg_button)
        Me.SplitContainer1.Panel1.Controls.Add(Me.event_msg_type)
        Me.SplitContainer1.Panel1.Controls.Add(Me.typeSelection)
        Me.SplitContainer1.Panel1.Controls.Add(Me.event_speed_label)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.event_node_name_label)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ListBox1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label12)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label3)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.delete_node_button)
        Me.SplitContainer1.Panel2.Controls.Add(Me.reset_detections_button)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ignore_node_button)
        Me.SplitContainer1.Panel2.Controls.Add(Me.gps_override_button)
        Me.SplitContainer1.Panel2.Controls.Add(Me.number_detections_label)
        Me.SplitContainer1.Panel2.Controls.Add(Me.center_map_button)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label16)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label23)
        Me.SplitContainer1.Panel2.Controls.Add(Me.gps_long_label)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ComboBox1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.last_comm_label)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label14)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label15)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label21)
        Me.SplitContainer1.Panel2.Controls.Add(Me.gps_lat_label)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label20)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label17)
        Me.SplitContainer1.Size = New System.Drawing.Size(810, 222)
        Me.SplitContainer1.SplitterDistance = 375
        Me.SplitContainer1.TabIndex = 5
        '
        'mark_unread_button
        '
        Me.mark_unread_button.Enabled = False
        Me.mark_unread_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mark_unread_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mark_unread_button.Location = New System.Drawing.Point(241, 152)
        Me.mark_unread_button.Name = "mark_unread_button"
        Me.mark_unread_button.Size = New System.Drawing.Size(111, 26)
        Me.mark_unread_button.TabIndex = 4
        Me.mark_unread_button.Text = "Mark Unread"
        Me.mark_unread_button.UseVisualStyleBackColor = True
        '
        'nodeSelection
        '
        Me.nodeSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.nodeSelection.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.nodeSelection.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nodeSelection.FormattingEnabled = True
        Me.nodeSelection.Items.AddRange(New Object() {"Show All"})
        Me.nodeSelection.Location = New System.Drawing.Point(32, 26)
        Me.nodeSelection.Name = "nodeSelection"
        Me.nodeSelection.Size = New System.Drawing.Size(169, 24)
        Me.nodeSelection.TabIndex = 1
        '
        'ignore_msg_button
        '
        Me.ignore_msg_button.Enabled = False
        Me.ignore_msg_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ignore_msg_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ignore_msg_button.Location = New System.Drawing.Point(241, 184)
        Me.ignore_msg_button.Name = "ignore_msg_button"
        Me.ignore_msg_button.Size = New System.Drawing.Size(111, 26)
        Me.ignore_msg_button.TabIndex = 5
        Me.ignore_msg_button.Text = "Ignore Event"
        Me.ignore_msg_button.UseVisualStyleBackColor = True
        '
        'event_msg_type
        '
        Me.event_msg_type.AutoSize = True
        Me.event_msg_type.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.event_msg_type.Location = New System.Drawing.Point(259, 82)
        Me.event_msg_type.Name = "event_msg_type"
        Me.event_msg_type.Size = New System.Drawing.Size(0, 16)
        Me.event_msg_type.TabIndex = 3
        '
        'typeSelection
        '
        Me.typeSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.typeSelection.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.typeSelection.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.typeSelection.FormattingEnabled = True
        Me.typeSelection.Items.AddRange(New Object() {"Show All", "Awake Beacon", "Detection Alarm", "GPS Update"})
        Me.typeSelection.Location = New System.Drawing.Point(32, 76)
        Me.typeSelection.Name = "typeSelection"
        Me.typeSelection.Size = New System.Drawing.Size(169, 24)
        Me.typeSelection.TabIndex = 2
        '
        'event_speed_label
        '
        Me.event_speed_label.AutoSize = True
        Me.event_speed_label.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.event_speed_label.Location = New System.Drawing.Point(259, 128)
        Me.event_speed_label.Name = "event_speed_label"
        Me.event_speed_label.Size = New System.Drawing.Size(0, 16)
        Me.event_speed_label.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(63, 8)
        Me.Label1.MinimumSize = New System.Drawing.Size(115, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Filter By Node"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'event_node_name_label
        '
        Me.event_node_name_label.AutoSize = True
        Me.event_node_name_label.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.event_node_name_label.Location = New System.Drawing.Point(259, 35)
        Me.event_node_name_label.Name = "event_node_name_label"
        Me.event_node_name_label.Size = New System.Drawing.Size(0, 16)
        Me.event_node_name_label.TabIndex = 2
        '
        'ListBox1
        '
        Me.ListBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 16
        Me.ListBox1.Location = New System.Drawing.Point(8, 111)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.ScrollAlwaysVisible = True
        Me.ListBox1.Size = New System.Drawing.Size(217, 100)
        Me.ListBox1.TabIndex = 3
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(236, 107)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(103, 16)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Travel Speed"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(60, 58)
        Me.Label2.MinimumSize = New System.Drawing.Size(115, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(115, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Filter By Type"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(236, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(87, 16)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Rock Label"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(236, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(112, 16)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Message Type"
        '
        'delete_node_button
        '
        Me.delete_node_button.Enabled = False
        Me.delete_node_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.delete_node_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.delete_node_button.Location = New System.Drawing.Point(268, 179)
        Me.delete_node_button.Name = "delete_node_button"
        Me.delete_node_button.Size = New System.Drawing.Size(144, 30)
        Me.delete_node_button.TabIndex = 11
        Me.delete_node_button.Text = "Delete Node"
        Me.delete_node_button.UseVisualStyleBackColor = True
        '
        'reset_detections_button
        '
        Me.reset_detections_button.Enabled = False
        Me.reset_detections_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.reset_detections_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.reset_detections_button.Location = New System.Drawing.Point(268, 137)
        Me.reset_detections_button.Name = "reset_detections_button"
        Me.reset_detections_button.Size = New System.Drawing.Size(144, 30)
        Me.reset_detections_button.TabIndex = 10
        Me.reset_detections_button.Text = "Reset Detection Count"
        Me.reset_detections_button.UseVisualStyleBackColor = True
        '
        'ignore_node_button
        '
        Me.ignore_node_button.Enabled = False
        Me.ignore_node_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ignore_node_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ignore_node_button.Location = New System.Drawing.Point(268, 95)
        Me.ignore_node_button.Name = "ignore_node_button"
        Me.ignore_node_button.Size = New System.Drawing.Size(144, 30)
        Me.ignore_node_button.TabIndex = 9
        Me.ignore_node_button.Text = "Ignore Node"
        Me.ignore_node_button.UseVisualStyleBackColor = True
        '
        'gps_override_button
        '
        Me.gps_override_button.Enabled = False
        Me.gps_override_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.gps_override_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gps_override_button.Location = New System.Drawing.Point(268, 53)
        Me.gps_override_button.Name = "gps_override_button"
        Me.gps_override_button.Size = New System.Drawing.Size(144, 30)
        Me.gps_override_button.TabIndex = 8
        Me.gps_override_button.Text = "Manually Input GPS"
        Me.gps_override_button.UseVisualStyleBackColor = True
        '
        'number_detections_label
        '
        Me.number_detections_label.AutoSize = True
        Me.number_detections_label.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.number_detections_label.Location = New System.Drawing.Point(157, 187)
        Me.number_detections_label.Name = "number_detections_label"
        Me.number_detections_label.Size = New System.Drawing.Size(0, 16)
        Me.number_detections_label.TabIndex = 8
        '
        'center_map_button
        '
        Me.center_map_button.Enabled = False
        Me.center_map_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.center_map_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.center_map_button.Location = New System.Drawing.Point(268, 11)
        Me.center_map_button.Name = "center_map_button"
        Me.center_map_button.Size = New System.Drawing.Size(144, 30)
        Me.center_map_button.TabIndex = 7
        Me.center_map_button.Text = "Center Map"
        Me.center_map_button.UseVisualStyleBackColor = True
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(3, 75)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(76, 16)
        Me.Label16.TabIndex = 7
        Me.Label16.Text = "GPS Data"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(23, 187)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(140, 16)
        Me.Label23.TabIndex = 8
        Me.Label23.Text = "Number of Detections:"
        '
        'gps_long_label
        '
        Me.gps_long_label.AutoSize = True
        Me.gps_long_label.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gps_long_label.Location = New System.Drawing.Point(103, 117)
        Me.gps_long_label.Name = "gps_long_label"
        Me.gps_long_label.Size = New System.Drawing.Size(0, 16)
        Me.gps_long_label.TabIndex = 8
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Enabled = False
        Me.ComboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(6, 33)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(228, 24)
        Me.ComboBox1.Sorted = True
        Me.ComboBox1.TabIndex = 6
        '
        'last_comm_label
        '
        Me.last_comm_label.AutoSize = True
        Me.last_comm_label.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.last_comm_label.Location = New System.Drawing.Point(157, 166)
        Me.last_comm_label.Name = "last_comm_label"
        Me.last_comm_label.Size = New System.Drawing.Size(0, 16)
        Me.last_comm_label.TabIndex = 8
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(3, 14)
        Me.Label14.MinimumSize = New System.Drawing.Size(115, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(115, 16)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = "Selected Node"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(26, 96)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(58, 16)
        Me.Label15.TabIndex = 8
        Me.Label15.Text = "Latitude:"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(23, 166)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(131, 16)
        Me.Label21.TabIndex = 8
        Me.Label21.Text = "Last Communication:"
        '
        'gps_lat_label
        '
        Me.gps_lat_label.AutoSize = True
        Me.gps_lat_label.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gps_lat_label.Location = New System.Drawing.Point(103, 96)
        Me.gps_lat_label.Name = "gps_lat_label"
        Me.gps_lat_label.Size = New System.Drawing.Size(0, 16)
        Me.gps_lat_label.TabIndex = 8
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(3, 145)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(51, 16)
        Me.Label20.TabIndex = 7
        Me.Label20.Text = "Status"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(26, 117)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(70, 16)
        Me.Label17.TabIndex = 8
        Me.Label17.Text = "Longitude:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(17, 14)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 24)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Event History"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(396, 14)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(196, 24)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Individual Node Status"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.NumericUpDown1)
        Me.GroupBox2.Controls.Add(Me.show_MTTY_console_button)
        Me.GroupBox2.Controls.Add(Me.repopulate_map_button)
        Me.GroupBox2.Controls.Add(Me.refresh_page_button)
        Me.GroupBox2.Controls.Add(Me.WebBrowser1)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(12, 258)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(810, 372)
        Me.GroupBox2.TabIndex = 12
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Rock Mapping"
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Location = New System.Drawing.Point(453, 337)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(57, 29)
        Me.NumericUpDown1.TabIndex = 15
        Me.NumericUpDown1.Value = New Decimal(New Integer() {999, 0, 0, 0})
        '
        'show_MTTY_console_button
        '
        Me.show_MTTY_console_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.show_MTTY_console_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.show_MTTY_console_button.Location = New System.Drawing.Point(660, 336)
        Me.show_MTTY_console_button.Name = "show_MTTY_console_button"
        Me.show_MTTY_console_button.Size = New System.Drawing.Size(144, 30)
        Me.show_MTTY_console_button.TabIndex = 16
        Me.show_MTTY_console_button.Text = "Show MTTY Console"
        Me.show_MTTY_console_button.UseVisualStyleBackColor = True
        '
        'repopulate_map_button
        '
        Me.repopulate_map_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.repopulate_map_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.repopulate_map_button.Location = New System.Drawing.Point(155, 336)
        Me.repopulate_map_button.Name = "repopulate_map_button"
        Me.repopulate_map_button.Size = New System.Drawing.Size(144, 30)
        Me.repopulate_map_button.TabIndex = 14
        Me.repopulate_map_button.Text = "Repopulate Map"
        Me.repopulate_map_button.UseVisualStyleBackColor = True
        '
        'refresh_page_button
        '
        Me.refresh_page_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.refresh_page_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.refresh_page_button.Location = New System.Drawing.Point(5, 336)
        Me.refresh_page_button.Name = "refresh_page_button"
        Me.refresh_page_button.Size = New System.Drawing.Size(144, 30)
        Me.refresh_page_button.TabIndex = 13
        Me.refresh_page_button.Text = "Refresh Page"
        Me.refresh_page_button.UseVisualStyleBackColor = True
        '
        'WebBrowser1
        '
        Me.WebBrowser1.IsWebBrowserContextMenuEnabled = False
        Me.WebBrowser1.Location = New System.Drawing.Point(5, 29)
        Me.WebBrowser1.Margin = New System.Windows.Forms.Padding(0)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.ScrollBarsEnabled = False
        Me.WebBrowser1.Size = New System.Drawing.Size(800, 291)
        Me.WebBrowser1.TabIndex = 1
        Me.WebBrowser1.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(514, 344)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(70, 16)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "detections"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(373, 344)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(78, 16)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Limit to past"
        '
        'RockStatusTimer
        '
        Me.RockStatusTimer.Enabled = True
        Me.RockStatusTimer.Interval = 1000
        '
        'MainScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(834, 642)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(850, 680)
        Me.MinimumSize = New System.Drawing.Size(850, 680)
        Me.Name = "MainScreen"
        Me.Text = "Talon Rock Monitor"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents nodeSelection As System.Windows.Forms.ComboBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents typeSelection As System.Windows.Forms.ComboBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents event_msg_type As System.Windows.Forms.Label
    Friend WithEvents event_node_name_label As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ignore_msg_button As System.Windows.Forms.Button
    Friend WithEvents event_speed_label As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents number_detections_label As System.Windows.Forms.Label
    Friend WithEvents gps_long_label As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents last_comm_label As System.Windows.Forms.Label
    Friend WithEvents gps_lat_label As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents gps_override_button As System.Windows.Forms.Button
    Friend WithEvents center_map_button As System.Windows.Forms.Button
    Friend WithEvents ignore_node_button As System.Windows.Forms.Button
    Friend WithEvents reset_detections_button As System.Windows.Forms.Button
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents mark_unread_button As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents delete_node_button As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents show_MTTY_console_button As System.Windows.Forms.Button
    Friend WithEvents repopulate_map_button As System.Windows.Forms.Button
    Friend WithEvents refresh_page_button As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents RockStatusTimer As System.Windows.Forms.Timer
End Class
