﻿using System.Collections.Generic;
using System.Linq;

namespace Szeregowanie1.DataTypes
{
    class Instance
    {
        public List<TaskToSchedule> Tasks { get; set; }
        public int Length { get { return Tasks.Sum(t => t.Length); } }
    }
}