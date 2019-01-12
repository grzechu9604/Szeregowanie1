using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szeregowanie1.Helpers
{
    static class RandomHelper
    {
        private static readonly Random _random = new Random(1);
        private static readonly object _lock = new object();
        
        public static double GetRandomDouble()
        {
            lock(_lock)
            {
                return _random.NextDouble();
            }
        }

        public static int GetRandomInt(int min, int max)
        {
            lock (_lock)
            {
                return _random.Next(min, max);
            }
        }

        public static int GetRandomInt(int max)
        {
            lock (_lock)
            {
                return _random.Next(max);
            }
        }

        public static int GetRandomInt()
        {
            lock (_lock)
            {
                return _random.Next();
            }
        }
    }
}
