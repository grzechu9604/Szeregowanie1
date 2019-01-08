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
        public RandomInstanceCreator(int maxTabuListLength)
        {
            tabuList = new TabuList(maxTabuListLength);
            Generators = new List<Func<SolvedInstance, double, int, SolvedInstance>>()
            {
                TriangleOrderGenerator, RandomGenerator, TriangleOrderGenerator, RandomGenerator
            };
        }

        public void ResetTabuList()
        {
            tabuList.Reset();
        }

        private readonly TabuList tabuList;
        public List<Func<SolvedInstance, double, int, SolvedInstance>> Generators;
        private const int MaxDepth = 1000;

        public SolvedInstance Generate(SolvedInstance instance, double h)
        {
            var geneatedInstances = new SolvedInstance[Generators.Count];

            Parallel.For(0, Generators.Count, i =>
            {
                geneatedInstances[i] = Generators[i].Invoke(instance, h, MaxDepth);
            });

            return geneatedInstances.OrderBy(i => i.Value).First();
        }

        private SolvedInstance RandomGenerator(SolvedInstance solved, double h, int maxDepth)
        {
            var newInstance = new SolvedInstance(solved.Instance, solved.Instance.Tasks.OrderBy(t => Guid.NewGuid()).ToList(), h, 0);
            if (tabuList.IsOnTabuList(newInstance) && maxDepth-- > 0)
            {
                return RandomGenerator(solved, h, maxDepth);
            }
            else
            {
                return newInstance;
            }
        }

        private SolvedInstance TriangleOrderGenerator(SolvedInstance solved, double h, int maxDepth)
        {
            int dueTime = (int)Math.Ceiling(solved.Instance.Length * h);

            var threshold = new Random().NextDouble();
            var tasksToDoBeforeDueTime = solved.Instance.Tasks.Where(task => Math.Abs(Guid.NewGuid().GetHashCode()) / (double)int.MaxValue > threshold).ToList();

            int startTime = Math.Max(0, dueTime - tasksToDoBeforeDueTime.Sum(task => task.Length));

            var newOrder = new List<TaskToSchedule>();
            int currentTime = startTime;
            while (tasksToDoBeforeDueTime.Any() && (currentTime + tasksToDoBeforeDueTime.First().Length) <= dueTime)
            {
                newOrder.Add(tasksToDoBeforeDueTime.First());
                tasksToDoBeforeDueTime.RemoveAt(0);
            }
            newOrder = newOrder.OrderBy(task => task.CostForLead).ToList();
            var tasksToDoAfterDueTime = solved.Instance.Tasks.Except(newOrder);
            newOrder.AddRange(tasksToDoAfterDueTime.OrderByDescending(task => task.CostForDelay));

            var newInstance = new SolvedInstance(solved.Instance, newOrder, h, startTime);
            if (tabuList.IsOnTabuList(newInstance) && maxDepth-- > 0)
            {
                return TriangleOrderGenerator(solved, h, maxDepth);
            }
            else
            {
                return newInstance;
            }
        }

        //private SolvedInstance ThirdGenerator(SolvedInstance solved, double h)
        //{
        //    int dueTime = (int)Math.Ceiling(solved.Instance.Length * h);

        //    var tasksOrderedByCostForLead = solved.Instance.Tasks.OrderByDescending(t => t.CostForLead).ToList();
        //    var tasksOrderedByCostForDelay = solved.Instance.Tasks.OrderByDescending(t => t.CostForDelay).ToList();

        //    var tasksToDoBeforeDueTime = new List<TaskToSchedule>();
        //    var tasksToDoAfterDueTime = new List<TaskToSchedule>();

        //    while (tasksOrderedByCostForDelay.Any() || tasksOrderedByCostForLead.Any())
        //    {
        //        tasksOrderedByCostForDelay.FirstOrDefault()
        //    }
        //}

        //private SolvedInstance SwapperGenerator(SolvedInstance solved, double h)
        //{
        //    int dueTime = (int)Math.Ceiling(instance.Length * h);

        //    var tasksOrderedByCostForLead = instance.Tasks.OrderByDescending(t => t.CostForLead).ToList();
        //    var tasksOrderedByCostForDelay = instance.Tasks.OrderByDescending(t => t.CostForDelay).ToList();

        //    var tasksToDoBeforeDueTime = new List<TaskToSchedule>();
        //    var tasksToDoAfterDueTime = new List<TaskToSchedule>();

        //    while (tasksOrderedByCostForDelay.Any() || tasksOrderedByCostForLead.Any())
        //    {
        //        tasksOrderedByCostForDelay.FirstOrDefault()
        //    }
        //}
    }
}
