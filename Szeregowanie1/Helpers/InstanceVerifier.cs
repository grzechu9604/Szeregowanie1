using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szeregowanie1.DataTypes;

namespace Szeregowanie1.Helpers
{
    class InstanceVerifier
    {
        public const string DEFAULT_FILE_EXTENSION = "txt";

        public string FileName { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(FileName))
                throw new InvalidOperationException("Pusta nazwa pliku! Można przeprowadzić weryfikacji!");

            var info = GetInfoFromGivenFile();

            var splittedFileName = FileName.Split('_', '.').ToArray();

            string fileNameWithoutExtension = splittedFileName[0];
            int k = Convert.ToInt32(splittedFileName[1]);
            double h = Convert.ToDouble(splittedFileName[2]) / 10;

            var instanceToVerify = GetInstance(fileNameWithoutExtension, k);

            info.Wait();
            var splittedInfo = info.Result.Split(' ');
            int declaredValue = Convert.ToInt32(splittedInfo[0]);
            int declaredStartTime = Convert.ToInt32(splittedInfo[1]);

            List<int> indexesOrder = splittedInfo.Skip(2).Select(i => Convert.ToInt32(i)).ToList();
            var tasksOrder = PrepareTaskOrderLiest(indexesOrder, instanceToVerify.Tasks);
            var solvedInstance = new SolvedInstance(instanceToVerify, tasksOrder, h, declaredStartTime);

            return solvedInstance.Value.Equals(declaredValue);
        }

        private List<TaskToSchedule> PrepareTaskOrderLiest(List<int> indexesOrder, List<TaskToSchedule> tasks)
        {
            var tasksOrder = new List<TaskToSchedule>();

            indexesOrder.ForEach(i => tasksOrder.Add(tasks.First(t => t.Index.Equals(i))));

            return tasksOrder;
        }

        private Instance GetInstance(string fileNameWithoutExtension, int k)
        {
            string fileNameWithExtension = string.Format("{0}.{1}", fileNameWithoutExtension, DEFAULT_FILE_EXTENSION);
            var instanceToVerify = new Instance();

            using (StreamReader sr = new StreamReader(fileNameWithExtension))
            {
                sr.ReadLine(); //First line is not usefull in this context
                var reader = new InstanceReader()
                {
                    FileNameWithoutExtension = fileNameWithoutExtension,
                    StreamReader = sr
                };

                for (int i = 0; i < k - 1; i++)
                {
                    reader.ReadNext(i);
                }

                instanceToVerify = reader.ReadNext(k);
            }

            return instanceToVerify;
        }

        private async Task<string> GetInfoFromGivenFile()
        {
            string info;

            using (StreamReader sr = new StreamReader(FileName))
            {
                info = await sr.ReadLineAsync();
            }

            return info;
        }
    }
}
