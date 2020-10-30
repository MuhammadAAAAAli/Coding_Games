/*
Which is the minimum number of squares you can split a given rectangle into? 
Observation: please keep in mind that the entire surface of the rectangle has to be split into squares.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace MinSquaresFromARectangle
{
    public class MinimumSquares
    {
        private static readonly Dictionary<Tuple<int, int>, int> HashTable = new Dictionary<Tuple<int, int>, int>();

        private static void Main(string[] args)
        {
            if (args.FirstOrDefault() != null)
                Console.Out.WriteLine("Minimum number of squares for width {0} and height {1} is : {2}", args[0],
                    args[1],
                    Calculate(int.Parse(args[0]), int.Parse(args[1])));
            else
                Console.Out.WriteLine("Minimum number of squares for width {0} and height {1} is : {2}", 25, 76,
                    Calculate(25, 76));
        }

        public static int Calculate(int width, int height)
        {
            return CalculateInternal(width, height).ToList().Min();
        }

        public static IEnumerable<int> CalculateInternal(int width, int height)
        {
            if (SituationAlreadyCalculated(width, height) != -1)
                yield return SituationAlreadyCalculated(width, height);
            else
            {
                var x = GetMinimumNumberOfSquares(width, height);
                if (x == -1)
                {
                    for (var i = 1; i < (width > height ? height : width); i++)
                    {
                        var optionOne = 1 + CalculateInternal(i, height - i).ToList().Min() +
                                        CalculateInternal(width - i, height).ToList().Min();
                        var optionTwo = 1 + CalculateInternal(width - i, i).ToList().Min() +
                                        CalculateInternal(width, height - i).ToList().Min();
                        yield return optionOne;
                        yield return optionTwo;
                        RememberOption(width, height, optionOne < optionTwo ? optionOne : optionTwo);
                    }
                }
                else
                    yield return x;
            }
        }

        private static int SituationAlreadyCalculated(int width, int height)
        {
            OrderHeightAndWeightForHashTable(ref width, ref height);
            if (HashTable.ContainsKey(Tuple.Create(width, height)))
                return HashTable[Tuple.Create(width, height)];
            return -1;
        }

        private static void RememberOption(int width, int height, int minimumMoves)
        {
            OrderHeightAndWeightForHashTable(ref width, ref height);
            if (!HashTable.ContainsKey(Tuple.Create(width, height)))
                HashTable.Add(Tuple.Create(width, height), minimumMoves);
            else if (HashTable[Tuple.Create(width, height)] > minimumMoves)
                HashTable[Tuple.Create(width, height)] = minimumMoves;
        }

        private static void OrderHeightAndWeightForHashTable(ref int width, ref int height)
        {
            var index = height;
            if (width >= height) return;
            height = width;
            width = index;
        }

        private static int GetMinimumNumberOfSquares(int width, int height)
        {
            if (width == 0 || height == 0)
                return 0;
            if (width == height)
                return 1;
            if ((width > height ? width%height : height%width) == 0)
                return (width > height ? width/height : height/width);
            if (width == 1)
                return height;
            if (height == 1)
                return width;
            return -1;
        }
    }
}
