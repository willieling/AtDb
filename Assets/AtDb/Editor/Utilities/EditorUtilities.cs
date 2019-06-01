using System;
using UnityEditor;

namespace AtDb
{
    public static class EditorUtilities
    {
        public static void HorizontalLayout(Action action)
        {
            EditorGUILayout.BeginHorizontal();
            action();
            EditorGUILayout.EndHorizontal();
        }

        public static void HorizontalLayout<T>(Action<T> action, T arg0)
        {
            EditorGUILayout.BeginHorizontal();
            action(arg0);
            EditorGUILayout.EndHorizontal();
        }

        public static void VerticalLayout(Action action)
        {
            EditorGUILayout.BeginVertical();
            action();
            EditorGUILayout.EndVertical();
        }

        public static void VerticalLayout<T>(Action<T> action, T arg0)
        {
            EditorGUILayout.BeginVertical();
            action(arg0);
            EditorGUILayout.EndVertical();
        }
    }
}