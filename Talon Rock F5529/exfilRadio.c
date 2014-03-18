/*
 * exfilRadio.c
 *
 *  Created on: Mar 18, 2014
 *      Author: C14JeanLuc.Duckworth
 */

#include "commonInclude.h"
#include "exfilRadio.h"

/* Baudot table */
static const char letters_arr[33] = "\000E\nA SIU\rDRJNFCKTZLWHYPQOBG\000MXV\000";
static const char figures_arr[33] = "\0003\n- \a87\r$4',!:(5\")2#6019?&\000./;\000";

void initializeExfilRadio() {

	//PPT GPIO
	P2DIR |= BIT2;		//P2.2 output
	P2OUT |= BIT2;		//P2.2 on

	//TODO: MAYBE REMOVE THIS?
	//P2DIR |= BIT0;                            // P2.0/TA1.1
	//P2SEL |= BIT0;                            // P2.0 and TA1.1 options

	//AFSK (Tone Generator)
	P2DIR |= BIT0;							// P2.0 (TA1.1 output?)
	P2SEL |= BIT0;							// P2.0 options

	/* Setup the baud PWM */
	TB0CCR0 = PERIOD_BAUD;						// PWM Period
	TB0CCTL1 = OUTMOD_7;						// CCR1 reset/set
	TB0CCTL0 |= BIT4;							//Enable Timer B0 interrupts
	TB0CCR1 = DUTY_BAUD;						// CCR1 PWM duty cycle
	TB0CTL = TASSEL_1 + MC_1 + TACLR;			// SMCLK, up mode

	/* Setup the mark PWM */
	TA1CCR0 = PERIOD_MARK;						// PWM Period
	TA1CCTL1 = OUTMOD_7;						// CCR1 reset/set
	TA1CCR1 = DUTY_MARK;						// CCR1 PWM duty cycle
	TA1CTL = TASSEL_1 + MC_1;					// SMCLK, up mode

	//Initialize object
	exfilObject.string_to_send = NULL;
	exfilObject.topExfilQueued = NULL;
	exfilObject.ready_to_send = 1;

	//Just a test
	//Disable 75 baud interrupt
	//TB0CCTL0 = 0x00;
	TB0CCTL0 &= ~BIT4;

} //initializeExfilRadio()

enum baudot_mode
{
  NONE,
  LETTERS,
  FIGURES
};

//Ensure something is between each message!!
enum send_select
{
  MODULE_ID,
  	  SEMICOLON_1,
  MESSAGE_TO_SEND,
  CHECKSUM,
  FINISHED
};

unsigned char sendHAMCharToBaudot(char c, const char *array)
{
	unsigned char i;
	for (i = 0; i < 32; i++) {
		if (array[i] == c) {
			return i;
		} //if()
	} //for()

  return 0;
} //sendHAMCharToBaudot

