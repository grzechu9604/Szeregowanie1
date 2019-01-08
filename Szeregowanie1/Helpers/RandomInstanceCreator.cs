using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szeregowanie1.DataTypes;

namespace Szeregowanie1.Helpers
{
    class RandomInstanceCreator
    {
        public static List<Func<Instance, double, SolvedInstance>> Generators = new List<Func<Instance, double, SolvedInstance>>()
        {
            FirstGenerator, SecondGenerator
        };

        public static SolvedInstance Generate(Instance instance, double h)
        {
            var geneatedInstances = new SolvedInstance[Generators.Count];

            Parallel.For(0, Generators.Count, i =>
            {
                geneatedInstances[i] = Generators[i].Invoke(instance, h);
            });

            return geneatedInstances.OrderBy(i => i.Value).First();
        }

        private static SolvedInstance SecondGenerator(Instance instance, double h)
        {
            return new SolvedInstance(instance, instance.Tasks.OrderBy(t => new Guid()).ToList(), h, 0);
        }

        private static SolvedInstance FirstGenerator(Instance instance, double h)
        {
            int dueTime = (int)Math.Ceiling(instance.Length * h);

            var threshold = new Random().NextDouble();
            var tasksToDoBeforeDueTime = instance.Tasks.Where(task => Math.Abs(Guid.NewGuid().GetHashCode()) / (double)int.MaxValue > threshold).ToList();

            int startTime = Math.Max(0, dueTime - tasksToDoBeforeDueTime.Sum(task => task.Length));

            var newOrder = new List<TaskToSchedule>();
            int currentTime = startTime;
            while (tasksToDoBeforeDueTime.Any() && (currentTime + tasksToDoBeforeDueTime.First().Length) <= dueTime)
            {
                newOrder.Add(tasksToDoBeforeDueTime.First());
                tasksToDoBeforeDueTime.RemoveAt(0);
            }
            newOrder = newOrder.OrderBy(task => task.CostForLead).ToList();
            var tasksToDoAfterDueTime = instance.Tasks.Except(newOrder);
            newOrder.AddRange(tasksToDoAfterDueTime.OrderByDescending(task => task.CostForDelay));

            return new SolvedInstance(instance, newOrder, h, startTime);
        }
    }
}
