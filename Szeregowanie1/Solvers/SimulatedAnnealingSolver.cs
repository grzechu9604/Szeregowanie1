using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szeregowanie1.DataTypes;

namespace Szeregowanie1.Solvers
{
    class SimulatedAnnealingSolver : IInstanceSolver
    {
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
            return beginningSolution;
        }
    }
}
