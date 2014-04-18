/*
 * XBeeModule.h
 *
 *  Created on: Mar 19, 2014
 *      Author: Bryan Aragon
 */

#ifndef XBEEMODULE_H_
#define XBEEMODULE_H_

/* Definitions */
#define XBEE_START_DELIMITER 0x7E
#define XBEE_BROADCAST_ADDR 0x000000000000FFFF


#define START_DELIMITER 0x7E

//Frame Types
#define TX_REQUEST 0x10
#define RX_INDICATOR 0x90

//Data Starts for each Frame Type
#define RX_DATASTART 15 //Data starts at byte 15 for rx indicator frames
#define TX_FRAME_LENGTH_WO_TXDATA 14

#define NO_FRAME_ID 0x00
#define GET_FRAME_ID 0x01

#define XBEE_ADDR_LENGTH 0x08

#define RESERVED_BYTES 0xFFFE

#define MAX_BROADCAST_RAD 0x00

#define USE_TO_PARAM 0x00

//Functions
void initXbee();

/**
 *Sends a message through Xbee API to a particular address
 *Address: XBee address to send data to
 *txData: data to send to desired XBee
 *
 *How to call this method
 *	long long xbeeAddr=0x1234567890ABCDEF; //Put xbee address into long long
	long long *pAddress=&xbeeAddr; //Get pointer to xbee address
	sendMessage(pAddress,"hello"); //Call sendMessage
 *
 */
void sendMessage(long long xbeeAddr,char txData[]);

/**
 *Sends a broadcast message through Xbee API.
 *Finished: 12 Feb 2014
  */
void broadcastMessage(char txData[]);

/**
 * Extracts the message from a Xbee RX data
 *
 * Returns:
 * Pointer to the beginning of the RX data
 */
unsigned char* receiveMessage();

/**
 * Calculates checksum.
 *
 * Finished: 12 Feb 2014
 */
char getChecksum(char frameType, char frameID, long long *address,
		long reservedBytes, char broadcastRad, char transmitOpt, char *data,unsigned int dataLength);



void sendByte(unsigned char byte);
void sendInt(unsigned int intToSend);
void sendXbeeAddr(long long* xbeeAddr);
void sendByteArray(char byteArray[],int length);
unsigned char receiveByte(void);
void USCI0RX_ISR(void);

#endif /* XBEEMODULE_H_ */
