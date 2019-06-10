using AtDb.ErrorSystem;
using AtDb.Reader;
using System;
using System.Collections.Generic;

namespace AtDb.Enums
{
    public class EnumCacher : IErrorLogger
    {
        public const int NO_VALUE = -1;

        public readonly Dictionary<string, EnumContainer> cachedEnums = new Dictionary<string, EnumContainer>();

        private readonly ClassMaker classMaker;

        public ErrorLogger ErrorLogger { get; private set; }

        public EnumCacher(ClassMaker classMaker)
        {
            this.classMaker = classMaker;
            ErrorLogger = new ErrorLogger();
        }

        public EnumGatherer GetGatherer()
        {
            return new EnumGatherer(this);
        }

        public void Clear()
        {
            cachedEnums.Clear();
        }

        public string[] GetEnumValues(string enumName)
        {
            EnumContainer container = cachedEnums[enumName];
            return container.values;
        }

        public int GetEnumValueIndex(string enumName, string enumValue)
        {
            string[] values = GetEnumValues(enumName);
            int index = FindIndex(values, enumValue);
            return index;
        }

        public void CacheEnum(string name, string[] values, EnumContainer.EnumStyle style)
        {
            if(style == EnumContainer.EnumStyle.AlreadyExistsNoExport)
            {
                ErrorLogger.AddError("Enum '{0}' is labeled as already existing.", name);
                return;
            }

            EnumContainer container = new EnumContainer(name, values, style);
            cachedEnums.Add(name, container);
        }

        public void CacheExistingEnum(string name)
        {
            string[] values = ParseEnumForValues(name);
            EnumContainer container = new EnumContainer(name, values, EnumContainer.EnumStyle.AlreadyExistsNoExport);
            cachedEnums.Add(name, container);
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