using TinyJSON;
using UnityEditor;
using UnityEngine;

namespace AtDb
{
    public class TableMetaDataModifier : EditorWindow
    {
        private readonly TableMetadata defaultMetaData = new TableMetadata(true, true);
        private readonly ObjectInspector objectInspector = new ObjectInspector();

        private TableMetadata loadedMetadata;

        private string jsonText;

        [MenuItem("AtDb/Table Metadata")]
        private static void ShowWindow()
        {
            GetWindow(typeof(TableMetaDataModifier));
        }

        private void OnGUI()
        {
            EditorUtilities.VerticalLayout(DrawLoadingUi);
            EditorUtilities.VerticalLayout(DrawJsonManipulationUi);
        }

        private void DrawLoadingUi()
        {
            GUILayout.Label("Table metadata json:");
            EditorUtilities.HorizontalLayout(DrawMetaDataLoadSaveUi);
        }

        private void DrawMetaDataLoadSaveUi()
        {
            jsonText = EditorGUILayout.TextField(jsonText, GUILayout.Width(400), GUILayout.Height(30));
            DrawLoadButton();

            DrawLoadDefaultButton();
        }

        private void DrawLoadButton()
        {
            if (GUILayout.Button("Load"))
            {
                LoadJsonFromtextField();
            }
        }

        private void LoadJsonFromtextField()
        {
            loadedMetadata = JSON.Load(jsonText).Make<TableMetadata>();
        }

        private void DrawLoadDefaultButton()
        {
            if (GUILayout.Button("Load Default"))
            {
                LoadDefaultMetadata();
            }
        }

        private void LoadDefaultMetadata()
        {
            loadedMetadata = new TableMetadata(defaultMetaData);
            SetInspectorObject();
        }

        private void SetInspectorObject()
        {
            objectInspector.SetObject(loadedMetadata);
            ShowObjectJson();
        }

        private void DrawJsonManipulationUi()
        {
            DrawMetaDataFields();
            DrawExportButton();
        }

        private void DrawMetaDataFields()
        {
            objectInspector.DrawMembersUi();
        }

        private void DrawExportButton()
        {
            if (GUILayout.Button("Export"))
            {
                ShowObjectJson();
            }
        }

        private void ShowObjectJson()
        {
            jsonText = JSON.Dump(loadedMetadata, Constants.encodeOptions);
        }
    }

}