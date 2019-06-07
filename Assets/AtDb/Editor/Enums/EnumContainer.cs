namespace AtDb.Enums
{
    public class EnumContainer
    {
        public enum EnumStyle
        {
            AlreadyExistsNoExport,
            Normal,
            Flagged,
            Composite
        }

        public readonly string name;
        public readonly string[] values;
        public readonly EnumStyle style;

        public EnumContainer(string name, string[] values, EnumStyle style)
        {
            this.name = name;
            this.values = values;
            this.style = style;
        }
    }
}