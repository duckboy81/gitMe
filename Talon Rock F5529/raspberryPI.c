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

void initRaspberryPI(void) {

	//Setup TimerA0 to toggle at one second
	TA0CCR0 = 32768-1;
	TA0CTL = TASSEL_1 + MC_1 + TACLR;
	TA0CCTL0 |= CCIE;

} //initRaspberryPI()

void handleRaspberryPI() {

	if (!raspberryPIEnabled) {
		if (raspberryPISec > MIN_RASP_PI_WAIT) {
			//Disable timer interrupt
			//TODO: Does this work? Does it interfere with other bits on TACTL?
			TA0CTL = MC_0;
			TA0CCTL0 &= ~CCIE;

			//Enable raspberry pi
			P1DIR &= ~BIT6;                           // Set P1.6 to input direction
			P1IES &= ~BIT6;                           // P1.6 Lo->Hi edge
			P1IFG &= ~BIT6;                           // P1.6 IFG cleared
			P1IE |= BIT6;                             // P1.6 interrupt enabled
		} //if()
	} else {
		//TODO: (Backburner) Do some watch dog type stuff
	} //if-else()
} //handleRaspberryPI()

// Port 1 interrupt service routine
#pragma vector=PORT1_VECTOR
__interrupt void Port_1(void)
{
	switch(P1IFG) {
	case BIT6:
		//Add message to message queue
		addMessageQueue(&topQueuedMessage, DETECT_MESSAGE, "");
		break;
	default: break;
	} //switch()


	P1IFG &= ~BIT6;                         // Clear P1.6 IFG
} //Port_1()

// Timer A0 interrupt service routine
#pragma vector=TIMER0_A0_VECTOR
__interrupt void Timer_A(void)
{
	//Add to timer
	raspberryPISec++;

	//Wake CPU
	__bic_SR_register_on_exit(LPM0_bits);
} //Timer_A()
