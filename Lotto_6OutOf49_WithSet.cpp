
#include "stdafx.h"
#include<iostream>
#include <stdlib.h>
#include <cstdlib> 
#include <fstream>
#include <time.h>
#include <set>
#include <algorithm>
using namespace std;

// ===================================================================================

const int howManyNumbersDoWeWant = 3;
int maximumNumberOfDigitsPerKey = 6;

clock_t tStart;
int markThatHashCode1IsEnough = -1;
long long int howManyOptionsUpToNow = 0;
ofstream outfile("new.txt");
set<int> memoization[999999];

void afisareNumereConsola(int numere[])
{
	for (int i = 0; i <= howManyNumbersDoWeWant; i++)
		cout << numere[i] << " ";
	cout << endl;

}

void afisareNumereInFisier(int numere[])
{
	for (int i = 0; i <= howManyNumbersDoWeWant; i++)
		outfile << numere[i] << " ";
	outfile << endl;

}

void updateMemoization(int hashCode1, int hashCode2)
{
	if (hashCode2 != 0)
		memoization[hashCode1].insert(hashCode2);
	else
		memoization[hashCode1].insert(markThatHashCode1IsEnough);
	howManyOptionsUpToNow++;
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

bool isNewToMemoizationTable(set<int> variantaCurentaSortata)
{
	int howManyDigits = 0;
	int hashCode1 = 0;
	int hashCode2 = 0;
	for (set<int>::iterator it = variantaCurentaSortata.begin(); it != variantaCurentaSortata.end(); ++it)
	{
		if ((howManyDigits <= maximumNumberOfDigitsPerKey - 2) || (howManyDigits == maximumNumberOfDigitsPerKey - 1 && *it < 10))
			buildHashCode(*it, hashCode1, howManyDigits);
		else
			buildHashCode(*it, hashCode2, howManyDigits);
	}
	//cout << hashCode1 << " " << hashCode2 << endl;
	if (hashCode2 == 0 && !memoization[hashCode1].empty() && memoization[hashCode1].find(markThatHashCode1IsEnough) != memoization[hashCode1].end())
		return false;
	else
	{
		if (!memoization[hashCode1].empty() && memoization[hashCode1].find(hashCode2) != memoization[hashCode1].end())
			return false;
		else
		{
			updateMemoization(hashCode1, hashCode2);
			return true;
		}
	}
}


void backtracking(set<int> variantaCurenta)
{
	if (variantaCurenta.size() == howManyNumbersDoWeWant + 1)
	{
		if (isNewToMemoizationTable(variantaCurenta))
			if (howManyOptionsUpToNow % 10000 == 0)
				printf("Time taken: %.2fs .... and we had %llu options !\n", (double)(clock() - tStart) / CLOCKS_PER_SEC, howManyOptionsUpToNow);
	}
	else
	{
		for (int i = 1; i <= 49; i++)
		{
			if (variantaCurenta.find(i) == variantaCurenta.end())
			{
				variantaCurenta.insert(i);
				backtracking(variantaCurenta);
				variantaCurenta.erase(i);
			}
		}
	}
}


// ===================================================================================


int _tmain(int argc, _TCHAR* argv[])
{

	tStart = clock();

	set<int> lotoNumbers;
	backtracking(lotoNumbers);
	cout << "====================================================" << endl << endl;
	printf("Time taken: %.2fs\nAnd we had %llu options !\n\n", (double)(clock() - tStart) / CLOCKS_PER_SEC, howManyOptionsUpToNow);

	return 0;
}

