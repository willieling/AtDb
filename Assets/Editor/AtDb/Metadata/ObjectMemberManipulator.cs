using System;
using UnityEditor;
using UnityEngine;

namespace AtDb
{
    public class ObjectMemberManipulator
    {
        private string[] options = { "false", "true" };

        public object DrawMemberUi(object currentValue)
        {
            const string INT = "System.int";
            const string BOOL = "System.Boolean";
            const string STRING = "System.string";

            string type = currentValue.GetType().ToString();
            switch (type)
            {
                case INT:
                    currentValue = DrawIntUi((int)currentValue);
                    break;
                case BOOL:
                    currentValue = DrawBoolUi((bool)currentValue);
                    break;
                case STRING:
                    currentValue = DrawStringUi((string)currentValue);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return currentValue;
        }

        private int DrawIntUi(int currentValue)
        {
            return EditorGUILayout.IntField(currentValue);
        }

        private bool DrawBoolUi(bool currentValue)
        {
            const int TRUE = 1;
            const int FALSE = 0;
            const int DROP_DOWN_OFFSET = 100;
            const int RECT_WIDTH = 100;

            int value = currentValue == true ? TRUE : FALSE;

            Rect rect = GUILayoutUtility.GetLastRect();
            rect.Set(rect.x + DROP_DOWN_OFFSET, rect.y, RECT_WIDTH, rect.height);

            value = EditorGUI.Popup(rect,
                value, options);

            return value == TRUE ? true : false;
        }

        private string DrawStringUi(string currentValue)
        {
            return EditorGUILayout.TextField(currentValue);
        }
    } 
}
