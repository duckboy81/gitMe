
#ifndef _GPSMODULE_H
#define	_GPSMODULE_H

/* Define definitions */
#define GPGGA 0x01

//#define GPS_INFO_TIME 0
//#define GPS_INFO_SAT_USED 6

/* Defined in commonInclude.h -- uncomment for standalone module*/
//#define MIN_GPS_LOCK_CYCLES 120		//Max value 65536
//#define MAX_GPS_LOCK_TIME 5 		//(Standard about 600 seconds)

#define MAX_GPS_BUFFER_LENGTH 82 //Should be a min of 80

/* Define functions */

unsigned char processGPSData(void);
void sendGPSStringHAM(unsigned int moduleID);
unsigned char isGPSFinished(void);
void turnOnGPS(void);
void turnOffGPS(void);
void initializeGPS(void);
__interrupt void USCI0RX_ISR(void);

#endif
