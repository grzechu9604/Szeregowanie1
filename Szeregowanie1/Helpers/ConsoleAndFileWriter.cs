using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szeregowanie1.Helpers
{
    class ConsoleAndFileWriter
    {
        private const string _fileName = "result.txt";

        public static void WriteLine(string line)
        {
            Console.WriteLine(line);
            using (var streamWriter = new StreamWriter(_fileName, true))
            {
                streamWriter.WriteLine(line);
            }
        }
    }
}
