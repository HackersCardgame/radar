Module M_Globals
    Public Mode As Integer
    Public Threshold As Integer
    Public Samples As Integer
    Public ADC_Prescaler As Integer
    Public IO_B6 As Integer
    Public IO_B7 As Integer
    Public LED As Integer
    Public Alert As Boolean
    Public Baudrate As Integer
    Public Speed_Filter As Integer
    Public Motion_Delay As Integer
    Public Motion_Delay_Highbyte As Integer
    Public Motion_Delay_Lowbyte As Integer
    Public Min_Speed As Integer
    Public Min_Speed_Highbyte As Integer
    Public Min_Speed_Lowbyte As Integer
    Public Max_Speed As Integer
    Public Max_Speed_Highbyte As Integer
    Public Max_Speed_Lowbyte As Integer

    Public Const Version As String = "1.05"

    Public Function GetHiByte(ByVal Word As Integer) As Integer
        If Word < 0 Then
            GetHiByte = ((Word And &H7F00) \ 256) Or 128
        Else
            GetHiByte = (Word And &H7F00) \ 256
        End If
    End Function

    Public Function GetLoByte(ByVal Word As Integer) As Integer
        GetLoByte = Word And &HFF
    End Function

    Public Sub SetLoByte(Word As Integer, ByVal LoValue As Integer)
        Word = (Word And &HFF00) Or LoValue
    End Sub

    Public Sub SetHiByte(Word As Long, ByVal HiValue As Integer)
        Word = (Word And &HFF) Or ((HiValue And &HFF) * 256)
    End Sub
End Module
