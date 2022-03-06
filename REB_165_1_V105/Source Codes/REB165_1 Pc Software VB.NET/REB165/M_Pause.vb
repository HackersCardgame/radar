Imports System.Threading

Module M_Pause
    Sub Pause(ByVal Pausenlänge As Single)
        Dim Dummy As Boolean
        Dim Pruefzahl As Single
        Dim Start As Single
        Dim Ende As Single

        Dummy = True
        Start = Microsoft.VisualBasic.DateAndTime.Timer
        Ende = Start + Pausenlänge

        Do While Microsoft.VisualBasic.DateAndTime.Timer < Ende
            Pruefzahl = Start - Microsoft.VisualBasic.DateAndTime.Timer
            If Pruefzahl > 0 And Dummy = True Then
                Dummy = False
                Ende = Ende - Pruefzahl
            End If

            Application.DoEvents()
        Loop
    End Sub
End Module
