//ASCII-to-BAUDOT sudo code from http://www.timzaman.com/?p=138

#include "msp430g2553.h"
#include "commonInclude.h"
#include "radioHAM.h"

//Worse case is a function switch between each letter
static unsigned char sendHAMStringBuffer[MAX_HAM_STRING_LENGTH + 1];
static unsigned char sendHAMStringPosition = 0;
static unsigned char sendHAMBitPosition = 0;
static unsigned char sendHAMReady = 1;

/* Baudot table */
static const char letters_arr[33] = "\000E\nA SIU\rDRJNFCKTZLWHYPQOBG\000MXV\000";
static const char figures_arr[33] = "\0003\n- \a87\r$4',!:(5\")2#6019?&\000./;\000";

/*
 * Initializes HAM radio settings, does not transmit, enable interrupts
 */
void initializeHAMRadio() {

	//PPT GPIO
	P2DIR |= BIT2;		//P2.2 output
	P2OUT |= BIT2;		//P2.2 on

	//TODO: MAYBE REMOVE THIS?
	P1DIR |= 0x40;                            // P1.6/TA0.0
	P1SEL |= 0x40;                            // P1.6 and TA1/2 options

	//AFSK (Tone Generator)
	P2DIR |= 0x02;							// P2.1/TA1.1 output
	P2SEL |= 0x02;							// P2.1 options

	/* Setup the baud PWM */
	TA0CCR0 = PERIOD_BAUD;						// PWM Period
	TA0CCTL1 = OUTMOD_7;						// CCR1 reset/set
	TA0CCTL0 = 0x10;							//Enable Timer A0 interrupts, bit 4=1
	TA0CCR1 = DUTY_BAUD;						// CCR1 PWM duty cycle
	TA0CTL = TASSEL_2 + MC_1;					// SMCLK, up mode

	/* Setup the mark PWM */
	TA1CCR0 = PERIOD_MARK;						// PWM Period
	TA1CCTL1 = OUTMOD_7;						// CCR1 reset/set
	TA1CCR1 = DUTY_MARK;						// CCR1 PWM duty cycle
	TA1CTL = TASSEL_2 + MC_1;					// SMCLK, up mode

	//Just a test
	//Disable 45.45 baud interrupt
	TA0CCTL0 = 0x00;

	__enable_interrupt();

	sendHAMReady = 1;
} //initializeHAMRadio()

enum baudot_mode
{
  NONE,
  LETTERS,
  FIGURES
};

//Ensure something is between each message!!
enum send_select
{
  MESSAGE_ID,
  	  SEMICOLON_1,
  MODULE_ID,
  	  SEMICOLON_2,
  MESSAGE_TO_SEND,
  CHECKSUM,
  FINISHED
};

