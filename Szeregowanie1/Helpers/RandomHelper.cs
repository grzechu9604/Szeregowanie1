using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szeregowanie1.Helpers
{
    static class RandomHelper
    {
        private static Random _random = new Random(0);
        
        public static double GetRandomDouble()
        {
            return _random.NextDouble();
        }

        public static int GetRandomInt(int min, int max)
        {
            return _random.Next(min, max);
        }

        public static int GetRandomInt(int max)
        {
            return _random.Next(max);
        }
    }
}
