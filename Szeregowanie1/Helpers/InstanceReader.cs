using System.Collections.Generic;
using System.IO;
using System.Linq;
using Szeregowanie1.DataTypes;

namespace Szeregowanie1.Helpers
{
    class InstanceReader
    {
        public StreamReader StreamReader { get; set; }
        public Instance ReadNext()
        {
            var tasks = new List<TaskToSchedule>();

            var amountOfTasks = FromStreamListReader.ReadIntList(StreamReader).First();
            var taskReader = new TaskToScheduleReader() { StreamReader = StreamReader };
            for (int i = 0; i < amountOfTasks; i++)
            {
                tasks.Add(taskReader.ReadNext());
            }

            return new Instance() { Tasks = tasks };
        }
    }
}
