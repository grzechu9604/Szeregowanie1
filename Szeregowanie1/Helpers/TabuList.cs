using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szeregowanie1.DataTypes;

namespace Szeregowanie1.Helpers
{
    class TabuList
    {
        private readonly List<SolvedInstance> _forbiddenInstances = new List<SolvedInstance>();
        private readonly int _tabuListMaxLength;

        public TabuList(int maxTabuListLength)
        {
            _tabuListMaxLength = maxTabuListLength;
        }

        public bool IsOnTabuList(SolvedInstance instance) => _forbiddenInstances.Any(i => i.Equals(instance));

        public void AddToTabuList(SolvedInstance instance)
        {
            if (_forbiddenInstances.Count > 0)
            {
                _forbiddenInstances.Insert(0, instance);
                if (_forbiddenInstances.Count > _tabuListMaxLength)
                {
                    _forbiddenInstances.RemoveAt(_tabuListMaxLength - 1);
                }
            }
            else
            {
                _forbiddenInstances.Add(instance);
            }
        }
    }
}
