using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class NumberToWord
    {
        static string[] units = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        static string[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        public static string ConvertNumberToString(int number)
        {
            if (number < 20)
                return units[number];

            if (number < 100)
                return tens[number / 10] + (number % 10 != 0 ? " " + units[number % 10] : "");

            if (number < 1000)
                return units[number / 100] + " Hundred" + (number % 100 != 0 ? " And " + ConvertNumberToString(number % 100) : "");

            if (number < 10000)
            {
                int thousands = number / 1000;
                int remainder = number % 1000;
                return units[thousands] + " Thousand" + (remainder != 0 ? " " + ConvertNumberToString(remainder) : "");
            }

            if (number < 100000)
            {
                int tenThousands = number / 10000;
                int remainder = number % 10000;
                return tens[tenThousands] + " Thousand" + (remainder != 0 ? " " + ConvertNumberToString(remainder) : "");
            }

            if (number < 1000000)
            {
                int hundredThousands = number / 100000;
                int remainder = number % 100000;
                return units[hundredThousands] + " Hundred Thousand" + (remainder != 0 ? " " + ConvertNumberToString(remainder) : "");
            }

            return "Number out of range";
        }
    }
}
