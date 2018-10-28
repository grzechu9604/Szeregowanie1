using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szeregowanie1.DataTypes;

namespace Szeregowanie1.Helpers
{
    class SolvedInstanceWriter
    {
        public async static void Write(SolvedInstance solvedInstance)
        {
            string fileName = GenerateFileName(solvedInstance.Instance.FileNameWithoutExtension,
                solvedInstance.Instance.K, solvedInstance.HParameter);

            using (StreamWriter sr = new StreamWriter(fileName))
            {
                string valuesToOutput = $"{solvedInstance.Value} {solvedInstance.StartTime}";

                StringBuilder sb = new StringBuilder(valuesToOutput);
                solvedInstance.TasksOrder.ForEach(t => sb.AppendFormat(" {0}", t.Index));

                await sr.WriteLineAsync(sb.ToString());
            }
        }

        private static string GenerateFileName(string instanceFileName, int k, double h)
        {
            int onlyFirstDecimalPlaceOfH = (int)((h % 1) * 10);
            return $"{instanceFileName}_{k}_{onlyFirstDecimalPlaceOfH}.txt";
        }
    }
}
