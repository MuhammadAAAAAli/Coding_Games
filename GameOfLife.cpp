// The universe of the Game of Life is an infinite, two-dimensional orthogonal grid of square cells, each of which is in one of two possible states, live or dead, (or populated and unpopulated, respectively). Every cell interacts with its eight neighbours, which are the cells that are horizontally, vertically, or diagonally adjacent. At each step in time, the following transitions occur:
// Any live cell with fewer than two live neighbours dies, as if by underpopulation.
// Any live cell with two or three live neighbours lives on to the next generation.
// Any live cell with more than three live neighbours dies, as if by overpopulation.
// Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.

#include "stdafx.h"
#include<iostream>
#include<random>
#include <chrono>
#include <thread>

using namespace std;
int a[100][100], n = 25, m = 25;
int random(int min, int max) //range : [min, max)
{
	static bool first = true;
	if (first)
	{
		srand(time(NULL)); //seeding for the first time only!
		first = false;
	}
	return min + rand() % (max - min);
}

int vecini(int k, int h)
{
	int nr = 0;
	for (int i = -1; i < 2; i++)
		for (int j = -1; j < 2; j++)
			if (a[k + i][h + j] == 1 && !(i == 0 && j == 0))
				nr++;
	return nr;
}

void afisare() 
{
	for (int i = 0; i < n; i++)
	{
		for (int j = 0; j < m; j++)
		{
			if (a[i][j] == 1)
				cout << " * ";
			else
				cout << "   ";
		}
		cout << endl;
	}
}

void primaInitializare() 
{
	for (int i = 0; i<n; i++)
		for (int j = 0; j < m; j++)
		{
			int nr = random(1, 100);
			if (nr % 3 == 0)
			{
				a[i][j] = 1;
			}
			else
			{
				a[i][j] = 0;
			}
		}
}

void NextGeneration() 
{
	int b[100][100];
	for (int i = 0; i < n; i++)
		for (int j = 0; j < m; j++)
			b[i][j] = 0;
	for (int i = 1; i<n - 2; i++)
		for (int j = 1; j < m - 2; j++)
		{
			if (a[i][j] == 1)
			{
				if (vecini(i, j) < 2)
					b[i][j] = 0;
				else if (vecini(i, j) == 2 || vecini(i, j) == 3)
					b[i][j] = 1;
				else if (vecini(i, j) > 3)
					b[i][j] = 0;
			}
			else
				if (vecini(i, j) == 3)
					b[i][j] = 1;
		}


	for (int i = 0; i < n; i++)
		for (int j = 0; j < m; j++)
			a[i][j] = b[i][j];
}


int main()
{
	primaInitializare();
	afisare();
	while (true)
	{
		NextGeneration();
		std::this_thread::sleep_for(std::chrono::milliseconds(1000));
		system("CLS");
		afisare();
	}

	

    return 0;
}

