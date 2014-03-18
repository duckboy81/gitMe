/*
 * gpsModule.c
 *
 *  Created on: Mar 6, 2014
 *      Author: C14JeanLuc.Duckworth
 *
 * This module will establish and continue to report GPS location until a specified
 * number of GPS coordinates come through with at least a three satellite lock.
 *
 */

#include "commonInclude.h"
#include "gpsModule.h"

gpsStore mainGPS;

void initializeGPS(void) {

	P8DIR |= BIT1;	//GPIO direction output
	P8OUT &= ~BIT1;	//Bring 8.1 down

	P4SEL |= BIT4 + BIT5;	//P4.4,5 = USCI_A1 TXD/RXD

	UCA1CTL1 |= UCSWRST;
	UCA1CTL1 |= UCSSEL_2;	//SMCLK - Should run at 1,048,576Hz

	//UCA1BR = 109 = UCA1BR0 + UCA1BRA1 (Baud rate of 9600)
	UCA1BR0 = 109;
	UCA1BR1 = 0;

	UCA1MCTL |= UCBRS_2 + UCBRF_0;
	UCA1CTL1 &= ~UCSWRST;	//Initialize USCI_A1 state machine

	strcpy(mainGPS.fix_quality, "0");
	strcpy(mainGPS.num_satellites, "00");

	turnOnGPS();

} //initializeGPS()

//Sifts through data and breaks necessary data into separate arrays
char handleGPSData(void) {

	char i, temp_GPGGAinfo[MAX_GPS_BUFFER_LENGTH];
	char* token_pointer;

	if (mainGPS.GPGGAflag) {
		//Increment second counter
		mainGPS.secLapsed++;

		//Copy data into new string
		strcpy(temp_GPGGAinfo, mainGPS.GPGGAinfo);

		//Determine fix information
		token_pointer = strtok(temp_GPGGAinfo, ",");
		for(i=1;i<7;i++) {
			token_pointer = strtok(NULL, ",");
		} //for()

		//Check for valid fix value of 1 or 2
//		if (!token_pointer ||  (*token_pointer != '1' && *token_pointer != '2')) {
		if (token_pointer &&  (*token_pointer == '1' || *token_pointer == '2')) {

			//Copy data into new string (again because the first
			// strtok cuts up the sring) (this also prevents overwriting
			// good information with poor information, should the GPS
			// lose a previously-established lock)
			strcpy(temp_GPGGAinfo, mainGPS.GPGGAinfo);

			//Re-enable GPS interrupts
	//		UCA1IE |= UCRXIE; //Enable USCI_A1 RX interrupt
			UCA1CTL1 &= ~UCDORM;

			//Reset counter
			i = 0;

			token_pointer = strtok(temp_GPGGAinfo, ",");
			while(token_pointer) {

				//Place data in correct place in gps structure
				switch(i) {
				case 1://Timedate
					strcpy(mainGPS.zuluTime, token_pointer);
					break;
				case 2://Latitude
					strcpy(mainGPS.latitude, token_pointer);
					break;
				case 3://Lat-Direction
					strcpy(mainGPS.latitude_dir, token_pointer);
					break;
				case 4://Longitude
					strcpy(mainGPS.longitude, token_pointer);
					break;
				case 5://Lng-Direction
					strcpy(mainGPS.longitude_dir, token_pointer);
					break;
				case 6: //GPS Fix quality
					strcpy(mainGPS.fix_quality, token_pointer);
					break;
				case 7: //GPS Num Satellites
					strcpy(mainGPS.num_satellites, token_pointer);
					break;
				default:
					break;
				} //switch()

				token_pointer = strtok(NULL, ",");
				i++;
			} //while()

			mainGPS.acceptableLockCount++;
		} //if()
	} //if()

	//Handle finished GPS
	if (!isGPSFinished()) {
		//	UCA1IE |= UCRXIE; //Enable USCI_A1 RX interrupt
		UCA1CTL1 &= ~UCDORM;
	} //if()

	return TRUE;
} //handleGPSData

