using AtDb.Reader;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AtDb
{
    public class DatabaseExporterWindow : EditorWindow
    {
        private readonly DatabaseExporter exporter = new DatabaseExporter();

        private void OnEnable()
        {
            exporter.DatabaseSourcePath = Path.GetFullPath(Application.dataPath + "/../ExcelFiles");
            exporter.DatabaseExportPath = Path.GetFullPath(Application.dataPath + "/DefaultDatabase");
            exporter.GeneratedEnumsPath = Path.GetFullPath(Application.dataPath + "/GeneratedEnums");

            exporter.Initialize(new DatabaseExporterConfiguration());
        }

        [MenuItem("AtDb/Export Database")]
        private static void ShowWindow()
        {
            GetWindow<DatabaseExporterWindow>();
        }

        private void OnGUI()
        {
            DrawPathsUi();
            DrawExportUi();
        }

        private void DrawPathsUi()
        {
            EditorGUILayout.LabelField("Paths");

            ++EditorGUI.indentLevel;
            {
                EditorGUILayout.BeginVertical();
                {
                    exporter.DatabaseSourcePath = DrawSelectablePathUi("Database Source Path", exporter.DatabaseSourcePath);
                    exporter.DatabaseExportPath = DrawSelectablePathUi("Database Export Path", exporter.DatabaseExportPath);
                    exporter.GeneratedEnumsPath = DrawSelectablePathUi("Exported Enums Path", exporter.GeneratedEnumsPath);
                }
                EditorGUILayout.EndVertical();
            }
            --EditorGUI.indentLevel;
        }

        private string DrawSelectablePathUi(string label, string path)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(150));
                EditorGUILayout.TextField(path);

                //GUI.Label("test");
                if (GUILayout.Button("Choose Folder", GUILayout.Width(100)))
                {
                    path = EditorUtility.OpenFolderPanel("Select path", path, string.Empty);
                }
            }
            EditorGUILayout.EndHorizontal();

            return path;
        }

        private void DrawExportUi()
        {
            if(GUILayout.Button("Export"))
            {
                exporter.Export();
            }
        }
    }
}