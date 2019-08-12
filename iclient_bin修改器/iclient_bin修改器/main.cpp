#include <stdio.h>
#include <string.h>
#include <io.h> //c_file
#include <direct.h> 
#include <iostream> 
#include <Windows.h>
#include <fstream>
using namespace std;

char filename[30][30] = { 0 };
void getfile()
{
	struct _finddata_t c_file;
	long hFile;
	int count = 0;

	_chdir("");
	hFile = _findfirst("*.bin", &c_file);
	if (hFile != -1)
	{
		do {
			sprintf(filename[count], "%s", c_file.name);
			count++;
		} while (_findnext(hFile, &c_file) == 0);
	}
}

HANDLE Open_mass_storage(char *file)
{
	TCHAR filename_t[30] = { 0 };

	mbstowcs(filename_t, file, strlen(file));
	HANDLE hdisk = CreateFile(filename_t,
		GENERIC_READ | GENERIC_WRITE,
		FILE_SHARE_READ | FILE_SHARE_WRITE,
		nullptr,
		OPEN_EXISTING,
		0, NULL);
	if (hdisk == INVALID_HANDLE_VALUE)
	{
		int err = GetLastError();
		printf("error1\n");
		system("pause");
	}
	return hdisk;
}

void Write_mass_storage(HANDLE pFile, char senddata[])
{
	DWORD write;
	LARGE_INTEGER position = { 0 };

	position.QuadPart = 393216;  //set position address
	bool ok = SetFilePointerEx(pFile, position, nullptr, FILE_BEGIN);
	if (!ok)
	{
		printf("error4\n");
		system("pause");
	}
	ok = WriteFile(pFile, senddata, 1076, &write, nullptr);
	if (!ok)
	{
		printf("error5\n");
		system("pause");
	}
}

int main()
{
	int count = 0, i = 0;
	char senddata[2048];
	HANDLE pFile;

	ifstream fin("default", ios::in | ios::binary);
	if (!fin)
	{
		printf("default檔案不存在\n");
		system("pause");
		return 0;
	}
	while (!fin.eof()) {
		fin.get(senddata[count]);
		count++;
	}
	fin.close();

	while (1)
	{
		getfile();
		if (!filename[i][0])
		{
			printf("完成%d個檔案\n",i);
			system("pause");
			return 0;
		}

		pFile = Open_mass_storage(filename[i]);
		Write_mass_storage(pFile, senddata);
		CloseHandle(pFile);
		i++;
	}
}