using System;
using Szeregowanie1.DataTypes;
using Szeregowanie1.Helpers;

namespace Szeregowanie1.Solvers
{
    class SimulatedAnnealingSolver : IInstanceSolver
    {
        public int TabuListLength { get; set; } = 10000;
        public int MaxTimeOfProcessingInSeconds { get; set; } = 50;

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
            var instanceCreator = new RandomInstanceCreator(TabuListLength);
            var bestSolution = beginningSolution;

            var start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalSeconds < MaxTimeOfProcessingInSeconds)
            {
                SolvedInstance newSolution = instanceCreator.Generate(bestSolution, h);

                if (newSolution.Value < bestSolution.Value)
                {
                    bestSolution = newSolution;
                }
            }

            return bestSolution;
        }
    }
}
