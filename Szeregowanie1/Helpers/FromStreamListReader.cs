using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Szeregowanie1.Helpers
{
    class FromStreamListReader
    {
        public static List<int> ReadIntList(StreamReader sr)
        {
            var line = sr.ReadLine();
            var elements = line.Split(' ').Where(e => !string.IsNullOrWhiteSpace(e));
            return elements.Select(e => Convert.ToInt32(e)).ToList();
        }
    }
}
