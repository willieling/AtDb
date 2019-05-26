using System;
using UnityEditor;
using UnityEngine;

namespace AtDb.Metadata
{
    public class ObjectMemberManipulator
    {
        private readonly string[] boolOptions = { "false", "true" };
        private readonly EnumValueCacher enumCacher;

        public ObjectMemberManipulator()
        {
            enumCacher = new EnumValueCacher();
        }

        public object DrawMemberUi(object currentValue)
        {
            const string INT = "System.int";
            const string BOOL = "System.Boolean";
            const string STRING = "System.String";
            const string DATA_STYLE = "AtDb.DataStyle";

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
                case DATA_STYLE:
                    currentValue = DrawEnumUi((DataStyle)currentValue, typeof(DataStyle));
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

            int value = currentValue == true ? TRUE : FALSE;
            value = DrawPopup(value, boolOptions);

            return value == TRUE ? true : false;
        }

        private string DrawStringUi(string currentValue)
        {
            return EditorGUILayout.TextField(currentValue);
        }

        private int DrawEnumUi(Enum currentValue, Type enumType)
        {
            string[] names = enumCacher.GetNames(enumType);
            int valueIndex = GetIndex(currentValue, names);
            int newIndex = DrawPopup(valueIndex, names);
            return newIndex;
        }

        private int GetIndex(Enum currentValue, string[] names)
        {
            string stringValue = currentValue.ToString();
            int index = 0;
            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];
                if (stringValue == name)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private int DrawPopup(int value, string[] options)
        {
            const int RECT_WIDTH = 100;

            Rect rect = GUILayoutUtility.GetLastRect();
            rect.Set(rect.x + Constants.INPUT_OFFSET, rect.y, RECT_WIDTH, rect.height);

            value = EditorGUI.Popup(rect,
                value, options);
            return value;
        }
    } 
}
