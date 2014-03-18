#include "commonInclude.h"

//Initialize global variables
unsigned char gpsComplete = 0;
char* gpsPositionString = NULL;
const long long exfilAddress = EXFIL_XBEE_ADDR;

#if EXFIL_NODE
	long long sensorUGSTable[MAX_SENSOR_NETWORK_SIZE];
	exfil_object exfilObject;
#endif

	message_queue* topQueuedMessage;

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
	checkSum = ~checkSum;

	return checkSum;
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

void addMessageQueue(message_queue** topQueuedMessage, char* messageType, char* messageToAdd) {

	//Create the node pointer
	message_queue* new_message = malloc(sizeof(message_queue));

	//Initialize variable
	new_message->next_message = NULL;
	new_message->status = INITIAL;
	new_message->age = 0;

	//Allocate space for the message (+2 => For null char and semicolon)
	new_message->message = malloc(sizeof(char)*(strlen(messageToAdd)+strlen(messageType)+2));

	strcpy(new_message->message, messageType);	//Copy over the message type
	strcat(new_message->message, ";");			//Append a semicolon
	strcat(new_message->message, messageToAdd);	//Append the message


	if (*topQueuedMessage != NULL) {
		lastQueuedMessage(*topQueuedMessage)->next_message = new_message;
	} else {
		*topQueuedMessage = new_message;
	} //if-else()
} //addMessageQueue()

void removeTopMessage(message_queue** topQueuedMessage) {

	message_queue* temp_node_pointer;

	//Check to see if the top node is empty
	if (!*topQueuedMessage) return;

	//Save node pointer
	temp_node_pointer = *topQueuedMessage;

	//Redirect top pointer
	*topQueuedMessage = temp_node_pointer->next_message;

	//Free resources
	free(temp_node_pointer->message);
	free(temp_node_pointer);
} //removeTopMessage()

message_queue* lastQueuedMessage(message_queue* topQueuedMessage) {
	message_queue* tempMessageQueuePointer = topQueuedMessage;

	while(tempMessageQueuePointer->next_message) {
		tempMessageQueuePointer = tempMessageQueuePointer->next_message;
	} //while()

	return tempMessageQueuePointer;
} //lastQueuedMessage()

void addQueuedNode(exfil_queue** topExfilQueue, unsigned int nodeToAdd) {

	//Create new node
	exfil_queue* new_node = malloc(sizeof(exfil_queue));

	//Initialize variable
	new_node->next_queued_node = NULL;
	new_node->node_number = nodeToAdd;

	if (*topExfilQueue != NULL) {
		lastQueuedNode(*topExfilQueue)->next_queued_node = new_node;
	} else {
		*topExfilQueue = new_node;
	} //if-else()
} //addQueuedNode()

void removeTopQueuedNode(exfil_queue** topExfilQueue) {

	exfil_queue* temp_exfil_pointer;

	//Check to see if the top node is empty
	if (!*topExfilQueue) return;

	//Save node pointer
	temp_exfil_pointer = *topExfilQueue;

	//Redirect top pointer
	*topExfilQueue = temp_exfil_pointer->next_queued_node;

	//Free resources
	free(temp_exfil_pointer);
} //removeTopQueuedNode()

exfil_queue* lastQueuedNode(exfil_queue* topExfilQueue) {
	exfil_queue* tempExfilQueuePointer = topExfilQueue;

	while(tempExfilQueuePointer->next_queued_node) {
		tempExfilQueuePointer = tempExfilQueuePointer->next_queued_node;
	} //while()

	return topExfilQueue;
} //lastQueuedNode()

char isXbeeInTable(long long xbeeAddress) {

	int i;

	for(i=0; i<MAX_SENSOR_NETWORK_SIZE; i++) {
		if (sensorUGSTable[i] == xbeeAddress) {
			return TRUE;
		} //if()
	} //for()

	//XBee not found
	return FALSE;
} //isXbeeInTable
