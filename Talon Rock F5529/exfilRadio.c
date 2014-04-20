/*
 * exfilRadio.c
 *
 *  Created on: Mar 18, 2014
 *      Author: C14JeanLuc.Duckworth
 */

#include "commonInclude.h"
#include "exfilRadio.h"
#include "XBeeModule.h"

exfil_object exfilObject;


/* Baudot table */
static const char letters_arr[33] = "\000E\nA SIU\rDRJNFCKTZLWHYPQOBG\000MXV\000";
static const char figures_arr[33] = "\0003\n- \a87\r$4',!:(5\")2#6019?&\000./;\000";

void initializeExfilRadio() {

	//PPT GPIO
	P2DIR |= BIT2;		//P2.2 output
	P2OUT |= BIT2;		//P2.2 on

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
	exfilObject.topExfilQueue = NULL;
	exfilObject.twiddle_time = 0;
	exfilObject.ready_to_send = 1;
	exfilObject.msg_tx_counter = 0;

	//The first entry in the table is always itself
	exfilObject.xbee_table[0] = EXFIL_XBEE_ADDR;

	//Just a test
	//Disable 75 baud interrupt
	//TB0CCTL0 = 0x00;
	TB0CCTL0 &= ~BIT4;

} //initializeExfilRadio()

void handleExfilQueue() {

	exfil_queue* temp_node_pointer;

	//Are there messages in the queue
	if (exfilObject.topExfilQueue) {

		temp_node_pointer = exfilObject.topExfilQueue;

		//Increase the top message's age
		temp_node_pointer->age++;

		//If the radio is not ready, no point in dealing with the other messages
		//if (!isHAMReady()) return;

		//Check on first node
		switch(temp_node_pointer->status) {
		case EXFIL_REQ_WAIT:
			//Ham radio not ready, don't do anything
			if (!isHAMReady()) break;

			//The top node is waiting for approval, the radio is ready, give the node approval!
			temp_node_pointer->status = EXFIL_ACCEPT_MSG;

			//Reset the age
			temp_node_pointer->age = 0;

		case EXFIL_ACCEPT_MSG:

			//Make sure we are not sending a message to ourself
			if (temp_node_pointer->node_number != 0) {

				temp_node_pointer->status = EXFIL_ACCEPT_WAIT;

				//(xbee) tell the node it is allowed to send its message
				sendMessage(exfilObject.xbee_table[temp_node_pointer->node_number], NETWORK_EX_APPROVAL);
			} else {

				//This message is from ourself, go ahead and forward it to the next stage
				temp_node_pointer->status = EXFIL_ACCEPT_ACK;
			} //if-else

			//Reset the age
			temp_node_pointer->age = 0;
			break;

		case EXFIL_ACCEPT_WAIT:

			//Delete the message after 15 seconds
			if (temp_node_pointer->age > 15) {
				temp_node_pointer = removeThisQueuedMessage(temp_node_pointer);
			} //if()
			break;

		case EXFIL_ACCEPT_ACK:
			//Ham radio not ready, don't do anything
			if (!isHAMReady()) return;

			if (sendHAMString(temp_node_pointer->message, temp_node_pointer->node_number)) {
				temp_node_pointer->status = EXFIL_FIN;

				//Make sure we are not sending a message to ourself
				if (temp_node_pointer->node_number != 0) {
					//(xbee) tell the node the message it sent was accepted
					sendMessage(exfilObject.xbee_table[temp_node_pointer->node_number], NETWORK_EX_MSG_ACK);
				} //if()

			} else {
				//Not successful, delete it
				exfilObject.topExfilQueue->status = EXFIL_FIN;
			} //if-else()

			//Reset the age
			temp_node_pointer->age = 0;
			break;

		case EXFIL_ACCEPT_ACK_SENT:
			temp_node_pointer->status = EXFIL_FIN;

			//Make sure we are not sending a message to ourself
			if (temp_node_pointer->node_number != 0) {
				//(xbee) tell the node the message it sent was accepted
				sendMessage(exfilObject.xbee_table[temp_node_pointer->node_number], NETWORK_EX_MSG_ACK);
			} //if()

			//Reset the age
			temp_node_pointer->age = 0;
			break;

		case EXFIL_FIN:
			temp_node_pointer = removeThisQueuedMessage(temp_node_pointer);
			break;

		default: break;
		} //switch()

		/* Handle all of the message */
		while(temp_node_pointer != NULL) {
			switch(temp_node_pointer->status) {
			/* may not need this case -- except for self messages */
			case EXFIL_REQ_ACK:

				if (temp_node_pointer->node_number == 0) {
					temp_node_pointer->status = EXFIL_ACCEPT_ACK;
					break;
				} else {
					temp_node_pointer->status = EXFIL_REQ_WAIT;
					//(xbee) tell the node to wait
					sendMessage(exfilObject.xbee_table[temp_node_pointer->node_number], NETWORK_EX_REQ_ACK);
				} //if-else()

				break;

			case EXFIL_REQ_WAIT:
				//Nothing required -- the node should continue to wait
				break;

			default: break;
			} //switch()

			temp_node_pointer = temp_node_pointer->next_queued_node;
		} //while()

	} //if()

	//Turn off the PTT button if enough time has elapsed
	//if (exfilObject.time_since_last_tx > 5)
} //handleExfilQueue()

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

	int i = 0, prefixZerosCounter;
	unsigned char j = 3; //Starts at two because of FIG_SHIFT and $-sign at the beginning
	unsigned char currChar, checkSumString[5], moduleIDString[5];
	unsigned int checkSum;

	if (!isHAMReady()) { return FALSE; }

	//Reset ready flag
	exfilObject.ready_to_send = 0;

	//Allocate space for send string
	/*
	 * Space for 2-Figure shifts, 1-DollarSign, 1-Null terminator,
	 * 	n-the string itself, 5-checksum, 1-line feed, 1-ampersand, 5-module number, 1-semicolon
	 */
	do {
		exfilObject.string_to_send = realloc(exfilObject.string_to_send, sizeof(char) * (strlen(stringToSend) * 2 + 18));
	} while(exfilObject.string_to_send == NULL);

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
			//Check the first four numbers for leading zeros -- skip them if so
			for(prefixZerosCounter = 0; prefixZerosCounter<4; prefixZerosCounter++) {
				if (moduleIDString[prefixZerosCounter] == '0') {
					i++;
				} //if()
			} //for()
			currChar = moduleIDString[i];
			break;

		case MESSAGE_TO_SEND:
			currChar = stringToSend[i];
			break;

		case CHECKSUM:
			if (i == 0) {
				currChar = '&';
				i = 3; //This will keep only the last two digits
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

			if (currChar != 0) {
				exfilObject.string_to_send[j] = currChar;
			} //if()
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

	//Reset twiddle time
	exfilObject.twiddle_time = 0;

	/* Enabling the PTT button is now handled
	 * by a timer system in the ISR.  This ensures
	 * the radio has some pause before it transmits
	 * more data.
	 */
	//Enable PTT
	//enablePTT();

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
	P2OUT |= BIT2;	//P2.2
} //enablePTT()

