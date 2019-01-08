using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szeregowanie1.Helpers;

namespace Szeregowanie1.DataTypes
{
    public class SolvedInstance
    {
        public Instance Instance { get; private set; }
        public int Value { get; private set; }
        public int StartTime { get; private set; }
        public List<TaskToSchedule> TasksOrder { get; private set; }
        public int DueTime { get; private set; }
        public double HParameter { get; private set; }

        public SolvedInstance(Instance instance, List<TaskToSchedule> tasksOrder, double h, int startTime)
        {
            if (startTime < 0)
            {
                throw new Exception("Zaczął przed 0!");
            }

            Instance = instance;
            HParameter = h;
            DueTime = (int)(HParameter * Instance.Length);
            TasksOrder = tasksOrder;
            StartTime = startTime;
            Value = GoalFunctionCalculator.Calculate(StartTime, TasksOrder, DueTime);
        }

        public override bool Equals(object obj)
        {
            if (obj is SolvedInstance && (obj as SolvedInstance).StartTime.Equals(StartTime))
            {
                for (int i = 0; i < TasksOrder.Count; i++)
                {
                    if (!TasksOrder[i].Index.Equals((obj as SolvedInstance).TasksOrder[i].Index))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public List<TaskToSchedule> GetLateTasks()
        {
            int time = StartTime;
            var lateTasks = new List<TaskToSchedule>();

            foreach (var task in TasksOrder)
            {
                time += task.Length;

                if (time >= DueTime)
                {
                    lateTasks.Add(task);
                }
            }
            return lateTasks;
        }

        public List<TaskToSchedule> GetEarlyTasks()
        {
            int time = StartTime;
            var earlyTasks = new List<TaskToSchedule>();

            foreach (var task in TasksOrder)
            {
                time += task.Length;

                if (time < DueTime)
                {
                    earlyTasks.Add(task);
                }
            }
            return earlyTasks;
        }
    }
}
