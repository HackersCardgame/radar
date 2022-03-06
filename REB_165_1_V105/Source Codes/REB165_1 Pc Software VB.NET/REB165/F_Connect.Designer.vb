<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class F_Connect
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(F_Connect))
        Me.F_Connection = New System.Windows.Forms.GroupBox()
        Me.C_SearchCom = New System.Windows.Forms.Button()
        Me.L_Baudrate = New System.Windows.Forms.Label()
        Me.C_Baudrate = New System.Windows.Forms.ComboBox()
        Me.C_Connect = New System.Windows.Forms.Button()
        Me.L_Comport = New System.Windows.Forms.Label()
        Me.C_Comport = New System.Windows.Forms.ComboBox()
        Me.P_Logo = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.P_Info = New System.Windows.Forms.PictureBox()
        Me.L_Info1 = New System.Windows.Forms.Label()
        Me.L_Info2 = New System.Windows.Forms.Label()
        Me.L_Info3 = New System.Windows.Forms.Label()
        Me.L_Version = New System.Windows.Forms.Label()
        Me.F_Connection.SuspendLayout()
        CType(Me.P_Logo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.P_Info, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'F_Connection
        '
        Me.F_Connection.BackColor = System.Drawing.SystemColors.Control
        Me.F_Connection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.F_Connection.Controls.Add(Me.C_SearchCom)
        Me.F_Connection.Controls.Add(Me.L_Baudrate)
        Me.F_Connection.Controls.Add(Me.C_Baudrate)
        Me.F_Connection.Controls.Add(Me.C_Connect)
        Me.F_Connection.Controls.Add(Me.L_Comport)
        Me.F_Connection.Controls.Add(Me.C_Comport)
        Me.F_Connection.Location = New System.Drawing.Point(6, 358)
        Me.F_Connection.Name = "F_Connection"
        Me.F_Connection.Size = New System.Drawing.Size(499, 79)
        Me.F_Connection.TabIndex = 4
        Me.F_Connection.TabStop = False
        Me.F_Connection.Text = "Connection"
        '
        'C_SearchCom
        '
        Me.C_SearchCom.Image = CType(resources.GetObject("C_SearchCom.Image"), System.Drawing.Image)
        Me.C_SearchCom.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.C_SearchCom.Location = New System.Drawing.Point(228, 23)
        Me.C_SearchCom.Name = "C_SearchCom"
        Me.C_SearchCom.Size = New System.Drawing.Size(67, 37)
        Me.C_SearchCom.TabIndex = 6
        Me.C_SearchCom.Text = "Autodetect"
        Me.C_SearchCom.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.C_SearchCom.UseVisualStyleBackColor = True
        '
        'L_Baudrate
        '
        Me.L_Baudrate.AutoSize = True
        Me.L_Baudrate.Location = New System.Drawing.Point(90, 23)
        Me.L_Baudrate.Name = "L_Baudrate"
        Me.L_Baudrate.Size = New System.Drawing.Size(132, 13)
        Me.L_Baudrate.TabIndex = 5
        Me.L_Baudrate.Text = "Baud Rate (38400 default)"
        '
        'C_Baudrate
        '
        Me.C_Baudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.C_Baudrate.FormattingEnabled = True
        Me.C_Baudrate.Items.AddRange(New Object() {"9600", "19200", "38400", "76800", "250000", "500000", "1000000"})
        Me.C_Baudrate.Location = New System.Drawing.Point(93, 39)
        Me.C_Baudrate.Name = "C_Baudrate"
        Me.C_Baudrate.Size = New System.Drawing.Size(81, 21)
        Me.C_Baudrate.TabIndex = 4
        '
        'C_Connect
        '
        Me.C_Connect.Image = CType(resources.GetObject("C_Connect.Image"), System.Drawing.Image)
        Me.C_Connect.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.C_Connect.Location = New System.Drawing.Point(404, 19)
        Me.C_Connect.Name = "C_Connect"
        Me.C_Connect.Size = New System.Drawing.Size(87, 42)
        Me.C_Connect.TabIndex = 0
        Me.C_Connect.Text = "Connect"
        Me.C_Connect.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.C_Connect.UseVisualStyleBackColor = True
        '
        'L_Comport
        '
        Me.L_Comport.AutoSize = True
        Me.L_Comport.Location = New System.Drawing.Point(14, 23)
        Me.L_Comport.Name = "L_Comport"
        Me.L_Comport.Size = New System.Drawing.Size(50, 13)
        Me.L_Comport.TabIndex = 3
        Me.L_Comport.Text = "Com Port"
        '
        'C_Comport
        '
        Me.C_Comport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.C_Comport.FormattingEnabled = True
        Me.C_Comport.Items.AddRange(New Object() {"COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10", "COM11", "COM12", "COM13", "COM14", "COM15", "COM16", "COM17", "COM18", "COM19", "COM20", "COM21", "COM22", "COM23", "COM24", "COM25", "COM26", "COM27", "COM28", "COM29", "COM30"})
        Me.C_Comport.Location = New System.Drawing.Point(17, 39)
        Me.C_Comport.Name = "C_Comport"
        Me.C_Comport.Size = New System.Drawing.Size(67, 21)
        Me.C_Comport.TabIndex = 1
        '
        'P_Logo
        '
        Me.P_Logo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.P_Logo.Image = CType(resources.GetObject("P_Logo.Image"), System.Drawing.Image)
        Me.P_Logo.Location = New System.Drawing.Point(6, 5)
        Me.P_Logo.Name = "P_Logo"
        Me.P_Logo.Size = New System.Drawing.Size(497, 347)
        Me.P_Logo.TabIndex = 5
        Me.P_Logo.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.White
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(38, 288)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(438, 31)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Radar Evaluation Board REB165" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'P_Info
        '
        Me.P_Info.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.P_Info.Location = New System.Drawing.Point(128, 138)
        Me.P_Info.Name = "P_Info"
        Me.P_Info.Size = New System.Drawing.Size(248, 107)
        Me.P_Info.TabIndex = 7
        Me.P_Info.TabStop = False
        Me.P_Info.Visible = False
        '
        'L_Info1
        '
        Me.L_Info1.AutoSize = True
        Me.L_Info1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L_Info1.Location = New System.Drawing.Point(168, 165)
        Me.L_Info1.Name = "L_Info1"
        Me.L_Info1.Size = New System.Drawing.Size(169, 20)
        Me.L_Info1.TabIndex = 8
        Me.L_Info1.Text = "Detecting COM Port"
        Me.L_Info1.Visible = False
        '
        'L_Info2
        '
        Me.L_Info2.AutoSize = True
        Me.L_Info2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L_Info2.Location = New System.Drawing.Point(201, 185)
        Me.L_Info2.Name = "L_Info2"
        Me.L_Info2.Size = New System.Drawing.Size(103, 20)
        Me.L_Info2.TabIndex = 9
        Me.L_Info2.Text = "Port: COMX"
        Me.L_Info2.Visible = False
        '
        'L_Info3
        '
        Me.L_Info3.AutoSize = True
        Me.L_Info3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L_Info3.Location = New System.Drawing.Point(192, 205)
        Me.L_Info3.Name = "L_Info3"
        Me.L_Info3.Size = New System.Drawing.Size(121, 20)
        Me.L_Info3.TabIndex = 10
        Me.L_Info3.Text = "Baud: XXXXX"
        Me.L_Info3.Visible = False
        '
        'L_Version
        '
        Me.L_Version.AutoSize = True
        Me.L_Version.BackColor = System.Drawing.Color.White
        Me.L_Version.Location = New System.Drawing.Point(12, 331)
        Me.L_Version.Name = "L_Version"
        Me.L_Version.Size = New System.Drawing.Size(45, 13)
        Me.L_Version.TabIndex = 11
        Me.L_Version.Text = "Version:"
        '
        'F_Connect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(508, 443)
        Me.Controls.Add(Me.L_Version)
        Me.Controls.Add(Me.L_Info3)
        Me.Controls.Add(Me.L_Info2)
        Me.Controls.Add(Me.L_Info1)
        Me.Controls.Add(Me.P_Info)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.P_Logo)
        Me.Controls.Add(Me.F_Connection)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "F_Connect"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Radar Evaluation Board"
        Me.F_Connection.ResumeLayout(False)
        Me.F_Connection.PerformLayout()
        CType(Me.P_Logo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.P_Info, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()


    End Sub


    Friend WithEvents F_Connection As System.Windows.Forms.GroupBox
    Friend WithEvents C_Connect As System.Windows.Forms.Button
    Friend WithEvents L_Comport As System.Windows.Forms.Label
    Friend WithEvents C_Comport As System.Windows.Forms.ComboBox
    Friend WithEvents P_Logo As System.Windows.Forms.PictureBox
    Friend WithEvents L_Baudrate As System.Windows.Forms.Label
    Friend WithEvents C_Baudrate As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents C_SearchCom As System.Windows.Forms.Button
    Friend WithEvents P_Info As System.Windows.Forms.PictureBox
    Friend WithEvents L_Info1 As System.Windows.Forms.Label
    Friend WithEvents L_Info2 As System.Windows.Forms.Label
    Friend WithEvents L_Info3 As System.Windows.Forms.Label
    Friend WithEvents L_Version As System.Windows.Forms.Label

    Private Sub Form1_Shown(sender As Object, e As EventArgs) _
     Handles Me.Shown

        ''MessageBox.Show("You are in the Form.Shown event.")
        C_SearchCom.PerformClick()


    End Sub

End Class


