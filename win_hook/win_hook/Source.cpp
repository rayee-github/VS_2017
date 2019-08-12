#include<stdio.h>
#include <Windows.h>

int main()
{
	HWND hmd = FindWindow(NULL, "¤pºâ½L");

	while (hmd)
	{
		PostMessageW(hmd, WM_CHAR, '1', 0);Sleep(500);
		PostMessageW(hmd, WM_CHAR, '+', 0);Sleep(500);
		Sleep(500);
	}
	system("pause");
}