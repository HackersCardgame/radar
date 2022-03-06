/*
 * main.c
 *
 * Created: 08.03.2013 21:47:46
 *  Author: Sebastian Weidmann www.weidmann-elektronik.de
 *
 ****************************************************************
 * Changelog
 ****************************************************************
 * 17.02.2014
 * Version: 1.01
 * -Baudrate programmierbar
 * -Warnungen bei Kompilieren behoben
 ****************************************************************
 * 01.11.2014
 * Version: 1.02
 * -Neue schnellere FFT Routine (ChanFFT) implementiert
 * -pow(2, Samples) Befehl durch 1 << Samples ersetzt, da es Fehler in der Berechnung gab
 ****************************************************************
 * 29.12.2014
 * Version: 1.03
 * -Fehler in der Anwendung der _delay_ms Routine behoben. F_CPU in Delay Routine ist richtig definiert
 * -_delay_ms,_delay_us Routine mit Hilfe der Hilfsroutine delay_ms,delay_us nun variabel konfigurierbar
 * -Offline Motion Detector: Pause(ms) nach erfolgreicher Detektion ab sofort programmierbar
 * -Offline Motion Detector: Geschwindigkeitsfilter im FFT Mode implementiert
 * -Neuen Befehl HEX FF zum Verlassen der PC Mode Schleife hinzugefügt
 ****************************************************************
 * 22.03.2017
 * Version: 1.04
 * -Code optimiert und umstrukturiert
 * -Projekt Update auf Atmel Studio 7
 * -Geschwindigkeitsausgabe über USART1 im Offline Motion Detector
 * -Bugfix: Board nicht mehr ansprechbar, wenn fehlerhafte Werte imm EEPROM stehen
 ****************************************************************
 * 11.04.2017
 * Version: 1.05
 * -Bugfix: Geschwindigkeitsberechnung mit kmhfaktor
 ****************************************************************
 */

#ifndef F_CPU
#define F_CPU 16000000UL													// Controller CPU Frequenz
#endif

#include <avr/io.h>
#include <math.h>
#include <avr/pgmspace.h>
#include <util/delay.h>
#include <string.h>
#include <avr/interrupt.h>
#include <avr/eeprom.h>
#include <stdlib.h>
#include <stdint.h>
#include "ffft.h"
#include "uart.h"
#include "adc.h"
#include "globals.h"

void Read_Config(void);														//Board Einstellungen aus EEPROM laden
void Set_Config(void);														//Board Einstellungen vom Pc empfangen und im EEPROM speichern
void PC_Mode(void);
void delay_ms(uint16_t count);												//Hilfsroutine um _delay_ms variabel zu machen
void delay_us(uint16_t count);												//Hilfsroutine um _delay_us variabel zu machen
float getKhmFaktor(uint16_t SampleCount, uint8_t ADC_Prescaler);			//khm Umrechnungsfaktor

int16_t adcval;														//ADC Wert
uint16_t i;															//Zählvariable
uint16_t i2;															//Zählvariable
uint8_t uartinput;													//Eingelesener UART Wert
int16_t DatenarrayR[1024];											//FFT Datenarray R
uint8_t Threshold;													//Schwellwert
uint8_t Samples;														//Anzahl Samples 2 hoch X
uint8_t Mode;														//Mode 1/Raw Data, 2/FFT
uint8_t ADC_Prescaler;												//ADC Prescaler
uint8_t IOB6;														//IOB6 ON/OFF
uint8_t IOB7;														//IOB7 ON/OFF
uint8_t LED;															//LED ON/OFF
uint16_t SampleCount;												//Anzahl Samples
int32_t UBRR_VAL;													//Baudraten Wert
uint8_t Motion_Delay_Highbyte;										//Pause in ms nach erfolgreicher Detektion beim Offline Motion Detector
uint8_t Motion_Delay_Lowbyte;										//Pause in ms nach erfolgreicher Detektion beim Offline Motion Detector
uint16_t Motion_Delay;												//Pause in ms nach erfolgreicher Detektion beim Offline Motion Detector						
uint8_t Speed_Filter;												//Speed_Filter ON/OFF
uint8_t Min_Speed;													//Speed_Filter Minimale Geschwindigkeit
uint8_t Min_Speed_Highbyte;											//Speed_Filter Minimale Geschwindigkeit Highbyte
uint8_t Min_Speed_Lowbyte;											//Speed_Filter Minimale Geschwindigkeit Lowbyte
uint8_t Max_Speed;													//Speed_Filter Maximale Geschwindigkeit
 uint8_t Max_Speed_Highbyte;											//Speed_Filter Maximale Geschwindigkeit Highbyte
