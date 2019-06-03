using System;
using System.Collections.Generic;
using AtDb.Reader;

namespace AtDb.Enums
{
    public class EnumCacher
    {
        public const int NO_VALUE = -1;

        public readonly Dictionary<string, string[]> cachedEnums = new Dictionary<string, string[]>();

        private readonly ClassMaker classMaker;

        public EnumCacher(ClassMaker classMaker)
        {
            this.classMaker = classMaker;
        }

        public EnumGatherer GetGatherer()
        {
            return new EnumGatherer(this);
        }

        public string[] GetEnumValues(string enumName)
        {
            string[] values = cachedEnums[enumName];
            return values;
        }

        public int GetEnumValueIndex(string enumName, string enumValue)
        {
            string[] values = GetEnumValues(enumName);
            int index = FindIndex(values, enumValue);
            return index;
        }

        public void CacheValues(string name, string[] values)
        {
            cachedEnums.Add(name, values);
        }

        public void CacheValues(string name)
        {
            string[] values = ParseEnumForValues(name);
            cachedEnums.Add(name, values);
        }

        private int FindIndex(string[] values, string enumValue)
        {
            int index = NO_VALUE;
            for (int i = 0; i < values.Length; ++i)
            {
                if(string.Equals(values[i], enumValue, StringComparison.OrdinalIgnoreCase))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private string[] ParseEnumForValues(string name)
        {
            Type enumType = classMaker.GetType(name);
            string[] values = Enum.GetNames(enumType);
            return values;
        }
    }
}