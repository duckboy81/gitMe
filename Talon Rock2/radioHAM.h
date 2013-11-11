//ASCII-to-BAUDOT sudo code from http://www.timzaman.com/?p=138

#ifndef _RADIOHAM_H
#define	_RADIOHAM_H

/* Define definitions */

////2100 Hz
//#define PERIOD_MARK 499
//#define DUTY_MARK 250
//
////2450Hz
//#define PERIOD_SHIFT 427
//#define DUTY_SHIFT 214

//45.45Hz
//#define PERIOD_BAUD 0x5A1E
//#define DUTY_BAUD 0x2D0F

////56Hz (appears very stable 1 error out of at least 48 txs)
//#define PERIOD_BAUD 18750
//#define DUTY_BAUD 9375

//2100 Hz
#define PERIOD_MARK 476
#define DUTY_MARK 238

//2450Hz
#define PERIOD_SHIFT 408
#define DUTY_SHIFT 204

//56Hz
#define PERIOD_BAUD 17857
#define DUTY_BAUD 8929

#define IS_MARK 1
#define IS_SHIFT 0

#define LETTERS_SHIFT 31
#define FIGURES_SHIFT 27
#define LINEFEED 2
#define CARRRTN  8
#define is_lowercase(ch) ((ch) >= 'a' && (ch) <= 'z')
#define is_uppercase(ch) ((ch) >= 'A' && (ch) <= 'Z')

/* Defined in commonInclude.h -- uncomment for standalone module*/
//#define MAX_HAM_STRING_LENGTH 100

//extern queue radioHAMQueue;

/* Define functions */

void initializeHAMRadio();
static unsigned char sendHAMCharToBaudot(char c, const char *array);
unsigned char sendHAMString(char* stringToSend, unsigned int moduleID, unsigned char messageID);
static void sendHAMSetMark(void);
static void sendHAMSetShift(void);
unsigned char isHAMReady(void);
static void enablePTT(void);
static void disablePTT(void);
__interrupt void TA0_ISR(void);

#endif
