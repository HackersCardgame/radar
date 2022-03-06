'
'2013 www.weidmann-elektronik.de
'
Module M_FFT
    Public REX() As Double  'REX[ ] holds the real part of the frequency domain
    Public IMX() As Double 'IMX[ ] holds the imaginary part of the frequency domain
    Public n As Integer
    Public FFT_Outputarray() As Double
    Dim Pi As Double

    Public Sub FFT()
        Dim NM1 As Integer
        Dim ND2 As Integer
        Dim m As Integer
        Dim j As Integer
        Dim i As Integer
        Dim K As Integer
        Dim l As Integer
        Dim LE As Integer
        Dim LE2 As Integer
        Dim JM1 As Integer
        Dim IP As Integer
        Dim SI As Single
        Dim SR As Single
        Dim UR As Single
        Dim UI As Single

        Dim TR As Single
        Dim TI As Single

        Pi = 3.14159265 'Set constants

        NM1 = n - 1
        ND2 = n / 2
        m = Math.Log(n) / Math.Log(2)
        j = ND2

        For i = 1 To n - 2 'Bit reversal sorting
            If i >= j Then GoTo 1190
            TR = REX(j)
            TI = IMX(j)
            REX(j) = REX(i)
            IMX(j) = IMX(i)
            REX(i) = TR
            IMX(i) = TI
1190:
            K = ND2
1200:
            If K > j Then GoTo 1240
            j = j - K
            K = K / 2
            GoTo 1200
1240:
            j = j + K
        Next i

        For l = 1 To m 'Loop for each stage
            LE = 2 ^ l
            LE2 = LE / 2
            UR = 1
            UI = 0
            SR = Math.Cos(Pi / LE2) 'Calculate sine & cosine values
            SI = -Math.Sin(Pi / LE2)
            For j = 1 To LE2 'Loop for each sub DFT
                JM1 = j - 1
                For i = JM1 To NM1 Step LE 'Loop for each butterfly
                    IP = i + LE2
                    TR = REX(IP) * UR - IMX(IP) * UI 'Butterfly calculation
                    TI = REX(IP) * UI + IMX(IP) * UR
                    REX(IP) = REX(i) - TR
                    IMX(IP) = IMX(i) - TI
                    REX(i) = REX(i) + TR
                    IMX(i) = IMX(i) + TI
                Next i
                TR = UR
                UR = TR * SR - UI * SI
                UI = TR * SI + UI * SR
            Next j
        Next l
    End Sub
End Module
