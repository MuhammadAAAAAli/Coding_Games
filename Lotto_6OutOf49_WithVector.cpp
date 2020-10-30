

#include "stdafx.h"
#include<iostream>
#include <stdlib.h>
#include <cstdlib> 
#include <fstream>
#include <time.h>
#include <set>
using namespace std;

// ===================================================================================

const int howManyNumbersDoWeWant = 3;
int maximumNumberOfDigitsPerKey = 5;

clock_t tStart;
long long int howManyOptionsUpToNow = 0;
//ofstream outfile("new.txt");
set<int> memoizationLogaritmic[99999];
int memoizationInstant[99999];

void initializareMemoization()
{
	for (int i = 0; i <= 99999; i++)
		memoizationInstant[i] == 0;
}

bool numbersDoNotRepeat(int* numere, int flag)   // verificare sa nu se repete cifre
{
	for (int i = 0; i < flag; i++)
	{
		if (numere[flag] == numere[i])
			return false;
	}
	return true;
}

void schimbareaVariabilelor(int& a, int& b)
{
	int temp = a;
	a = b;
	b = temp;
}

void afisareNumereConsola(int numere[])
{
	for (int i = 0; i <= howManyNumbersDoWeWant; i++)
		cout << numere[i] << " ";
	cout << endl;

}

//void afisareNumereInFisier(int numere[])
//{
//	for (int i = 0; i <= howManyNumbersDoWeWant; i++)
//		outfile << numere[i] << " ";
//	outfile << endl;
//
//}

void bubbleSort(int* a)
{
	int deCateOriAmParcursForul = 0;
	bool amTerminatDeSortat = false;
	while (amTerminatDeSortat == false)
	{
		amTerminatDeSortat = true;                          // presupunem
		for (int i = 0; i < howManyNumbersDoWeWant - deCateOriAmParcursForul; i++)
		{
			if (a[i] > a[i + 1])
			{
				schimbareaVariabilelor(a[i], a[i + 1]);
				amTerminatDeSortat = false;
			}
		}
		deCateOriAmParcursForul++;
	}
}


void updateMemoization(int hashCode1, int hashCode2)
{
	if (hashCode2 != 0)
		memoizationLogaritmic[hashCode1].insert(hashCode2);
	else
		memoizationInstant[hashCode1] = 1;
	howManyOptionsUpToNow++;
}

void deepCopy(int* variantaCurenta, int* temp)
{
	for (int i = 0; i <= howManyNumbersDoWeWant; i++)
		temp[i] = variantaCurenta[i];
}


void buildHashCode(int theSmallestNumber, int& hashCode, int& howManyDigits)
{
	if (theSmallestNumber >= 10)
	{
		hashCode = hashCode * 100 + theSmallestNumber;
		howManyDigits += 2;
	}
	else
	{
		hashCode = hashCode * 10 + theSmallestNumber;
		howManyDigits++;
	}
}

bool isNewToMemoizationTable(int* variantaCurenta)
{
	int howManyDigits = 0;
	int hashCode1 = 0;
	int hashCode2 = 0;
	int variantaCurentaSortata[howManyNumbersDoWeWant + 1];
	deepCopy(variantaCurenta, variantaCurentaSortata);
	bubbleSort(variantaCurentaSortata);
	for (int i = 0; i <= howManyNumbersDoWeWant; i++)
	{
		if ((howManyDigits <= maximumNumberOfDigitsPerKey - 2) || (howManyDigits == maximumNumberOfDigitsPerKey - 1 && variantaCurentaSortata[i] < 10))
			buildHashCode(variantaCurentaSortata[i], hashCode1, howManyDigits);
		else
			buildHashCode(variantaCurentaSortata[i], hashCode2, howManyDigits);
	}
	//	cout << hashCode1 << " " << hashCode2 << endl;
	if (hashCode2 == 0 && memoizationInstant[hashCode1] == 1)
		return false;
	else
		if (memoizationLogaritmic[hashCode1].find(hashCode2) != memoizationLogaritmic[hashCode1].end())
			return false;
	updateMemoization(hashCode1, hashCode2);
	return true;
}


void backtracking(int* variantaCurenta, int flag)
{
	for (int i = 1; i <= 49; i++)
	{
		variantaCurenta[flag] = i;
		if (numbersDoNotRepeat(variantaCurenta, flag))
		{
			if (flag == howManyNumbersDoWeWant)
			{
				//howManyOptionsUpToNow++;
				isNewToMemoizationTable(variantaCurenta);
				//if (isNewToMemoizationTable(variantaCurenta))
				//{
				/*if (howManyOptionsUpToNow % 2000000 == 0)
				{
				printf("Time taken: %.2fs .... and we had %llu options !\n", (double)(clock() - tStart) / CLOCKS_PER_SEC, howManyOptionsUpToNow);
				}*/
				//afisareNumereConsola(variantaCurenta);
				//}
			}
			else
				backtracking(variantaCurenta, flag + 1);
		}
	}
}


// ===================================================================================


int _tmain(int argc, _TCHAR* argv[])
{
	initializareMemoization();
	cout << "Go go go !" << endl;
	int lotoNumbers[howManyNumbersDoWeWant + 1];
	tStart = clock();
	backtracking(lotoNumbers, 0);
	cout << "====================================================" << endl << endl;
	printf("Time taken: %.2fs\nAnd we had %llu options !\n\n", (double)(clock() - tStart) / CLOCKS_PER_SEC, howManyOptionsUpToNow);

	return 0;
}

