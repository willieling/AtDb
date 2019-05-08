using AtDb.Reader;
using UnityEditor;
using UnityEngine;

namespace AtDb
{
    public class DatabaseExporterWindow : EditorWindow
    {
        private readonly DatabaseExporter databaseReader = new DatabaseExporter();

        private string databaseSourcePath;
        private string databaseExportPath;
        private string generatedEnumsPath;

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
                    databaseSourcePath = DrawSelectablePathUi("Database Source Path", databaseSourcePath);
                    databaseExportPath = DrawSelectablePathUi("Database Export Path", databaseExportPath);
                    generatedEnumsPath = DrawSelectablePathUi("Exported Enums Path", generatedEnumsPath);
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
                databaseReader.Export(databaseSourcePath);
            }
        }
    }
}