#include "msp430f5529.h"
#include <string.h>
#include <stdlib.h>

//Loads code explicit to exfil or sensor node
#define EXFIL_NODE 1

#ifndef _COMMONINCLUDE_H
#define	_COMMONINCLUDE_H

/* Common structures */
typedef struct message_queue {
	struct message_queue* next_message;
	char* message;
	char status;
	int age;		//In seconds
} message_queue;

typedef struct exfil_queue {
	struct exfil_queue* next_queued_node;
	unsigned int node_number;
} exfil_queue;

typedef struct exfil_object {
	struct exfil_queue* topExfilQueued;
	unsigned char string_position;
	unsigned char bit_position;
	char ready_to_send;
	unsigned char* string_to_send;
} exfil_object;

enum message_status
{
  MSG_INITIAL,			//Exfil does not know about message
  MSG_SYN,				//Exfil does not know about message, though we sent a request to it
  MSG_EXFIL_ACK_WAIT,	//Exfil knows about it and is telling us to wait
  MSG_EXFIL_ACK_REQ,	//Exfil knows about it and wants it
  MSG_EXFIL_FIN			//Exfil says the message was recv'd -- remove this message from queue
};

/* Define definitions */

#define GPS_MESSAGE		"G"
#define STATUS_MESSAGE	"S"
#define DETECT_MESSAGE	"D"

/* main.c */
#define EXFIL_XBEE_ADDR 0xABCDEFABCDEF2014
#define MAX_SENSOR_NETWORK_SIZE 100

/* gpsModule.h */
	//With an update rate of 1 data update per second, the below
	//	definition will require the GPS to have XX seconds of good GPS locks
	//	(greater than three satellites) before it reports the GPS info out.
#define MIN_GPS_LOCK_CYCLES 5		//Max value 65536
#define MAX_GPS_LOCK_TIME 5 		//(Standard about 600 seconds)

/* queue.h */
#define QUEUESIZE       	005	//Max value 32767
#define MAX_STRING_LENGTH	100		//Max value 32767

/* radioHAM.h */
#define MAX_NODES
//#define MAX_HAM_STRING_LENGTH MAX_STRING_LENGTH

/* raspberryPI.h */
#define MIN_RASP_PI_WAIT 10		//Time in seconds

/* All files */
#define TRUE    1
#define FALSE   0

/* Global variables */
extern unsigned char gpsComplete;
extern char* gpsPositionString;
extern const long long exfilAddress;

#if EXFIL_NODE
	extern long long sensorUGSTable[MAX_SENSOR_NETWORK_SIZE];
	extern exfil_object exfilObject;
#endif

//#else
	extern message_queue* topQueuedMessage;
//#endif


/* Define Functions */
void charNumberToString(unsigned char numberToConvert, unsigned char outputString[3]);
void intNumberToString(unsigned int numberToConvert, unsigned char outputString[5]);
unsigned int signedStringChecksum(char* stringToChecksum);
unsigned int stringChecksum(unsigned char* stringToChecksum);

void addMessageQueue(message_queue** topQueuedMessage, char* messageType, char* messageToAdd);
void removeTopMessage(message_queue** topQueuedMessage);
message_queue* lastQueuedMessage(message_queue* topQueuedMessage);

void addQueuedNode(exfil_queue** topExfilQueue, unsigned int nodeToAdd);
void removeTopQueuedNode(exfil_queue** topExfilQueue);
exfil_queue* lastQueuedNode(exfil_queue* topExfilQueue);

#endif
