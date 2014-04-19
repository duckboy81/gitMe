#include <msp430g2553.h>
#include "commonInclude.h"
#include "radioHAM.h"
#include "gpsModule.h"

/*
 * main.c
 */

/*
 * LMS
 * 	Tap: 56
 * 	Delay: 0
 * 	2u: 0.003
 * 	Gm: 0.9999
 *
 * BPF
 * 	Tap: 80
 * 	FW: 100
 */

int main(void) {
    WDTCTL = WDTPW | WDTHOLD;	// Stop watchdog timer

	initializeHAMRadio();
	initializeGPS();

	while(!isGPSFinished()) {

		if (processGPSData()) {

			sendGPSStringHAM(0x4D2);
//			//signedPrintString("\n OK!\n");
//			 if (isHAMReady()) {
//				//signedPrintString("Waiting for GPS Fix \n");
//				 sendHAMString("Waiting for GPS Fix", 0x4D2, 'G');
//				//sendHAMString("Waiting for GPS Fix \n");
//			 } //if()
		} //if()
	} //while()

	while(!isHAMReady());
	sendHAMString("HAM STRING DONE!", 0x4D2, 'G');

	while(!isHAMReady());
	sendGPSStringHAM(0x4D2);

} //main()
