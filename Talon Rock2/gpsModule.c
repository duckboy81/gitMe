#include "msp430g2553.h"
#include "commonInclude.h"
#include "gpsModule.h"
#include "radioHAM.h"

/*
 * This module will establish and continue to report GPS location until a specified
 * number of GPS coordinates come through with at least a three satellite lock.
 *
 */

//#define MAX_GPS_LOCK_TIME 600 //Time in seconds (defined in commonInclude.h)

//Declare variables
/* SORRY FOR THE CRAPPY METHODOLOGY IN PUTTING THESE VARIABLES TOGETHER.
 * COMBINING THE VARIABLES CAUSED THE uCONTROLLER TO CONSTANTLY RESET.
 */
static unsigned char GPGGAinfo[MAX_GPS_BUFFER_LENGTH];
static unsigned char gpsFixStatus = 0;
static char GPGGAflag = 0;
static unsigned int acceptableLockCount = 0;
static signed char bufferPosition = -1;
static signed char bufferRemaining = -1;
static unsigned char secLapsed = 0;

/*
 *  Will send GPS data to serial connection for debug.
 *  Will also count up the number of valid GPS fixed
 *
 *  return:
 *  	unsigned char - A true/false showing if new data was processed
 */
unsigned char processGPSData() {
	if (GPGGAflag == TRUE) {
		/* Uncomment two lines to enable USB-serial console output */
		newLine();
		printString(GPGGAinfo);
		GPGGAflag = 0;
		secLapsed++;

		//Count up the fix lock
		if (gpsFixStatus == 1 || gpsFixStatus == 2) {
			acceptableLockCount++;
		} //if()

		return TRUE;
	} //if()

	return FALSE;
} //processGPSData()

/*
 * Adds message to buffer string
 *
 * Inputs -
 * 	gpsHAMBuffer - Must be an array of at least 75
 * 	moduleID - The ID of the requesting UGS
 */
void sendGPSStringHAM(unsigned int moduleID) {
	char gpsHAMBuffer[MAX_GPS_BUFFER_LENGTH];
	unsigned char i = 0;

	//Need to copy the info into a new buffer
	while(GPGGAinfo[i] != '\0') {
		gpsHAMBuffer[i] = GPGGAinfo[i];
		i++;
	} //while()

	gpsHAMBuffer[i] = '\0';

	sendHAMString(gpsHAMBuffer, moduleID, 'G');
//	queueEnqueue(radioHAMQueue, gpsHAMBuffer);
} //prepareGPSStringHAM()


unsigned char isGPSFinished() {
	if (acceptableLockCount > MIN_GPS_LOCK_CYCLES || secLapsed > MAX_GPS_LOCK_TIME) {
		turnOffGPS();
		return TRUE;
	} //if()

	return FALSE;
} //isGPSLocked()

//Resets
void turnOnGPS() {
	//P1OUT |= BIT5;
	acceptableLockCount = 0;
	secLapsed = 0;
	//IE2 |= UCA0RXIE; // Enable USCI_A0 RX interrupt
} //turnOnGPS()

void turnOffGPS() {
	//IE2 &= ~UCA0RXIE; // Enable USCI_A0 RX interrupt
	//P1OUT &= ~BIT5;
} //turnOffGPS()

void initializeGPS(void) {
	//Enable and turn off GPS power port
	//P1DIR |= BIT5;
	//P1OUT &= ~BIT5;

	/* Removed the next two lines b/c it was changing the freq output
	 * of the HAM radio module
	 */

	BCSCTL1 = CALBC1_1MHZ; // Set DCO
	DCOCTL = CALDCO_1MHZ;
	P1SEL = BIT1 + BIT2 ; // P1.1 = RXD, P1.2=TXD
	P1SEL2 = BIT1 + BIT2 ; // P1.1 = RXD, P1.2=TXD
	UCA0CTL1 |= UCSSEL_2; // SMCLK
	UCA0BR0 = 104; // 1MHz 9600
	UCA0BR1 = 0; // 1MHz 9600
	UCA0MCTL = UCBRS0; // Modulation UCBRSx = 1
	UCA0CTL1 &= ~UCSWRST; // **Initialize USCI state machine**
	IE2 |= UCA0RXIE; // Enable USCI_A0 RX interrupt

	__enable_interrupt();
} //initializeGPS()

#pragma vector=USCIAB0RX_VECTOR
__interrupt void USCI0RX_ISR(void)
{
	char availableData = (GPGGAinfo[4] == 'G' && GPGGAinfo[5] == 'A');

	//Check for delimiter
	if (UCA0RXBUF == '$') {
		bufferPosition = 0;
		bufferRemaining = -1;
	} //if()

	//Check for near-end of tx
	if (UCA0RXBUF == '*') {
		bufferRemaining = 3;
	} //if()


	//Ensure we are getting $GPGGA info and not anything else
	if (bufferPosition <= 5 || (bufferPosition > 5 && availableData == GPGGA)) {

		//Add data to buffer
		if (bufferPosition >= 0 && bufferPosition < (MAX_GPS_BUFFER_LENGTH - 1)) {
			GPGGAinfo[bufferPosition] = UCA0RXBUF;
			bufferPosition++;
			bufferRemaining--;
		} //if()
	} //if()

	//Handle completed buffer
	if (bufferRemaining == 0) {
		GPGGAinfo[bufferPosition] = '\0';

		if (availableData == GPGGA) {
			GPGGAflag = 1;
		} //if()

		bufferRemaining = -1;
	} //if()

} //USCI0RX_ISR()
