using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Feistel
{
    class Feistel
    {
        private const int sizeOfBlock = 64; //размер одного блока
        private const int sizeOfChar = 16; //размер одного символа (in Unicode 16 bit)

        bool crypt; //true - шифрование
                    //false - расшифровка

        private const int quantityOfRounds = 16; //количество раундов

        string[] Blocks; //сами блоки в двоичном формате

        private string Key;
        List<string> FileStr;

        public Feistel(string key, List<string> FileStr, bool crypt)
        {
            this.FileStr = FileStr;
            this.crypt = crypt;
            Key = StringToBinaryFormat(key);
            Key = CorrectKeyWord(Key, 64);
        }

        private string CorrectKeyWord(string input, int lengthKey) // Метод, доводящий длину ключа до нужной длины.
        {
            if (input.Length > lengthKey)
                input = input.Substring(0, lengthKey);
            else
                while (input.Length < lengthKey)
                    input = "0" + input;

            return input;
        }

        private string StringToRightLength(string input) //Метод, доводящий строку до такого размера, чтобы она делилась на размер блока. 
        {
            while (((input.Length * sizeOfChar) % sizeOfBlock) != 0)
                input += "#";                           //Размер увеличивается с помощью добавления к исходной строке символа “решетка”.

            return input;
        }

        private string StringToBinaryFormat(string input) //Метод, переводящий строку в двоичный формат.
        {
            string output = "";

            for (int i = 0; i < input.Length; i++)
            {
                string char_binary = Convert.ToString(input[i], 2); //переводит символ в двоичный код

                while (char_binary.Length < sizeOfChar) //добавляет в начало 0, чтобы символ был равен sizeOfChar
                    char_binary = "0" + char_binary;

                output += char_binary;

                byte[] buff = Encoding.Unicode.GetBytes(output);
                byte[] buff1 = Encoding.Unicode.GetBytes("C");
                BitArray B = new BitArray(buff1);
                byte[] buff2 = Encoding.Unicode.GetBytes("B");
                BitArray B2 = new BitArray(buff1);
                BitArray b = B.Xor(B2);


            }
            return output;
        }

        private void CutBinaryStringIntoBlocks(string input) // Метод, разбивающий строку в двоичном формате на блоки.
        {
            Blocks = new string[input.Length / sizeOfBlock];

            int lengthOfBlock = input.Length / Blocks.Length;

            for (int i = 0; i < Blocks.Length; i++)
                Blocks[i] = input.Substring(i * lengthOfBlock, lengthOfBlock);
        }

        private string CryptionOneRound(string input, int i)
        {
            string L = input.Substring(0, input.Length / 2);
            string R = input.Substring(input.Length / 2, input.Length / 2);
            if (crypt)
                return Logics.XOR(R, KeyToNextRound(i)) + Logics.XOR(L, F(Logics.XOR(R, KeyToNextRound(i))));
            else
                return Logics.XOR(F(L), R) + Logics.XOR(L, KeyToNextRound(i));
        }

        private string F(string input) //Функция F
        {
            string output = Logics.CyclicShiftRight(input.Substring(0, 8), 3) +
                Logics.XOR(input.Substring(15, 8), input.Substring(16, 8)) +
                Logics.CyclicShiftLeft(input.Substring(8, 8), 5) +
                Logics.NOT(input.Substring(24, 8));
            return output;
        }

        private string KeyToNextRound(int i) //Вычисление ключа для следующего раунда 
        {
            string key = "";
            string KeyCSR = "";
            if (crypt)
                KeyCSR = Logics.CyclicShiftRight(Key, (i + 1) * 3);
            else
                KeyCSR = Logics.CyclicShiftRight(Key, (quantityOfRounds - i) * 3);
            for (int j = 0; j < Key.Length; j++)
                if (j % 2 == 1)
                    key += KeyCSR[j];
            return key;
        }
        private string StringFromBinaryToNormalFormat(string input) //Метод, переводящий строку с двоичными данными в символьный формат.
        {
            string output = "";

            while (input.Length > 0)
            {
                string char_binary = input.Substring(0, sizeOfChar);
                input = input.Remove(0, sizeOfChar);

                int a = 0;
                int degree = char_binary.Length - 1;

                foreach (char c in char_binary)
                    a += Convert.ToInt32(c.ToString()) * (int)Math.Pow(2, degree--);

                output += ((char)a).ToString();
            }
            while (output[output.Length - 1] == '#') //если последние символы решетки, то скорей всего те что мы добавляли при шифровании
                output = output.Remove(output.Length - 1, 1);

            return output;
        }

        public List<string> Cryption()
        {
            List<string> fileStr = new List<string>();
            foreach (string s in FileStr)
            {
                string result = "";
                if (s != "")
                {
                    string str = StringToRightLength(s);
                    str = StringToBinaryFormat(str);
                    CutBinaryStringIntoBlocks(str);

                    for (int j = 0; j < quantityOfRounds; j++)
                        for (int i = 0; i < Blocks.Length; i++)
                            Blocks[i] = CryptionOneRound(Blocks[i], j);

                    for (int i = 0; i < Blocks.Length; i++)
                        result += Blocks[i];
                    result = StringFromBinaryToNormalFormat(result);
                }
                fileStr.Add(result);
            }
            return fileStr;
        }
    }
}
