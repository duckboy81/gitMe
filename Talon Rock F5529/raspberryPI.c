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

//Reports the time since last actual reported hit, as seen by the system
unsigned int raspberryPISensorTripTimer = 0;

//These two variables attempt to prevent false positives
//	by ensuring there were at least X number of hits in the
//	last YY seconds before triggering a valid hit response.
unsigned int raspberryPITempHitCounter = 0;
unsigned int raspberryPITimeSinceHit = 0;

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

	/* Check to see if the time since last hit is more than allowable
	 * 	if it is, reset the hit counter.
	 */
	if (raspberryPITimeSinceHit >= MAX_RASP_PI_TIME_BETWEEN_HITS) {
		//Reset hit counter
		raspberryPITempHitCounter = 0;
	} //if()


	//Increment hit counter
	raspberryPITempHitCounter++;

	/* If there has been enough hits within a certain amount of time,
	 * 	and we haven't sent a detection notice within the last X amount
	 * 	of seconds, register a detection.
	 */
	if (raspberryPITempHitCounter > MIN_RASP_PI_HITS_IN_TIME &&
			raspberryPITimeSinceHit < MAX_RASP_PI_TIME_BETWEEN_HITS &&
			raspberryPISensorTripTimer >= MIN_RASP_PI_WAIT_BETWEEN_DETECTIONS) {

		//Add message to message queue
		addMessageQueue(DETECT_MESSAGE, "");

		//Reset counters
		raspberryPISensorTripTimer = 0;
		raspberryPITempHitCounter = 0;
	} //if()

	//Reset time since last hit counter
	raspberryPITimeSinceHit = 0;

	P1IFG = 0; //Clear the IFG
} //Port_1()
