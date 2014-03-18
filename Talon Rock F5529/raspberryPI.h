/*
 * raspberryPI.h
 *
 *  Created on: Mar 11, 2014
 *      Author: C14JeanLuc.Duckworth
 */

#ifndef RASPBERRYPI_H_
#define RASPBERRYPI_H_

/* Define functions */
void initRaspberryPI(void);
void handleRaspberryPI(void);
__interrupt void Port_1(void);


#endif /* RASPBERRYPI_H_ */
