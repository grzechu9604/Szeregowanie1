using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szeregowanie1.DataTypes;

namespace Szeregowanie1.Solvers
{
    interface IInstanceSolver
    {
        SolvedInstance Solve(Instance instance, double h);
    }
}
