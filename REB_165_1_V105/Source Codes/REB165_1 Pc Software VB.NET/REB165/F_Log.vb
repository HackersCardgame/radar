Public Class F_Log

    Private Sub C_Clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles C_Clear.Click
        T_Log.Text = ""
    End Sub

    Private Sub C_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles C_Save.Click
        SaveFileDialog1.InitialDirectory = "C:\"
        SaveFileDialog1.FileName = "Logfile.txt"
        SaveFileDialog1.Filter = " Texte (*.txt)|*.txt|" & " Alle Dateien (*.*)|*.*"
        SaveFileDialog1.Title = "Datei zum Speichern auswählen"

        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            IO.File.WriteAllText(SaveFileDialog1.FileName, T_Log.Text)
            MsgBox("Datei erfolgreich gespeichert.", vbInformation)
            T_Log.Clear()
        End If
    End Sub
End Class