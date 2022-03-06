'
'2013 www.weidmann-elektronik.de
'
Imports System.IO.Ports
Public Class F_Connect
    Private Sub C_Connect_Click(sender As System.Object, e As System.EventArgs) Handles C_Connect.Click
        Dim Readbuffer As String                                'Serialbuffer

        C_Connect.Enabled = False                               'Button deaktivieren

        Try
            F_Main.SerialPort1.PortName = C_Comport.Text        'Port
            F_Main.SerialPort1.BaudRate = C_Baudrate.Text       'Baudrate
            F_Main.SerialPort1.Open()                           'ComPort öffnen
            F_Main.SerialPort1.BaseStream.WriteByte(&HFA)       'Board Identification Hex FA

            Pause(0.5)                                          'Pause in Sekunden
            Readbuffer = F_Main.SerialPort1.ReadExisting        'Read Serial Data

            If InStr(Readbuffer, "REB165") <> 0 Then          'Board prüfen
                C_Connect.Enabled = True                        'Button aktivieren
                Hide()
                SaveSetting("REB165", "Settings", "Baudrate", C_Baudrate.Text)        'Einstellung speichern
                SaveSetting("REB165", "Settings", "Comport", C_Comport.Text)          'Einstellung speichern

                ReadBoard_Config()                              'Board Konfiguration empfangen
                Init_GUI()                                      'Benutzeroberfläche initialisieren
                F_Main.InitChart()                              'Diagramm initialisieren
                F_Main.SetkhmDefaultFaktor()                    'Khm Default Faktor setzen
                F_Main.Show()                                   'Formular anzeigen
                F_Main.Sample()                                 'Sample Schleife starten
                Exit Sub
            Else
                F_Main.SerialPort1.Close()                      'ComPort schließen
                MsgBox("REB165 Board not found", vbCritical)  'Fehlermeldung
                C_Connect.Enabled = True                        'Button aktivieren
                Exit Sub
            End If
        Catch
            MsgBox(Err.Description, vbCritical)                 'Fehlermeldung
            C_Connect.Enabled = True                            'Button aktivieren
            Exit Sub
        End Try
    End Sub

    Private Sub F_Connect_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        End     'Programm beenden
    End Sub

    Private Sub F_Connect_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim Baud As String
        Dim Com As String

        F_Main.Text = "Radar Evaluation Board v." & Version & " www.weidmann-elektronik.de"     'Formular benennen
        Me.Text = "Radar Evaluation Board v." & Version                                         'Formular benennen
        L_Version.Text = "Version: " & Version                                                  'Version anzeigen

        Baud = GetSetting("REB165", "Settings", "Baudrate", "")                               'gespeicherte Einstellung lesen
        Com = GetSetting("REB165", "Settings", "Comport", "")                                 'gespeicherte Einstellung lesen

        If Baud = "" Then                                                                       'gespeicherte Einstellung auf Existenz prüfen
            C_Baudrate.Text = "38400"                                                           'Standard setzen
        Else
            C_Baudrate.Text = Baud                                                              'gespeicherten Wert setzen
        End If

        If Com = "" Then                                                                        'gespeicherte Einstellung auf Existenz prüfen
            C_Comport.Text = "COM4"                                                             'Standard setzen
        Else
            C_Comport.Text = Com                                                               'gespeicherten Wert setzen
        End If
    End Sub

    Sub ReadBoard_Config()
        F_Main.SerialPort1.BaseStream.WriteByte(&HFC)       'Board Konfiguration empfangen Hex FC
        Pause(0.5)                                          'Pause in Sekunden

        Mode = F_Main.SerialPort1.ReadByte                  'Mode übernehmen 1/Raw Data, 2/FFT
        Threshold = F_Main.SerialPort1.ReadByte             'Schwellwert übernehmen
        Samples = 2 ^ F_Main.SerialPort1.ReadByte           'Anzahl Samples übernehmen
        ADC_Prescaler = F_Main.SerialPort1.ReadByte         'ADC Prescaler übernehmen
        IO_B6 = F_Main.SerialPort1.ReadByte                 'IO_B6 übernehmen 1 aktiviert, 0 deaktiviert
        IO_B7 = F_Main.SerialPort1.ReadByte                 'IO_B7 übernehmen 1 aktiviert, 0 deaktiviert
        LED = F_Main.SerialPort1.ReadByte                   'LED übernehmen 1 aktiviert, 0 deaktiviert
        Baudrate = F_Main.SerialPort1.ReadByte              'Baudrate übernehmen 0,1,3,25 (= Baudrate 1.000.000,500.000,250.000,38.400)
        Speed_Filter = F_Main.SerialPort1.ReadByte          'Speed_Filter übernehmen 1 aktiviert, 0 deaktiviert
        Motion_Delay_Highbyte = F_Main.SerialPort1.ReadByte                     'Motion_Delay_Highbyte übernehmen
        Motion_Delay_Lowbyte = F_Main.SerialPort1.ReadByte                      'Motion_Delay_Highbyte übernehmen
        Motion_Delay = (Motion_Delay_Highbyte * 256) + Motion_Delay_Lowbyte     'Pause 16-Bit Wert (0-65535) mit dem Highbyte und Lowbyte berechnen
        Min_Speed_Highbyte = F_Main.SerialPort1.ReadByte                        'Min_Speed_Highbyte übernehmen
        Min_Speed_Lowbyte = F_Main.SerialPort1.ReadByte                         'Min_Speed_Lowbyte übernehmen
        Min_Speed = (Min_Speed_Highbyte * 256) + Min_Speed_Lowbyte              'Minimale Geschwingdigkeit 16-Bit Wert (0-65535) mit dem Highbyte und Lowbyte berechnen
        Max_Speed_Highbyte = F_Main.SerialPort1.ReadByte                        'Max_Speed_Highbyte übernehmen
        Max_Speed_Lowbyte = F_Main.SerialPort1.ReadByte                         'Max_Speed_Lowbyte übernehmen
        Max_Speed = (Max_Speed_Highbyte * 256) + Max_Speed_Lowbyte              'Minimale Geschwingdigkeit 16-Bit Wert (0-65535) mit dem Highbyte und Lowbyte berechnen
    End Sub

    Sub Init_GUI()
        F_Main.C_Treshold.Text = Threshold                                  'Schwellwert in Auswahl übernehmen
        F_Main.C_Samples.Text = Samples                                     'Anzahl Samples in Auswahl übernehmen
        F_Main.C_Sampledelay.Text = "auto"                                  'Sample Delay Standard setzen
        F_Main.C_IOB6.Checked = IO_B6                                       'IO_B6 in Auswahl übernehmen
        F_Main.C_IOB7.Checked = IO_B7                                       'IO_B7 in Auswahl übernehmen
        F_Main.C_LED.Checked = LED                                          'LED in Auswahl übernehmen
        F_Main.C_Speed_Filter.Checked = Speed_Filter                        'Speed_Filter in Auswahl übernehmen
        F_Main.T_Motion_Delay.Text = Motion_Delay                           'Motion_Delay in Auswahl übernehmen
        F_Main.T_Filter_Min.Text = Min_Speed                                'Min_Speed in Auswahl übernehmen
        F_Main.T_Filter_Max.Text = Max_Speed                                'Max_Speed in Auswahl übernehmen

        'Prescaler prüfen und in Auswahl übernehmen
        If ADC_Prescaler = 2 Then F_Main.C_Samplerate.Text = "8 MHz"
        If ADC_Prescaler = 4 Then F_Main.C_Samplerate.Text = "4 MHz"
        If ADC_Prescaler = 8 Then F_Main.C_Samplerate.Text = "2 MHz"
        If ADC_Prescaler = 16 Then F_Main.C_Samplerate.Text = "1 MHz"
        If ADC_Prescaler = 32 Then F_Main.C_Samplerate.Text = "500 KHz"
        If ADC_Prescaler = 64 Then F_Main.C_Samplerate.Text = "250 KHz"
        If ADC_Prescaler = 128 Then F_Main.C_Samplerate.Text = "125 KHz"

        If Mode = 1 Then                                                    'Mode prüfen
            F_Main.C_Mode.Text = "Time Signal (Raw Data)"                   'Mode Auswahl setzen
        Else
            F_Main.C_Mode.Text = "FFT (Fast Fourier Transform)"             'Mode Auswahl setzen
        End If

        If Baudrate = 0 Then F_Main.C_Baudrate.Text = "1000000"
        If Baudrate = 1 Then F_Main.C_Baudrate.Text = "500000"
        If Baudrate = 3 Then F_Main.C_Baudrate.Text = "250000"
        If Baudrate = 12 Then F_Main.C_Baudrate.Text = "76800"
        If Baudrate = 25 Then F_Main.C_Baudrate.Text = "38400"
        If Baudrate = 51 Then F_Main.C_Baudrate.Text = "19200"
        If Baudrate = 103 Then F_Main.C_Baudrate.Text = "9600"
    End Sub

    Private Sub C_SearchCom_Click(sender As System.Object, e As System.EventArgs) Handles C_SearchCom.Click
        Dim Readbuffer As String                                                'Serialbuffer
        Dim Found As Boolean                                                    'Status

        C_SearchCom.Enabled = False                                             'Button deaktivieren
        C_Connect.Enabled = False                                               'Button deaktivieren
        L_Info1.Visible = True                                                  'Info Label einblenden
        L_Info2.Visible = True                                                  'Info Label einblenden
        L_Info3.Visible = True                                                  'Info Label einblenden
        P_Info.Visible = True                                                   'Info Box einblenden
        Found = False                                                           'Status initialisieren

        For Each Port As String In SerialPort.GetPortNames()                    'verfügbare COM-Ports ermitteln
            For i = 1 To 7                                                      'Baudraten durchprüfen
                Try
                    F_Main.SerialPort1.PortName = Port                          'Port setzen

                    If i = 1 Then F_Main.SerialPort1.BaudRate = "9600" 'Baudrate setzen
                    If i = 2 Then F_Main.SerialPort1.BaudRate = "19200" 'Baudrate
                    If i = 3 Then F_Main.SerialPort1.BaudRate = "38400" 'Baudrate
                    If i = 4 Then F_Main.SerialPort1.BaudRate = "76800" 'Baudrate
                    If i = 5 Then F_Main.SerialPort1.BaudRate = "250000" 'Baudrate
                    If i = 6 Then F_Main.SerialPort1.BaudRate = "500000" 'Baudrate
                    If i = 7 Then F_Main.SerialPort1.BaudRate = "1000000" 'Baudrate

                    L_Info2.Text = "Port: " & Port                              'Port in Info anzeigen
                    L_Info3.Text = "Baud: " & F_Main.SerialPort1.BaudRate       'Baud in Info anzeigen

                    F_Main.SerialPort1.Open()                                   'ComPort öffnen
                    F_Main.SerialPort1.BaseStream.WriteByte(&HFA)               'Board Identification Hex FA

                    Pause(0.5)                                                  'Pause in Sekunden
                    Readbuffer = F_Main.SerialPort1.ReadExisting                'Read Serial Data

                    If InStr(Readbuffer, "REB165") <> 0 Then                  'Board prüfen
                        C_Comport.Text = Port                                   'Port in Auswahl setzen
                        C_Baudrate.Text = F_Main.SerialPort1.BaudRate           'Baudrate in Auswahl setzen
                        Found = True                                            'Status setzen
                        F_Main.SerialPort1.Close()                              'ComPort schließen
                        Exit For
                    Else
                        F_Main.SerialPort1.Close()                              'ComPort schließen
                    End If
                Catch
                End Try
            Next (i)
        Next Port

        C_SearchCom.Enabled = True                                              'Button aktivieren
        C_Connect.Enabled = True                                                'Button aktivieren
        P_Info.Visible = False                                                  'Info Box ausblenden
        L_Info1.Visible = False                                                 'Info Label ausblenden
        L_Info2.Visible = False                                                 'Info Label ausblenden
        L_Info3.Visible = False

        If Found = False Then                                                   'Status prüfen
            MsgBox("REB165 Board not found", vbCritical)                      'Fehlermeldung
        Else
            C_Connect.PerformClick()                                            'Verbindung aufbauen
        End If
    End Sub
End Class