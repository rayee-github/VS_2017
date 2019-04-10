#include <Windows.h>
#include <stdio.h>
#include <time.h>    
#include <fstream>
#include <string.h>
#include "SerialPort.h"
#include <iostream>
using namespace std;

#define MAX_DATA_LENGTH 255

char incomingData[MAX_DATA_LENGTH];
SerialPort *VCOM;

int main(void)
{
	char portName[15];
	char sendString[255];
	char channel[255];
	char sampling_rate[255];
	int port = 3;

	printf("select port:");
	cin >> port;
	printf("select channel:");
	cin >> channel;
	printf("select sampling rate:");
	cin >> sampling_rate;
	system("CLS");

	sprintf_s(sendString, "channel:%s\nsampling rate:%s\n\n", channel, sampling_rate);
	printf("%s", sendString);

	sprintf_s(portName, "\\\\.\\COM%d", port);
	VCOM = new SerialPort(portName);

	if (VCOM->isConnected()){
		bool hasWritten = VCOM->writeSerialPort(sendString, MAX_DATA_LENGTH);
		if (hasWritten) printf("Data Written Successfully\n");
		else printf("Data was not written\n");
	}
}