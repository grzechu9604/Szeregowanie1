using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szeregowanie1.DataTypes;

namespace Szeregowanie1.Solvers
{
    public class NaiveInstanceSolver : IInstanceSolver
    {
        public SolvedInstance Solve(Instance instance, double h)
        {
            int dueTime = Convert.ToInt32(Math.Floor(instance.Length * h));

            var tasksBetterToDoBeforeDueTime = instance.Tasks
                .Where(t => t.CostForDelay > t.CostForLead)
                .OrderByDescending(t => t.CostForLead)
                .ToList();

            var newOrder = new List<TaskToSchedule>();
            var newOrdersLength = 0;

            tasksBetterToDoBeforeDueTime.ForEach(t =>
            {
                if (t.Length + newOrdersLength < dueTime)
                {
                    newOrder.Insert(0, t);
                    newOrdersLength += t.Length;
                }
            });


            int startTime = dueTime - newOrder.Sum(t => t.Length);

            newOrder.AddRange(instance.Tasks
                .Where(t => !newOrder.Contains(t))
                .OrderByDescending(t => t.CostForDelay));


            return new SolvedInstance(instance, newOrder, h, startTime);
        }
    }
}
