using AtDb.Enums;
using AtDb.Reader;
using AtDb.Reader.Container;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AtDb
{
    public class DatabaseFileWriter
    {
        private EnumCacher enumCacher;

        private Func<object, string> serializationFunction;
        private Func<string, string> compressionFunction;

        public string DatabaseSourcePath { get; set; }
        public string DatabaseExportPath { get; set; }
        public string GeneratedEnumsPath { get; set; }

        public DatabaseFileWriter(EnumCacher enumCacher)
        {
            this.enumCacher = enumCacher;
        }

        public void Initialize(DatabaseExporterConfiguration configuration)
        {
            serializationFunction = configuration.SerializationFunction;
            compressionFunction = configuration.CompressionFunction;
        }

        public void ExportAllData(IEnumerable<ModelDataContainer> modelContainers)
        {
            DirectorUtilities.CreateDirectoryIfNeeded(GeneratedEnumsPath);
            DirectorUtilities.CreateDirectoryIfNeeded(DatabaseExportPath);

            foreach (KeyValuePair<string, EnumContainer> kvp in enumCacher.cachedEnums)
            {
                ExportEnumToFile(kvp.Value);
            }

            foreach (ModelDataContainer container in modelContainers)
            {
                ExportDataToFile(container);
            }

            
            UnityEditor.EditorUtility.DisplayDialog("Export", "Export complete!", "ok");
        }

        private void ExportEnumToFile(EnumContainer container)
        {
            const string ENUM_FILE_PATH = "{0}/{1}.cs";

            string fullPath = string.Format(ENUM_FILE_PATH, GeneratedEnumsPath, container.name);
            string serializedEnum = SerializeEnum(container);

            CreateFile(fullPath, serializedEnum);
        }

        private string SerializeEnum(EnumContainer container)
        {
            StringBuilder sb = new StringBuilder("using System;");
            sb.AppendLine("");
            sb.AppendLine("namespace GeneratedEnums");
            sb.AppendLine("{");
            if(container.isFlagged)
            {
                sb.AppendLine("    [Flags]");
            }
            sb.AppendFormat("    public enum {0}\n", container.name);
            sb.AppendLine("    {");
            AddEnumValues(container, sb);
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void AddEnumValues(EnumContainer container, StringBuilder sb)
        {
            bool flag = container.isFlagged;
            for (int i = 0; i < container.values.Length; ++i)
            {
                string value = container.values[i];
                string suffix = flag ? string.Format(" = 1 << {0}", i) : string.Empty;
                sb.AppendFormat("        {0}{1},\n", value, suffix);
            }
        }

        private void ExportDataToFile(ModelDataContainer container)
        {
            const string DATA_FILE_PATH = "{0}/{1}";

            string filename = container.metadata.TableName;
            string fullPath = string.Format(DATA_FILE_PATH, DatabaseExportPath, filename);
            string serializedData = SerializeModel(container);

            CreateFile(fullPath, serializedData);
        }

        private string SerializeModel(ModelDataContainer container)
        {
            string serializedData = serializationFunction.Invoke(container.data);
            if (container.metadata.Compress)
            {
                serializedData = compressionFunction.Invoke(serializedData);
            }
            return serializedData;
        }

        private void CreateFile(string fullPath, string data)
        {
            File.WriteAllText(fullPath, data);
        }
    }
}