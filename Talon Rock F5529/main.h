/*
 * main.h
 *
 *  Created on: Mar 6, 2014
 *      Author: C14JeanLuc.Duckworth
 */

#ifndef MAIN_H_
#define MAIN_H_

/*
 * Initializes interrupts:
 * 	-UART: XBee
 * 	-UART: GPS Rx
 * 	-GPIO: Detection Bit
 * 	-Timer: Queued message sender
 * 	-Timer: Manual AFSK bit switch
 */
void main_initialize(void);
void FAULT_ISR(void);

#endif /* MAIN_H_ */
