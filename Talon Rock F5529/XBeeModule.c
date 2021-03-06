/*
 * XBeeModule.c
 *
 *  Created on: Mar 19, 2014
 *      Author: Bryan Aragon
 */

#include "commonInclude.h"
#include "XBeeModule.h"
#include "exfilRadio.h"

xbee_object xbeeObject;

#define RX BIT4
#define TX BIT3

void initXbee() {
	//Reset state machine
	UCA0CTL1 |= UCSWRST;

	UCA0CTL1|= UCSSEL_2;		//SMCLK, runs at 1.04MHz
	UCA0BR0 = 109;
	UCA0BR1 = 0;				// 1.05MHz 9600
	UCA0MCTL = UCBRS1;			// Modulation UCBRSx = 4

	//Port Function Select
	P3SEL |= RX|TX;				//Assigns pins to RX(P3.4), TX(P3.3)

	//Activate LED
	P4DIR |= BIT7;//Set LEDs to output
	P4OUT |= BIT7;//Toggle LEDs

#if EXFIL_NODE
	//Activate LED
	P1DIR |= BIT0;//Set LEDs to output
	P1OUT |= BIT0;//Toggle LEDs
#endif

	//Enable state machine
	UCA0CTL1 &= ~UCSWRST;

	UCA0IE|=UCRXIE; //Enable RX interrupt
	UCA0IFG &= ~UCRXIFG; // Clear flag

	//Initialize network information
	sendByte(0x7E);
	sendByte(0x00);
	sendByte(0x05);
	sendByte(0x09);
	sendByte(0xB5);
	sendByte(0x48);
	sendByte(0x50);
	sendByte(0x04);
	sendByte(0xA5);

} //initXbee()

/**
 *Sends a message through Xbee API to a particular address
 *Address: XBee address to send data to
 *txData: data to send to desired XBee
 */
void sendMessage(long long xbeeAddr, char* txData){
	//Calculate message length and checksum
	unsigned int messageLength=strlen(txData)+TX_FRAME_LENGTH_WO_TXDATA;
	volatile unsigned char checksum;
	checksum=getChecksum(TX_REQUEST,GET_FRAME_ID,&xbeeAddr,RESERVED_BYTES,MAX_BROADCAST_RAD,USE_TO_PARAM,txData);

	//Send Delimiter
	sendByte(START_DELIMITER);

	//Send message length
	sendInt(messageLength);

	//Send Frame type and GetFrameID
	sendByte(TX_REQUEST);
	sendByte(GET_FRAME_ID);

	//Send destination address
	sendXbeeAddr(&xbeeAddr);

	//Send the rest of Xbee header bytes
	sendInt(RESERVED_BYTES);
	sendByte(MAX_BROADCAST_RAD);
	sendByte(USE_TO_PARAM);

	//Send actual data
	sendByteArray(txData);

	//Send checksum
	sendByte(checksum);

	//Toggle an LED
	//P4OUT ^= BIT7;
} //sendMessage()

/**
 *Sends a broadcast message through Xbee API.
 *
 *How to call this method
 *Example:
 *	long long xbeeAddr=0x1234567890ABCDEF;
	long long *pAddress=&xbeeAddr;
	sendMessage(pAddress,"hello");
 */
void broadcastMessage(char txData[]){
	sendMessage(XBEE_BROADCAST_ADDR,txData);
} //broadcastMessage()

/**
 * Extracts the message from a Xbee RX data
 *
 * Returns:
 * Pointer to the beginning of the RX data
 */
unsigned char* receiveMessage(){
	unsigned char* message;
	unsigned char pMessage=receiveByte();
	message=&pMessage;
	return message;
} //receiveMessage

/**
 * Calculates checksum.
 */
