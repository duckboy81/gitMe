#include <msp430g2553.h>
#include "commonInclude.h"
#include "radioHAM.h"
#include "gpsModule.h"

/*
 * main.c
 */

//TODO: ALWAYS SEND FIG/LTR HAM CODE FIRST!
//TODO: ENCODE CHECKSUM
//TODO: MOVE FIG/LTR CHECK INTO INTERRUPT

int main(void) {
    WDTCTL = WDTPW | WDTHOLD;	// Stop watchdog timer

	initializeHAMRadio();
	initializeGPS();

	while(!isGPSFinished()) {

		if (processGPSData()) {

			signedPrintString("\n OK!\n");
			 if (isHAMReady()) {
				//signedPrintString("Waiting for GPS Fix \n");
				 sendHAMString("Waiting for GPS Fix", 0123, 'G');
				//sendHAMString("Waiting for GPS Fix \n");
			 } //if()
		} //if()
	} //while()

	while(!isHAMReady());
	sendHAMString("HAM STRING DONE!", 0123, 'G');

} //main()
