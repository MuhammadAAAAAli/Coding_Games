
// A ray of light comes in from the upper left corner of an MxN sized window, with 45˚ angled edges. 
// The ray is reflected when it reaches the first or the last line, or the first or last column respectively. 
// Display all the positions reached until the ray travels to a window corner.


using System;
using System.Text;

namespace LightBeamPath
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LightBeamPath.Check();
        }
    }
    
    public class LightBeamPath
    {
        private const bool EmptyMatrix = true;
        private static int _moveNumber = 1;

        public static void Check()
        {
            const int row = 10;
            const int colomns = 5;
            var matrix = new int[row, colomns];
            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < colomns; j++)
                {
                    matrix[i, j] = 0;
                }
            }

            matrix[0, 0] = _moveNumber;
            var down = true;
            var right = true;
            var currentColomn = 0;
            var currentRow = 0;

            while (BeamIsNotInACorner(currentColomn, colomns, currentRow, row))
            {
                SetDirection(currentRow, row, currentColomn, colomns, ref down, ref right);
                DoNextMove(down, right, matrix, ref currentRow, ref currentColomn, ref _moveNumber);
            }
            CreatePrintableStringBuilder(row, colomns, matrix);
        }

        private static void CreatePrintableStringBuilder(int row, int colomns, int[,] matrix)
        {
            var toPrint = new StringBuilder();

            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < colomns; j++)
                {
                    toPrint.Append(matrix[i, j] < 10
                        ? string.Format("  {0}  |", matrix[i, j])
                        : string.Format("  {0} |", matrix[i, j]));
                }
                var minus = new StringBuilder();
                for (var k = 0; k < 6 * colomns; k++)
                {
                    minus.Append("-");
                }
                toPrint.Append("\n" + minus + "\n");
            }
            Console.Out.WriteLine(toPrint);
        }

        private static void DoNextMove(bool down, bool right, int[,] matrix, ref int currentRow, ref int currentColomn,
            ref int moveNumber)
        {
            ++moveNumber;
            if (down && right)
                matrix[++currentRow, ++currentColomn] = CheckCellIfEmpty(matrix, currentRow, currentColomn, moveNumber);
            else if (!down && right)
                matrix[--currentRow, ++currentColomn] = CheckCellIfEmpty(matrix, currentRow, currentColomn, moveNumber);
            else if (!down && !right)
                matrix[--currentRow, --currentColomn] = CheckCellIfEmpty(matrix, currentRow, currentColomn, moveNumber);
            else if (down && !right)
                matrix[++currentRow, --currentColomn] = CheckCellIfEmpty(matrix, currentRow, currentColomn, moveNumber);
        }

        private static int CheckCellIfEmpty(int[,] matrix, int currentRow, int currentColomn, int moveNumber)
        {
            return matrix[currentRow, currentColomn] == 0 ? moveNumber : matrix[currentRow, currentColomn];
        }

        private static void SetDirection(int currentRow, int row, int currentColomn, int colomns, ref bool down,
            ref bool right)
        {
            if (currentRow == row - 1)
                down = false;
            else if (currentRow == 0)
                down = true;
            else if (currentColomn == colomns - 1)
                right = false;
            else if (currentColomn == 0)
                right = true;
        }

        private static bool BeamIsNotInACorner(int currentColomn, int colomns, int currentRow, int lines)
        {
            return !(IsTopRightCorner(currentColomn, colomns, currentRow) ||
                     IsBottomRightCorner(currentColomn, colomns, currentRow, lines) ||
                     IsBottomLeftCorner(currentColomn, currentRow, lines) ||
                     IsTopLeftCorner(currentColomn, currentRow));
        }

        private static bool IsBottomLeftCorner(int currentColomn, int currentRow, int lines)
        {
            return currentColomn == 0 && currentRow == lines - 1;
        }

        private static bool IsTopLeftCorner(int currentColomn, int currentRow)
        {
            return currentColomn == 0 && currentRow == 0 && !EmptyMatrix;
        }

        private static bool IsBottomRightCorner(int currentColomn, int colomns, int currentRow, int lines)
        {
            return currentColomn == colomns - 1 && currentRow == lines - 1;
        }

        private static bool IsTopRightCorner(int currentColomn, int colomns, int currentRow)
        {
            return currentColomn == colomns - 1 && currentRow == 0;
        }
    }
}
