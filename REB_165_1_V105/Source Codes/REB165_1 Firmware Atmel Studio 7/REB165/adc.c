/*
 * adc.c
 *
 * Created: 26.03.2011 00:14:34
 *   Author: Sebastian Weidmann www.weidmann-elektronik.de
 */ 
/* ADC initialisieren */

#include <avr/io.h>

void ADC_Init( uint8_t Prescaler) {
  ADMUX = (0<<REFS1) | (1<<REFS0);      // AVcc als Referenz benutzen

  if (Prescaler == 2) {ADCSRA = (0<<ADPS2 | 0<<ADPS1 | 1<<ADPS0);}		//Frequenzvorteiler setzen 8 MHz
  if (Prescaler == 4) {ADCSRA = (0<<ADPS2 | 1<<ADPS1 | 0<<ADPS0);}		//Frequenzvorteiler setzen 4 MHz
  if (Prescaler == 8) {ADCSRA = (0<<ADPS2 | 1<<ADPS1 | 1<<ADPS0);}		//Frequenzvorteiler setzen 2 MHz
  if (Prescaler == 16) {ADCSRA = (1<<ADPS2 | 0<<ADPS1 | 0<<ADPS0);}		//Frequenzvorteiler setzen 1 MHz
  if (Prescaler == 32) {ADCSRA = (1<<ADPS2 | 0<<ADPS1 | 1<<ADPS0);}		//Frequenzvorteiler setzen 500 KHz
  if (Prescaler == 64) {ADCSRA = (1<<ADPS2 | 1<<ADPS1 | 0<<ADPS0);}		//Frequenzvorteiler setzen 250 KHz
  if (Prescaler == 128) {ADCSRA = (1<<ADPS2 | 1<<ADPS1 | 1<<ADPS0);}	//Frequenzvorteiler setzen 125 KHz
 
  ADCSRA |= (1<<ADEN);                  // ADC aktivieren
 
  /* nach Aktivieren des ADC wird ein "Dummy-Readout" empfohlen, man liest
     also einen Wert und verwirft diesen, um den ADC "warmlaufen zu lassen" */
 
  ADCSRA |= (1<<ADSC);                  // eine ADC-Wandlung 
  while (ADCSRA & (1<<ADSC) ) {}        // auf Abschluss der Konvertierung warten
  /* ADCW muss einmal gelesen werden, sonst wird Ergebnis der nächsten
     Wandlung nicht übernommen. */
}
 
/* ADC Einzelmessung */
uint16_t ADC_Read( uint8_t channel )
{
  // Kanal waehlen, ohne andere Bits zu beeinflußen
  ADMUX = (ADMUX & ~(0x1F)) | (channel & 0x1F);
  ADCSRA |= (1<<ADSC);            // eine Wandlung "single conversion"
  while (ADCSRA & (1<<ADSC) ) {}  // auf Abschluss der Konvertierung warten
  return ADCW;                    // ADC auslesen und zurückgeben
}
 
 /* ADC Mehrfachmessung mit Mittelwertbbildung */
uint16_t ADC_Read_Avg( uint8_t channel, uint8_t average )
{
  uint32_t result = 0;
 uint8_t i;
 
  for (i = 0; i < average; ++i )
    result += ADC_Read( channel );
 
  return (uint16_t)( result / average );
}