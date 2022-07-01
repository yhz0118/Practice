using System;

// 2의 승수를 판별하는 함수를 작성하여보자
// Write a functino that classifies sqrt of 2
// solution: use AND gate operator to find 0 -> return true, else return false
// main: print the result value

namespace practice
{
    public static class Pow
    {
        public static bool PowOfTwo(this int self)
        {
            int output = self & (self - 1);

            return output == 0;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input the number: ");

            int num = int.Parse(Console.ReadLine());

            var result = Pow.PowOfTwo(num);

            Console.WriteLine(result);
        }
    }
}
