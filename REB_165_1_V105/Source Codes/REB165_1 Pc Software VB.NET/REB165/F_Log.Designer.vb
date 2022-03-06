<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class F_Log
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(F_Log))
        Me.T_Log = New System.Windows.Forms.TextBox()
        Me.C_Save = New System.Windows.Forms.Button()
        Me.C_Clear = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.SuspendLayout()
        '
        'T_Log
        '
        Me.T_Log.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.T_Log.BackColor = System.Drawing.Color.White
        Me.T_Log.Location = New System.Drawing.Point(2, 57)
        Me.T_Log.Multiline = True
        Me.T_Log.Name = "T_Log"
        Me.T_Log.ReadOnly = True
        Me.T_Log.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.T_Log.Size = New System.Drawing.Size(496, 295)
        Me.T_Log.TabIndex = 0
        '
        'C_Save
        '
        Me.C_Save.Location = New System.Drawing.Point(12, 12)
        Me.C_Save.Name = "C_Save"
        Me.C_Save.Size = New System.Drawing.Size(78, 30)
        Me.C_Save.TabIndex = 1
        Me.C_Save.Text = "Save"
        Me.C_Save.UseVisualStyleBackColor = True
        '
        'C_Clear
        '
        Me.C_Clear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.C_Clear.Location = New System.Drawing.Point(409, 12)
        Me.C_Clear.Name = "C_Clear"
        Me.C_Clear.Size = New System.Drawing.Size(80, 30)
        Me.C_Clear.TabIndex = 2
        Me.C_Clear.Text = "Clear"
        Me.C_Clear.UseVisualStyleBackColor = True
        '
        'F_Log
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(501, 355)
        Me.Controls.Add(Me.C_Clear)
        Me.Controls.Add(Me.C_Save)
        Me.Controls.Add(Me.T_Log)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "F_Log"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Log"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents T_Log As System.Windows.Forms.TextBox
    Friend WithEvents C_Save As System.Windows.Forms.Button
    Friend WithEvents C_Clear As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
End Class
