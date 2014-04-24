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

//WDTPW		-- Just the PW for configuring the WDT
//WDTSSEL0  -- sets WDT timer clock source to ACLK
//WDTCNTCL	-- Resets the WDT counter
//WDTIS0 + WDTIS1	-- Sets the interval select to (WDT CLK)/(2^19) 3.4375*16seconds
//WDTIS2	-- Sets the interval select to (WDT CLK)/(2^15) ~3.4375*1 seconds

#define WDT_FEED WDTPW + WDTSSEL0 + WDTCNTCL + WDTIS0 + WDTIS1 //Safer option, approx 60 sec WDT
//#define WDT_FEED WDTPW + WDTSSEL0 + WDTCNTCL + WDTIS2

int _system_pre_init(void) {

  WDTCTL = WDT_FEED + WDTHOLD;	//Configure and hold watchdog timer
  return 1;

} //_system_pre_init

int main(void) {

	main_initialize();

//	addMessageQueue(GPS_MESSAGE, "bell curves for days");
//	sendMessage(0x0013A20040B2C0D2, "OKAY");
//	sendMessage(XBEE_BROADCAST_ADDR, "OKAY");
//	sendMessage(EXFIL_XBEE_ADDR, "+");

	//Use a combination of polling and power savings to handle incoming data
	while(TRUE) {
		WDTCTL = WDT_FEED; //Feed the dog

		if (isGPSOn()) {
			handleGPSData();
		} //if()

#if !EXFIL_NODE
		handleRaspberryPI();
#endif

		//Handle messages to send
		handleMessageQueue();

#if EXFIL_NODE

		//Just a backup to make sure the PTT does not get stuck on
		if (exfilObject.ready_to_send) {
			disablePTT();
		} //if()

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
