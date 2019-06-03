namespace AtDb.Enums
{
    public class EnumContainer
    {
        public readonly string name;
        public readonly string[] values;
        public readonly bool isFlagged;

        public static EnumContainer CreateEnumContainer(string name, string[] values)
        {
            return new EnumContainer(name, values, false);
        }

        public static EnumContainer CreateFlaggedEnumContainer(string name, string[] values)
        {
            return new EnumContainer(name, values, true);
        }

        private EnumContainer(string name, string[] values, bool isFlagged)
        {
            this.name = name;
            this.values = values;
            this.isFlagged = isFlagged;
        }
    }
}