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
            var veryfier = new InstanceVerifier();

            Verify("sch10_1_2.txt", veryfier);
            Verify("sch10_10_8.txt", veryfier);
            Verify("sch50_2_2.txt", veryfier);
            Verify("sch50_9_8.txt", veryfier);
            Verify("sch1000_5_6.txt", veryfier);

            //var instances = new List<Instance>();
            //string fileName = "sch10.txt";

            //using (var sr = new StreamReader(fileName))
            //{
            //    int instancesAmount = FromStreamListReader.ReadIntList(sr).First();
            //    var instanceReader = new InstanceReader()
            //    {
            //        StreamReader = sr,
            //        FileNameWithoutExtension = fileName.Split('.')[0]
            //    };

            //    for (int i = 0; i < instancesAmount; i++)
            //    {
            //        instances.Add(instanceReader.ReadNext(i + 1));
            //    }
            //}

            //var solver = new InstanceSolver();


            //var solvedInstance = solver.SolveAlg(instances.First(i => i.K.Equals(1)), 0.2);
            //SolvedInstanceWriter.Write(solvedInstance);

        }

        static void Verify(string fileName, InstanceVerifier verifier)
        {
            verifier.FileName = fileName;
            Console.WriteLine(verifier.IsValid());
        }
    }
}
