#pragma once
extern "C"
{
	__declspec(dllexport) void encode(char *filename, int parameter);//加密函式
	__declspec(dllexport) void decode(char *filename, int parameter);//解密函式
}