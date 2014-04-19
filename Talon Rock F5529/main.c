/*
 * main.c
 *
 *  Created on: Mar 6, 2014
 *      Author: C14JeanLuc.Duckworth
 */

#include "commonInclude.h"
#include "main.h"
#include "gpsModule.h"
#include "raspberryPI.h"
#include "XBeeModule.h"
#include "exfilRadio.h"

int _system_pre_init(void) {

  WDTCTL = WDTPW + WDTHOLD;	//Disable watchdog timer

  return 1;

} //_system_pre_init

int main(void) {

	main_initialize();

//	addMessageQueue(GPS_MESSAGE, "bell curves for days");
//	sendMessage(0x0013A20040B2C0D2, "OKAY");
//	sendMessage(XBEE_BROADCAST_ADDR, "OKAY");

	//Use a combination of polling and power savings to handle incoming data
	while(TRUE) {
		if (isGPSOn()) {
			handleGPSData();
		} //if()

#if !EXFIL_NODE
		handleRaspberryPI();
#endif

		//Handle messages to send
		handleMessageQueue();

#if EXFIL_NODE
		//Handle nodes in queue -- exfilRadio
		handleExfilQueue();
#endif

		__bis_SR_register(LPM0_bits + GIE);       // Enter LPM0, interrupts enabled
	} //while()

} //main()

void main_initialize(void) {

#if EXFIL_NODE
	initializeExfilRadio();
#endif

	initRealTimeClock();
	initXbee();
	initializeGPS();

} //main_initialize()

//Fault interrupt
#pragma vector=SYSNMI_VECTOR
__interrupt void FAULT_ISR(void) {
	__no_operation();
} //FAULT_ISR()