uint8_t Max_Speed_Lowbyte;											//Speed_Filter Maximale Geschwindigkeit Lowbyte
uint8_t velocity;
complex_t *bfly;					//Arraypointer für FFT erstellen
uint16_t *spectrum;					//Arraypointer für FFT erstellen
uint16_t Max_X;														//Maxwert Offline Motion Detector FFT Mode
uint16_t Max_Y;														//Maxwert Offline Motion Detector FFT Mode
float kmhfaktor;

int main(void)
{
	bfly = NULL;						//Pointer initialisieren
	spectrum = NULL;					//Pointer initialisieren
	velocity = 0;
	Max_X = 0;
	Max_Y = 0;
	
	Read_Config();						//Read EEPROM Parameter
	ADC_Init(ADC_Prescaler);			//Init ADC
	uart_init(UBRR_VAL, PORT_USART0);	//Init UART0
	uart_init(103, PORT_USART1);	//Init UART1 (Geschwindigkeisausgabe im Offline Motion Detector mit Baud 9600)
	
	DDRA = (1<<PA4);					//PORTA.4 as Output
	DDRB = (1<<PB6) | (1<<PB7);			//PORTB.6 as Output,PORTB.7 as Output

	PORTA &= ~(1 << PA4);				//PORTA.4 LOW
	PORTB &= ~(1 << PB6);				//PORTB.6 LOW
	PORTB |= (1 << PB7);				//PORTB.7 HIGH

	kmhfaktor = getKhmFaktor(SampleCount,ADC_Prescaler);		//Umrechnungsfaktor anhand der Parameter ermitteln

	while (1){
		uartinput = uart_getc(PORT_USART0);		//Read ADC Value
		
		////////////////////////////////////////////////////////////////////////////////////////////////////////		
		//Offline Motion Detector Begin
		////////////////////////////////////////////////////////////////////////////////////////////////////////
		if (Mode EQUALS 1) {									//Mode 1 Raw Data Motion Detector
			adcval = ADC_Read(0) / 4;						//Read ADC Value (8 Bit)

			if (adcval > Threshold) {						//ADC Wert > Schwellwert?
				if (LED EQUALS 1) {PORTA |= (1 << PA4);}		//PORTA.4 HIGH (LED)
				if (IOB6 EQUALS 1) {PORTB |= (1 << PB6);}		//PORTB.6 HIGH
				if (IOB7 EQUALS 1) {PORTB &= ~(1 << PB7);}		//PORTB.7 LOW
				
				delay_ms(Motion_Delay);
				PORTA &= ~(1 << PA4);						//PORTA.4 LOW (LED)
				PORTB &= ~(1 << PB6);						//PORTB.6 LOW
				PORTB |= (1 << PB7);						//PORTB.7 HIGH
			}
		}
		
		
		if (Mode EQUALS 2) {									//Mode 2 FFT Motion Detector (mehr Reichweite und Begrenzung des Gewschwindigkeitsbereiches)
			for (i = 0 ; i < SampleCount ; i++){			//Anzahl Samples durchlaufen (bsp. 2 hoch 8 = 256 Samples)
				adcval = ADC_Read(0) ;						//Read ADC Value
				DatenarrayR[i] = adcval;					//ADC Wert in FFT Array R übergeben
			}

			switch(SampleCount) {
				case 64:
					fft_input_64(DatenarrayR, bfly);	//FFT Datenübergabe
					fft_execute_64(bfly);				//FFT rechnen
					fft_output_64(bfly, spectrum);		//FFT Ergebnis
					break;
				case 128:
					fft_input_128(DatenarrayR, bfly);	//FFT Datenübergabe
					fft_execute_128(bfly);				//FFT rechnen
					fft_output_128(bfly, spectrum);		//FFT Ergebnis
					break;
				case 256:
					fft_input_256(DatenarrayR, bfly);	//FFT Datenübergabe
					fft_execute_256(bfly);				//FFT rechnen
					fft_output_256(bfly, spectrum);		//FFT Ergebnis
					break;
				case 512:
					fft_input_512(DatenarrayR, bfly);	//FFT Datenübergabe
					fft_execute_512(bfly);				//FFT rechnen
					fft_output_512(bfly, spectrum);		//FFT Ergebnis
					break;
				case 1024:
					fft_input_1024(DatenarrayR, bfly);	//FFT Datenübergabe
					fft_execute_1024(bfly);				//FFT rechnen
					fft_output_1024(bfly, spectrum);		//FFT Ergebnis
					break;
				default:
					fft_input_256(DatenarrayR, bfly);	//FFT Datenübergabe
					fft_execute_256(bfly);				//FFT rechnen
					fft_output_256(bfly, spectrum);		//FFT Ergebnis
					break;
			}

			Max_X = 0;
			Max_Y = 0;

			for(i = 2; i < (SampleCount /2); i++) {			//Ergebnisarray durchlaufen
				if (spectrum[i] > Max_Y) {					//Größten Peak ermitteln
					Max_Y = spectrum[i];					//Peak merken
					Max_X = i;								//Peak Position merken
				}
			}

			if (Max_Y > Threshold) {														//FFT Wert > Schwellwert?
				if ((Max_X >= Min_Speed AND Max_X <= Max_Speed) OR Speed_Filter EQUALS 0) {		//Wenn Speed_Filter aktiv, dann Min und Max Geschwindigkeit prüfen
					velocity = Max_X / kmhfaktor;												//Geschwindigkeit in kmh berechnen und auf Ganzzahl runden
					uart_putc(velocity,PORT_USART1);											//Ausgabe der Geschwindigkeit über UART1
					
					if (LED EQUALS 1) {PORTA |= (1 << PA4);}									//Wenn Option LED aktiviert, dann PORTA.4 HIGH (LED)
					if (IOB6 EQUALS 1) {PORTB |= (1 << PB6);}									//Wenn Option IOB6 aktiviert, dann PORTB.6 HIGH
					if (IOB7 EQUALS 1) {PORTB &= ~(1 << PB7);}									//Wenn Option IOB7 aktiviert, dann PORTB.7 LOW
					delay_ms(Motion_Delay);
					PORTA &= ~(1 << PA4);				//PORTA.4 LOW (LED)
					PORTB &= ~(1 << PB6);				//PORTB.6 LOW
					PORTB |= (1 << PB7);				//PORTB.7 HIGH
				}
			}
		}
		////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Offline Motion Detector End
		////////////////////////////////////////////////////////////////////////////////////////////////////////

		if (uartinput EQUALS 0xFA) {									//Hex FA = Pc Connection
			uart_puts("REB165",PORT_USART0);							//Board Identifikation senden
			PC_Mode();													//In Pc Modus wechseln und Befehle empfangen
			kmhfaktor = getKhmFaktor(SampleCount,ADC_Prescaler);		//Umrechnungsfaktor anhand der Parameter neu ermitteln
		}
	
	}

}


