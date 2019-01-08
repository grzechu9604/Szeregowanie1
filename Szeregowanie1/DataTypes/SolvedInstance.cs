﻿using System;
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
    }
}
