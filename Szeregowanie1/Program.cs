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
            var upperBounds = new Dictionary<Tuple<int, double>, int>
            {
                { new Tuple<int, double>(10, 0.2), 1869 },
                { new Tuple<int, double>(20, 0.2), 5545 },
                { new Tuple<int, double>(50, 0.2), 35797 },
                { new Tuple<int, double>(100, 0.2), 124446 },
                { new Tuple<int, double>(200, 0.2), 572886 },
                { new Tuple<int, double>(500, 0.2), 3315019 },
                { new Tuple<int, double>(1000, 0.2), 13395234 }

            };

            var veryfier = new InstanceVerifier();

            var instances = new List<Instance>();
            var fileNames = new List<string>
            {
                "sch10.txt", "sch20.txt", "sch50.txt", "sch100.txt", "sch200.txt", "sch500.txt", "sch1000.txt"
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
                //new BaseInstanceSolver(),
                new NaiveInstanceSolver()
            };

            var hList = new List<double>
            {
                0.2
            };

            Console.WriteLine($"ilość zadań " +
                            $"| F biblioteki " +
                            $"| F obliczone " +
                            $"| błąd %" +
                            $"| t (ms)");

            Parallel.ForEach(solvers, solver =>
            {
                instances.Where(i => i.K.Equals(10)).ToList().ForEach(i =>
                {
                    hList.ForEach(h =>
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();
                        var result = solver.Solve(i, h);
                        stopwatch.Stop();
                        var elapsedTime = stopwatch.ElapsedTicks / (double)TimeSpan.TicksPerMillisecond;
                        var upperBound = upperBounds[new Tuple<int, double>(i.Tasks.Count, h)];
                        double mistakeRate = (result.Value - upperBound) / (double)upperBound;

                        SolvedInstanceWriter.Write(result);
                        
                        Console.WriteLine($"{i.Tasks.Count} " +
                            $"| {upperBound} " +
                            $"| {result.Value} " +
                            $"| {mistakeRate}" +
                            $"| {elapsedTime}");
                    });
                });
            });

            var toVerifyList = new List<string>
            {
                "sch10_10_2.txt", "sch20_10_2.txt", "sch50_10_2.txt", "sch100_10_2.txt", "sch200_10_2.txt", "sch500_10_2.txt", "sch1000_10_2.txt"
            };

            var verifier = new InstanceVerifier();
            toVerifyList.ForEach(file => Verify(file, verifier));
        }

        static void Verify(string fileName, InstanceVerifier verifier)
        {
            verifier.FileName = fileName;
            Console.WriteLine(verifier.IsValid());
        }
    }
}
