using System;
using System.Collections.Generic;
using System.Text;

namespace AtDb.Enums
{
    public class EnumGenerator
    {
        private readonly EnumCacher enumCacher;

        public EnumGenerator(EnumCacher enumCacher)
        {
            this.enumCacher = enumCacher;
        }

        public Dictionary<string, string> Generate()
        {
            Dictionary<string, string> generatedEnums = new Dictionary<string, string>();
            foreach (KeyValuePair<string, EnumContainer> kvp in enumCacher.cachedEnums)
            {
                string serializedValue = SerializeEnum(kvp.Value);
                generatedEnums.Add(kvp.Key, serializedValue);
            }

            return generatedEnums;
        }

        private string SerializeEnum(EnumContainer container)
        {
            string serializedEnum;
            switch (container.style)
            {
                case EnumContainer.EnumStyle.AlreadyExistsNoExport:
                    serializedEnum = "Error: Trying to generate an enum that's already defined";
                    break;
                case EnumContainer.EnumStyle.Flagged:
                    serializedEnum = CreateFlaggedEnum(container);
                    break;
                case EnumContainer.EnumStyle.Composite:
                    serializedEnum = CreateCompositeEnum(container);
                    break;
                default:
                    serializedEnum = CreateNormalEnum(container);
                    break;
            }
            return serializedEnum;
        }

        private string CreateNormalEnum(EnumContainer container)
        {
            StringBuilder sb = new StringBuilder("using System;");
            sb.AppendLine("");
            sb.AppendLine("namespace GeneratedEnums");
            sb.AppendLine("{");
            sb.AppendFormat("    public enum {0}\n", container.name);
            sb.AppendLine("    {");
            AddNormalValues(container, sb);
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void AddNormalValues(EnumContainer container, StringBuilder sb)
        {
            foreach (string value in container.values)
            {
                sb.AppendFormat("        {0},\n", value);
            }
        }

        private string CreateFlaggedEnum(EnumContainer container)
        {
            StringBuilder sb = new StringBuilder("using System;");
            sb.AppendLine("");
            sb.AppendLine("namespace GeneratedEnums");
            sb.AppendLine("{");
            sb.AppendLine("    [Flags]");
            sb.AppendFormat("    public enum {0}\n", container.name);
            sb.AppendLine("    {");
            AddFlaggedValues(container, sb);
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void AddFlaggedValues(EnumContainer container, StringBuilder sb)
        {
            for (int i = 0; i < container.values.Length; i++)
            {
                string value = container.values[i];
                sb.AppendFormat("        {0} = 1 << {1},\n", value, i);
            }
        }

        private string CreateCompositeEnum(EnumContainer container)
        {
            StringBuilder sb = new StringBuilder("using System;");
            sb.AppendLine("");
            sb.AppendLine("namespace GeneratedEnums");
            sb.AppendLine("{");
            sb.AppendLine("    [Flags]");
            sb.AppendFormat("    public enum {0}\n", container.name);
            sb.AppendLine("    {");
            AddCompositeValues(container, sb);
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void AddCompositeValues(EnumContainer container, StringBuilder sb)
        {
            const char DELIMITER = ',';

            for (int i = 0; i < container.values.Length; i++)
            {
                string value = container.values[i];
                string[] parts = value.Split(DELIMITER);

                switch(parts.Length)
                {
                    case 1:
                        sb.AppendFormat("        {0}\n", value);
                        break;
                    default:
                        CreateCompositeLine(sb, parts);
                        break;
                }
            }
        }

        private void CreateCompositeLine(StringBuilder sb, string[] parts)
        {
            const int NO_SHIFT = -1;

            string comment = null;
            int shiftValue = NO_SHIFT;
            List<string> flags = SortParts(parts, ref comment, ref shiftValue);

            sb.AppendFormat("        {0} = ", parts[0]);

            AddFlags(sb, flags);

            if (shiftValue != NO_SHIFT)
            {
                sb.AppendFormat(" {1} 1 << {0}", shiftValue, flags.Count > 0 ? "|" : string.Empty);
            }

            sb.Append(",");

            if (comment != null)
            {
                sb.AppendLine(comment);
            }
            else
            {
                sb.Append("\n");
            }
        }

        private List<string> SortParts(string[] parts, ref string comment, ref int shiftValue)
        {
            const string COMMENT_MARKER = "//";
            const string SHIFT_MARKER = "<<";
            const int SHIFT_LENGTH = 2;
            const int FLAG_START_INDEX = 1;

            List<string> flags = new List<string>(parts.Length);

            for (int i = FLAG_START_INDEX; i < parts.Length; i++)
            {
                string part = parts[i];
                part = part.Trim();
                if (part.StartsWith(SHIFT_MARKER))
                {
                    string integer = part.Remove(0, SHIFT_LENGTH);
                    shiftValue = int.Parse(integer);
                }
                else
                {
                    if (part.StartsWith(COMMENT_MARKER))
                    {
                        comment = part;
                    }
                    else
                    {
                        flags.Add(part);
                    }
                }
            }

            return flags;
        }

        private static void AddFlags(StringBuilder sb, List<string> flags)
        {
            if (flags.Count > 0)
            {
                sb.Append(flags[0]);

                for (int i = 1; i < flags.Count; ++i)
                {
                    sb.AppendFormat(" | {0}", flags[i]);
                }
            }
        }
    }
}