char getChecksum(char frameType, char frameID, long long *address,
		long reservedBytes, char broadcastRad, char transmitOpt, char *data){

		volatile char checksum=0;
		int i=0;
		int j=0;
		char* pAddr=(char*)address;
		char* pData=data;

		checksum=frameType;
		checksum+=frameID;

		for(i=0;i<XBEE_ADDR_LENGTH;i++){
			checksum+=*pAddr;
			pAddr+=0x01;
		}

		long reservedBytes1=reservedBytes & 0xFF00;
		reservedBytes1=reservedBytes1>>8; //shifts 0xFFxx to 0x00FF
		long reservedBytes2=reservedBytes & 0x00FF;

		checksum+=reservedBytes1;
		checksum+=reservedBytes2;

		checksum+=broadcastRad;
		checksum+=transmitOpt;

		for(j=0;j<strlen(data);j++){
			checksum+=*pData;
			pData+=0x01;
		}

		checksum=0xFF-checksum;

		return checksum;
} //getChecksum()

void sendByte(unsigned char byte ){
	while (!(UCA0IFG&UCTXIFG));			// USCI_A0 TX buffer ready?
	UCA0TXBUF = byte;					// TX -> RXed character
} //sendByte()

void sendInt(unsigned int intToSend){
	unsigned int upperByte;
	unsigned int lowerByte;

	upperByte=intToSend & 0xFF00;//clear lower bits
	upperByte=upperByte >> 8; //shift to make it a char
	lowerByte=intToSend & 0x00FF; //clear upper bits

	sendByte(upperByte);
	sendByte(lowerByte);
} //sendInt()

void sendXbeeAddr(long long* xbeeAddr){
	unsigned char i=0; //counter for current place in address
	char* pXbeeCurrentByte=(char*)xbeeAddr+XBEE_ADDR_LENGTH-1;
	//For long long data, MSP430 puts the MSB last

	unsigned char byteToSend;

	while(i<XBEE_ADDR_LENGTH){ //while there is more to send
		byteToSend=*pXbeeCurrentByte;
		sendByte(byteToSend);
		pXbeeCurrentByte-=0x01; //decrement 1 to get next byte
		i++;
	} //while()
} //sendXbeeAddr

void sendByteArray(char* byteArray) {
	int i;

	for(i=0; i<strlen(byteArray); i++) {
		sendByte(byteArray[i]);
	} //while()
} //sendByteArray()

unsigned char receiveByte(void){
	char rxByte;
	rxByte=UCA0RXBUF;
	return rxByte;
} //receiveByte()


