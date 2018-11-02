using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szeregowanie1.DataTypes;

namespace Szeregowanie1.Solvers
{
    class InstanceSolver
    {
        public SolvedInstance Solve(Instance instance, double h)
        {
            return new SolvedInstance(instance, instance.Tasks, h, 0);
        }

        public SolvedInstance SolveAlg(Instance instance, double h)
        {
            var byCostForDelayDesc = instance.Tasks.OrderBy(t => t.CostForDelay).Select(t => t.Index).ToList();
            var byCostForLeadAsc = instance.Tasks.OrderByDescending(t => t.CostForLead).Select(t => t.Index).ToList();

            var newOrder = new List<TaskToSchedule>();
            while (byCostForLeadAsc.Any() || byCostForDelayDesc.Any())
            {
                if (newOrder.Sum(t => t.Length) < instance.Length * h)
                {
                    var first = byCostForLeadAsc.First();
                    newOrder.Add(instance.Tasks.First(t => t.Index.Equals(first)));
                    byCostForLeadAsc.Remove(first);
                    byCostForDelayDesc.Remove(first);
                }
                else
                {
                    byCostForLeadAsc.Clear();
                    byCostForDelayDesc.ForEach(index => newOrder.Add(instance.Tasks.First(t => t.Index.Equals(index))));
                    byCostForDelayDesc.Clear();
                }

            }

            return new SolvedInstance(instance, newOrder, h, 0);
        }
    }
}
