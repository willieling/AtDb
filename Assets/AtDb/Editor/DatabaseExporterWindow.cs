using AtDb.Reader;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AtDb
{
    public class DatabaseExporterWindow : EditorWindow
    {
        private readonly DatabaseExporter databaseReader = new DatabaseExporter();

        private void Awake()
        {
            databaseReader.DatabaseSourcePath = Path.GetFullPath(Application.dataPath + "/../ExcelFiles");
            databaseReader.DatabaseExportPath = Path.GetFullPath(Application.dataPath + "/DefaultDatabase");
            databaseReader.GeneratedEnumsPath = Path.GetFullPath(Application.dataPath + "/GeneratedEnums");
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
                    databaseReader.DatabaseSourcePath = DrawSelectablePathUi("Database Source Path", databaseReader.DatabaseSourcePath);
                    databaseReader.DatabaseExportPath = DrawSelectablePathUi("Database Export Path", databaseReader.DatabaseExportPath);
                    databaseReader.GeneratedEnumsPath = DrawSelectablePathUi("Exported Enums Path", databaseReader.GeneratedEnumsPath);
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
                databaseReader.Export();
            }
        }
    }
}