static unsigned char sendHAMCharToBaudot(char c, const char *array)
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
unsigned char sendHAMString(char* stringToSend, unsigned int moduleID, unsigned char messageID) {
	enum baudot_mode currentMode = NONE;
	enum send_select currentSendMsg = MESSAGE_ID;
	signed int i = 0;
	unsigned char j = 3; //Starts at two because of FIG_SHIFT and $-sign at the beginning
	unsigned char currChar;
	unsigned char checkSum;
	unsigned char checkSumString[5];
	unsigned char moduleIDString[5];

	if (!isHAMReady()) { return FALSE; }

	//Reset ready flag
	sendHAMReady = 0;

	//Disable 45.45 baud interrupt
	TA0CCTL0 = 0x00;

	//Place a FIG_LTR notice at the beginning of the string
	sendHAMStringBuffer[0] = FIGURES_SHIFT;
	sendHAMStringBuffer[1] = FIGURES_SHIFT;

	//Place a $ sign at the beginning of the string (in Baudot)
	sendHAMStringBuffer[2] = 9;

	//Obtain the check sum of stringToSend
	checkSum = signedStringChecksum(stringToSend);

	//Add variability to checksum
	checkSum += moduleID;
	checkSum += messageID;

	//Convert checksum to ASCII
	intNumberToString(checkSum, checkSumString);

	//Convert moduleID to ASCII
	intNumberToString(moduleID, moduleIDString);


	//Add string to global
	//while(checkSumPos < 5 || stringToSend[i] != '\0') {
	while(currentSendMsg != FINISHED) {
		switch(currentSendMsg) {
		case MESSAGE_ID:
			currChar = messageID;
			break;

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
	    	sendHAMStringBuffer[j] = LINEFEED;
	    } else if (currChar == '\r') {
	    	sendHAMStringBuffer[j] = CARRRTN;
	    } else if (is_lowercase(currChar) || is_uppercase(currChar)) {
	    	//Convert to upper-case letter
	    	if (is_lowercase(currChar)) {
	    		currChar -= 32;
	    	} //if()

			if (currentMode != LETTERS) {
				sendHAMStringBuffer[j] = LETTERS_SHIFT;
				j++;
				currentMode = LETTERS;
			} //if()

			sendHAMStringBuffer[j] = sendHAMCharToBaudot(currChar, letters_arr);
	    } else {
	    	currChar = sendHAMCharToBaudot(currChar, figures_arr);

			if (currChar != 0 && currentMode != FIGURES)
			{
				sendHAMStringBuffer[j] = FIGURES_SHIFT;
				j++;
				currentMode = FIGURES;
			} //if()

			sendHAMStringBuffer[j] = currChar;
	    } //if-else

	    i++;
	    j++;

		switch(currentSendMsg) {
		case MESSAGE_ID:
			//Next mode
			i = 0;
			currentSendMsg++;
			break;

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
		if (currentSendMsg == SEMICOLON_1 || currentSendMsg == SEMICOLON_2) {
			if (currentMode != FIGURES) {
				sendHAMStringBuffer[j] = FIGURES_SHIFT;
				j++;
				currentMode = FIGURES;
			} //if()

			sendHAMStringBuffer[j] = 0x1E;
			j++;
			currentSendMsg++;
		} //if()
	} //while()

	//Add line-feed
	sendHAMStringBuffer[j] = LINEFEED;
	j++;

	//Terminate buffer string
	sendHAMStringBuffer[j] = '\0';

	//Reset string position
	sendHAMStringPosition = 0;

	//Reset bit position
	sendHAMBitPosition = 0;

	//Reset baud PWM
	TA0CCTL1 = OUTMOD_7;

	//Enable PTT
	enablePTT();

	//Enable 45.45 baud interrupt
	TA0CCTL0 = 0x10;

	return TRUE;
} //sendHAMString()

//Set a one
static void sendHAMSetMark() {
	TA1CCR0 = PERIOD_MARK;					// PWM Period
	TA1CCTL1 = OUTMOD_7;					// CCR1 reset/set
	TA1CCR1 = DUTY_MARK;					// CCR1 PWM duty cycle
} //sendHAMSetMark

//Set a zero
static void sendHAMSetShift() {
	TA1CCR0 = PERIOD_SHIFT;					// PWM Period
	TA1CCTL1 = OUTMOD_7;					// CCR1 reset/set
	TA1CCR1 = DUTY_SHIFT;					// CCR1 PWM duty cycle
} //sendHAMSetMark

unsigned char isHAMReady() {
	return sendHAMReady;
} //isHAMReady()

static void enablePTT() {
	P2OUT &= ~BIT2;
} //enablePTT()

static void disablePTT() {
	P2OUT |= BIT2;
} //disablePTT()

#pragma vector=TIMER0_A0_VECTOR
__interrupt void TA0_ISR(void)
{
	unsigned char currByte;

	//Prep byte
	currByte = sendHAMStringBuffer[sendHAMStringPosition];


	if (currByte == '\0') {
		sendHAMReady = 1;
		disablePTT();

		//Disable 45.45 baud interrupt
		TA0CCTL0 = 0x00;
	} else {
		sendHAMReady = 0;

		/* Send BIT */
		if (sendHAMBitPosition == 0) {
			sendHAMSetShift();
			sendHAMBitPosition++;
		} else if (sendHAMBitPosition == 6) {
			sendHAMSetMark();
			sendHAMBitPosition = 0;
			sendHAMStringPosition++;
		} else {
			//Determine if the current BIT in the current BYTE is a one or zero
			if (currByte & (1 << (sendHAMBitPosition - 1) )) {
				sendHAMSetMark();
			} else {
				sendHAMSetShift();
			} //if-else

			sendHAMBitPosition++;
		} //if-else()

		/* End sending BIT */

	} //if-else

} //TimerA_ISR()
