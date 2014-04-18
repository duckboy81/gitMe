/*
 * raspberryPI.c
 *
 *  Created on: Mar 11, 2014
 *      Author: C14JeanLuc.Duckworth
 */

#include "commonInclude.h"
#include "raspberryPI.h"

unsigned int raspberryPISec = 0;
unsigned int raspberryPIEnabled = 0;
unsigned int raspberryPISensorTripTimer = 0;

void handleRaspberryPI() {

	if (!raspberryPIEnabled) {
		if (raspberryPISec > MIN_RASP_PI_WAIT) {
			//Activate LED
			P1DIR |= BIT0;//Set LEDs to output
			P1OUT |= BIT0;//Toggle LEDs

			//Enable raspberry pi
			P1DIR &= ~BIT6;                           // Set P1.6 to input direction
			P1IES &= ~BIT6;                           // P1.6 Lo->Hi edge
			P1IFG &= ~BIT6;                           // P1.6 IFG cleared
			P1IE |= BIT6;                             // P1.6 interrupt enabled

			raspberryPIEnabled = TRUE;
		} //if()
	} else {
		//TODO: (If time develop this) Do some watch dog type stuff
	} //if-else()
} //handleRaspberryPI()

// Port 1 interrupt service routine
#pragma vector=PORT1_VECTOR
__interrupt void Port_1(void)
{
	//Toggle an LED
	P1OUT ^= BIT0;

	if (raspberryPISensorTripTimer >= MIN_RASP_PI_WAIT_BETWEEN_DETECTIONS) {
		//Add message to message queue
		addMessageQueue(DETECT_MESSAGE, "");

		raspberryPISensorTripTimer = 0;
	} //if()

	P1IFG = 0; //Clear the IFG
} //Port_1()
