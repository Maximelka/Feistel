using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feistel
{
    class Logics
    {
        public static string NOT(string input)
        {
            string output = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '1')
                    output += "0";
                else
                    output += "1";
            }
            return output;
        }

        public static string CyclicShiftRight(string input, int l) // Циклический сдвиг вправо
        {
            int L = input.Length;
            string output = input.Substring(L - l, l);
            for (int i = 0; i < L - l; i++)
                output += input[i];
            return output;
        }

        public static string CyclicShiftRight1(string input, int l) // Циклический сдвиг вправо
        {
            int L = input.Length;
            string output = "";
            for (int i = 0; i < l; i++)
            {
                output = input.Substring(L - 1, 1);
                for (int j = 0; j < L - 1; j++)
                    output += input[j];
                input = output;
                output = "";
            }
            return input;
        }

        public static string CyclicShiftLeft(string input, int l) //Циклический сдвиг влево
        {
            int L = input.Length;
            string output = "";
            for (int i = l; i < L; i++)
                output += input[i];
            output += input.Substring(0, l);
            return output;
        }
        public static string XOR(string s1, string s2) //XOR двух строк с двоичными данными.
        {
            string result = "";

            for (int i = 0; i < s1.Length; i++)
            {
                bool a = Convert.ToBoolean(Convert.ToInt32(s1[i].ToString()));
                bool b = Convert.ToBoolean(Convert.ToInt32(s2[i].ToString()));

                if (a ^ b)
                    result += "1";
                else
                    result += "0";
            }
            return result;
        }
    }
}
