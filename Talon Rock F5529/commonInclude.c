#include "commonInclude.h"
#include "XBeeModule.h"
#include "gpsModule.h"

//Initialize global variables
unsigned int statusReportTimeWait = 0;  //In seconds (Max value 65536)
char initialStatusSent = FALSE;
unsigned char gpsComplete = 0;
char* gpsPositionString = NULL;
const long long exfilAddress = EXFIL_XBEE_ADDR;
message_queue* topQueuedMessage = NULL;



void initRealTimeClock() {

	//Setup TimerA0 to toggle at one second
	TA0CCR0 = 32768-1;
	TA0CTL = TASSEL_1 + MC_1 + TACLR;
	TA0CCTL0 |= CCIE;

} //initRealTimeClock()

void handleMessageQueue() {
	//msg in queue?
		//foreach(msg to send)
			//msg_init?
				//send request to exfil
				//(EXFIL) add self to list
				//(EXFIL) move msg marker to ack
			//msg_syn && msg_age > 120 sec
				//reset age
				//send request to exfil
			//msg_ack && msg_age > (60sec/min*10min)
				//reset age
				//status = msg_syn
				//send request to exfil
			//msg_req && msg_age > (60sec/min*10min)
				//reset age
				//status = msg_syn
				//send request to exfil
		//<--foreach()

	message_queue* current_message;

	if (topQueuedMessage) {
		current_message = topQueuedMessage;

		while(current_message != NULL) {
			switch(current_message->status) {
			case MSG_INITIAL:
				current_message->age = 0;
#if EXFIL_NODE
				//Some sly parameter passing so the exfil can send messages like a regular UGS
				addQueuedNode(0, current_message->message);
				current_message->status = MSG_EXFIL_FIN;
#else
				//(xbee) send message req to exfil UGS
				current_message->status = MSG_SYN;
				sendMessage(EXFIL_XBEE_ADDR, NETWORK_NODE_REQ);
#endif
				break;

			case MSG_SYN:
				if (current_message->age > 120) {
					current_message->age = 0;

					//(xbee) send request to exfil UGS
					sendMessage(EXFIL_XBEE_ADDR, NETWORK_NODE_REQ);
				} //if()
				break;

			case MSG_EXFIL_ACK_WAIT:
				if (current_message->age > 600) {
					current_message->age = 0;
					current_message->status = MSG_SYN;

					//(xbee) send request to exfil UGS
					sendMessage(EXFIL_XBEE_ADDR, NETWORK_NODE_REQ);
				} //if()
				break;

			case MSG_EXFIL_REQ:
				current_message->status = MSG_EXFIL_ACK_REQ;

				//(xbee) send the actual message
				sendMessage(EXFIL_XBEE_ADDR, current_message->message);

				//Reset the status update timer
				statusReportTimeWait = 0;
				break;

			case MSG_EXFIL_ACK_REQ:
				if (current_message->age > 120) {
					current_message->age = 0;
					current_message->status = MSG_SYN;

					//(xbee) send request to exfil UGS
					sendMessage(EXFIL_XBEE_ADDR, NETWORK_NODE_REQ);
				} //if()
				break;

			case MSG_EXFIL_FIN:
				current_message = removeThisMessage(current_message);

				if (!current_message) return;
				break;

			default: break;
			} //switch

			current_message->age++;
			current_message = current_message->next_message;
		} //while()
	} //if()

} //handleMessageQueue()

/*
 * Inputs
 * 	numberToConvert - The number in unsigned char format
 * 	outputString[3] - The buffer space for the output string (not null terminated!!)
 */
void charNumberToString(unsigned char numberToConvert, unsigned char outputString[3]) {
	signed char i;

	for(i=2; i>=0; i--) {
		outputString[i] = numberToConvert % 10 + 0x30; //A zero in ASCII is code 0x30

		if (numberToConvert != 0) {
			numberToConvert = numberToConvert / 10;
		} //if()
	} //for()

} //charNumberToString

/*
 * Inputs
 * 	numberToConvert - The number in unsigned char format
 * 	outputString[5] - The buffer space for the output string (not null terminated!!)
 */
void intNumberToString(unsigned int numberToConvert, unsigned char outputString[5]) {
	signed char i;

	for(i=4; i>=0; i--) {
		outputString[i] = numberToConvert % 10 + 0x30; //A zero in ASCII is code 0x30

		if (numberToConvert != 0) {
			numberToConvert = numberToConvert / 10;
		} //if()
	} //for()

} //intNumberToString

unsigned int signedStringChecksum(char* stringToChecksum) {
	char i;
	unsigned int checkSum = 0x0000;

	//Add up all the bits
	for(i = 0; stringToChecksum[i] != '\0'; i++) {
		checkSum += stringToChecksum[i];
	} //for()

	//Invert the checksum to prevent an all zero message
	return ~checkSum;
} //signedStringChecksum()

