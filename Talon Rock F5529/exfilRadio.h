/*
 * exfilRadio.h
 *
 *  Created on: Mar 18, 2014
 *      Author: C14JeanLuc.Duckworth
 */
//ASCII-to-BAUDOT sudo code from http://www.timzaman.com/?p=138

#ifndef EXFILRADIO_H_
#define EXFILRADIO_H_

//2200Hz
#define PERIOD_MARK 14
#define DUTY_MARK 7

//2540Hz
#define PERIOD_SHIFT 12
#define DUTY_SHIFT 6

//56Hz
//#define PERIOD_BAUD 17857
//#define DUTY_BAUD 8929

//75Hz
#define PERIOD_BAUD 438
#define DUTY_BAUD 219

#define IS_MARK 1
#define IS_SHIFT 0

#define LETTERS_SHIFT 31
#define FIGURES_SHIFT 27
#define LINEFEED 2
#define CARRRTN  8
#define is_lowercase(ch) ((ch) >= 'a' && (ch) <= 'z')
#define is_uppercase(ch) ((ch) >= 'A' && (ch) <= 'Z')


/* Define functions */
void initializeExfilRadio(void);
void handleExfilQueue(void);

unsigned char sendHAMCharToBaudot(char c, const char *array);
char sendHAMString(char* stringToSend, unsigned int moduleID);
void sendHAMSetMark(void);
void sendHAMSetShift(void);
char isHAMReady(void);
void enablePTT(void);
void disablePTT(void);
__interrupt void TA11_ISR(void);


#endif /* EXFILRADIO_H_ */
