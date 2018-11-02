using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

            var instances = new List<Instance>();
            var fileNames = new List<string>
            {
                "sch10.txt", "sch50.txt", "sch1000.txt"
            };

            fileNames.ForEach(fileName =>
            {
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
            });

            var solvers = new List<IInstanceSolver>
            {
                new BaseInstanceSolver(),
                new NaiveInstanceSolver()
            };

            var hList = new List<double>
            {
                0.2, 0.4, 0.6, 0.8
            };

            Parallel.ForEach(solvers, solver =>
            {
                instances.ForEach(i =>
                {
                    hList.ForEach(h =>
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();
                        var result = solver.Solve(i, h);
                        stopwatch.Stop();
                        var elapsedTime = stopwatch.ElapsedTicks / (double)TimeSpan.TicksPerMillisecond;
                        Console.WriteLine($"{i.K} {h} {solver.GetType()} rezultat: {result.Value} ms: {elapsedTime}");
                    });
                });
            });


            //SolvedInstanceWriter.Write(solvedInstance);

        }

        static void Verify(string fileName, InstanceVerifier verifier)
        {
            verifier.FileName = fileName;
            Console.WriteLine(verifier.IsValid());
        }
    }
}