unsigned int stringChecksum(unsigned char* stringToChecksum) {
	char i;
	unsigned int checkSum = 0x0000;

	//Add up all the bits
	for(i = 0; stringToChecksum[i] != '\0'; i++) {
		checkSum += stringToChecksum[i];
	} //for()

	//Invert the checksum to prevent an all zero message
	checkSum = ~checkSum;

	return checkSum;
} //stringChecksum()

void addMessageQueue(char* messageType, char* messageToAdd) {

	//Create the node pointer
	message_queue* new_message = malloc(sizeof(message_queue));

	//Initialize variable
	new_message->next_message = NULL;
	new_message->status = MSG_INITIAL;
	new_message->age = 0;

	//Allocate space for the message (+2 => For null char and semicolon)
	new_message->message = malloc(sizeof(char)*(strlen(messageToAdd)+strlen(messageType)+2));

	strcpy(new_message->message, messageType);	//Copy over the message type
	strcat(new_message->message, ";");			//Append a semicolon
	strcat(new_message->message, messageToAdd);	//Append the message


	if (topQueuedMessage != NULL) {
		lastQueuedMessage()->next_message = new_message;
	} else {
		topQueuedMessage = new_message;
	} //if-else()
} //addMessageQueue()

void handleMessageReqAck(void) {

	/* Find the first message waiting for a response to this req ack */

	message_queue* temp_node_pointer = topQueuedMessage;

	while(temp_node_pointer != NULL) {
		if (temp_node_pointer->status == MSG_SYN) {
			temp_node_pointer->status = MSG_EXFIL_ACK_WAIT;
			return;
		} //if()

		temp_node_pointer = temp_node_pointer->next_message;
	} //while()

} //handleMessageReqAck()

void handleMessageApproval(void) {

	/* Find the first message waiting for approval to send */

	message_queue* temp_node_pointer = topQueuedMessage;

	while(temp_node_pointer != NULL) {
		if (temp_node_pointer->status == MSG_SYN || temp_node_pointer->status == MSG_EXFIL_ACK_WAIT) {
			temp_node_pointer->status = MSG_EXFIL_REQ;
			return;
		} //if()

		temp_node_pointer = temp_node_pointer->next_message;
	} //while()

} //handleMessageApproval()

void handleMessageAck(void) {

	/* Find the first message waiting for a response on it's message tx */

	message_queue* temp_node_pointer = topQueuedMessage;

	while(temp_node_pointer != NULL) {
		if (temp_node_pointer->status == MSG_EXFIL_ACK_REQ) {
			temp_node_pointer->status = MSG_EXFIL_FIN;
			return;
		} //if()

		temp_node_pointer = temp_node_pointer->next_message;
	} //while()

} //handleMessageAck()

void removeTopMessage() {

	message_queue* temp_node_pointer;

	//Check to see if the top node is empty
	if (!topQueuedMessage) return;

	//Save node pointer
	temp_node_pointer = topQueuedMessage;

	//Redirect top pointer
	topQueuedMessage = temp_node_pointer->next_message;

	//Free resources
	free(temp_node_pointer->message);
	free(temp_node_pointer);
} //removeTopMessage()

message_queue* removeThisMessage(message_queue* message_to_remove) {

	message_queue* temp_node_pointer = topQueuedMessage;

	//The top message is the message we would like to remove
	if (topQueuedMessage ==  message_to_remove) {
		removeTopMessage();
		return topQueuedMessage;
	} //if()

	//Should not be the top message at this point
	while(temp_node_pointer->next_message != NULL) {

		//Check for a match
		if (temp_node_pointer->next_message == message_to_remove) {
			//Redirect next_message pointer
			temp_node_pointer->next_message = message_to_remove->next_message;

			//Free data
			free(message_to_remove->message);
			free(message_to_remove);

			return temp_node_pointer->next_message;
		} //if()

		temp_node_pointer = temp_node_pointer->next_message;
	} //while()

	return NULL;
} //removeThisMessage()

message_queue* lastQueuedMessage() {
	message_queue* tempMessageQueuePointer = topQueuedMessage;

	while(tempMessageQueuePointer->next_message) {
		tempMessageQueuePointer = tempMessageQueuePointer->next_message;
	} //while()

	return tempMessageQueuePointer;
} //lastQueuedMessage()