//Pass a null terminated string
//Returns TRUE if able to send string to HAM buffer
char sendHAMString(char* stringToSend, unsigned int moduleID) {
	enum baudot_mode currentMode = NONE;
	enum send_select currentSendMsg = MODULE_ID;

	int i = 0;
	unsigned char j = 3; //Starts at two because of FIG_SHIFT and $-sign at the beginning
	unsigned char currChar, checkSum, checkSumString[5], moduleIDString[5];

	if (!isHAMReady()) { return FALSE; }

	//Reset ready flag
	exfilObject.ready_to_send = 0;

	//Allocate space for send string
	if (exfilObject.string_to_send) {
		free(exfilObject.string_to_send);
	} //if()

	/*
	 * Space for 2-Figure shifts, 1-DollarSign, 1-Null terminator,
	 * 	n-the string itself, 5-checksum, 1-line feed
	 */
	exfilObject.string_to_send = malloc(sizeof(char) * (10 + strlen(stringToSend)));

	//Disable 75 baud interrupt
//	TA0CCTL0 = 0x00;
	TB0CCTL0 &= ~BIT4;

	//Place a FIG_LTR notice at the beginning of the string
	exfilObject.string_to_send[0] = FIGURES_SHIFT;
	exfilObject.string_to_send[1] = FIGURES_SHIFT;

	//Place a $ sign at the beginning of the string (in Baudot)
	exfilObject.string_to_send[2] = 9;

	//Obtain the check sum of stringToSend
	checkSum = signedStringChecksum(stringToSend);

	//Add variability to checksum
	checkSum += moduleID;

	//Convert checksum to ASCII
	intNumberToString(checkSum, checkSumString);

	//Convert moduleID to ASCII
	intNumberToString(moduleID, moduleIDString);

	//Add string to global
	//while(checkSumPos < 5 || stringToSend[i] != '\0') {
	while(currentSendMsg != FINISHED) {
		switch(currentSendMsg) {
		case MODULE_ID:
			currChar = moduleIDString[i];
			break;

		case MESSAGE_TO_SEND:
			currChar = stringToSend[i];
			break;

		case CHECKSUM:
			if (i == 0) {
				currChar = '&';
			} else {
				currChar = checkSumString[i - 1];
			} //if-else
		}//switch(currentSendMsg)

	    /* Some characters are available in both sets */
	    if (currChar == '\n') {
	    	exfilObject.string_to_send[j] = LINEFEED;
	    } else if (currChar == '\r') {
	    	exfilObject.string_to_send[j] = CARRRTN;
	    } else if (is_lowercase(currChar) || is_uppercase(currChar)) {
	    	//Convert to upper-case letter
	    	if (is_lowercase(currChar)) {
	    		currChar -= 32;
	    	} //if()

			if (currentMode != LETTERS) {
				exfilObject.string_to_send[j] = LETTERS_SHIFT;
				j++;
				currentMode = LETTERS;
			} //if()

			exfilObject.string_to_send[j] = sendHAMCharToBaudot(currChar, letters_arr);
	    } else {
	    	currChar = sendHAMCharToBaudot(currChar, figures_arr);

			if (currChar != 0 && currentMode != FIGURES)
			{
				exfilObject.string_to_send[j] = FIGURES_SHIFT;
				j++;
				currentMode = FIGURES;
			} //if()

			exfilObject.string_to_send[j] = currChar;
	    } //if-else

	    i++;
	    j++;

		switch(currentSendMsg) {
		case MODULE_ID:
			//Next mode
			if (i == 5) {
				i=0;
				currentSendMsg++;
			} //if(next mode)
			break;

		case MESSAGE_TO_SEND:
			//Next mode
			if (stringToSend[i] == '\0') {
				i=0;
				currentSendMsg++;
			} //if(next mode)
			break;

		case CHECKSUM:
			//Next mode
			if (i == 6) {
				i=0;
				currentSendMsg++;
			} //if(next mode)
			break;

		}//switch(currentSendMsg)

		//Are we in between a message? Should be all odd figures
		if (currentSendMsg == SEMICOLON_1) {
			if (currentMode != FIGURES) {
				exfilObject.string_to_send[j] = FIGURES_SHIFT;
				j++;
				currentMode = FIGURES;
			} //if()

			exfilObject.string_to_send[j] = 0x1E; //A semicolon
			j++;
			currentSendMsg++;
		} //if()
	} //while()

	//Add line-feed
	exfilObject.string_to_send[j] = LINEFEED;
	j++;

	//Terminate buffer string
	exfilObject.string_to_send[j] = '\0';

	//Reset string position
	exfilObject.string_position = 0;

	//Reset bit position
	exfilObject.bit_position = 0;

	//Reset baud PWM
	//TODO: Is this needed?
	TB0CCTL1 = OUTMOD_7;

	//Enable PTT
	enablePTT();

	//Enable 75 baud interrupt
	TB0CCTL0 |= BIT4;

	return TRUE;
} //sendHAMString()

//Set a one
void sendHAMSetMark() {
	TA1CCR0 = PERIOD_MARK;					// PWM Period
	TA1CCTL1 = OUTMOD_7;					// CCR1 reset/set
	TA1CCR1 = DUTY_MARK;					// CCR1 PWM duty cycle
} //sendHAMSetMark

//Set a zero
void sendHAMSetShift() {
	TA1CCR0 = PERIOD_SHIFT;					// PWM Period
	TA1CCTL1 = OUTMOD_7;					// CCR1 reset/set
	TA1CCR1 = DUTY_SHIFT;					// CCR1 PWM duty cycle
} //sendHAMSetMark

char isHAMReady() {
	return exfilObject.ready_to_send;
} //isHAMReady()

void enablePTT() {
	P2OUT &= ~BIT2; //P2.2
} //enablePTT()

void disablePTT() {
	P2OUT |= BIT2;	//P2.2
} //disablePTT()

//Timer0_B7 CC0
#pragma vector=TIMER0_B0_VECTOR
__interrupt void TA11_ISR(void) {

	unsigned char currByte;

	//Prep byte
	currByte = exfilObject.string_to_send[exfilObject.string_position];


	if (currByte == '\0') {
		exfilObject.ready_to_send = 1;
		disablePTT();

		//Disable 75 baud interrupt
		TB0CCTL0 = 0x00;

		//Wake CPU
		//__bic_SR_register_on_exit(LPM0_bits);
	} else {
		exfilObject.ready_to_send = 0;

		/* Send BIT */
		if (exfilObject.bit_position == 0) {
			sendHAMSetShift();
			exfilObject.bit_position++;
		} else if (exfilObject.bit_position == 6) {
			sendHAMSetMark();
			exfilObject.bit_position = 0;
			exfilObject.string_position++;
		} else {
			//Determine if the current BIT in the current BYTE is a one or zero
			if (currByte & (1 << (exfilObject.bit_position - 1) )) {
				sendHAMSetMark();
			} else {
				sendHAMSetShift();
			} //if-else

			exfilObject.bit_position++;
		} //if-else()

		/* End sending BIT */

	} //if-else
} //TA11_ISR()
