#include<iostream>
#include <time.h>   /* 時間相關函數 */

using namespace std;

void writeExcel()
{
	int max = 35, min = 20;
	int array1, array2, array3;
	int i;
	FILE *fp = NULL;
	fopen_s(&fp,"test.xlsx", "w");

	for (i = 0; i < 6000; i++)
	{
		array1 = rand() % (max - min + 1) + min;
		array2 = rand() % (6  + 1);
		if (array2 >= 3)
			array2 = 0 - array2;

		array3 = rand() % (12 + 1);
		if (array3 >= 6)
			array3 = 0 - array3;
		fprintf(fp, "%d\t%d\t%d\n", array1, array1*2+array2, array1 * 4 + array3);
	}
	fclose(fp);
}
int main()
{
	srand(time(NULL));
	writeExcel();

	system("pause");
	return 0;
}