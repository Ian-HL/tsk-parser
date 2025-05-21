
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSKParser.Test
{
    internal class Utils
    {

        internal static void CompareFiles(string filePathA, string filePathB)
        {
            using (FileStream fsA = new FileStream(filePathA, FileMode.Open, FileAccess.Read))
            using (FileStream fsB = new FileStream(filePathB, FileMode.Open, FileAccess.Read))
            {
                int byteA, byteB;
                long index = 0;

                while ((byteA = fsA.ReadByte()) != -1 && (byteB = fsB.ReadByte()) != -1)
                {
                    if (byteA != byteB)
                    {
                        Console.WriteLine($"Difference at index {index} (Decimal) - {index:X} (Hex): A = {byteA:X2}, B = {byteB:X2}");
                    }
                    index++;
                }

                // Check if one file is longer than the other
                if (fsA.ReadByte() != -1 || fsB.ReadByte() != -1)
                {
                    Console.WriteLine("Files have different lengths.");
                }
            }
        }
        // ... existing code ...
    }
}
