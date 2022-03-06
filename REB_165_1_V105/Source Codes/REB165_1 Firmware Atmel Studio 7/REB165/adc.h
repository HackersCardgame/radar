/*
 * adc.h
 *
 * Created: 21.03.2017 11:41:50
 *  Author: Author: Sebastian Weidmann www.weidmann-elektronik.de
 */ 


#ifndef ADC_H_
#define ADC_H_

void ADC_Init(uint8_t Adc_Prescaler);								//ADC Initialisierung
uint16_t ADC_Read( uint8_t channel );								//ADC Wert lesen
uint16_t ADC_Read_Avg( uint8_t channel, uint8_t average );			//ADC Wert Mittelwert lesen

#endif /* ADC_H_ */