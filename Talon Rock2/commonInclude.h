
#ifndef _COMMONINCLUDE_H
#define	_COMMONINCLUDE_H

/* Define definitions */

/* gpsModule.h */
	//With an update rate of 1 data update per second, the below
	//	definition will require the GPS to have XX seconds of good GPS locks
	//	(greater than three satellites) before it reports the GPS info out.
#define MIN_GPS_LOCK_CYCLES 120		//Max value 65536
#define MAX_GPS_LOCK_TIME 5 		//(Standard about 600 seconds)

/* queue.h */
#define QUEUESIZE       	005	//Max value 32767
#define MAX_STRING_LENGTH	100		//Max value 32767

/* radioHAM.h */
#define MAX_HAM_STRING_LENGTH MAX_STRING_LENGTH

/* All files */
#define TRUE    1
#define FALSE   0

/* Define Functions */
void charNumberToString(unsigned char numberToConvert, unsigned char outputString[3]);
void intNumberToString(unsigned int numberToConvert, unsigned char outputString[5]);
unsigned int signedStringChecksum(char* stringToChecksum);
unsigned int stringChecksum(unsigned char* stringToChecksum);
void signedPrintString(char* stringToPrint);
void printString(unsigned char* stringToPrint);
void newLine(void);

#endif
