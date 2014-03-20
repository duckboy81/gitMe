#include "msp430f5529.h"
#include <string.h>
#include <stdlib.h>
#include <stdio.h>

//Loads code explicit to exfil or sensor node
#define EXFIL_NODE 1

#ifndef _COMMONINCLUDE_H
#define	_COMMONINCLUDE_H

/* Define definitions */

#define GPS_MESSAGE		"G"
#define STATUS_MESSAGE	"S"
#define DETECT_MESSAGE	"D"

/* main.c */
#define EXFIL_XBEE_ADDR 0x0013A20040B2C0D2
#define MAX_SENSOR_NETWORK_SIZE 100			//Max size of 99,999 nodes

/* gpsModule.h */
	//With an update rate of 1 data update per second, the below
	//	definition will require the GPS to have XX seconds of good GPS locks
	//	(greater than three satellites) before it reports the GPS info out.
#define MIN_GPS_LOCK_CYCLES 60		//Max value 65536
#define MAX_GPS_LOCK_TIME 600 		//(Standard about 600 seconds)

/* queue.h */
#define QUEUESIZE       	005	//Max value 32767
#define MAX_STRING_LENGTH	100		//Max value 32767

/* radioHAM.h */
#define MIN_TIME_BETWEEN_EXFIL_MSGS 2
#define PRE_MSG_TWIDDLES 40

/* raspberryPI.h */
#define MIN_RASP_PI_WAIT 1		//Time in seconds

/* XBeeModule.h */
#define MAX_XBEE_BUFFER_LEN 100

  // NODE -> EXFIL
#define NETWORK_NODE_REQ "0"		//A node is requesting to send a message
#define NETWORK_NODE_REQ_INT 48

  // EXFIL -> NODE
#define NETWORK_EX_REQ_ACK "1"	//The exfil is acknowledging the node's request
#define NETWORK_EX_REQ_ACK_INT 49

#define NETWORK_EX_APPROVAL "2"	//The exfil is providing a node with approval to send a message
#define NETWORK_EX_APPROVAL_INT 50

#define NETWORK_EX_MSG_ACK "3"	//The exfil has successfully received the message
#define NETWORK_EX_MSG_ACK_INT 51



/* All files */
#define TRUE    1
#define FALSE   0

/* Common structures */
typedef struct message_queue {
	struct message_queue* next_message;
	char* message;
	char status;
	unsigned int age;		//In seconds
} message_queue;

typedef struct exfil_queue {
	struct exfil_queue* next_queued_node;
	char* message;
	char status;
	unsigned int age;		//In seconds
	unsigned int node_number;
} exfil_queue;

typedef struct exfil_object {
	exfil_queue* topExfilQueue;
	long long xbee_table[MAX_SENSOR_NETWORK_SIZE];
	unsigned char string_position;
	unsigned char bit_position;
	int twiddle_time;
	char ready_to_send;
	unsigned char* string_to_send;
	int time_since_last_tx;		//Used to make sure radio does not get destroyed from over use
} exfil_object;

typedef struct xbee_object {
	char bufferSpace[MAX_XBEE_BUFFER_LEN];
	signed char bufferPosition;
	//signed char bufferRemaining;

	unsigned char xbeeFrameType;
	unsigned int xbeeMessageLength;
	unsigned long long xbeeSenderAddr;
	unsigned int xbeeSenderNodeAddr;
	//unsigned char xbeeData[10] = {0};
	//unsigned char xbeeFlag=0; //flag that there is a message to process; 0=no message; 1=is message

} xbee_object;

enum message_status
{
  MSG_INITIAL,			//Exfil does not know about message
  MSG_SYN,				//Exfil does not know about message, though we sent a request to it
  MSG_EXFIL_ACK_WAIT,	//Exfil knows about it and is telling us to wait
  MSG_EXFIL_REQ,		//Send the message to the exfil UGS because it is requesting it
  MSG_EXFIL_ACK_REQ,	//We sent the message to the exfil ugs, we haven't heard anything back
  MSG_EXFIL_FIN			//Exfil says the message was recv'd -- remove this message from queue
};

enum exfil_message_status
{
  EXFIL_REQ_ACK,		//We need to tell a node we accepted its request, tell it to wait
  EXFIL_REQ_WAIT,		//We told a node to wait
  EXFIL_ACCEPT_MSG,		//We need to tell a node it is allowed to send its message (only 1 node at a time)
  EXFIL_ACCEPT_WAIT,	//We are waiting for the node's message (only 1 node at a time)
  EXFIL_ACCEPT_ACK,		//We need to tell a node its message was accepted (only 1 node at a time)
  EXFIL_FIN				//We are done with the message (only 1 node at a time)
};

/* Global variables */
extern unsigned char gpsComplete;
extern char* gpsPositionString;
extern const long long exfilAddress;

extern long long sensorUGSTable[MAX_SENSOR_NETWORK_SIZE];
extern exfil_object exfilObject;

extern xbee_object xbeeObject;
extern message_queue* topQueuedMessage;

extern unsigned int raspberryPISec;
extern unsigned int raspberryPIEnabled;



/* Define Functions */
void initRealTimeClock(void);
void handleMessageQueue(void);

void charNumberToString(unsigned char numberToConvert, unsigned char outputString[3]);
void intNumberToString(unsigned int numberToConvert, unsigned char outputString[5]);
unsigned int signedStringChecksum(char* stringToChecksum);
unsigned int stringChecksum(unsigned char* stringToChecksum);

void addMessageQueue(char* messageType, char* messageToAdd);
void handleMessageReqAck(void);
void handleMessageApproval(void);
void handleMessageAck(void);
void removeTopMessage(void);
message_queue* removeThisMessage(message_queue* message_to_remove);
message_queue* lastQueuedMessage(void);

void addQueuedNode(unsigned int nodeToAdd, char* messageToAdd);
void removeTopQueuedNode(void);
exfil_queue* removeThisQueuedMessage(exfil_queue* message_to_remove);
exfil_queue* lastQueuedNode(void);

int addXbee(long long xbeeAddress);
int findXbee(long long xbeeAddress);

__interrupt void Port_1(void);

#endif
