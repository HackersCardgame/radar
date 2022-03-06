'
'2013 www.weidmann-elektronik.de
'
'****************************************************************
'* Changelog
'****************************************************************
'* 17.02.2014
'* Version: 1.01
'* -Baudrate programmierbar
'* -Com-Port und Baudrate Autosuche hinzugefügt
'* -Mehr Geschwindigkeit durch höhere Baudraten
'* -km/h Faktor Dezimaltrennzeichen Bug behoben
'* -X, Y Koordinatenanzeige bei Mouseover hinzugefügt
'* -Anzeige der Messzeit hinzugefügt
'* -Geschwindigkeitsfilter hinzugefügt
'****************************************************************
'* 01.11.2014
'* Version: 1.02
'* -Anpassung auf neue schnellere AVR FFT Routine (ChanFFT)
'* -Einfrieren beim Minimieren des Fensters behoben6
'* -Weitere Baudraten hinzugefügt
'* -REB165_1 in REB165 umbenannt
'* -Überspringen der FFT Berechnung, wenn Datenübertragung unvollständig
'****************************************************************
'* 03.01.2015
'* Version: 1.03
'* -Software sendet beim Beenden der Software den Befehl FF an den Controller um den PC Mode in den Offline Motion Detector zu verlassen
'* -Geschwindigkeitsfilter mit Min und Max Geschwindigkeit implementiert
'* -Pause(ms) nach erfolgreicher Detektion ab sofort programmierbar (Motion Delay)
'****************************************************************
'* 03.04.2017
'* Version: 1.04
'* -Neues Logo
'****************************************************************
'* 12.04.2017
'* Version: 1.05
'* -Keine Änderungen. Änderung nur an der Firmware
'****************************************************************