void addQueuedNode(unsigned int nodeToAdd, char* messageToAdd) {

	//Create new node
	exfil_queue* new_node = malloc(sizeof(exfil_queue));

	//Initialize variable
	new_node->message = NULL;
	new_node->next_queued_node = NULL;
	new_node->node_number = nodeToAdd;
	new_node->age = 0;
	new_node->status = EXFIL_REQ_ACK;

	if (messageToAdd) {
		new_node->message = malloc(sizeof(char) * (strlen(messageToAdd) + 1));
		strcpy(new_node->message, messageToAdd);
	} //if()

	if (exfilObject.topExfilQueue != NULL) {
		lastQueuedNode()->next_queued_node = new_node;
	} else {
		exfilObject.topExfilQueue = new_node;
	} //if-else()
} //addQueuedNode()

void removeTopQueuedNode() {

	exfil_queue* temp_exfil_pointer;

	//Check to see if the top node is empty
	if (!exfilObject.topExfilQueue) return;

	//Save node pointer
	temp_exfil_pointer = exfilObject.topExfilQueue;

	//Redirect top pointer
	exfilObject.topExfilQueue = temp_exfil_pointer->next_queued_node;

	//Free resources
	free(temp_exfil_pointer->message);
	free(temp_exfil_pointer);
} //removeTopQueuedNode()

exfil_queue* removeThisQueuedMessage(exfil_queue* message_to_remove) {

	exfil_queue* temp_node_pointer = exfilObject.topExfilQueue;

	//The top message is the message we would like to remove
	if (exfilObject.topExfilQueue ==  message_to_remove) {
		removeTopQueuedNode();
		return exfilObject.topExfilQueue;
	} //if()

	//Should not be the top message at this point
	while(temp_node_pointer->next_queued_node != NULL) {

		//Check for a match
		if (temp_node_pointer->next_queued_node == message_to_remove) {
			//Redirect next_message pointer
			temp_node_pointer->next_queued_node = message_to_remove->next_queued_node;

			//Free data
			free(message_to_remove->message);
			free(message_to_remove);

			return temp_node_pointer->next_queued_node;
		} //if()

		temp_node_pointer = temp_node_pointer->next_queued_node;
	} //while()

	return NULL;
} //removeThisQueuedMessage

exfil_queue* lastQueuedNode() {
	exfil_queue* tempExfilQueuePointer = exfilObject.topExfilQueue;

	while(tempExfilQueuePointer->next_queued_node != NULL) {
		tempExfilQueuePointer = tempExfilQueuePointer->next_queued_node;
	} //while()

	return tempExfilQueuePointer;
} //lastQueuedNode()

int addXbee(long long xbeeAddress) {

	int i, indexPosition = findXbee(xbeeAddress);

	//Check to see if the xbee is already in the table
	if (indexPosition != -1) return indexPosition;

	for(i=0; i<MAX_SENSOR_NETWORK_SIZE; i++) {
		if (exfilObject.xbee_table[i] == 0x00) {
			exfilObject.xbee_table[i] = xbeeAddress;

			return i;
		} //if()
	} //for()

	//Could not add the xbee to the table
	return -1;
} //addXbee()

int findXbee(long long xbeeAddress) {
	int i;

	for(i=0; i<MAX_SENSOR_NETWORK_SIZE; i++) {
		if (exfilObject.xbee_table[i] == xbeeAddress) {
			return i;
		} //if()
	} //for()

	//XBee not found
	return -1;
} //isXbeeInTable()

// Timer A0 interrupt service routine
#pragma vector=TIMER0_A0_VECTOR
__interrupt void Timer_A(void)
{
#if !EXFIL_NODE
	//Add to timer
	if (raspberryPISec <= MIN_RASP_PI_WAIT) {
		raspberryPISec++;
	} //if()

	//Add to timer
	if (raspberryPISensorTripTimer <= MIN_RASP_PI_WAIT_BETWEEN_DETECTIONS) {
		raspberryPISensorTripTimer++;
	} //if()
#else
	if (exfilObject.time_since_last_tx != -1 && exfilObject.time_since_last_tx <= MIN_TIME_BETWEEN_EXFIL_MSGS) {
		exfilObject.time_since_last_tx++;
	} //if()
#endif

	//Add to timer
	if (statusReportTimeWait <= INITIAL_STATUS_REPORT_SEC && !initialStatusSent) {
		statusReportTimeWait++;
	} else if (!initialStatusSent) {
		addMessageQueue(STATUS_MESSAGE, "");
		initialStatusSent = TRUE;
		statusReportTimeWait = 0;
	} else if (statusReportTimeWait <= STATUS_REPORT_INTERVAL) {
		statusReportTimeWait++;
	} else {
		addMessageQueue(STATUS_MESSAGE, "");
		statusReportTimeWait = 0;
	} //if-else()

	//Increment second counter
	if (isGPSOn()) {
		incrementGPSTimeCounter();
	} //if()

	//Wake CPU -- allows us to trigger message checks every second
	__bic_SR_register_on_exit(LPM0_bits);
} //Timer_A()
