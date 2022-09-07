using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace FileSystem
{

    class Program
    {

        public static void Write(Stream s, byte value)
        {
            var buf = new byte[1] { value };
            s.Write(buf, 0, buf.Length);
        }

        public static void Main()
        {
            string output = @"C:\Users\user\OneDrive\Desktop\FileSystem\output.bin";

            using (var fs = new FileStream(output, FileMode.Create))
            {
                for (int i = 0; i < 20; i++) Write(fs, 0);

                for (int i = 0; i < 30; i++) Write(fs, 1);

                for (int i = 0; i < 40; i++) Write(fs, 2);
            }
        }
    }
}
