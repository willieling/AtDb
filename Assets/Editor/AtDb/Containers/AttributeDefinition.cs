using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtDb.Reader.Container
{
    public class AttributeDefinition
    {

        public int StartIndex { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int EndIndex { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }

        public AttributeDefinition(int index, string name, string type)
        {
            StartIndex = index;
            EndIndex = index;
            Type = type;
            Name = name;
        }

        public void IncrementEndIndex()
        {
            ++EndIndex;
        }
    }
}
