using System;
using Szeregowanie1.DataTypes;
using Szeregowanie1.Helpers;

namespace Szeregowanie1.Solvers
{
    class TabuSearchSolver : IInstanceSolver
    {
        public int TabuListLength { get; set; } = 10000;
        public int MaxTimeOfProcessingInSeconds { get; set; } = 50;

        public int MaxStepsWithoutImprovement { get; set; } = 12; 

        public SolvedInstance Solve(Instance instance, double h)
        {
            var beginningSolution = CreateStartingPoint(instance, h);
            return SolveUsingTabuSearch(beginningSolution, h);
        }

        private SolvedInstance CreateStartingPoint(Instance instance, double h)
        {
            var naiveSolver = new NaiveInstanceSolver();
            return naiveSolver.Solve(instance, h);
        }

        public SolvedInstance SolveUsingTabuSearch(SolvedInstance beginningSolution, double h)
        {
            var instanceCreator = new RandomInstanceCreator(TabuListLength);
            var bestSolution = beginningSolution;
            var currentSolution = bestSolution;

            int stepsWithoutImprovement = 0;
            var start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalSeconds < MaxTimeOfProcessingInSeconds)
            {
                currentSolution = instanceCreator.Generate(currentSolution, h);

                if (currentSolution.Value < bestSolution.Value)
                {
                    bestSolution = currentSolution;
                    stepsWithoutImprovement = 0;
                }
                else
                {
                    stepsWithoutImprovement++;
                }

                if (stepsWithoutImprovement > MaxStepsWithoutImprovement)
                {
                    stepsWithoutImprovement = 0;
                    currentSolution = bestSolution;
                }
            }

            return bestSolution;
        }
    }
}
