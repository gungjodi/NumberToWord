using System;
using System.Globalization;

namespace TechOneTest
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isExit = false;
            bool isNegative = false;

            while (isExit == false)
            {
                Console.Write("Input digit : ");
                decimal value;
                if (Decimal.TryParse(Console.ReadLine(), out value))
                {
                    if (value.Equals("exit"))
                    {
                        isExit = true;
                    }
                    else if (!NumToWord.validCent(value.ToString()))
                    {
                        Console.WriteLine("Wrong Cents Input");
                    }
                    else
                    {
                        try
                        {
                            if (value < 0)
                            {
                                isNegative = true;
                            }
                            Console.WriteLine("{0}", NumToWord.convertNum(value.ToString("F", new CultureInfo("en-US").NumberFormat), isNegative));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("false");
                }
                Console.WriteLine("");
            }
        }
    }

    class NumToWord
    {
        private static String[] beyondStrings = { "", " THOUSAND ", " MILLION ", " BILLION ", " TRILLION " };
        private static String[] tenStrings = {"","TEN","TWENTY","THIRTY","FORTY","FIFTY","SIXTY","SEVENTY",
                                              "EIGHTY","NINETY"};
        private static String[] numStrings = {"","ONE","TWO","THREE","FOUR","FIVE","SIX","SEVEN","EIGHT",
                                              "NINE","TEN","ELEVEN","TWELVE","THIRTEEN","FOURTEEN","FIFTEEN",
                                              "SIXTEEN","SEVENTEEN","EIGHTEEN","NINETEEN"};

        public static string processNum(int number, int isCent)
        {
            string val = "";

            if (number % 100 < 20)
            {
                val = numStrings[number % 100];
                number /= 100;
            }
            else
            {
                val = numStrings[number % 10];
                number /= 10;

                val = tenStrings[number % 10] + (val == "" ? "" : "-") + val;
                number /= 10;
            }
            if (isCent == 1)
            {
                return val + " CENTS";
            }
            if (number == 0)
            {
                return val + "";
            }
            return numStrings[number] + " HUNDRED " + (val == "" ? "" : "AND ") + val + "";
        }

        public static string processCent(int number)
        {
            if (number > 0)
            {
                return processNum(number, 1);
            }
            return "";
        }

        public static bool validCent(string num)
        {
            string[] checkArr = num.Split('.');
            if (checkArr.Length > 1)
            {
                if (checkArr[1].Length > 2)
                {
                    return false;
                }
            }
            return true;
        }

        public static string convertNum(string number, bool isNegative)
        {
            string negative = "";
            string cent = "";
            string final = "";

            string[] parts = number.Split('.');
            int num = int.Parse(parts[0]);

            if (isNegative)
            {
                negative = "NEGATIVE ";
                num = -num;
            }

            int dec = 0;
            if (parts.Length > 1)
            {
                dec = int.Parse(parts[1]);
            }

            int n = num % 1000;
            for (int i = 0; num > 0; i++)
            {
                if (n != 0)
                {
                    string s = processNum(n, 0);
                    final = s + beyondStrings[i] + final;
                }

                num = (num / 1000);
                n = num % 1000;
            }

            if (dec > 0)
            {
                cent += processNum(dec, 1);
            }
            if (dec > 0)
            {
                if (final.Equals(""))
                {
                    return (negative + cent).Trim();
                }
                return (negative + final + " DOLLARS" + " AND " + cent).Trim();
            }
            if (final.Equals(""))
            {
                return "Zero" + (cent).Trim();
            }
            return (negative + final + " DOLLARS " + cent).Trim();
        }
    }
}
