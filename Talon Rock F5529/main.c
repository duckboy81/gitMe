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

#if EXFIL_NODE

#include "exfilRadio.h"

#else

#include "raspberryPI.h"

#endif

int _system_pre_init(void) {

  WDTCTL = WDTPW + WDTHOLD;	//Disable watchdog timer

  return 1;

} //_system_pre_init

int main(void) {

	main_initialize();

	//Use a combination of polling and power savings to handle incoming GPS data
	while(TRUE) {
		if (isGPSOn()) {
			handleGPSData();
		} //if()

#if !EXFIL_NODE
		handleRaspberryPI();
#endif

		sendHAMString("TEST", 1234);

		__bis_SR_register(LPM0_bits + GIE);       // Enter LPM0, interrupts enabled
	} //while()

} //main()

void main_initialize(void) {

#if EXFIL_NODE
	initializeExfilRadio();
#else
	initRaspberryPI();
#endif

	//init_xbee();
	//initializeGPS();


} //main_initialize()
