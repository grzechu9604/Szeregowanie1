using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Szeregowanie1.DataTypes;
using Szeregowanie1.Helpers;
using Szeregowanie1.Solvers;

namespace Szeregowanie1
{
    class Program
    {
        static void Main(string[] args)
        {
            var instances = new List<Instance>();
            string fileName = "sch10.txt";

            using (var sr = new StreamReader(fileName))
            {
                int instancesAmount = FromStreamListReader.ReadIntList(sr).First();
                var instanceReader = new InstanceReader()
                {
                    StreamReader = sr,
                    FileNameWithoutExtension = fileName.Split('.')[0]
                };

                for (int i = 0; i < instancesAmount; i++)
                {
                    instances.Add(instanceReader.ReadNext(i + 1));
                }
            }

            var solver = new InstanceSolver();
            instances.ForEach(i =>
            {
                var si = solver.Solve(i, 0.2);
                SolvedInstanceWriter.Write(si);
            });
        }
    }
}
