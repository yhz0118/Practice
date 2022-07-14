using System;


namespace MinMax
{

    class Program
    {
        public static int[] minmax(int min, int max)
        {
            int[] res = new int[2];

            if (min > max)
            {
                res[0] = max;
                res[1] = min;
            }

            else
            {
                res[0] = min;
                res[1] = max;
            }

            return res;
        }


        public static void Main(string[] args)
        {
            int[] result = Program.minmax(2, 7);

            Console.WriteLine("Minimum value : {0}", result[0]);
            Console.WriteLine("Maximum value : {0}", result[1]);
        }
    }
}