using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Szeregowanie1.DataTypes;
using Szeregowanie1.Helpers;

namespace Szeregowanie1
{
    class Program
    {
        static void Main(string[] args)
        {
            var instances = new List<Instance>();

            using (var sr = new StreamReader("sch10.txt"))
            {
                int instancesAmount = FromStreamListReader.ReadIntList(sr).First();
                var instanceReader = new InstanceReader() { StreamReader = sr };

                for (int i = 0; i < instancesAmount; i++)
                {
                    instances.Add(instanceReader.ReadNext());
                }
            }

            instances.ForEach(i => Console.WriteLine(i.Length));
        }
    }
}
