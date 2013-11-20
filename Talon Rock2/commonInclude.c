#include "msp430g2553.h"
#include "commonInclude.h"

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


void signedPrintString(char* stringToPrint) {
	char i;
	for(i = 0; stringToPrint[i] != '\0'; i++) {
		while (!(IFG2&UCA0TXIFG));
		UCA0TXBUF = stringToPrint[i];
	} //for()

	//Add checksum
//	while (!(IFG2&UCA0TXIFG));
//	UCA0TXBUF = signedStringChecksum(stringToPrint);

} //printString()

void printString(unsigned char* stringToPrint) {
	char i;
	for(i = 0; stringToPrint[i] != '\0'; i++) {
		while (!(IFG2&UCA0TXIFG));
		UCA0TXBUF = stringToPrint[i];
	} //for()

	//Add checksum
//	while (!(IFG2&UCA0TXIFG));
//	UCA0TXBUF = stringChecksum(stringToPrint);

} //printString()

void newLine() {
	while (!(IFG2&UCA0TXIFG));
	UCA0TXBUF = 0x0A;
	while (!(IFG2&UCA0TXIFG));
	UCA0TXBUF = 0x0D;
} //newLine()