void compileGPSToString() {

	char lengthOfString;

	lengthOfString = strlen(mainGPS.zuluTime)
		+	strlen(mainGPS.latitude)
		+	strlen(mainGPS.latitude_dir)
		+ 	strlen(mainGPS.longitude)
		+	strlen(mainGPS.longitude_dir)
		+ 	strlen(mainGPS.fix_quality)
		+ 	strlen(mainGPS.num_satellites);

	//Clear any used data
	if (gpsPositionString) {
		free(gpsPositionString);
	} //if()

	gpsPositionString = malloc(lengthOfString * sizeof(char) + 7);

	//Copy in the data
 	strcpy(gpsPositionString, mainGPS.zuluTime);
	strcat(gpsPositionString, ",");
	strcat(gpsPositionString, mainGPS.latitude);
	strcat(gpsPositionString, ",");
	strcat(gpsPositionString, mainGPS.latitude_dir);
	strcat(gpsPositionString, ",");
	strcat(gpsPositionString, mainGPS.longitude);
	strcat(gpsPositionString, ",");
	strcat(gpsPositionString, mainGPS.longitude_dir);
	strcat(gpsPositionString, ",");
	strcat(gpsPositionString, mainGPS.fix_quality);
	strcat(gpsPositionString, ",");
	strcat(gpsPositionString, mainGPS.num_satellites);
} //gpsToString()

char isGPSFinished() {
	if (mainGPS.acceptableLockCount > MIN_GPS_LOCK_CYCLES || mainGPS.secLapsed > MAX_GPS_LOCK_TIME) {
		turnOffGPS();

		//Setup GPS output string
		compileGPSToString();

		//Send coordinates to EXFIL node
		addMessageQueue(&topQueuedMessage, GPS_MESSAGE, gpsPositionString);

		return TRUE;
	} //if()

	return FALSE;
} //isGPSLocked()

void turnOnGPS() {
	P8OUT &= ~BIT1;

	mainGPS.acceptableLockCount = 0;
	mainGPS.secLapsed = 0;
	mainGPS.bufferPosition = -1;
	mainGPS.bufferRemaining = -1;
	mainGPS.isOn = 1;

	UCA1IE |= UCRXIE; //Enable USCI_A1 RX interrupt
	UCA1CTL1 &= ~UCDORM;
} //turnOnGPS()

void turnOffGPS() {

//	UCA1IE &= ~UCRXIE; //Disable USCI_A1 RX interrupt
	UCA1CTL1 |= UCDORM;
	P8OUT |= BIT1;
	mainGPS.isOn = 0;
} //turnOffGPS()

char isGPSOn() {
	return mainGPS.isOn;
}

#pragma vector=USCI_A1_VECTOR
__interrupt void USCI1RX_ISR(void) {

	char availableData;

	//See UCAxIV, Interrupt Vector Generator desc. (pg 997)
	switch(__even_in_range(UCA1IV, 4)) {
	case 0: break;		//Vector 0: No interrupts
	case 2:				//Vector 2: UCRXIFG

		availableData = (mainGPS.GPGGAinfo[4] == 'G' && mainGPS.GPGGAinfo[5] == 'A');

		//Check for delimiter
		if (UCA1RXBUF == '$') {
			mainGPS.bufferPosition = 0;
			mainGPS.bufferRemaining = -1;
		} //if()

		//Check for near-end of tx
		if (UCA1RXBUF == '*') {
			mainGPS.bufferRemaining = 3;
		} //if()

		//Ensure we are getting $GPGGA info and not anything else
		if (mainGPS.bufferPosition <= 5 || (mainGPS.bufferPosition > 5 && availableData == GPGGA)) {

			//Add data to buffer
			if (mainGPS.bufferPosition >= 0 && mainGPS.bufferPosition < (MAX_GPS_BUFFER_LENGTH - 1)) {
				mainGPS.GPGGAinfo[mainGPS.bufferPosition] = UCA1RXBUF;
				mainGPS.bufferPosition++;
				mainGPS.bufferRemaining--;
			} //if()
		} //if()

		//Handle completed buffer
		if (mainGPS.bufferRemaining == 0) {
			mainGPS.GPGGAinfo[mainGPS.bufferPosition] = '\0';

			if (availableData == GPGGA) {
				//Polling will be re-enabled as the power state of the CPU
				// is now active.  Set a flag for the poll to look for.
				mainGPS.GPGGAflag = 1;

				//Disable USCI_A1 RX interrupt to prevent any memory collisions
//				UCA1IE &= ~UCRXIE;
				UCA1CTL1 |= UCDORM;

				//Wake CPU
				__bic_SR_register_on_exit(LPM0_bits);
			} //if()

			mainGPS.bufferRemaining = -1;
		} //if()
	case 4: break;		//Vector 4: UXTXIFG
	default: break;
	} //switch()
} //USCI1RX_ISR()
