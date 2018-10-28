using System.Collections.Generic;
using System.IO;
using System.Linq;
using Szeregowanie1.DataTypes;

namespace Szeregowanie1.Helpers
{
    class InstanceReader
    {
        public string FileNameWithoutExtension { get; set; }
        public StreamReader StreamReader { get; set; }
        public Instance ReadNext(int id)
        {
            var tasks = new List<TaskToSchedule>();

            var amountOfTasks = FromStreamListReader.ReadIntList(StreamReader).First();
            var taskReader = new TaskToScheduleReader() { StreamReader = StreamReader };
            for (int i = 0; i < amountOfTasks; i++)
            {
                tasks.Add(taskReader.ReadNext(i));
            }

            return new Instance()
            {
                Tasks = tasks,
                K = id,
                FileNameWithoutExtension = FileNameWithoutExtension
            };
        }
    }
}
