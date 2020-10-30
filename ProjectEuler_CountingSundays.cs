// How many Sundays fell on the first of the month during the twentieth century (1 Jan 1901 to 31 Dec 2000)?

using System;

namespace EulerProblem
{
    internal class Program
    {
        private static bool isLeapYear(int year)
        {
            for (var i = 1900; i < 2001; i = i + 4)
            {
                if (i == year)
                {
                    if (year % 100 == 0)
                    {
                        if (year % 400 == 0)
                            return true;
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        private static bool esteUltimaZiALuni(int day, int month, int year)
        {
            if (day == 31 &&
                (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12))
                return true;

            if (day == 30 && (month == 4 || month == 6 || month == 9 || month == 11))
                return true;

            if (month == 2)
            {
                if (isLeapYear(year) && day == 29)
                    return true;
                if (!isLeapYear(year) && day == 28)
                    return true;
            }
            return false;
        }


        private static void Main(string[] args)
        {
            int day = 0, month = 1, year = 1901, ziuaSaptamani = 0, count = 0;

            while (year < 2001)
            {
                day++;
                ziuaSaptamani++;
                if (ziuaSaptamani == 8)
                {
                    ziuaSaptamani = 1;
                }
                if (esteUltimaZiALuni(day, month, year))
                {
                    day = 1;
                    month++;
                    if (month == 13)
                    {
                        month = 1;
                        year++;
                    }
                }
                if (day == 1 && ziuaSaptamani == 7)
                {
                    count++;
                }
            }
            Console.WriteLine(count);
        }
    }
}