void disablePTT() {
	P2OUT &= ~BIT2; //P2.2
} //disablePTT()

//Timer0_B7 CC0
#pragma vector=TIMER0_B0_VECTOR
__interrupt void TA11_ISR(void) {

	unsigned char currByte;

	//Reset the PTT timer
	if (exfilObject.time_since_last_tx > MIN_TIME_BETWEEN_EXFIL_MSGS) {

		exfilObject.time_since_last_tx = -1;
		enablePTT();

	} else if (exfilObject.time_since_last_tx != -1) {
		return;
	} //if()

	//Prep byte
	if (exfilObject.twiddle_time != -1) {
		currByte = 0x1F;
	} else {
		currByte = exfilObject.string_to_send[exfilObject.string_position];
		exfilObject.twiddle_time = -1;
	} //if-else

	if (currByte == '\0' && exfilObject.msg_tx_counter < NUM_TIMES_RETRANS_MSG - 1) {
		exfilObject.msg_tx_counter++;
		exfilObject.string_position = 0;
		exfilObject.bit_position = 0;
		exfilObject.twiddle_time = PRE_MSG_TWIDDLES / 2;

	} else if (currByte == '\0') {
		exfilObject.ready_to_send = 1;

		disablePTT();

		//Disable 75 baud interrupt
		TB0CCTL0 = 0x00;

		//Reset hot mic timer
		exfilObject.time_since_last_tx = 0;

		//Reset retransmission timer
		exfilObject.msg_tx_counter = 0;

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

			if (exfilObject.twiddle_time > PRE_MSG_TWIDDLES) {
				exfilObject.twiddle_time = -1;
				exfilObject.string_position = 0;
			} else if (exfilObject.twiddle_time != -1) {
				exfilObject.twiddle_time++;
			} //if()
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