Public Class F_Main
    Private Sub F_Main_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            SerialPort1.BaseStream.WriteByte(&HFF)          'PC Mode im Controller verlassen
        Catch ex As Exception
        End Try

        End     'Programm beenden
    End Sub

    Sub Sample()
        Dim SerialData As Integer                                   'Seriell empfangenes Datenbyte
        Dim i As Integer                                            'Hilfvariable i
        Dim ReceivedBytes As Integer                                'Empfangene Daten
        Dim Maxvalue_I As Integer                                   'Höchster Messwert Index
        Dim Maxvalue As Integer                                     'Höchster Messwert
        Dim Messcounter As Integer                                  'Anzahl der Messungen
        Dim Messzeit_Mittelwert As Double                           'Zeit einer Messung inkl. Auswertung, Mittelwert

        Messcounter = 0                                             'Zähler initialisieren
        Messzeit_Mittelwert = 0                                     'Mittelwert initialisieren

        Do
            Messcounter = Messcounter + 1
            Dim Startzeit As Date = System.DateTime.Now

            Application.DoEvents()                                  'Windows Fenster aktualisieren

            Try
                If C_MC_FFT.Checked = True Then                     'Auswahl prüfen
                    SerialPort1.BaseStream.WriteByte(&HFE)          'Controller gerechnete FFT empfangen Hex FE
                Else
                    SerialPort1.BaseStream.WriteByte(&HFB)          'Rohdaten empfangen Hex FB
                End If

                If C_Sampledelay.Text = "auto" Then                 'Sample Delay prüfen
                    If Mode = 1 Or (Mode = 2 And C_MC_FFT.Checked = False) Then
                        Do While SerialPort1.BytesToRead < Samples      'Warten, bis die richtige Anzahl an Samples empfangen wurde (schnell)
                            If (CInt(System.DateTime.Now.Subtract(Startzeit).TotalMilliseconds)) > 500 Then
                                Exit Do 'Timeout
                            End If
                        Loop
                    End If

                    If Mode = 2 And C_MC_FFT.Checked = True Then
                        Do While SerialPort1.BytesToRead < (Samples / 2)      'Warten, bis die richtige Anzahl an Samples empfangen wurde (schnell)
                            If (CInt(System.DateTime.Now.Subtract(Startzeit).TotalMilliseconds)) > 500 Then
                                Exit Do 'Timeout
                            End If
                        Loop
                    End If
                Else
                    Pause(C_Sampledelay.Text / 1000)                'Sample Zyklus verzögern, Pause in Sekunden
                End If

                Chart1.Series(0).Points.Clear()                         'Diagramm leeren
                ReceivedBytes = SerialPort1.BytesToRead                 'Anzahl empfangener Bytes übergeben

                Alert = False                                           'Alarm auf FALSE
                L_Alert.Visible = False                                 'Alarmanzeige ausblenden

                If Mode = 1 Then                                        'Mode prüfen (1=Raw Signals, 2=FFT)
                    L_Speed.Visible = False                             'Geschwindigkeitsanzeige ausblenden

                    For i = 0 To ReceivedBytes - 1
                        SerialData = SerialPort1.ReadByte               'Messwert einlesen
                        Chart1.Series(0).Points.AddXY(i, SerialData) 'Messwert zeichnen
                        If Alert = False Then                           'Alarm prüfen
                            If SerialData > Threshold Then              'Alarm übersteigt den Schwellwert?
                                L_Alert.Visible = True                  'Alarmanzeige einblenden

                                If C_EnableLog.Checked = True Then                                  'Prüfen ob Log an ist
                                    F_Log.T_Log.Text = F_Log.T_Log.Text & My.Computer.Clock.LocalTime & " ALARM " & SerialData & vbCrLf     'Wert dokumentieren
                                End If
                            End If
                        End If
                    Next i
                Else
                    L_Speed.Visible = True                              'Geschwindigkeitsanzeige einblenden

                    If C_MC_FFT.Checked = False Then                    'Controller gerechnete FFT prüfen
                        n = Samples                                     'Anzahl Samples in FFT Variable n übergeben
                        ReDim REX(Samples)                              'FFT Array REX dimensionieren
                        ReDim IMX(Samples)                              'FFT Array IMX dimensionieren
                        ReDim FFT_Outputarray(Samples)                  'FFT Ausgabearray dimensionieren
                        For i = 0 To ReceivedBytes - 1
                            SerialData = SerialPort1.ReadByte           'Messwert einlesen
                            REX(i) = SerialData                         'Messwert in REX Array übergeben
                            IMX(i) = 0                                  'IMX bleibt immer 0
                        Next i

                        If ReceivedBytes = Samples Then

                            FFT()                                           'FFT am Pc rechnen

                            For i = 0 To (Samples - 1) / 2                  'Ergebnis / 2, da eine FFT immer im Ergebnis gespiegelt ist. Interessant ist nur die Hälfte
                                FFT_Outputarray(i) = Math.Sqrt(IMX(i) * IMX(i) + REX(i) * REX(i))       'Endwert berechnen (Magnitude)
                                FFT_Outputarray(i) = FFT_Outputarray(i) / 20                                'Wert für Ausgabe im Diagramm verkleinern
                                If i = 0 Then FFT_Outputarray(i) = 0 'Der erste Wert einer FFT ist immer unbrauchbar. Wert wird verworfen
                                Chart1.Series(0).Points.AddXY(i, FFT_Outputarray(i)) 'Messwert zeichnen
                            Next i

                            'Maximum Wert ermitteln und somit das stärkste Objekt ermitteln
                            Maxvalue_I = 0                                                      'Höchster Messwert Index initialisieren
                            Maxvalue = 0                                                        'Höchster Messwert initialisieren
                            For i = 0 To (Samples - 1) / 2
                                If FFT_Outputarray(i) > Maxvalue Then
                                    Maxvalue = FFT_Outputarray(i)                               'Höchsten Messwert übergeben
                                    Maxvalue_I = i                                              'Höchsten Messwert Index übergeben
                                End If
                            Next i

                            If Maxvalue >= Threshold Then                                       'Höchster Messwert > Schwellwert?
                                If C_Speed_Filter.Checked = False Or (C_Speed_Filter.Checked = True And Maxvalue_I >= CDbl(T_Filter_Min.Text)) Then
                                    If C_kmh.Checked = True Then
                                        L_Speed.Text = "Speed (km/h): " & Math.Round(Maxvalue_I / CDbl(Val(T_Kmhfaktor.Text)), 0)       'Geschwindigkeit anzeigen
                                    Else
                                        L_Speed.Text = "Speed: " & Maxvalue_I                           'Geschwindigkeit anzeigen
                                    End If

                                    If C_EnableLog.Checked = True Then                                  'Prüfen ob Log an ist
                                        F_Log.T_Log.Text = F_Log.T_Log.Text & My.Computer.Clock.LocalTime & " " & L_Speed.Text & vbCrLf     'Wert dokumentieren
                                    End If

                                    Chart1.Series(0).Points(Maxvalue_I).MarkerSize = 15 'Markergröße definieren
                                    Chart1.Series(0).Points(Maxvalue_I).MarkerStyle = DataVisualization.Charting.MarkerStyle.Square 'Marker einzeichnen
                                End If
                            End If
                        End If
                    Else
                        Maxvalue_I = 0                                                      'Höchster Messwert Index initialisieren
                        Maxvalue = 0                                                        'Höchster Messwert initialisieren

                        For i = 0 To ReceivedBytes - 1
                            SerialData = SerialPort1.ReadByte                               'Messwert einlesen
                            Chart1.Series(0).Points.AddXY(i, SerialData)                    'Messwert zeichnen

                            If SerialData > Maxvalue Then                                   'Messwert übersteigt den Schwellwert?
                                Maxvalue = SerialData                                       'Höchsten Messwert übergeben
                                Maxvalue_I = i                                              'Höchsten Messwert Index übergeben
                            End If
                        Next i

                        If Maxvalue >= Threshold Then                                       'Höchster Messwert > Schwellwert?
                            If C_kmh.Checked = True Then
                                L_Speed.Text = "Speed (km/h): " & Math.Round(Maxvalue_I / CDbl(Val(T_Kmhfaktor.Text)), 0)       'Geschwindigkeit anzeigen
                            Else
                                L_Speed.Text = "Speed: " & Maxvalue_I                           'Geschwindigkeit anzeigen
                            End If

                            If C_EnableLog.Checked = True Then                                  'Prüfen ob Log an ist
                                F_Log.T_Log.Text = F_Log.T_Log.Text & My.Computer.Clock.LocalTime & " " & L_Speed.Text & vbCrLf     'Wert dokumentieren
                            End If


                            Chart1.Series(0).Points(Maxvalue_I).MarkerSize = 15             'Markergröße definieren
                            Chart1.Series(0).Points(Maxvalue_I).MarkerStyle = DataVisualization.Charting.MarkerStyle.Square     'Marker einzeichnen
                        End If
                    End If
                End If
            Catch ex As Exception
                'Kleine Programmfehler ignorieren
            End Try

            SerialPort1.ReadExisting()          'seriellen Puffer leeren für nächsten Zyklus

            Messzeit_Mittelwert = Messzeit_Mittelwert + CInt(System.DateTime.Now.Subtract(Startzeit).TotalMilliseconds)     'Zeit einer Messung inkl. Auswertung, Mittelwert bilden

            If Messcounter >= 10 Then
                T_Measurespeed.Text = "Measure Speed: " & Math.Round(Messzeit_Mittelwert / 10, 2) & " Milliseconds"           'Zeit einer Messung inkl. Auswertung, Mittelwert
                Messcounter = 0                                                 'Zähler initialisieren
                Messzeit_Mittelwert = 0                                         'Mittelwert initialisieren
            End If

            If Me.WindowState = FormWindowState.Minimized Then                  'Prüfen ob Fenster minimiert wurde
                Pause(0.5)                                                      '0.5 Sek warten um ein maximieren wieder möglich zu machen
            End If
        Loop
    End Sub

    Private Sub C_write_Click(sender As System.Object, e As System.EventArgs) Handles C_Write.Click
        C_Write.Enabled = False                                                'Button deaktivieren

        Threshold = C_Treshold.Text                                            'Schwellwert Auswahl übergeben
        Samples = C_Samples.Text                                               'Anzahl der Samples Auswahl übergeben
        IO_B6 = C_IOB6.Checked                                                 'IO_B6 Auswahl übergeben
        IO_B7 = C_IOB7.Checked                                                 'IO_B7 Auswahl übergeben
        LED = C_LED.Checked                                                    'LED Auswahl übergeben

        'Mode prüfen und Auswahl übergeben
        If C_Mode.Text = "Time Signal (Raw Data)" Then Mode = 1
        If C_Mode.Text = "FFT (Fast Fourier Transform)" Then Mode = 2

        SerialPort1.BaseStream.WriteByte(&HFD)                                          'Board auf Senden der Konfiguration setzen Hex FD
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CByte(Mode))                                   'Mode übergeben
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CByte(C_Treshold.Text))                        'Schwellwert übergeben
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CByte(Math.Log(CDbl(C_Samples.Text), 2)))      'Samples in 8 Bit übergeben (2 Hoch 4, 2 Hoch 5,...)
        Pause(0.3)
        'Prescaler Auswahl prüfen und in 8 Bit übertragen
        If C_Samplerate.Text = "8 MHz" Then SerialPort1.BaseStream.WriteByte(CByte(2))
        If C_Samplerate.Text = "4 MHz" Then SerialPort1.BaseStream.WriteByte(CByte(4))
        If C_Samplerate.Text = "2 MHz" Then SerialPort1.BaseStream.WriteByte(CByte(8))
        If C_Samplerate.Text = "1 MHz" Then SerialPort1.BaseStream.WriteByte(CByte(16))
        If C_Samplerate.Text = "500 KHz" Then SerialPort1.BaseStream.WriteByte(CByte(32))
        If C_Samplerate.Text = "250 KHz" Then SerialPort1.BaseStream.WriteByte(CByte(64))
        If C_Samplerate.Text = "125 KHz" Then SerialPort1.BaseStream.WriteByte(CByte(128))
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CInt(C_IOB6.Checked) * -1)     'IO_B6 Auswahl übergeben
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CInt(C_IOB7.Checked) * -1)     'IO_B7 Auswahl übergeben
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CInt(C_LED.Checked) * -1)      'LED Auswahl übergeben
        Pause(0.3)
        If C_Baudrate.Text = "9600" Then SerialPort1.BaseStream.WriteByte(CByte(103)) 'Baudrate Auswahl übergeben
        If C_Baudrate.Text = "19200" Then SerialPort1.BaseStream.WriteByte(CByte(51)) 'Baudrate Auswahl übergeben
        If C_Baudrate.Text = "38400" Then SerialPort1.BaseStream.WriteByte(CByte(25)) 'Baudrate Auswahl übergeben
        If C_Baudrate.Text = "76800" Then SerialPort1.BaseStream.WriteByte(CByte(12)) 'Baudrate Auswahl übergeben
        If C_Baudrate.Text = "250000" Then SerialPort1.BaseStream.WriteByte(CByte(3)) 'Baudrate Auswahl übergeben
        If C_Baudrate.Text = "500000" Then SerialPort1.BaseStream.WriteByte(CByte(1)) 'Baudrate Auswahl übergeben
        If C_Baudrate.Text = "1000000" Then SerialPort1.BaseStream.WriteByte(CByte(0)) 'Baudrate Auswahl übergeben
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CByte(GetHiByte(T_Motion_Delay.Text)))         'Motion_Delay Highbyte übergeben
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CByte(GetLoByte(T_Motion_Delay.Text)))         'Motion_Delay Lowbyte übergeben
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CInt(C_Speed_Filter.Checked) * -1)             'Speed_Filter Auswahl übergeben
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CByte(GetHiByte(T_Filter_Min.Text)))           'Filter_Min Highbyte übergeben
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CByte(GetLoByte(T_Filter_Min.Text)))           'Filter_Min Lowbyte übergeben
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CByte(GetHiByte(T_Filter_Max.Text)))           'Filter_Max Highbyte übergeben
        Pause(0.3)
        SerialPort1.BaseStream.WriteByte(CByte(GetLoByte(T_Filter_Max.Text)))           'Filter_Max Lowbyte übergeben

        InitBaudrate()                                                                  'Baudrate im Programm neu setzen
        InitChart()                                                                     'Diagramm initialisieren
        C_Write.Enabled = True                                                          'Button aktivieren
    End Sub

    Sub InitBaudrate()
        If C_Baudrate.Text = "9600" Then SerialPort1.BaudRate = 9600 'Baudrate setzen
        If C_Baudrate.Text = "19200" Then SerialPort1.BaudRate = 19200 'Baudrate setzen
        If C_Baudrate.Text = "38400" Then SerialPort1.BaudRate = 38400 'Baudrate setzen
        If C_Baudrate.Text = "76800" Then SerialPort1.BaudRate = 76800 'Baudrate setzen
        If C_Baudrate.Text = "250000" Then SerialPort1.BaudRate = 250000 'Baudrate setzen
        If C_Baudrate.Text = "500000" Then SerialPort1.BaudRate = 500000 'Baudrate setzen
        If C_Baudrate.Text = "1000000" Then SerialPort1.BaudRate = 1000000 'Baudrate setzen
    End Sub

    Sub InitChart()

        If C_Mode.Text = "Time Signal (Raw Data)" Then                          'Auswahl prüfen
            Chart1.Series(0).Name = "Time Signal"                               'Chart benennen
            Chart1.Series(1).Name = "Threshold"                                 'Chart benennen
            Chart1.Series(2).Name = "Filter Min"                                'Chart benennen
            Chart1.Series(3).Name = "Filter Max"                                'Chart benennen

            Chart1.ChartAreas(0).AxisX.Title = "Samples"                        'Achse benennen
            Chart1.ChartAreas(0).AxisY.Title = "ADC Value"                      'Achse benennen

            Chart1.ChartAreas(0).AxisX.Maximum = Samples - 1                    'Maximale X Achse definieren
            Chart1.ChartAreas(0).AxisX.Minimum = 0                              'Minimale X Achse definieren
            Chart1.ChartAreas(0).AxisY.Maximum = 255                            'Maximale Y Achse definieren
            Chart1.ChartAreas(0).AxisY.Minimum = 0                              'Minimale Y Achse definieren
            'Achsenintervall setzen
            If Samples = 8 Then Chart1.ChartAreas(0).AxisX.Interval = 1
            If Samples = 16 Then Chart1.ChartAreas(0).AxisX.Interval = 2
            If Samples = 32 Then Chart1.ChartAreas(0).AxisX.Interval = 5
            If Samples = 64 Then Chart1.ChartAreas(0).AxisX.Interval = 10
            If Samples = 128 Then Chart1.ChartAreas(0).AxisX.Interval = 20
            If Samples = 256 Then Chart1.ChartAreas(0).AxisX.Interval = 25
            If Samples = 512 Then Chart1.ChartAreas(0).AxisX.Interval = 50
            If Samples = 1024 Then Chart1.ChartAreas(0).AxisX.Interval = 100

            Chart1.Series(0).BorderWidth = 2                                    'Linienstärke definieren
            Chart1.Series(1).BorderWidth = 2                                    'Linienstärke definieren
            Chart1.Series(2).BorderWidth = 2                                    'Linienstärke definieren
            Chart1.Series(3).BorderWidth = 2                                    'Linienstärke definieren

            Chart1.Series(1).Points.Clear()                                     'Diagramm leeren
            Chart1.Series(1).Points.AddXY(0, Threshold)                         'Schwellwert einzeichnen
            Chart1.Series(1).Points.AddXY(Samples - 1, Threshold)               'Schwellwert einzeichnen

            Chart1.Series(2).Points.Clear()                                     'Diagramm leeren
            Chart1.Series(3).Points.Clear()                                     'Diagramm leeren
        Else
            Chart1.Series(0).Name = "FFT"
            Chart1.Series(1).Name = "Threshold"
            Chart1.Series(2).Name = "Filter Min"
            Chart1.Series(3).Name = "Filter Max"
            Chart1.ChartAreas(0).AxisX.Title = "Frequency"
            Chart1.ChartAreas(0).AxisY.Title = "Signal"

            Chart1.ChartAreas(0).AxisX.Maximum = (Samples) / 2                  'Maximale X Achse definieren
            Chart1.ChartAreas(0).AxisX.Minimum = 0                              'Minimale X Achse definieren
            Chart1.ChartAreas(0).AxisY.Maximum = 500                            'Maximale Y Achse definieren
            Chart1.ChartAreas(0).AxisY.Minimum = 0                              'Minimale Y Achse definieren

            'Achsenintervall setzen
            If Samples = 8 Then Chart1.ChartAreas(0).AxisX.Interval = 1
            If Samples = 16 Then Chart1.ChartAreas(0).AxisX.Interval = 2
            If Samples = 32 Then Chart1.ChartAreas(0).AxisX.Interval = 5
            If Samples = 64 Then Chart1.ChartAreas(0).AxisX.Interval = 10
            If Samples = 128 Then Chart1.ChartAreas(0).AxisX.Interval = 20
            If Samples = 256 Then Chart1.ChartAreas(0).AxisX.Interval = 25
            If Samples = 512 Then Chart1.ChartAreas(0).AxisX.Interval = 50
            If Samples = 1024 Then Chart1.ChartAreas(0).AxisX.Interval = 100

            Chart1.Series(0).BorderWidth = 2                                    'Linienstärke definieren
            Chart1.Series(1).BorderWidth = 2                                    'Linienstärke definieren
            Chart1.Series(2).BorderWidth = 2                                    'Linienstärke definieren
            Chart1.Series(3).BorderWidth = 2                                    'Linienstärke definieren

            Chart1.Series(1).Points.Clear()                                     'Diagramm leeren
            Chart1.Series(1).Points.AddXY(0, Threshold)                         'Schwellwert einzeichnen
            Chart1.Series(1).Points.AddXY(Samples - 1, Threshold)               'Schwellwert einzeichnen

            Chart1.Series(2).Points.Clear()                                     'Diagramm leeren
            Chart1.Series(3).Points.Clear()                                     'Diagramm leeren
            If C_Speed_Filter.Checked = True Then
                Chart1.Series(2).Points.AddXY(CDbl(T_Filter_Min.Text), 0)                                       'Filter Min einzeichnen
                Chart1.Series(2).Points.AddXY(CDbl(T_Filter_Min.Text), Chart1.ChartAreas(0).AxisY.Maximum)      'Filter Min einzeichnen
                Chart1.Series(3).Points.AddXY(CDbl(T_Filter_Max.Text), 0)                                       'Filter Max einzeichnen
                Chart1.Series(3).Points.AddXY(CDbl(T_Filter_Max.Text), Chart1.ChartAreas(0).AxisY.Maximum)      'Filter Max einzeichnen
            End If

            SetkhmDefaultFaktor()
        End If
    End Sub

    Private Sub C_Mode_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles C_Mode.SelectedIndexChanged
        If C_Mode.Text = "Time Signal (Raw Data)" Then
            C_MC_FFT.Checked = False        'Controller FFT Auswahl ausschalten
            C_MC_FFT.Visible = False        'Controller FFT Auswahl ausblenden
            C_kmh.Checked = False           'Kmh Umrechnung ausschalten
            C_kmh.Visible = False           'Kmh Umrechnung Auswahl ausblenden
            T_Kmhfaktor.Visible = False     'Kmh Faktor ausblenden
            C_Speed_Filter.Visible = False  'Filter Auswahl ausblenden
            T_Filter_Min.Visible = False    'Filter ausblenden
            T_Filter_Max.Visible = False    'Filter ausblenden
            L_Filterbez.Visible = False     'Filter ausblenden
        Else
            C_MC_FFT.Visible = True         'Controller FFT Auswahl einblenden
            C_kmh.Visible = True            'Kmh Umrechnung Auswahl einblenden
            T_Kmhfaktor.Visible = True      'Kmh Faktor einblenden
            C_Speed_Filter.Visible = True   'Filter Auswahl einblenden
            T_Filter_Min.Visible = True     'Filter einblenden
            T_Filter_Max.Visible = True     'Filter einblenden
            L_Filterbez.Visible = True      'Filter ausblenden
        End If
    End Sub

    Private Sub BeendenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles BeendenToolStripMenuItem.Click
        End     'Programm beenden
    End Sub

    Sub SetkhmDefaultFaktor()
        T_Kmhfaktor.Text = "0"

        If C_Samples.Text = "1024" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.34"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.38"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.41"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.46"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.57"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.61"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.86"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.9"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "1.44"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "1.53"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "2.71"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "2.71"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "5.04"
        If C_Samples.Text = "1024" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "5.04"

        If C_Samples.Text = "512" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.18"
        If C_Samples.Text = "512" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.21"
        If C_Samples.Text = "512" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.21"
        If C_Samples.Text = "512" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.24"
        If C_Samples.Text = "512" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.29"
        If C_Samples.Text = "512" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.31"
        If C_Samples.Text = "512" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.43"
        If C_Samples.Text = "512" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.46"
        If C_Samples.Text = "512" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.72"
        If C_Samples.Text = "512" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.76"
        If C_Samples.Text = "512" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "1.35"
        If C_Samples.Text = "512" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "1.35"
        If C_Samples.Text = "512" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "2.52"
        If C_Samples.Text = "512" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "2.52"

        If C_Samples.Text = "256" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.1"
        If C_Samples.Text = "256" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.11"
        If C_Samples.Text = "256" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.12"
        If C_Samples.Text = "256" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.14"
        If C_Samples.Text = "256" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.16"
        If C_Samples.Text = "256" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.17"
        If C_Samples.Text = "256" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.23"
        If C_Samples.Text = "256" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.24"
        If C_Samples.Text = "256" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.37"
        If C_Samples.Text = "256" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.39"
        If C_Samples.Text = "256" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.68"
        If C_Samples.Text = "256" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.68"
        If C_Samples.Text = "256" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "1.26"
        If C_Samples.Text = "256" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "1.26"

        If C_Samples.Text = "128" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.07"
        If C_Samples.Text = "128" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.07"
        If C_Samples.Text = "128" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.08"
        If C_Samples.Text = "128" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.08"
        If C_Samples.Text = "128" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.09"
        If C_Samples.Text = "128" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.1"
        If C_Samples.Text = "128" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.13"
        If C_Samples.Text = "128" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.13"
        If C_Samples.Text = "128" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.19"
        If C_Samples.Text = "128" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.2"
        If C_Samples.Text = "128" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.34"
        If C_Samples.Text = "128" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.34"
        If C_Samples.Text = "128" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.63"
        If C_Samples.Text = "128" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.63"

        If C_Samples.Text = "64" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "64" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "64" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "64" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "64" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "64" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.07"
        If C_Samples.Text = "64" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.8"
        If C_Samples.Text = "64" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.8"
        If C_Samples.Text = "64" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.11"
        If C_Samples.Text = "64" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.12"
        If C_Samples.Text = "64" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.18"
        If C_Samples.Text = "64" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.18"
        If C_Samples.Text = "64" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.32"
        If C_Samples.Text = "64" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.32"

        If C_Samples.Text = "32" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "32" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "32" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "32" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "32" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "32" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "32" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "32" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "32" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.07"
        If C_Samples.Text = "32" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.07"
        If C_Samples.Text = "32" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.1"
        If C_Samples.Text = "32" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.11"
        If C_Samples.Text = "32" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.17"
        If C_Samples.Text = "32" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.17"

        If C_Samples.Text = "16" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "16" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "16" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "16" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "16" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "16" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "16" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "16" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "16" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "16" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "16" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.07"
        If C_Samples.Text = "16" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.07"
        If C_Samples.Text = "16" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.1"
        If C_Samples.Text = "16" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.1"

        If C_Samples.Text = "8" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "8" And C_Samplerate.Text = "8 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "8" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "8" And C_Samplerate.Text = "4 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "8" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "8" And C_Samplerate.Text = "2 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "8" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "8" And C_Samplerate.Text = "1 MHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "8" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "8" And C_Samplerate.Text = "500 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.05"
        If C_Samples.Text = "8" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "8" And C_Samplerate.Text = "250 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.06"
        If C_Samples.Text = "8" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = False Then T_Kmhfaktor.Text = "0.07"
        If C_Samples.Text = "8" And C_Samplerate.Text = "125 KHz" And C_MC_FFT.Checked = True Then T_Kmhfaktor.Text = "0.07"

    End Sub

    Private Sub C_MC_FFT_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles C_MC_FFT.CheckedChanged
        If C_Samples.Text = "32" And C_MC_FFT.Checked = True Then
            MsgBox("32 Samples von AVR FTT nicht unterstützt", vbInformation)
            C_Samples.Text = "64"
        End If

        If C_Samples.Text = "16" And C_MC_FFT.Checked = True Then
            MsgBox("16 Samples von AVR FTT nicht unterstützt", vbInformation)
            C_Samples.Text = "64"
        End If

        If C_Samples.Text = "8" And C_MC_FFT.Checked = True Then
            MsgBox("8 Samples von AVR FTT nicht unterstützt", vbInformation)
            C_Samples.Text = "64"
        End If

        SetkhmDefaultFaktor()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles C_ShowLog.Click
        F_Log.Visible = True
    End Sub

    Private Sub T_Kmhfaktor_TextChanged(sender As System.Object, e As System.EventArgs) Handles T_Kmhfaktor.TextChanged
        If InStr(T_Kmhfaktor.Text, ",") <> 0 Then
            T_Kmhfaktor.Text = Replace(T_Kmhfaktor.Text, ",", ".")
            T_Kmhfaktor.SelectionStart = Len(T_Kmhfaktor.Text)
        End If
    End Sub

    Private Sub Chart1_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Chart1.MouseMove
        Dim X As Integer
        Dim Y As Integer

        Try
            X = Chart1.ChartAreas(0).AxisX.PixelPositionToValue(e.X)
            Y = Chart1.ChartAreas(0).AxisY.PixelPositionToValue(e.Y)
            If Mode = "2" Then
                If X > 0 And X < (CDbl(C_Samples.Text) / 2) Then T_X.Text = "X: " & X
            Else
                If X > 0 And X < CDbl(C_Samples.Text) Then T_X.Text = "X: " & X
            End If

            If Y > 0 And Y < Chart1.ChartAreas(0).AxisY.Maximum Then T_Y.Text = "Y: " & Y
        Catch

        End Try
    End Sub

    Private Sub C_Samples_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles C_Samples.SelectedIndexChanged
        If C_Samples.Text = "32" And C_MC_FFT.Checked = True Then
            MsgBox("32 Samples von AVR FTT nicht unterstützt", vbInformation)
            C_Samples.Text = "64"
        End If

        If C_Samples.Text = "16" And C_MC_FFT.Checked = True Then
            MsgBox("16 Samples von AVR FTT nicht unterstützt", vbInformation)
            C_Samples.Text = "64"
        End If

        If C_Samples.Text = "8" And C_MC_FFT.Checked = True Then
            MsgBox("8 Samples von AVR FTT nicht unterstützt", vbInformation)
            C_Samples.Text = "64"
        End If
    End Sub

    Private Sub F_Main_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub T_Motion_Delay_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles T_Motion_Delay.KeyPress
        Select Asc(e.KeyChar)
            Case 48 To 57, 8, 32
                ' Zahlen, Backspace und Space zulassen
            Case Else
                ' alle anderen Eingaben unterdrücken
                e.Handled = True
        End Select
    End Sub

    Private Sub T_Motion_Delay_TextChanged(sender As System.Object, e As System.EventArgs) Handles T_Motion_Delay.TextChanged
        If T_Motion_Delay.Text = "" Then
            T_Motion_Delay.Text = "0"
        End If

        If CInt(T_Motion_Delay.Text) > 65535 Then
            T_Motion_Delay.Text = 65535
        End If
    End Sub

    Private Sub T_Filter_Min_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles T_Filter_Min.KeyPress
        Select Case Asc(e.KeyChar)
            Case 48 To 57, 8, 32
                ' Zahlen, Backspace und Space zulassen
            Case Else
                ' alle anderen Eingaben unterdrücken
                e.Handled = True
        End Select
    End Sub

    Private Sub T_Filter_Max_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles T_Filter_Max.KeyPress
        Select Case Asc(e.KeyChar)
            Case 48 To 57, 8, 32
                ' Zahlen, Backspace und Space zulassen
            Case Else
                ' alle anderen Eingaben unterdrücken
                e.Handled = True
        End Select
    End Sub

    Private Sub T_Filter_Min_TextChanged(sender As System.Object, e As System.EventArgs) Handles T_Filter_Min.TextChanged
        If T_Filter_Min.Text = "" Then
            T_Filter_Min.Text = "0"
        End If

        If CInt(T_Filter_Min.Text) > 1024 Then
            T_Filter_Min.Text = 1024
        End If
    End Sub

    Private Sub T_Filter_Max_TextChanged(sender As System.Object, e As System.EventArgs) Handles T_Filter_Max.TextChanged
        If T_Filter_Max.Text = "" Then
            T_Filter_Max.Text = "0"
        End If

        If CInt(T_Filter_Max.Text) > 1024 Then
            T_Filter_Max.Text = 1024
        End If
    End Sub
End Class
