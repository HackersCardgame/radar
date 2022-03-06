/*
 * uart.h
 *
 * Created: 21.03.2017 11:41:50
 *  Author: Sebastian Weidmann www.weidmann-elektronik.de
 */ 

#ifndef F_CPU
#define F_CPU 16000000UL													// Controller CPU Frequenz
#endif

#ifndef UART_H_
#define UART_H_

typedef enum {
	PORT_USART0,
	PORT_USART1
}uart_port_t;

#include "stdint-gcc.h"

void uart_init(uint32_t UBRR_VAL, uart_port_t port);		//UART Initialisierung
void uart_puts (const char *s, uart_port_t port);			//UART String senden
void uart_gets(char* Input, uart_port_t port);				//UART String empfangen
void uart_putc(char c, uart_port_t port);					//UART Char senden
int uart_wait_getc(uart_port_t port);						//UART warten bis Char empfangen wurde
int uart_getc(uart_port_t port);							//UART Char empfangen
unsigned char USART_RX(uart_port_t port);

#endif /* UART_H_ */