void PC_Mode(void){
	while(1){
		uartinput = uart_wait_getc(PORT_USART0);						//Warten, bis ein UART Befehl empfangen wurde
	
		if (uartinput EQUALS 0xFB) {									//Hex FB = Daten samplen und an Pc senden
			PORTA &= ~(1 << PA4);										//PORTA.4 LOW (LED)
			PORTB &= ~(1 << PB6);										//PORTB.6 LOW
			PORTB |= (1 << PB7);										//PORTB.7 HIGH

			for (i = 0 ; i < SampleCount ; i++){						//Anzahl Samples durchlaufen (bsp. 2 hoch 8 = 256 Samples)
				DatenarrayR[i] = ADC_Read(0) / 4;						//Read ADC Value (8 Bit)
			}
			
			for (i = 0 ; i < SampleCount ; i++){
				if (Mode EQUALS 1) {									//Mode prüfen
					if (DatenarrayR[i] > Threshold) {					//ADC Wert > Schwellwert?
						if (LED EQUALS 1) {PORTA |= (1 << PA4);}		//Wenn Option LED aktiviert, dann PORTA.4 HIGH (LED)
						if (IOB6 EQUALS 1) {PORTB |= (1 << PB6);}		//Wenn Option IOB6 aktiviert, dann PORTB.6 HIGH
						if (IOB7 EQUALS 1) {PORTB &= ~(1 << PB7);}		//Wenn Option IOB7 aktiviert, dann PORTB.7 LOW
					}
				}
				uart_putc(DatenarrayR[i],PORT_USART0);							// ADC Wert an Pc senden
			}			
			
		}


		if (uartinput EQUALS 0xFE) {									//Hex FE = Daten samplen, FFT rechnen und an Pc senden
			for (i = 0 ; i < SampleCount ; i++){						//Anzahl Samples durchlaufen (bsp. 2 hoch 8 = 256 Samples)
				adcval = ADC_Read(0);									//Read ADC Value
				DatenarrayR[i] = adcval;								//ADC Wert in FFT Array R übergeben
			}
			
			switch(SampleCount) {
				case 64: 
					fft_input_64(DatenarrayR, bfly);	//FFT Datenübergabe
					fft_execute_64(bfly);				//FFT rechnen
					fft_output_64(bfly, spectrum);		//FFT Ergebnis
					break;
				case 128:
					fft_input_128(DatenarrayR, bfly);	//FFT Datenübergabe
					fft_execute_128(bfly);				//FFT rechnen
					fft_output_128(bfly, spectrum);		//FFT Ergebnis
					break;
				case 256: 
					fft_input_256(DatenarrayR, bfly);	//FFT Datenübergabe
					fft_execute_256(bfly);				//FFT rechnen
					fft_output_256(bfly, spectrum);		//FFT Ergebnis
					break;
				case 512:
					fft_input_512(DatenarrayR, bfly);	//FFT Datenübergabe
					fft_execute_512(bfly);				//FFT rechnen
					fft_output_512(bfly, spectrum);		//FFT Ergebnis
					break;
				case 1024:
					fft_input_1024(DatenarrayR, bfly);	//FFT Datenübergabe
					fft_execute_1024(bfly);				//FFT rechnen
					fft_output_1024(bfly, spectrum);		//FFT Ergebnis
					break;
				default: 
					fft_input_256(DatenarrayR, bfly);	//FFT Datenübergabe
					fft_execute_256(bfly);				//FFT rechnen
					fft_output_256(bfly, spectrum);		//FFT Ergebnis
					break;
			}
			
			for( i2 = 0; i2 < (SampleCount / 2); i2++)						//Anzahl Samples durchlaufen (bsp. 2 hoch 8 = 256 Samples)
			{
				if (i2 EQUALS 0){										//Prüfen auf ersten Array Wert 
					spectrum[i2] = 0;								//Der erste Wert eine FFT muss immer verworfen werden, da er unbrauchbar ist
				}
				
				if (i2 EQUALS 1){										//Prüfen auf ersten Array Wert
					spectrum[i2] = 0;								//Der erste Wert eine FFT muss immer verworfen werden, da er unbrauchbar ist
				}

				uart_putc(spectrum[i2],PORT_USART0);							//FFT Wert an Pc senden
			}
		}


		if (uartinput EQUALS 0xFA) {								//Hex FA = Pc Connection
			uart_puts("REB165",PORT_USART0);						//Board Identifikation senden
		}

		if (uartinput EQUALS 0xFC) {								//Board Config an Pc senden
			uart_putc(Mode,PORT_USART0);										//Mode senden
			uart_putc(Threshold,PORT_USART0);								    //Schwellwert senden
			uart_putc(Samples,PORT_USART0);										//Samples senden
			uart_putc(ADC_Prescaler,PORT_USART0);								//ADC Prescaler senden
			uart_putc(IOB6,PORT_USART0);										//IOB6 senden
			uart_putc(IOB7,PORT_USART0);										//IOB7 senden
			uart_putc(LED,PORT_USART0);											//LED senden
			uart_putc(UBRR_VAL,PORT_USART0);									//Baudrate senden
			uart_putc(Speed_Filter,PORT_USART0);								//Speed Filter AN/AUS senden
			uart_putc(Motion_Delay_Highbyte,PORT_USART0);						//Motion_Delay_Highbyte senden
			uart_putc(Motion_Delay_Lowbyte,PORT_USART0);						//Motion_Delay_Lowbyte senden
			uart_putc(Min_Speed_Highbyte,PORT_USART0);							//Min_Speed_Highbyte senden
			uart_putc(Min_Speed_Lowbyte,PORT_USART0);							//Min_Speed_Lowbyte senden
			uart_putc(Max_Speed_Highbyte,PORT_USART0);							//Max_Speed_Highbyte senden
			uart_putc(Max_Speed_Lowbyte,PORT_USART0);							//Max_Speed_Lowbyte senden
		}

		if (uartinput EQUALS 0xFD) {								//Hex FD = Board Config setzen
			Set_Config();											//Config vom Pc empfangen und im EEPROM speichern
		}

		if (uartinput EQUALS 0xFF) {								//Hex FF = PC Mode Schleife verlassen und zu Offline Motion Detector wechseln
			break;													//Schleife verlassen
		}
	}
	return;
}

