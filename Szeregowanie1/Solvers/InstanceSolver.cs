using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szeregowanie1.DataTypes;
using Szeregowanie1.Helpers;

namespace Szeregowanie1.Solvers
{
    class BaseInstanceSolver : IInstanceSolver
    {
        public SolvedInstance Solve(Instance instance, double h)
        {
            int dueTime = Convert.ToInt32(Math.Floor(instance.Length * h));

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

            var bestStartTime = 0;
            var bestValue = GoalFunctionCalculator.Calculate(bestStartTime, newOrder, dueTime);
            bool isNewStartTimeBetter;

            do
            {
                var newStartTime = bestStartTime + 1;
                var newValue = GoalFunctionCalculator.Calculate(newStartTime, newOrder, dueTime);

                isNewStartTimeBetter = newValue < bestValue;

                if (isNewStartTimeBetter)
                {
                    bestStartTime = newStartTime;
                    bestValue = newValue;
                }

            } while (isNewStartTimeBetter);
            
            return new SolvedInstance(instance, newOrder, h, bestStartTime);
        }
    }
}
