using System;
namespace practice
{
    class SpiralMatrix
    {
        private int m;
        private int n;
        public SpiralMatrix(int col, int row)
        {
            this.m = row;
            this.n = col;
        }
        public int[,] CreateSpiral()
        {
            int[,] spiral_mat = new int[n, m];
            int row = 0;
            int col = 0;
            //initialize the first direction of an arrorw
            string dir = "right";
            //starting from 1, increase 1 by row*column size
            for (int i = 1; i <= n * m; i++)
            {
                if (dir == "right" && (col > m - 1 || spiral_mat[row, col] != 0))
                {
                    dir = "down";
                    col--;
                    row++;
                }
                if (dir == "down" && (row > n - 1 || spiral_mat[row, col] != 0))
                {
                    dir = "left";
                    row--;
                    col--;
                }
                if (dir == "left" && (col < 0 || spiral_mat[row, col] != 0))
                {
                    dir = "up";
                    col++;
                    row--;
                }
                if (dir == "up" && row < 0 || spiral_mat[row, col] != 0)
                {
                    dir = "right";
                    row++;
                    col++;
                }
                spiral_mat[row, col] = i;

                if (dir == "right")
                {
                    col++;
                }
                if (dir == "down")
                {
                    row++;
                }
                if (dir == "left")
                {
                    col--;
                }
                if (dir == "up")
                {
                    row--;
                }
            }
            return spiral_mat;
        }
        public void DisplayMat(int[,] spiral_mat)
        {
            // display spiral matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write("{0,4}", spiral_mat[i, j]);
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //input n and m
            Console.Write("Enter column: ");
            int n = int.Parse(Console.ReadLine());
            Console.Write("Enter row: ");

            int m = int.Parse(Console.ReadLine());

            var spiral_mat = new SpiralMatrix(n, m);
            var result_mat = spiral_mat.CreateSpiral();

            spiral_mat.DisplayMat(result_mat);
        }
    }
}