using System.Collections.Generic;
using Szeregowanie1.DataTypes;

namespace Szeregowanie1.Helpers
{
    class GoalFunctionCalculator
    {

        public static int Calculate(int startTime, List<TaskToSchedule> tasksOrder, int dueTime)
        {
            int result = 0;
            int currentTime = startTime;

            tasksOrder.ForEach(t =>
            {
                result += CalculateCost(dueTime, currentTime, t);
                currentTime += t.Length;
            });

            return result;
        }

        private static int CalculateCost(int dueTime, int startTime, TaskToSchedule task)
        {
            int diffrence = (startTime + task.Length) - dueTime;
            return diffrence > 0 ? diffrence * task.CostForDelay : -diffrence * task.CostForLead;
        }
    }
}
