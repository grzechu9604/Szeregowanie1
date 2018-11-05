﻿using System;
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
            var upperBounds = new Dictionary<Tuple<int, double, int>, int>
            {
                { new Tuple<int, double, int>(10, 0.2, 1), 1936 },
                { new Tuple<int, double, int>(10, 0.2, 2), 1042 },
                { new Tuple<int, double, int>(10, 0.2, 3), 1586 },
                { new Tuple<int, double, int>(10, 0.2, 4), 2139 },
                { new Tuple<int, double, int>(10, 0.2, 5), 1187 },
                { new Tuple<int, double, int>(10, 0.2, 6), 1521 },
                { new Tuple<int, double, int>(10, 0.2, 7), 2170 },
                { new Tuple<int, double, int>(10, 0.2, 8), 1720 },
                { new Tuple<int, double, int>(10, 0.2, 9), 1574 },
                { new Tuple<int, double, int>(10, 0.2, 10), 1869 },
                { new Tuple<int, double, int>(100, 0.4, 1), 89588       },
                { new Tuple<int, double, int>(100, 0.4, 2), 74854       },
                { new Tuple<int, double, int>(100, 0.4, 3), 85363       },
                { new Tuple<int, double, int>(100, 0.4, 4), 87730       },
                { new Tuple<int, double, int>(100, 0.4, 5), 76424       },
                { new Tuple<int, double, int>(100, 0.4, 6), 86724       },
                { new Tuple<int, double, int>(100, 0.4, 7), 79854       },
                { new Tuple<int, double, int>(100, 0.4, 8), 95361       },
                { new Tuple<int, double, int>(100, 0.4, 9), 73605       },
                { new Tuple<int, double, int>(100, 0.4, 10), 72399       },
                { new Tuple<int, double, int>(500, 0.6, 1), 1581233        },
                { new Tuple<int, double, int>(500, 0.6, 2), 1715332        },
                { new Tuple<int, double, int>(500, 0.6, 3), 1644947        },
                { new Tuple<int, double, int>(500, 0.6, 4), 1640942        },
                { new Tuple<int, double, int>(500, 0.6, 5), 1468325        },
                { new Tuple<int, double, int>(500, 0.6, 6), 1413345        },
                { new Tuple<int, double, int>(500, 0.6, 7), 1634912        },
                { new Tuple<int, double, int>(500, 0.6, 8), 1542090        },
                { new Tuple<int, double, int>(500, 0.6, 9), 1684055        },
                { new Tuple<int, double, int>(500, 0.6, 10), 1520515        },
                { new Tuple<int, double, int>(1000, 0.8, 1), 6411581 },
                { new Tuple<int, double, int>(1000, 0.8, 2), 6112598 },
                { new Tuple<int, double, int>(1000, 0.8, 3), 5985538 },
                { new Tuple<int, double, int>(1000, 0.8, 4), 6096729 },
                { new Tuple<int, double, int>(1000, 0.8, 5), 6348242 },
                { new Tuple<int, double, int>(1000, 0.8, 6), 6082142 },
                { new Tuple<int, double, int>(1000, 0.8, 7), 6575879 },
                { new Tuple<int, double, int>(1000, 0.8, 8), 6069658 },
                { new Tuple<int, double, int>(1000, 0.8, 9), 6188416 },
                { new Tuple<int, double, int>(1000, 0.8, 10), 6147295 }

            };

            var veryfier = new InstanceVerifier();

            var instances = new List<Instance>();
            var fileNames = new List<string>
            {
                "sch10.txt", "sch100.txt", "sch500.txt", "sch1000.txt"
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
                new NaiveInstanceSolver()
            };
            
            var hList = new List<double>
            {
                0.2
            };

            Console.WriteLine($"ilość zadań " +
                            $"| K" +
                            $"| F biblioteki " +
                            $"| F obliczone " +
                            $"| błąd %" +
                            $"| t (s)");
            Solve("sch10", 0.2, instances, upperBounds);
            Console.Clear();

            Solve("sch10", 0.2, instances, upperBounds);
            Solve("sch100", 0.4, instances, upperBounds);
            Solve("sch500", 0.6, instances, upperBounds);
            Solve("sch1000", 0.8, instances, upperBounds);

            //Parallel.ForEach(solvers, solver =>
            //{
            //    instances.ToList().ForEach(i =>
            //    {
            //        hList.ForEach(h =>
            //        {
            //            var stopwatch = new Stopwatch();
            //            stopwatch.Start();
            //            var result = solver.Solve(i, h);
            //            stopwatch.Stop();
            //            var elapsedTime = stopwatch.ElapsedTicks / (double)TimeSpan.TicksPerMillisecond;
            //            var upperBound = upperBounds[new Tuple<int, double, int>(i.Tasks.Count, h, i.K)];
            //            double mistakeRate = (result.Value - upperBound) / (double)upperBound;

            //            SolvedInstanceWriter.Write(result);

            //            Console.WriteLine($"{i.Tasks.Count} " +
            //                $"| {i.K} " +
            //                $"| {upperBound} " +
            //                $"| {result.Value} " +
            //                $"| {mistakeRate}" +
            //                $"| {elapsedTime}");
            //        });
            //    });
            //});

            ////var toVerifyList = new List<string>
            ////{
            ////    "sch10_10_2.txt", "sch20_10_2.txt", "sch50_10_2.txt", "sch100_10_2.txt", "sch200_10_2.txt", "sch500_10_2.txt", "sch1000_10_2.txt"
            //};

            //var verifier = new InstanceVerifier();
            //toVerifyList.ForEach(file => Verify(file, verifier));
        }

        static void Solve(string filePath, double h, List<Instance> instances, Dictionary<Tuple<int, double, int>, int> upperBounds)
        {
            var solver = new NaiveInstanceSolver();
            instances.Where(i => i.FileNameWithoutExtension.Equals(filePath)).ToList().ForEach(i =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var result = solver.Solve(i, h);
                stopwatch.Stop();
                var elapsedTime = stopwatch.ElapsedTicks / (double)TimeSpan.TicksPerMillisecond;
                var upperBound = upperBounds[new Tuple<int, double, int>(i.Tasks.Count, h, i.K)];
                double mistakeRate = (result.Value - upperBound) / (double)upperBound *100;

                SolvedInstanceWriter.Write(result);

                Console.WriteLine($"{i.Tasks.Count} " +
                        $"| {i.K} " +
                        $"| {upperBound} " +
                        $"| {result.Value} " +
                        $"| {mistakeRate}" +
                        $"| {elapsedTime}");
            });

        }

        static void Verify(string fileName, InstanceVerifier verifier)
        {
            verifier.FileName = fileName;
            Console.WriteLine(verifier.IsValid());
        }
    }
}
