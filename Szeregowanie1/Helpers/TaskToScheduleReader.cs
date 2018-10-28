using System.IO;
using Szeregowanie1.DataTypes;

namespace Szeregowanie1.Helpers
{
    class TaskToScheduleReader
    {
        public StreamReader StreamReader { get; set; }
        public TaskToSchedule ReadNext(int index)
        {
            var list = FromStreamListReader.ReadIntList(StreamReader);
            return new TaskToSchedule()
            {
                Length = list[0],
                CostForLead = list[1],
                CostForDelay = list[2],
                Index = index
            };
        }
    }
}