void Read_Config(void){
	Threshold = eeprom_read_byte((uint8_t *)0x01);								//Schwellwert aus EEPROM lesen und in Variable übergeben
	Samples = eeprom_read_byte((uint8_t *)0x02);								//Samples aus EEPROM lesen und in Variable übergeben
	Mode = eeprom_read_byte((uint8_t *)0x03);									//Mode aus EEPROM lesen und in Variable übergeben
	ADC_Prescaler = eeprom_read_byte((uint8_t *)0x04);							//ADC_Prescaler aus EEPROM lesen und in Variable übergeben
	IOB6 = eeprom_read_byte((uint8_t *)0x05);									//IOB6 aus EEPROM lesen und in Variable übergeben
	IOB7 = eeprom_read_byte((uint8_t *)0x06);									//IOB7 aus EEPROM lesen und in Variable übergeben
	LED = eeprom_read_byte((uint8_t *)0x07);									//LED aus EEPROM lesen und in Variable übergeben
	UBRR_VAL = eeprom_read_byte((uint8_t *)0x08);								//uart Baudrate aus EEPROM lesen und in Variable übergeben
	Motion_Delay_Highbyte = eeprom_read_byte((uint8_t *)0x09);					//Pause in ms aus EEPROM lesen und in Variable übergeben
	Motion_Delay_Lowbyte = eeprom_read_byte((uint8_t *)0x0A);					//Pause in ms aus EEPROM lesen und in Variable übergeben
	Speed_Filter = eeprom_read_byte((uint8_t *)0x0B);							//Geschwindigkeitsfilter AN/AUS aus EEPROM lesen und in Variable übergeben
	Min_Speed_Highbyte = eeprom_read_byte((uint8_t *)0x0C);						//Geschwindigkeitsfilter Min Highbyte aus EEPROM lesen und in Variable übergeben
	Min_Speed_Lowbyte = eeprom_read_byte((uint8_t *)0x0D);						//Geschwindigkeitsfilter Min Lowbyte aus EEPROM lesen und in Variable übergeben
	Max_Speed_Highbyte = eeprom_read_byte((uint8_t *)0x0E);						//Geschwindigkeitsfilter Max Highbyte aus EEPROM lesen und in Variable übergeben
	Max_Speed_Lowbyte = eeprom_read_byte((uint8_t *)0x0F);						//Geschwindigkeitsfilter Max Lowbyte aus EEPROM lesen und in Variable übergeben
	
	if (Threshold EQUALS 255) {Threshold = 160;}																							//Inhalt prüfen und ggf. Standard Wert setzen
	if (Motion_Delay_Highbyte EQUALS 255 AND Motion_Delay_Lowbyte EQUALS 255) {Motion_Delay_Highbyte = 1; Motion_Delay_Lowbyte = 244;}		//Inhalt prüfen und ggf. Standard Wert setzen (500ms)
	if (Speed_Filter EQUALS 255) {Speed_Filter = 0;}																						//Inhalt prüfen und ggf. Standard Wert setzen
	if (Min_Speed_Highbyte EQUALS 255 AND Min_Speed_Lowbyte EQUALS 255) {Min_Speed_Highbyte = 0; Min_Speed_Lowbyte = 0;}					//Inhalt prüfen und ggf. Standard Wert setzen (0)
	if (Max_Speed_Highbyte EQUALS 255 AND Max_Speed_Lowbyte EQUALS 255) {Max_Speed_Highbyte = 4; Max_Speed_Lowbyte = 0;}					//Inhalt prüfen und ggf. Standard Wert setzen (0)
	if (IOB6 > 1) {IOB6 = 1;}																												//Inhalt prüfen und ggf. Standard Wert setzen
	if (IOB7 > 1) {IOB7 = 1;}																												//Inhalt prüfen und ggf. Standard Wert setzen
	if (LED > 1) {LED = 1;}																													//Inhalt prüfen und ggf. Standard Wert setzen
	if (Samples > 10){Samples = 8;}																											//Inhalt prüfen und ggf. Standard Wert setzen
	if (Mode > 2 OR Mode EQUALS 0){Mode = 1;}																								//Inhalt prüfen und ggf. Standard Wert setzen

	/* Inhalt prüfen und ggf. Standard Wert setzen */
	if(UBRR_VAL EQUALS_NOT 0 AND UBRR_VAL EQUALS_NOT 1 AND UBRR_VAL EQUALS_NOT 3 AND UBRR_VAL EQUALS_NOT 12 AND UBRR_VAL EQUALS_NOT 25 AND UBRR_VAL EQUALS_NOT 51 AND UBRR_VAL EQUALS_NOT 103){
		UBRR_VAL = 25;
	}
	
	if (ADC_Prescaler EQUALS_NOT 2 AND ADC_Prescaler EQUALS_NOT 4 AND ADC_Prescaler EQUALS_NOT 8 AND ADC_Prescaler EQUALS_NOT 16 AND ADC_Prescaler EQUALS_NOT 32 AND ADC_Prescaler EQUALS_NOT 64 AND ADC_Prescaler EQUALS_NOT 128){
		ADC_Prescaler = 128;
	}
		
	Motion_Delay = (Motion_Delay_Highbyte * 256) + Motion_Delay_Lowbyte;		//Pause 16-Bit Wert (0-65535) mit dem Highbyte und Lowbyte berechnen
	Min_Speed = (Min_Speed_Highbyte * 256) + Min_Speed_Lowbyte;					//Minimale Geschwingdigkeit 16-Bit Wert (0-65535) mit dem Highbyte und Lowbyte berechnen
	Max_Speed = (Max_Speed_Highbyte * 256) + Max_Speed_Lowbyte;					//Minimale Geschwingdigkeit 16-Bit Wert (0-65535) mit dem Highbyte und Lowbyte berechnen
	SampleCount = 1 << Samples;													//Anzahl Samples 2^Samples
	
	if (bfly EQUALS_NOT NULL){
		free(bfly);											//RAM Speicher freigeben
	}
				
	if (spectrum EQUALS_NOT NULL){
		free(spectrum);										//RAM Speicher freigeben
	}
	bfly = malloc(sizeof(complex_t)*SampleCount);			//RAM für FFT neu reservieren
	spectrum = malloc(sizeof(complex_t)*(SampleCount /2));	//RAM für FFT neu reservieren
}

