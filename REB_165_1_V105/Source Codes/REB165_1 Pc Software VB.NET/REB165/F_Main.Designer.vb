<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class F_Main
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series4 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(F_Main))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.DateiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BeendenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HilfeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.T_X = New System.Windows.Forms.ToolStripStatusLabel()
        Me.T_Y = New System.Windows.Forms.ToolStripStatusLabel()
        Me.T_Measurespeed = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.L_Speed = New System.Windows.Forms.Label()
        Me.L_Alert = New System.Windows.Forms.Label()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.G_Settings1 = New System.Windows.Forms.GroupBox()
        Me.L_Baudrate = New System.Windows.Forms.Label()
        Me.C_Baudrate = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.L_Sampledelay = New System.Windows.Forms.Label()
        Me.C_Sampledelay = New System.Windows.Forms.ComboBox()
        Me.L_Samplerate = New System.Windows.Forms.Label()
        Me.C_Samplerate = New System.Windows.Forms.ComboBox()
        Me.L_Samples = New System.Windows.Forms.Label()
        Me.C_Samples = New System.Windows.Forms.ComboBox()
        Me.G_Motion = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.T_Motion_Delay = New System.Windows.Forms.TextBox()
        Me.L_Motion_Delay = New System.Windows.Forms.Label()
        Me.L_LED = New System.Windows.Forms.Label()
        Me.L_IOB7 = New System.Windows.Forms.Label()
        Me.L_IOB6 = New System.Windows.Forms.Label()
        Me.C_LED = New System.Windows.Forms.CheckBox()
        Me.C_IOB7 = New System.Windows.Forms.CheckBox()
        Me.C_IOB6 = New System.Windows.Forms.CheckBox()
        Me.L_Threshold1 = New System.Windows.Forms.Label()
        Me.C_Treshold = New System.Windows.Forms.ComboBox()
        Me.C_Write = New System.Windows.Forms.Button()
        Me.G_Mode = New System.Windows.Forms.GroupBox()
        Me.L_Filterbez = New System.Windows.Forms.Label()
        Me.T_Filter_Max = New System.Windows.Forms.TextBox()
        Me.T_Filter_Min = New System.Windows.Forms.TextBox()
        Me.C_Speed_Filter = New System.Windows.Forms.CheckBox()
        Me.T_Kmhfaktor = New System.Windows.Forms.TextBox()
        Me.C_kmh = New System.Windows.Forms.CheckBox()
        Me.C_MC_FFT = New System.Windows.Forms.CheckBox()
        Me.C_Mode = New System.Windows.Forms.ComboBox()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.C_ShowLog = New System.Windows.Forms.Button()
        Me.C_EnableLog = New System.Windows.Forms.CheckBox()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.G_Settings1.SuspendLayout()
        Me.G_Motion.SuspendLayout()
        Me.G_Mode.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DateiToolStripMenuItem, Me.HilfeToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1077, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'DateiToolStripMenuItem
        '
        Me.DateiToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BeendenToolStripMenuItem})
        Me.DateiToolStripMenuItem.Name = "DateiToolStripMenuItem"
        Me.DateiToolStripMenuItem.Size = New System.Drawing.Size(46, 20)
        Me.DateiToolStripMenuItem.Text = "Datei"
        '
        'BeendenToolStripMenuItem
        '
        Me.BeendenToolStripMenuItem.Name = "BeendenToolStripMenuItem"
        Me.BeendenToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.BeendenToolStripMenuItem.Text = "Beenden"
        '
        'HilfeToolStripMenuItem
        '
        Me.HilfeToolStripMenuItem.Name = "HilfeToolStripMenuItem"
        Me.HilfeToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HilfeToolStripMenuItem.Text = "Hilfe"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.T_X, Me.T_Y, Me.T_Measurespeed})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 746)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1077, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'T_X
        '
        Me.T_X.Name = "T_X"
        Me.T_X.Size = New System.Drawing.Size(17, 17)
        Me.T_X.Text = "X:"
        '
        'T_Y
        '
        Me.T_Y.Name = "T_Y"
        Me.T_Y.Size = New System.Drawing.Size(17, 17)
        Me.T_Y.Text = "Y:"
        '
        'T_Measurespeed
        '
        Me.T_Measurespeed.Name = "T_Measurespeed"
        Me.T_Measurespeed.Size = New System.Drawing.Size(90, 17)
        Me.T_Measurespeed.Text = "Measure Speed:"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(5, 29)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(866, 714)
        Me.TabControl1.TabIndex = 2
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.L_Speed)
        Me.TabPage1.Controls.Add(Me.L_Alert)
        Me.TabPage1.Controls.Add(Me.Chart1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(858, 688)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Chart"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'L_Speed
        '
        Me.L_Speed.AutoSize = True
        Me.L_Speed.BackColor = System.Drawing.Color.Black
        Me.L_Speed.Font = New System.Drawing.Font("Arial", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L_Speed.ForeColor = System.Drawing.Color.SkyBlue
        Me.L_Speed.Location = New System.Drawing.Point(415, 14)
        Me.L_Speed.Name = "L_Speed"
        Me.L_Speed.Size = New System.Drawing.Size(122, 37)
        Me.L_Speed.TabIndex = 12
        Me.L_Speed.Text = "Speed:"
        '
        'L_Alert
        '
        Me.L_Alert.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.L_Alert.AutoSize = True
        Me.L_Alert.BackColor = System.Drawing.Color.Black
        Me.L_Alert.Font = New System.Drawing.Font("Arial", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L_Alert.ForeColor = System.Drawing.Color.SkyBlue
        Me.L_Alert.Location = New System.Drawing.Point(704, 14)
        Me.L_Alert.Name = "L_Alert"
        Me.L_Alert.Size = New System.Drawing.Size(142, 37)
        Me.L_Alert.TabIndex = 11
        Me.L_Alert.Text = "ALARM!"
        '
        'Chart1
        '
        Me.Chart1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Chart1.BackColor = System.Drawing.Color.Black
        ChartArea1.AxisX.IsLabelAutoFit = False
        ChartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.DimGray
        ChartArea1.AxisX.LineColor = System.Drawing.Color.DimGray
        ChartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.DimGray
        ChartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.DimGray
        ChartArea1.AxisX.Title = "Samples"
        ChartArea1.AxisX.TitleForeColor = System.Drawing.Color.White
        ChartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DimGray
        ChartArea1.AxisY.LineColor = System.Drawing.Color.DimGray
        ChartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.DimGray
        ChartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.DimGray
        ChartArea1.AxisY.Title = "ADC Value"
        ChartArea1.AxisY.TitleForeColor = System.Drawing.Color.White
        ChartArea1.BackColor = System.Drawing.Color.Black
        ChartArea1.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea1)
        Legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top
        Legend1.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend1)
        Me.Chart1.Location = New System.Drawing.Point(6, 6)
        Me.Chart1.Name = "Chart1"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series1.Color = System.Drawing.Color.GreenYellow
        Series1.Legend = "Legend1"
        Series1.Name = "Time Signal"
        Series2.ChartArea = "ChartArea1"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series2.Color = System.Drawing.Color.Red
        Series2.Legend = "Legend1"
        Series2.Name = "Threshold"
        Series3.ChartArea = "ChartArea1"
        Series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series3.Color = System.Drawing.Color.DodgerBlue
        Series3.Legend = "Legend1"
        Series3.Name = "Filter Min"
        Series4.ChartArea = "ChartArea1"
        Series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series4.Color = System.Drawing.Color.DodgerBlue
        Series4.Legend = "Legend1"
        Series4.Name = "Filter Max"
        Me.Chart1.Series.Add(Series1)
        Me.Chart1.Series.Add(Series2)
        Me.Chart1.Series.Add(Series3)
        Me.Chart1.Series.Add(Series4)
        Me.Chart1.Size = New System.Drawing.Size(846, 676)
        Me.Chart1.TabIndex = 0
        Me.Chart1.Text = "Chart1"
        '
        'G_Settings1
        '
        Me.G_Settings1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.G_Settings1.Controls.Add(Me.L_Baudrate)
        Me.G_Settings1.Controls.Add(Me.C_Baudrate)
        Me.G_Settings1.Controls.Add(Me.Label1)
        Me.G_Settings1.Controls.Add(Me.L_Sampledelay)
        Me.G_Settings1.Controls.Add(Me.C_Sampledelay)
        Me.G_Settings1.Controls.Add(Me.L_Samplerate)
        Me.G_Settings1.Controls.Add(Me.C_Samplerate)
        Me.G_Settings1.Controls.Add(Me.L_Samples)
        Me.G_Settings1.Controls.Add(Me.C_Samples)
        Me.G_Settings1.Location = New System.Drawing.Point(877, 209)
        Me.G_Settings1.Name = "G_Settings1"
        Me.G_Settings1.Size = New System.Drawing.Size(195, 123)
        Me.G_Settings1.TabIndex = 4
        Me.G_Settings1.TabStop = False
        Me.G_Settings1.Text = "Board Settings"
        '
        'L_Baudrate
        '
        Me.L_Baudrate.AutoSize = True
        Me.L_Baudrate.Location = New System.Drawing.Point(3, 17)
        Me.L_Baudrate.Name = "L_Baudrate"
        Me.L_Baudrate.Size = New System.Drawing.Size(50, 13)
        Me.L_Baudrate.TabIndex = 13
        Me.L_Baudrate.Text = "Baudrate"
        '
        'C_Baudrate
        '
        Me.C_Baudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.C_Baudrate.FormattingEnabled = True
        Me.C_Baudrate.Items.AddRange(New Object() {"9600", "19200", "38400", "76800", "250000", "500000", "1000000"})
        Me.C_Baudrate.Location = New System.Drawing.Point(96, 14)
        Me.C_Baudrate.Name = "C_Baudrate"
        Me.C_Baudrate.Size = New System.Drawing.Size(77, 21)
        Me.C_Baudrate.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(173, 98)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(20, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "ms"
        '
        'L_Sampledelay
        '
        Me.L_Sampledelay.AutoSize = True
        Me.L_Sampledelay.Location = New System.Drawing.Point(3, 98)
        Me.L_Sampledelay.Name = "L_Sampledelay"
        Me.L_Sampledelay.Size = New System.Drawing.Size(72, 13)
        Me.L_Sampledelay.TabIndex = 10
        Me.L_Sampledelay.Text = "Sample Delay"
        '
        'C_Sampledelay
        '
        Me.C_Sampledelay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.C_Sampledelay.FormattingEnabled = True
        Me.C_Sampledelay.Items.AddRange(New Object() {"auto", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100", "200", "300", "400", "500", "1000", "2000", "3000", "4000", "5000"})
        Me.C_Sampledelay.Location = New System.Drawing.Point(96, 95)
        Me.C_Sampledelay.Name = "C_Sampledelay"
        Me.C_Sampledelay.Size = New System.Drawing.Size(75, 21)
        Me.C_Sampledelay.TabIndex = 9
        '
        'L_Samplerate
        '
        Me.L_Samplerate.AutoSize = True
        Me.L_Samplerate.Location = New System.Drawing.Point(3, 71)
        Me.L_Samplerate.Name = "L_Samplerate"
        Me.L_Samplerate.Size = New System.Drawing.Size(93, 13)
        Me.L_Samplerate.TabIndex = 7
        Me.L_Samplerate.Text = "ADC Sample Rate"
        '
        'C_Samplerate
        '
        Me.C_Samplerate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.C_Samplerate.FormattingEnabled = True
        Me.C_Samplerate.Items.AddRange(New Object() {"8 MHz", "4 MHz", "2 MHz", "1 MHz", "500 KHz", "250 KHz", "125 KHz"})
        Me.C_Samplerate.Location = New System.Drawing.Point(96, 68)
        Me.C_Samplerate.Name = "C_Samplerate"
        Me.C_Samplerate.Size = New System.Drawing.Size(77, 21)
        Me.C_Samplerate.TabIndex = 6
        '
        'L_Samples
        '
        Me.L_Samples.AutoSize = True
        Me.L_Samples.Location = New System.Drawing.Point(3, 44)
        Me.L_Samples.Name = "L_Samples"
        Me.L_Samples.Size = New System.Drawing.Size(47, 13)
        Me.L_Samples.TabIndex = 5
        Me.L_Samples.Text = "Samples"
        '
        'C_Samples
        '
        Me.C_Samples.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.C_Samples.FormattingEnabled = True
        Me.C_Samples.Items.AddRange(New Object() {"8", "16", "32", "64", "128", "256", "512", "1024"})
        Me.C_Samples.Location = New System.Drawing.Point(96, 41)
        Me.C_Samples.Name = "C_Samples"
        Me.C_Samples.Size = New System.Drawing.Size(77, 21)
        Me.C_Samples.TabIndex = 4
        '
        'G_Motion
        '
        Me.G_Motion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.G_Motion.Controls.Add(Me.Label2)
        Me.G_Motion.Controls.Add(Me.T_Motion_Delay)
        Me.G_Motion.Controls.Add(Me.L_Motion_Delay)
        Me.G_Motion.Controls.Add(Me.L_LED)
        Me.G_Motion.Controls.Add(Me.L_IOB7)
        Me.G_Motion.Controls.Add(Me.L_IOB6)
        Me.G_Motion.Controls.Add(Me.C_LED)
        Me.G_Motion.Controls.Add(Me.C_IOB7)
        Me.G_Motion.Controls.Add(Me.C_IOB6)
        Me.G_Motion.Controls.Add(Me.L_Threshold1)
        Me.G_Motion.Controls.Add(Me.C_Treshold)
        Me.G_Motion.Location = New System.Drawing.Point(877, 338)
        Me.G_Motion.Name = "G_Motion"
        Me.G_Motion.Size = New System.Drawing.Size(195, 160)
        Me.G_Motion.TabIndex = 5
        Me.G_Motion.TabStop = False
        Me.G_Motion.Text = "Motion Detector"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(166, 128)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "ms"
        '
        'T_Motion_Delay
        '
        Me.T_Motion_Delay.Location = New System.Drawing.Point(109, 125)
        Me.T_Motion_Delay.Name = "T_Motion_Delay"
        Me.T_Motion_Delay.Size = New System.Drawing.Size(52, 20)
        Me.T_Motion_Delay.TabIndex = 19
        Me.T_Motion_Delay.Text = "0"
        '
        'L_Motion_Delay
        '
        Me.L_Motion_Delay.AutoSize = True
        Me.L_Motion_Delay.Location = New System.Drawing.Point(6, 128)
        Me.L_Motion_Delay.Name = "L_Motion_Delay"
        Me.L_Motion_Delay.Size = New System.Drawing.Size(87, 13)
        Me.L_Motion_Delay.TabIndex = 18
        Me.L_Motion_Delay.Text = "Delay after Alarm"
        '
        'L_LED
        '
        Me.L_LED.AutoSize = True
        Me.L_LED.Location = New System.Drawing.Point(6, 104)
        Me.L_LED.Name = "L_LED"
        Me.L_LED.Size = New System.Drawing.Size(72, 13)
        Me.L_LED.TabIndex = 16
        Me.L_LED.Text = "LED on Alarm"
        '
        'L_IOB7
        '
        Me.L_IOB7.AutoSize = True
        Me.L_IOB7.Location = New System.Drawing.Point(6, 81)
        Me.L_IOB7.Name = "L_IOB7"
        Me.L_IOB7.Size = New System.Drawing.Size(101, 13)
        Me.L_IOB7.TabIndex = 15
        Me.L_IOB7.Text = "IO B7 Low on Alarm"
        '
        'L_IOB6
        '
        Me.L_IOB6.AutoSize = True
        Me.L_IOB6.Location = New System.Drawing.Point(6, 58)
        Me.L_IOB6.Name = "L_IOB6"
        Me.L_IOB6.Size = New System.Drawing.Size(103, 13)
        Me.L_IOB6.TabIndex = 14
        Me.L_IOB6.Text = "IO B6 High on Alarm"
        '
        'C_LED
        '
        Me.C_LED.AutoSize = True
        Me.C_LED.Location = New System.Drawing.Point(109, 103)
        Me.C_LED.Name = "C_LED"
        Me.C_LED.Size = New System.Drawing.Size(15, 14)
        Me.C_LED.TabIndex = 13
        Me.C_LED.UseVisualStyleBackColor = True
        '
        'C_IOB7
        '
        Me.C_IOB7.AutoSize = True
        Me.C_IOB7.Location = New System.Drawing.Point(109, 80)
        Me.C_IOB7.Name = "C_IOB7"
        Me.C_IOB7.Size = New System.Drawing.Size(15, 14)
        Me.C_IOB7.TabIndex = 12
        Me.C_IOB7.UseVisualStyleBackColor = True
        '
        'C_IOB6
        '
        Me.C_IOB6.AutoSize = True
        Me.C_IOB6.Location = New System.Drawing.Point(109, 57)
        Me.C_IOB6.Name = "C_IOB6"
        Me.C_IOB6.Size = New System.Drawing.Size(15, 14)
        Me.C_IOB6.TabIndex = 11
        Me.C_IOB6.UseVisualStyleBackColor = True
        '
        'L_Threshold1
        '
        Me.L_Threshold1.AutoSize = True
        Me.L_Threshold1.Location = New System.Drawing.Point(6, 30)
        Me.L_Threshold1.Name = "L_Threshold1"
        Me.L_Threshold1.Size = New System.Drawing.Size(83, 13)
        Me.L_Threshold1.TabIndex = 10
        Me.L_Threshold1.Text = "Alarm Threshold"
        '
        'C_Treshold
        '
        Me.C_Treshold.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.C_Treshold.FormattingEnabled = True
        Me.C_Treshold.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "100", "101", "102", "103", "104", "105", "106", "107", "108", "109", "110", "111", "112", "113", "114", "115", "116", "117", "118", "119", "120", "121", "122", "123", "124", "125", "126", "127", "128", "129", "130", "131", "132", "133", "134", "135", "136", "137", "138", "139", "140", "141", "142", "143", "144", "145", "146", "147", "148", "149", "150", "151", "152", "153", "154", "155", "156", "157", "158", "159", "160", "161", "162", "163", "164", "165", "166", "167", "168", "169", "170", "171", "172", "173", "174", "175", "176", "177", "178", "179", "180", "181", "182", "183", "184", "185", "186", "187", "188", "189", "190", "191", "192", "193", "194", "195", "196", "197", "198", "199", "200", "201", "202", "203", "204", "205", "206", "207", "208", "209", "210", "211", "212", "213", "214", "215", "216", "217", "218", "219", "220", "221", "222", "223", "224", "225", "226", "227", "228", "229", "230", "231", "232", "233", "234", "235", "236", "237", "238", "239", "240", "241", "242", "243", "244", "245", "246", "247", "248", "249", "250", "251", "252", "253", "254", "255"})
        Me.C_Treshold.Location = New System.Drawing.Point(109, 27)
        Me.C_Treshold.Name = "C_Treshold"
        Me.C_Treshold.Size = New System.Drawing.Size(79, 21)
        Me.C_Treshold.TabIndex = 9
        '
        'C_Write
        '
        Me.C_Write.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.C_Write.Location = New System.Drawing.Point(927, 587)
        Me.C_Write.Name = "C_Write"
        Me.C_Write.Size = New System.Drawing.Size(87, 31)
        Me.C_Write.TabIndex = 9
        Me.C_Write.Text = "Write to Board"
        Me.C_Write.UseVisualStyleBackColor = True
        '
        'G_Mode
        '
        Me.G_Mode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.G_Mode.Controls.Add(Me.L_Filterbez)
        Me.G_Mode.Controls.Add(Me.T_Filter_Max)
        Me.G_Mode.Controls.Add(Me.T_Filter_Min)
        Me.G_Mode.Controls.Add(Me.C_Speed_Filter)
        Me.G_Mode.Controls.Add(Me.T_Kmhfaktor)
        Me.G_Mode.Controls.Add(Me.C_kmh)
        Me.G_Mode.Controls.Add(Me.C_MC_FFT)
        Me.G_Mode.Controls.Add(Me.C_Mode)
        Me.G_Mode.Location = New System.Drawing.Point(877, 48)
        Me.G_Mode.Name = "G_Mode"
        Me.G_Mode.Size = New System.Drawing.Size(195, 155)
        Me.G_Mode.TabIndex = 10
        Me.G_Mode.TabStop = False
        Me.G_Mode.Text = "Mode"
        '
        'L_Filterbez
        '
        Me.L_Filterbez.AutoSize = True
        Me.L_Filterbez.Location = New System.Drawing.Point(97, 128)
        Me.L_Filterbez.Name = "L_Filterbez"
        Me.L_Filterbez.Size = New System.Drawing.Size(29, 13)
        Me.L_Filterbez.TabIndex = 18
        Me.L_Filterbez.Text = "X <="
        Me.L_Filterbez.Visible = False
        '
        'T_Filter_Max
        '
        Me.T_Filter_Max.Location = New System.Drawing.Point(129, 125)
        Me.T_Filter_Max.Name = "T_Filter_Max"
        Me.T_Filter_Max.Size = New System.Drawing.Size(52, 20)
        Me.T_Filter_Max.TabIndex = 17
        Me.T_Filter_Max.Text = "0"
        Me.T_Filter_Max.Visible = False
        '
        'T_Filter_Min
        '
        Me.T_Filter_Min.Location = New System.Drawing.Point(129, 99)
        Me.T_Filter_Min.Name = "T_Filter_Min"
        Me.T_Filter_Min.Size = New System.Drawing.Size(52, 20)
        Me.T_Filter_Min.TabIndex = 16
        Me.T_Filter_Min.Text = "0"
        Me.T_Filter_Min.Visible = False
        '
        'C_Speed_Filter
        '
        Me.C_Speed_Filter.AutoSize = True
        Me.C_Speed_Filter.Location = New System.Drawing.Point(9, 101)
        Me.C_Speed_Filter.Name = "C_Speed_Filter"
        Me.C_Speed_Filter.Size = New System.Drawing.Size(119, 17)
        Me.C_Speed_Filter.TabIndex = 15
        Me.C_Speed_Filter.Text = "Speed Filter:    X >="
        Me.C_Speed_Filter.UseVisualStyleBackColor = True
        Me.C_Speed_Filter.Visible = False
        '
        'T_Kmhfaktor
        '
        Me.T_Kmhfaktor.Location = New System.Drawing.Point(129, 71)
        Me.T_Kmhfaktor.Name = "T_Kmhfaktor"
        Me.T_Kmhfaktor.Size = New System.Drawing.Size(52, 20)
        Me.T_Kmhfaktor.TabIndex = 14
        Me.T_Kmhfaktor.Text = "0"
        Me.T_Kmhfaktor.Visible = False
        '
        'C_kmh
        '
        Me.C_kmh.AutoSize = True
        Me.C_kmh.Location = New System.Drawing.Point(9, 73)
        Me.C_kmh.Name = "C_kmh"
        Me.C_kmh.Size = New System.Drawing.Size(84, 17)
        Me.C_kmh.TabIndex = 13
        Me.C_kmh.Text = "km/h Faktor"
        Me.C_kmh.UseVisualStyleBackColor = True
        Me.C_kmh.Visible = False
        '
        'C_MC_FFT
        '
        Me.C_MC_FFT.AutoSize = True
        Me.C_MC_FFT.Location = New System.Drawing.Point(9, 46)
        Me.C_MC_FFT.Name = "C_MC_FFT"
        Me.C_MC_FFT.Size = New System.Drawing.Size(179, 17)
        Me.C_MC_FFT.TabIndex = 12
        Me.C_MC_FFT.Text = "Calculate FFT on Microcontroller"
        Me.C_MC_FFT.UseVisualStyleBackColor = True
        '
        'C_Mode
        '
        Me.C_Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.C_Mode.FormattingEnabled = True
        Me.C_Mode.Items.AddRange(New Object() {"Time Signal (Raw Data)", "FFT (Fast Fourier Transform)"})
        Me.C_Mode.Location = New System.Drawing.Point(9, 19)
        Me.C_Mode.Name = "C_Mode"
        Me.C_Mode.Size = New System.Drawing.Size(177, 21)
        Me.C_Mode.TabIndex = 4
        '
        'SerialPort1
        '
        Me.SerialPort1.BaudRate = 38400
        Me.SerialPort1.ReadBufferSize = 8096
        Me.SerialPort1.WriteBufferSize = 8048
        '
        'C_ShowLog
        '
        Me.C_ShowLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.C_ShowLog.Location = New System.Drawing.Point(996, 504)
        Me.C_ShowLog.Name = "C_ShowLog"
        Me.C_ShowLog.Size = New System.Drawing.Size(76, 28)
        Me.C_ShowLog.TabIndex = 11
        Me.C_ShowLog.Text = "Show Log"
        Me.C_ShowLog.UseVisualStyleBackColor = True
        '
        'C_EnableLog
        '
        Me.C_EnableLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.C_EnableLog.AutoSize = True
        Me.C_EnableLog.Location = New System.Drawing.Point(910, 511)
        Me.C_EnableLog.Name = "C_EnableLog"
        Me.C_EnableLog.Size = New System.Drawing.Size(80, 17)
        Me.C_EnableLog.TabIndex = 14
        Me.C_EnableLog.Text = "Enable Log"
        Me.C_EnableLog.UseVisualStyleBackColor = True
        '
        'F_Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1077, 768)
        Me.Controls.Add(Me.C_EnableLog)
        Me.Controls.Add(Me.C_ShowLog)
        Me.Controls.Add(Me.G_Mode)
        Me.Controls.Add(Me.C_Write)
        Me.Controls.Add(Me.G_Motion)
        Me.Controls.Add(Me.G_Settings1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "F_Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Radar Evaluation Board v.X.XX"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.G_Settings1.ResumeLayout(False)
        Me.G_Settings1.PerformLayout()
        Me.G_Motion.ResumeLayout(False)
        Me.G_Motion.PerformLayout()
        Me.G_Mode.ResumeLayout(False)
        Me.G_Mode.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents DateiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BeendenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HilfeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents G_Settings1 As System.Windows.Forms.GroupBox
    Friend WithEvents L_Samplerate As System.Windows.Forms.Label
    Friend WithEvents C_Samplerate As System.Windows.Forms.ComboBox
    Friend WithEvents L_Samples As System.Windows.Forms.Label
    Friend WithEvents C_Samples As System.Windows.Forms.ComboBox
    Friend WithEvents L_Sampledelay As System.Windows.Forms.Label
    Friend WithEvents C_Sampledelay As System.Windows.Forms.ComboBox
    Friend WithEvents G_Motion As System.Windows.Forms.GroupBox
    Friend WithEvents L_LED As System.Windows.Forms.Label
    Friend WithEvents L_IOB7 As System.Windows.Forms.Label
    Friend WithEvents L_IOB6 As System.Windows.Forms.Label
    Friend WithEvents C_LED As System.Windows.Forms.CheckBox
    Friend WithEvents C_IOB7 As System.Windows.Forms.CheckBox
    Friend WithEvents C_IOB6 As System.Windows.Forms.CheckBox
    Friend WithEvents L_Threshold1 As System.Windows.Forms.Label
    Friend WithEvents C_Treshold As System.Windows.Forms.ComboBox
    Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents C_Write As System.Windows.Forms.Button
    Friend WithEvents G_Mode As System.Windows.Forms.GroupBox
    Friend WithEvents C_Mode As System.Windows.Forms.ComboBox
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort
    Friend WithEvents L_Alert As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents L_Speed As System.Windows.Forms.Label
    Friend WithEvents C_MC_FFT As System.Windows.Forms.CheckBox
    Friend WithEvents C_kmh As System.Windows.Forms.CheckBox
    Friend WithEvents T_Kmhfaktor As System.Windows.Forms.TextBox
    Friend WithEvents C_ShowLog As System.Windows.Forms.Button
    Friend WithEvents C_EnableLog As System.Windows.Forms.CheckBox
    Friend WithEvents L_Baudrate As System.Windows.Forms.Label
    Friend WithEvents C_Baudrate As System.Windows.Forms.ComboBox
    Friend WithEvents T_X As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents T_Y As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents T_Measurespeed As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents C_Speed_Filter As System.Windows.Forms.CheckBox
    Friend WithEvents T_Filter_Min As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents T_Motion_Delay As System.Windows.Forms.TextBox
    Friend WithEvents L_Motion_Delay As System.Windows.Forms.Label
    Friend WithEvents L_Filterbez As System.Windows.Forms.Label
    Friend WithEvents T_Filter_Max As System.Windows.Forms.TextBox

End Class
