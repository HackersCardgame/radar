#include "uart.h"

#include <avr/interrupt.h>
#include "globals.h"

void uart_init(uint32_t UBRR_VAL, uart_port_t port)
{
	//UBRR_VAL =0;						//Baudrate 1.000.000
	//UBRR_VAL =1;						//Baudrate 500.000
	//UBRR_VAL =3;						//Baudrate 250.000
	//UBRR_VAL =12;						//Baudrate 76.800
	//UBRR_VAL =25;						//Baudrate 38.400
	//UBRR_VAL =51;						//Baudrate 19.200
	//UBRR_VAL =103;					//Baudrate 9.600
	
	if(port EQUALS PORT_USART0){
		UCSR0B |= (1<<RXEN0);  // UART RX einschalten
		UCSR0B |= (1<<TXEN0);  // UART TX einschalten
		UCSR0C |= (1<<UCSZ01)|(1<<UCSZ00);  // Asynchron 8N1
		// UCSR0C = 3<<UCSZ00;
		UBRR0H = UBRR_VAL >> 8;
		UBRR0L = UBRR_VAL & 0xFF;
	}
	
	if(port EQUALS PORT_USART1){
		UCSR1B |= (1<<RXEN1);  // UART RX einschalten
		UCSR1B |= (1<<TXEN1);  // UART TX einschalten
		UCSR1C |= (1<<UCSZ11)|(1<<UCSZ10);  // Asynchron 8N1
	
		UBRR1H = UBRR_VAL >> 8;
		UBRR1L = UBRR_VAL & 0xFF;
	}	
}

void uart_putc(char c,uart_port_t port)
{
    if (port EQUALS PORT_USART0){
		while (!(UCSR0A & (1<<UDRE0)))  /* warten bis Senden moeglich */
		{
		}
		UDR0 = c;                      /* sende Zeichen */
	}
	
	if (port EQUALS PORT_USART1){
		while (!(UCSR1A & (1<<UDRE1)))  /* warten bis Senden moeglich */
		{
		}
		UDR1 = c;                      /* sende Zeichen */
	}
}
  
/* puts ist unabhaengig vom Controllertyp */
void uart_puts (const char* s, uart_port_t port)
{
	while (*s)
    {   /* so lange *s != '\0' also ungleich dem "String-Endezeichen" */
        uart_putc(*s,port);
        s++;
    }
}

unsigned char USART_RX(uart_port_t port)
{
	if (port EQUALS PORT_USART0){
		while(!(UCSR0A&(1<<RXC0)));
		return UDR0;
	}

	if (port EQUALS PORT_USART1){
		while(!(UCSR1A&(1<<RXC1)));
		return UDR1;
	}

	return 0;
}
 
void uart_gets( char* Input, uart_port_t port)
{
  char c = USART_RX(port);

  while( c != '\n' ) {
    *Input = c;
    Input++;
    c = USART_RX(port);
    //uart_putc( c );
  }
  *Input = '\0';
}

int uart_getc(uart_port_t port)
{
	if (port EQUALS PORT_USART0){
		if (!(UCSR0A & (1<<RXC0))){
			return 0;
		}
		else{
			return UDR0;                   // Zeichen aus UDR an Aufrufer zurueckgeben
		}
	}

	if (port EQUALS PORT_USART1){
		if (!(UCSR1A & (1<<RXC1))){
			return 0;
		}
		else{
			return UDR1;                   // Zeichen aus UDR an Aufrufer zurueckgeben
		}
	}
	return 0;
}

int uart_wait_getc(uart_port_t port)
{
	if (port EQUALS PORT_USART0){
		while (!(UCSR0A & (1<<RXC0)));   // warten bis Zeichen verfuegbar
		return UDR0;                   // Zeichen aus UDR an Aufrufer zurueckgeben
	}
	
	if (port EQUALS PORT_USART1){
		while (!(UCSR1A & (1<<RXC1)));   // warten bis Zeichen verfuegbar
		return UDR0;                   // Zeichen aus UDR an Aufrufer zurueckgeben
	}
	return 0;
}




