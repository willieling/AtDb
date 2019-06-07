using AtDb.Enums;
using AtDb.Reader;
using AtDb.Reader.Container;
using System;
using System.Collections.Generic;
using System.IO;

namespace AtDb
{
    public class DatabaseFileWriter
    {
        private readonly EnumCacher enumCacher;
        private readonly EnumGenerator enumGenerator;

        private Func<object, string> serializationFunction;
        private Func<string, string> compressionFunction;

        public string DatabaseSourcePath { get; set; }
        public string DatabaseExportPath { get; set; }
        public string GeneratedEnumsPath { get; set; }

        public DatabaseFileWriter(EnumCacher enumCacher)
        {
            this.enumCacher = enumCacher;
            enumGenerator = new EnumGenerator(this.enumCacher);
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

            Dictionary<string, string> generatedEnums = enumGenerator.Generate();

            foreach (KeyValuePair<string, string> kvp in generatedEnums)
            {
                ExportEnumToFile(kvp.Key, kvp.Value);
            }

            foreach (ModelDataContainer container in modelContainers)
            {
                ExportDataToFile(container);
            }

            
            UnityEditor.EditorUtility.DisplayDialog("Export", "Export complete!", "ok");
        }

        private void ExportEnumToFile(string name, string generatedEnum)
        {
            const string ENUM_FILE_PATH = "{0}/{1}.cs";

            string fullPath = string.Format(ENUM_FILE_PATH, GeneratedEnumsPath, name);
            CreateFile(fullPath, generatedEnum);
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