void Set_Config(void){
	Mode = uart_wait_getc(PORT_USART0);										//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x03, Mode);						//Mode in EEPROM speichern
	Threshold = uart_wait_getc(PORT_USART0);									//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x01, Threshold);					//Schwellwert in EEPROM speichern
	Samples = uart_wait_getc(PORT_USART0);										//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x02, Samples);					//Samples in EEPROM speichern
	ADC_Prescaler = uart_wait_getc(PORT_USART0);								//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x04, ADC_Prescaler);				//ADC_Prescaler in EEPROM speichern
	IOB6 = uart_wait_getc(PORT_USART0);										//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x05, IOB6);						//IOB6 in EEPROM speichern
	IOB7 = uart_wait_getc(PORT_USART0);										//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x06, IOB7);						//IOB7 in EEPROM speichern
	LED = uart_wait_getc(PORT_USART0);											//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x07, LED);						//LED in EEPROM speichern
	UBRR_VAL = uart_wait_getc(PORT_USART0);									//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x08, UBRR_VAL);					//Baudrate in EEPROM speichern
	Motion_Delay_Highbyte = uart_wait_getc(PORT_USART0);						//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x09, Motion_Delay_Highbyte);		//Highbyte in EEPROM speichern
	Motion_Delay_Lowbyte = uart_wait_getc(PORT_USART0);						//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x0A, Motion_Delay_Lowbyte);		//Lowbyte in EEPROM speichern
	Speed_Filter = uart_wait_getc(PORT_USART0);								//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x0B, Speed_Filter);				//Geschwindigkeitsfilter AN/AUS in EEPROM speichern
	Min_Speed_Highbyte = uart_wait_getc(PORT_USART0);							//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x0C, Min_Speed_Highbyte);			//Highbyte in EEPROM speichern
	Min_Speed_Lowbyte = uart_wait_getc(PORT_USART0);							//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x0D, Min_Speed_Lowbyte);			//Lowbyte in EEPROM speichern
	Max_Speed_Highbyte = uart_wait_getc(PORT_USART0);							//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x0E, Max_Speed_Highbyte);			//Highbyte in EEPROM speichern
	Max_Speed_Lowbyte = uart_wait_getc(PORT_USART0);							//Warten, bis UART Wert übergeben wurde
	eeprom_write_byte((uint8_t *)0x0F, Max_Speed_Lowbyte);			//Lowbyte in EEPROM speichern

	Motion_Delay = (Motion_Delay_Highbyte * 256) + Motion_Delay_Lowbyte;		//Pause 16-Bit Wert (0-65535) mit dem Highbyte und Lowbyte berechnen
	Min_Speed = (Min_Speed_Highbyte * 256) + Min_Speed_Lowbyte;					//Minimale Geschwingdigkeit 16-Bit Wert (0-65535) mit dem Highbyte und Lowbyte berechnen
	Max_Speed = (Max_Speed_Highbyte * 256) + Max_Speed_Lowbyte;					//Minimale Geschwingdigkeit 16-Bit Wert (0-65535) mit dem Highbyte und Lowbyte berechnen
	SampleCount = 1 << Samples;													//Anzahl Samples 2^Samples

	if (bfly EQUALS_NOT NULL){
		free(bfly);											//RAM Speicher freigeben
	}
	
	if (spectrum EQUALS_NOT NULL){
		free(spectrum);										//RAM Speicher freigeben
	}
	bfly = malloc(sizeof(complex_t)*SampleCount);			//RAM für FFT neu reservieren
	spectrum = malloc(sizeof(complex_t)*(SampleCount /2));	//RAM für FFT neu reservieren
	
	uart_init(UBRR_VAL, PORT_USART0);									//Init UART
	ADC_Init(ADC_Prescaler);										//Init ADC
}

