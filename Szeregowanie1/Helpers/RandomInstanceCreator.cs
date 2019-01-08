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
                TriangleOrderGenerator, RandomGenerator, TriangleOrderGenerator, TriangleOrderWithRandomSmallChangeGenerator, TriangleOrderWithRandomSmallChangeWithOrderByGenerator
            };
        }

        private readonly TabuList tabuList;
        public List<Func<SolvedInstance, double, int, SolvedInstance>> Generators;
        private const int MaxDepth = 50;

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
            var manipulatedStartTimeInstance = ManipulateStartTime(newInstance);
            if (tabuList.IsOnTabuList(manipulatedStartTimeInstance) && maxDepth-- > 0)
            {
                return RandomGenerator(solved, h, maxDepth);
            }
            else
            {
                return manipulatedStartTimeInstance;
            }
        }

        private SolvedInstance TriangleOrderGenerator(SolvedInstance solved, double h, int maxDepth)
        {
            int dueTime = solved.DueTime;

            var threshold = RandomHelper.GetRandomDouble();
            var tasksToDoBeforeDueTime = solved.Instance.Tasks.Where(task => Math.Abs(Guid.NewGuid().GetHashCode()) / (double)int.MaxValue > threshold).ToList();

            int startTime = Math.Max(0, dueTime - tasksToDoBeforeDueTime.Sum(task => task.Length));

            var newOrder = new List<TaskToSchedule>();
            int currentTime = startTime;
            while (tasksToDoBeforeDueTime.Any() && (currentTime + tasksToDoBeforeDueTime.First().Length) <= dueTime)
            {
                var taskToAdd = tasksToDoBeforeDueTime.First();
                currentTime += taskToAdd.Length;
                newOrder.Add(taskToAdd);
                tasksToDoBeforeDueTime.RemoveAt(0);
            }
            newOrder = newOrder.OrderBy(task => task.CostForLead).ToList();
            var tasksToDoAfterDueTime = solved.Instance.Tasks.Except(newOrder);
            newOrder.AddRange(tasksToDoAfterDueTime.OrderByDescending(task => task.CostForDelay));

            var newInstance = new SolvedInstance(solved.Instance, newOrder, h, startTime);
            var manipulatedStartTimeInstance = ManipulateStartTime(newInstance);
            if (tabuList.IsOnTabuList(manipulatedStartTimeInstance) && maxDepth-- > 0)
            {
                return TriangleOrderGenerator(solved, h, maxDepth);
            }
            else
            {
                return manipulatedStartTimeInstance;
            }
        }

        private SolvedInstance TriangleOrderWithRandomSmallChangeGenerator(SolvedInstance solved, double h, int maxDepth)
        {
            int dueTime = solved.DueTime;

            int smallRandom = RandomHelper.GetRandomInt(10);
            var tasksToAddToBeforeListDueTime = solved.GetLateTasks().OrderBy(t => Guid.NewGuid().GetHashCode()).Take(smallRandom);

            var tasksToDoBeforeDueTime = solved.GetEarlyTasks();
            tasksToDoBeforeDueTime.AddRange(tasksToAddToBeforeListDueTime);

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
            var manipulatedStartTimeInstance = ManipulateStartTime(newInstance);
            if (tabuList.IsOnTabuList(manipulatedStartTimeInstance) && maxDepth-- > 0)
            {
                return TriangleOrderGenerator(solved, h, maxDepth);
            }
            else
            {
                return manipulatedStartTimeInstance;
            }
        }

        private SolvedInstance TriangleOrderWithRandomSmallChangeWithOrderByGenerator(SolvedInstance solved, double h, int maxDepth)
        {
            int dueTime = solved.DueTime;

            int smallRandom = RandomHelper.GetRandomInt(10);
            var tasksToAddToBeforeListDueTime = solved.GetLateTasks().OrderBy(t => Guid.NewGuid().GetHashCode()).Take(smallRandom);

            var tasksToDoBeforeDueTime = solved.GetEarlyTasks();
            tasksToDoBeforeDueTime.AddRange(tasksToAddToBeforeListDueTime);
            tasksToDoBeforeDueTime = tasksToDoBeforeDueTime.OrderByDescending(t => t.CostForLead).ToList();

            int startTime = Math.Max(0, dueTime - tasksToDoBeforeDueTime.Sum(task => task.Length));

            var newOrder = new List<TaskToSchedule>();
            int currentTime = startTime;
            while (tasksToDoBeforeDueTime.Any() && (currentTime + tasksToDoBeforeDueTime.First().Length) <= dueTime)
            {
                newOrder.Add(tasksToDoBeforeDueTime.First());
                tasksToDoBeforeDueTime.RemoveAt(0);
            }
            newOrder.Reverse();
            var tasksToDoAfterDueTime = solved.Instance.Tasks.Except(newOrder);
            newOrder.AddRange(tasksToDoAfterDueTime.OrderByDescending(task => task.CostForDelay));

            var newInstance = new SolvedInstance(solved.Instance, newOrder, h, startTime);
            var manipulatedStartTimeInstance = ManipulateStartTime(newInstance);
            if (tabuList.IsOnTabuList(manipulatedStartTimeInstance) && maxDepth-- > 0)
            {
                return TriangleOrderGenerator(solved, h, maxDepth);
            }
            else
            {
                return manipulatedStartTimeInstance;
            }
        }

        //private SolvedInstance ThirdGenerator(SolvedInstance solved, double h, int maxDepth)
        //{
        //    int dueTime = solved.DueTime;

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

        private SolvedInstance ManipulateStartTime(SolvedInstance instance)
        {
            var firstInstance = ManipulateStartTimeIncrementing(instance);
            var secondInstance = ManipulateStartTimeDecrementing(instance);

            return firstInstance.Value < secondInstance.Value ? firstInstance : secondInstance;
        }

        private SolvedInstance ManipulateStartTimeIncrementing(SolvedInstance instance)
        {
            var newInstance = new SolvedInstance(instance.Instance, instance.TasksOrder, instance.HParameter, instance.StartTime + 1);
            if (newInstance.Value < instance.Value)
            {
                return ManipulateStartTimeIncrementing(newInstance);
            }
            else
            {
                return instance;
            }
        }

        private SolvedInstance ManipulateStartTimeDecrementing(SolvedInstance instance)
        {
            if (instance.StartTime > 0)
            {
                var newInstance = new SolvedInstance(instance.Instance, instance.TasksOrder, instance.HParameter, instance.StartTime - 1);
                if (newInstance.Value < instance.Value)
                {
                    return ManipulateStartTimeIncrementing(newInstance);
                }
            }

            return instance;
        }
    }
}
