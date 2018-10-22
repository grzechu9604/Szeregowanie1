using System.Collections.Generic;
using System.Linq;

namespace Szeregowanie1.DataTypes
{
    class Instance
    {
        public List<TaskToSchedule> Tasks { get; set; }
        public int Length { get { return Tasks.Sum(t => t.Length); } }
        public int K { get; set; }
        public string InstanceFileName { get; set; }
    }
}
