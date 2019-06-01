using System;
using System.Collections.Generic;

namespace AtDb.Metadata
{
    public class EnumValueCacher
    {
        private readonly Dictionary<Type, string[]> cache;

        public EnumValueCacher()
        {
            cache = new Dictionary<Type, string[]>();
        }
        public string[] GetNames(Type enumType)
        {
            string[] values;
            bool found = cache.TryGetValue(enumType, out values);
            if(!found)
            {
                values = Enum.GetNames(enumType);
                cache.Add(enumType, values);
            }

            return values;
        }
    }
}
