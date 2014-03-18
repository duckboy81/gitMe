/*
 * gpsModule.h
 *
 *  Created on: Mar 6, 2014
 *      Author: C14JeanLuc.Duckworth
 */

#include "commonInclude.h"

#ifndef GPSMODULE_H_
#define GPSMODULE_H_

#define MAX_GPS_BUFFER_LENGTH 85 //Min length should be 80 to allow space for all GPGGA data

typedef struct {
	/* RAW GPGGA Stream */
	char GPGGAinfo[MAX_GPS_BUFFER_LENGTH];

	/* Configuration variables */
	char GPGGAflag;
	unsigned int acceptableLockCount;
	signed char bufferPosition;
	signed char bufferRemaining;
	unsigned char secLapsed;
	char isOn;

	/* Decoded GPS Data */
	char zuluTime[11]; //eg. "170834.030"
	char latitude[10]; //eg. "3900.3150"
	char latitude_dir[2]; //eg. "N"
	char longitude[11]; //eg. "10452.5930"
	char longitude_dir[2]; //eg. "W"
	char fix_quality[2]; //eg. "2"
	char num_satellites[3]; //eg. "03"
} gpsStore;

/* Define definitions */
#define GPGGA 0x01
//#define MAX_GPS_BUFFER_LENGTH 82 //Should be a min of 80

/* Define functions */

//unsigned char processGPSData(void);
//void sendGPSStringHAM(unsigned int moduleID);

void initializeGPS(void);
char handleGPSData(void);
void compileGPSToString(void);
char isGPSFinished(void);
void turnOnGPS(void);
void turnOffGPS(void);
char isGPSOn(void);
__interrupt void USCI1RX_ISR(void);

#endif /* GPSMODULE_H_ */
