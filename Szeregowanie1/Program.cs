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

            var verifier = new InstanceVerifier();

            Verify("sch10_1_2.txt", verifier);
            Verify("sch10_2_2.txt", verifier);
            Verify("sch10_3_2.txt", verifier);
            Verify("sch10_4_2.txt", verifier);
            Verify("sch10_5_2.txt", verifier);
            Verify("sch10_6_2.txt", verifier);
            Verify("sch10_7_2.txt", verifier);
            Verify("sch10_8_2.txt", verifier);
            Verify("sch10_9_2.txt", verifier);
            Verify("sch10_10_2.txt", verifier);

        }

        static void Verify(string fileName, InstanceVerifier verifier)
        {
            verifier.FileName = fileName;
            Console.WriteLine(verifier.IsValid());
        }
    }
}
