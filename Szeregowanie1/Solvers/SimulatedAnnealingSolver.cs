using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szeregowanie1.DataTypes;
using Szeregowanie1.Helpers;

namespace Szeregowanie1.Solvers
{
    class SimulatedAnnealingSolver : IInstanceSolver
    {
        public int TabuListLength { get; set; } = 1000;
        public int MaxTimeOfProcessingInSeconds { get; set; } = 30;
        public int MaxAmountOfFailedLoops { get; set; } = 1000;

        public SolvedInstance Solve(Instance instance, double h)
        {
            var beginningSolution = CreateStartingPoint(instance, h);
            return SolveUsingSimulatedAnnealing(beginningSolution, h);
        }

        private SolvedInstance CreateStartingPoint(Instance instance, double h)
        {
            var naiveSolver = new NaiveInstanceSolver();
            return naiveSolver.Solve(instance, h);
        }

        public SolvedInstance SolveUsingSimulatedAnnealing(SolvedInstance beginningSolution, double h)
        {
            var tabuList = new TabuList(TabuListLength);

            var stopwatch = new Stopwatch();

            var bestSolution = beginningSolution;

            stopwatch.Start();
            while (stopwatch.Elapsed.Seconds < MaxTimeOfProcessingInSeconds)
            {
                SolvedInstance newSolution = RandomInstanceCreator.Generate(beginningSolution.Instance, h);

                int infinityLoopBreaker = 0;
                while (tabuList.IsOnTabuList(newSolution))
                {
                    newSolution = RandomInstanceCreator.Generate(beginningSolution.Instance, h);
                    if (infinityLoopBreaker++ > MaxAmountOfFailedLoops)
                    {
                        break;
                    }
                }

                tabuList.AddToTabuList(newSolution);

                if (newSolution.Value < bestSolution.Value)
                {
                    bestSolution = newSolution;
                }
            }

            return bestSolution;
        }
    }
}