void delay_ms(uint16_t count) {
	while(count--) {
		_delay_ms(1);
	}
}

void delay_us(uint16_t count) {
	while(count--) {
		_delay_us(1);
	}
}

float getKhmFaktor(uint16_t SampleCount, uint8_t ADC_Prescaler){
	
	if (SampleCount EQUALS 1024){
		if (ADC_Prescaler EQUALS 2) {return 0.38f;}		// 8 MHz
		if (ADC_Prescaler EQUALS 4) {return 0.46f;}		// 4 MHz
		if (ADC_Prescaler EQUALS 8) {return 0.61f;}		// 2 MHz
		if (ADC_Prescaler EQUALS 16) {return 0.9f;}		// 1 MHz
		if (ADC_Prescaler EQUALS 32) {return 1.53f;}	// 500 KHz
		if (ADC_Prescaler EQUALS 64) {return 2.71f;}	// 250 KHz
		if (ADC_Prescaler EQUALS 128) {return 5.04f;}	// 125 KHz
	}
	
	if (SampleCount EQUALS 512){
		if (ADC_Prescaler EQUALS 2) {return 0.21f;}		// 8 MHz
		if (ADC_Prescaler EQUALS 4) {return 0.24f;}		// 4 MHz
		if (ADC_Prescaler EQUALS 8) {return 0.31f;}		// 2 MHz
		if (ADC_Prescaler EQUALS 16) {return 0.46f;}	// 1 MHz
		if (ADC_Prescaler EQUALS 32) {return 0.76f;}	// 500 KHz
		if (ADC_Prescaler EQUALS 64) {return 1.35f;}	// 250 KHz
		if (ADC_Prescaler EQUALS 128) {return 2.52f;}	// 125 KHz
	}
	       
	if (SampleCount EQUALS 256){
		if (ADC_Prescaler EQUALS 2) {return 0.11f;}		// 8 MHz
		if (ADC_Prescaler EQUALS 4) {return 0.14f;}		// 4 MHz
		if (ADC_Prescaler EQUALS 8) {return 0.17f;}		// 2 MHz
		if (ADC_Prescaler EQUALS 16) {return 0.24f;}	// 1 MHz
		if (ADC_Prescaler EQUALS 32) {return 0.39f;}	// 500 KHz
		if (ADC_Prescaler EQUALS 64) {return 0.68f;}	// 250 KHz
		if (ADC_Prescaler EQUALS 128) {return 1.26f;}	// 125 KHz
	}
	
	if (SampleCount EQUALS 128){
		if (ADC_Prescaler EQUALS 2) {return 0.07f;}		// 8 MHz
		if (ADC_Prescaler EQUALS 4) {return 0.08f;}		// 4 MHz
		if (ADC_Prescaler EQUALS 8) {return 0.1f;}		// 2 MHz
		if (ADC_Prescaler EQUALS 16) {return 0.13f;}	// 1 MHz
		if (ADC_Prescaler EQUALS 32) {return 0.2f;}		// 500 KHz
		if (ADC_Prescaler EQUALS 64) {return 0.34f;}	// 250 KHz
		if (ADC_Prescaler EQUALS 128) {return 0.63f;}	// 125 KHz
	}
	
	if (SampleCount EQUALS 64){
		if (ADC_Prescaler EQUALS 2) {return 0.06f;}		// 8 MHz
		if (ADC_Prescaler EQUALS 4) {return 0.06f;}		// 4 MHz
		if (ADC_Prescaler EQUALS 8) {return 0.07f;}		// 2 MHz
		if (ADC_Prescaler EQUALS 16) {return 0.8f;}		// 1 MHz
		if (ADC_Prescaler EQUALS 32) {return 0.12f;}	// 500 KHz
		if (ADC_Prescaler EQUALS 64) {return 0.18f;}	// 250 KHz
		if (ADC_Prescaler EQUALS 128) {return 0.32f;}	// 125 KHz
	}
	
	if (SampleCount EQUALS 32){
		if (ADC_Prescaler EQUALS 2) {return 0.05f;}		// 8 MHz
		if (ADC_Prescaler EQUALS 4) {return 0.06f;}		// 4 MHz
		if (ADC_Prescaler EQUALS 8) {return 0.06f;}		// 2 MHz
		if (ADC_Prescaler EQUALS 16) {return 0.06f;}	// 1 MHz
		if (ADC_Prescaler EQUALS 32) {return 0.07f;}	// 500 KHz
		if (ADC_Prescaler EQUALS 64) {return 0.11f;}	// 250 KHz
		if (ADC_Prescaler EQUALS 128) {return 0.17f;}	// 125 KHz
	}

	if (SampleCount EQUALS 16){
		if (ADC_Prescaler EQUALS 2) {return 0.05f;}		// 8 MHz
		if (ADC_Prescaler EQUALS 4) {return 0.05f;}		// 4 MHz
		if (ADC_Prescaler EQUALS 8) {return 0.05f;}		// 2 MHz
		if (ADC_Prescaler EQUALS 16) {return 0.05f;}	// 1 MHz
		if (ADC_Prescaler EQUALS 32) {return 0.06f;}	// 500 KHz
		if (ADC_Prescaler EQUALS 64) {return 0.07f;}	// 250 KHz
		if (ADC_Prescaler EQUALS 128) {return 0.1f;}	// 125 KHz
	}

	if (SampleCount EQUALS 8){
		if (ADC_Prescaler EQUALS 2) {return 0.05f;}		// 8 MHz
		if (ADC_Prescaler EQUALS 4) {return 0.05f;}		// 4 MHz
		if (ADC_Prescaler EQUALS 8) {return 0.05f;}		// 2 MHz
		if (ADC_Prescaler EQUALS 16) {return 0.05f;}	// 1 MHz
		if (ADC_Prescaler EQUALS 32) {return 0.05f;}	// 500 KHz
		if (ADC_Prescaler EQUALS 64) {return 0.06f;}	// 250 KHz
		if (ADC_Prescaler EQUALS 128) {return 0.07f;}	// 125 KHz
	}

	return 1.0f;
}