//UART RX ISR: Toggles LED. Receives data and puts it into bufferSpace array.
#pragma vector=USCI_A0_VECTOR
__interrupt void USCI0RX_ISR(void) {

	unsigned int i, checksumHolder;

	//Look for delimiter
	if (UCA0RXBUF == XBEE_START_DELIMITER) {
		//Reset variables
		xbeeObject.bufferPosition = 0;
		xbeeObject.xbeeFrameType = 0;
		xbeeObject.xbeeMessageLength = 0;
		xbeeObject.xbeeSenderAddr = 0;
		xbeeObject.xbeeSenderNodeAddr = 0;
	} //if()

	//Check for gibberish (ie. No delimter yet, but we have characters)
	if (xbeeObject.bufferPosition == -1) {
		return;
	} //if()

	//Check for a full buffer
	if (xbeeObject.bufferPosition == MAX_XBEE_BUFFER_LEN) return;

	//Add data to buffer
	xbeeObject.bufferSpace[xbeeObject.bufferPosition] = UCA0RXBUF;
	xbeeObject.bufferPosition++;

	//Pull message length
	if (xbeeObject.bufferPosition == 3) {
		xbeeObject.xbeeMessageLength = (xbeeObject.bufferSpace[1] << 8) + xbeeObject.bufferSpace[2];	//Frame 1-2: Message Length
	} //if()

	//Check for end of message & handle it (necessary to prevent data overwrite)
	if (xbeeObject.bufferPosition == xbeeObject.xbeeMessageLength + 4) {

		//Prevent this message from being read again
		xbeeObject.bufferPosition = -1;

		//Calculate the checksum
		checksumHolder = 0;
		for(i=3; i<xbeeObject.xbeeMessageLength + 4; i++) {
			checksumHolder += xbeeObject.bufferSpace[i];
			checksumHolder &= 0x00FF;
		} //for()

		//Check for invalid checksum
		if (checksumHolder != 0x00FF) return;

		xbeeObject.xbeeFrameType = xbeeObject.bufferSpace[3]; //Frame 3: Frame Type

		/* CAUTION -- SIGNIFICANT IN-LINE/NON-OPTIMAL CODE FOR NEXT BLOCK OF CODE */

		//Handle any configuration messages
		if(xbeeObject.xbeeFrameType == AT_CMD_RESPONSE) {
			//Filter out response types
			if ((xbeeObject.bufferSpace[5] << 8) + xbeeObject.bufferSpace[6] == ATCMD_HP) {
				//Response with Preamble ID  -- check to see if it's okay
				if (xbeeObject.bufferSpace[7] == 0x00) { //status OK
					//Okay, now send the next configuration for the Network ID
					/* Initialize network information #2 */
					sendByte(0x7E);
					sendByte(0x00);
					sendByte(0x06);
					sendByte(0x09);
					sendByte(0xB6);
					sendByte(0x49);
					sendByte(0x44);
					sendByte(0x20);
					sendByte(0x14);
					sendByte(0x7F);
				} else {
					//Try sending the configuration message again
					/* Initialize network information */
					sendByte(0x7E);
					sendByte(0x00);
					sendByte(0x05);
					sendByte(0x09);
					sendByte(0xB5);
					sendByte(0x48);
					sendByte(0x50);
					sendByte(0x04);
					sendByte(0xA5);
				} //if-else()

			} else if((xbeeObject.bufferSpace[5] << 8) + xbeeObject.bufferSpace[6] == ATCMD_ID) {
				//Response with Network ID -- check to see if it's okay
				if (xbeeObject.bufferSpace[7] == 0x00) { //status OK
					//Okay, now send the write command
					sendByte(0x7E);
					sendByte(0x00);
					sendByte(0x04);
					sendByte(0x09);
					sendByte(0xB7);
					sendByte(0x57);
					sendByte(0x52);
					sendByte(0x96);
				} else {
					//Try sending the configuration message again
					/* Initialize network information #2 */
					sendByte(0x7E);
					sendByte(0x00);
					sendByte(0x06);
					sendByte(0x09);
					sendByte(0xB6);
					sendByte(0x49);
					sendByte(0x44);
					sendByte(0x20);
					sendByte(0x14);
					sendByte(0x7F);
				} //if-else()

			} else if((xbeeObject.bufferSpace[5] << 8) + xbeeObject.bufferSpace[6] == ATCMD_WR) {
				//Response with WR status -- check to see if it's okay
				if (xbeeObject.bufferSpace[7] == 0x00) { //status OK
					//Okay, now send the apply changes command
					sendByte(0x7E);
					sendByte(0x00);
					sendByte(0x04);
					sendByte(0x09);
					sendByte(0xB8);
					sendByte(0x41);
					sendByte(0x43);
					sendByte(0xBA);
				} else {
					//Try sending the write configuration message again
					sendByte(0x7E);
					sendByte(0x00);
					sendByte(0x04);
					sendByte(0x09);
					sendByte(0xB7);
					sendByte(0x57);
					sendByte(0x52);
					sendByte(0x96);
				} //if-else()
			} else if((xbeeObject.bufferSpace[5] << 8) + xbeeObject.bufferSpace[6] == ATCMD_AC) {
				//Response with AC status -- check to see if it's okay
				if (xbeeObject.bufferSpace[7] != 0x00) { //status not OK
					//Try sending the apply changes configuration message again
					sendByte(0x7E);
					sendByte(0x00);
					sendByte(0x04);
					sendByte(0x09);
					sendByte(0xB8);
					sendByte(0x41);
					sendByte(0x43);
					sendByte(0xBA);
				} //if-else()
			} //if-else()

			return;
		} else if (xbeeObject.xbeeFrameType != RX_INDICATOR) {
			//Not a configuration message and not a typical data message -- exit
			return;
		} //if-else()

		/* END IN-LINE CODING EYE SORE */

		//Terminate the buffer (eliminates the checksum from the buffer in the process)
		xbeeObject.bufferSpace[xbeeObject.xbeeMessageLength + 3] = '\0';

		xbeeObject.xbeeSenderAddr = xbeeObject.bufferSpace[4];
		xbeeObject.xbeeSenderAddr <<= 8;

		xbeeObject.xbeeSenderAddr += xbeeObject.bufferSpace[5];
		xbeeObject.xbeeSenderAddr <<= 8;

		xbeeObject.xbeeSenderAddr += xbeeObject.bufferSpace[6];
		xbeeObject.xbeeSenderAddr <<= 8;

		xbeeObject.xbeeSenderAddr += xbeeObject.bufferSpace[7];
		xbeeObject.xbeeSenderAddr <<= 8;

		xbeeObject.xbeeSenderAddr += xbeeObject.bufferSpace[8];
		xbeeObject.xbeeSenderAddr <<= 8;

		xbeeObject.xbeeSenderAddr += xbeeObject.bufferSpace[9];
		xbeeObject.xbeeSenderAddr <<= 8;

		xbeeObject.xbeeSenderAddr += xbeeObject.bufferSpace[10];
		xbeeObject.xbeeSenderAddr <<= 8;

		xbeeObject.xbeeSenderAddr += xbeeObject.bufferSpace[11];

#if EXFIL_NODE

		//Activate LED
		P1OUT ^= BIT0;//Toggle LED

		xbeeObject.xbeeSenderNodeAddr = addXbee(xbeeObject.xbeeSenderAddr);

		if (xbeeObject.bufferSpace[RX_DATASTART] == NETWORK_NODE_REQ_INT) {
			addQueuedNode(xbeeObject.xbeeSenderNodeAddr, NULL);
		} else {
			//Is this xbee the next in line?
			if (!exfilObject.topExfilQueue ||
					exfilObject.topExfilQueue->node_number != xbeeObject.xbeeSenderNodeAddr) {
				__no_operation();
				return;
			} //if()

			//Is the radio busy?
			//if (!isHAMReady()) return;

			//If this branch is taken, a node is trying to send its
			//	message twice.
			if (exfilObject.topExfilQueue->message) {
				return;
			} //if()

			//Copy the message into a buffer
			exfilObject.topExfilQueue->message = malloc(sizeof(char) * (strlen(&xbeeObject.bufferSpace[15]) + 1) );
			strcpy(exfilObject.topExfilQueue->message, &xbeeObject.bufferSpace[15]);

			exfilObject.topExfilQueue->status = EXFIL_ACCEPT_ACK;

			/*
			//Add the message to the radio and reply if successful
			if (sendHAMString(exfilObject.topExfilQueue->message, xbeeObject.xbeeSenderNodeAddr)) {
				exfilObject.topExfilQueue->status = EXFIL_ACCEPT_ACK_SENT;
			} else {
				//Not successful, delete it
				exfilObject.topExfilQueue->status = EXFIL_FIN;
			} //if-else()
			*/
		} //if()
#else
		//Check to make sure this message came from the exfil node
		if (xbeeObject.xbeeSenderAddr != EXFIL_XBEE_ADDR) return;

		switch(xbeeObject.bufferSpace[RX_DATASTART]) {
		case NETWORK_EX_REQ_ACK_INT:
			handleMessageReqAck();
			break;
		case NETWORK_EX_APPROVAL_INT:
			handleMessageApproval();
			break;
		case NETWORK_EX_MSG_ACK_INT:
			handleMessageAck();
			break;
		default: break;
		} //switch()
#endif
	} //if()

} //USCI0RX_ISR()
