using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AtDb
{
    public class OjbectInspector
    {
        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance
            | BindingFlags.NonPublic;

        private readonly ObjectMemberManipulator memberManipulator;

        private object inspectedObject;
        private FieldInfo[] objectFields;
        private PropertyInfo[] objectProperties;

        public OjbectInspector()
        {
            memberManipulator = new ObjectMemberManipulator();
        }

        public void SetObject(object inspectedObject)
        {
            this.inspectedObject = inspectedObject;
            SetObjectMemberInfos();
        }

        public void DrawMembersUi()
        {
            if (inspectedObject == null)
            {
                DrawNoObjectMessage();
                return;
            }

            EditorUtilities.VerticalLayout(DrawFieldsUi);
            EditorUtilities.VerticalLayout(DrawPropertiesUi);
        }

        private void SetObjectMemberInfos()
        {
            Type type = inspectedObject.GetType();
            objectFields = type.GetFields(bindingFlags);
            objectProperties = type.GetProperties(bindingFlags);

            RemoveBackingFields();
            RemoveNonAutoProperties();
        }

        private void RemoveBackingFields()
        {
            List<FieldInfo> fields = new List<FieldInfo>(objectFields);

            for (int i = 0; i < fields.Count; ++i)
            {
                FieldInfo field = fields[i];
                bool isBackingField = IsBackingField(field);
                if (isBackingField)
                {
                    fields.RemoveAt(i);
                    --i;
                }
            }

            objectFields = fields.ToArray();
        }

        private bool IsBackingField(FieldInfo field)
        {
            const string BACKING_FIELD_IDENTIFIER = "BackingField";
            return field.Name.Contains(BACKING_FIELD_IDENTIFIER);
        }

        private void RemoveNonAutoProperties()
        {
            List<PropertyInfo> properties = new List<PropertyInfo>(objectProperties);

            for (int i = 0; i < properties.Count; ++i)
            {
                PropertyInfo property = properties[i];
                bool hasBackingField = HasBackingField(property);
                if (!hasBackingField)
                {
                    properties.RemoveAt(i);
                    --i;
                }
            }

            objectProperties = properties.ToArray();
        }

        private bool HasBackingField(PropertyInfo property)
        {
            const bool checkInheritance = true;

            MethodInfo method = property.GetGetMethod();
            object[] attributes = method.GetCustomAttributes(typeof(CompilerGeneratedAttribute), checkInheritance);
            return attributes.Length > 0;

        }

        private void DrawNoObjectMessage()
        {
            GUILayout.Label("No object is set.");
        }

        private void DrawFieldsUi()
        {
            foreach (FieldInfo field in objectFields)
            {
                EditorUtilities.HorizontalLayout(DrawFieldUi, field);
            }
        }

        private void DrawFieldUi(FieldInfo field)
        {
            const int LABEL_WIDTH = 400;
            GUILayout.Label(field.Name.ToString(), GUILayout.Width(LABEL_WIDTH));

            object currentValue = field.GetValue(inspectedObject);
            object newValue = memberManipulator.DrawMemberUi(currentValue);
            field.SetValue(inspectedObject, newValue);
            //object value = memberInspector.DrawFieldUi(field);
            //field.SetValue(inspectedObject, value);
        }

        private void DrawPropertiesUi()
        {
            foreach (PropertyInfo property in objectProperties)
            {
                EditorUtilities.HorizontalLayout(DrawPropertyUi, property);
            }
        }

        private void DrawPropertyUi(PropertyInfo property)
        {
            const int LAYOUT_WIDTH = 400;
            GUILayout.Label(property.Name.ToString(), GUILayout.Width(LAYOUT_WIDTH));

            object currentValue = property.GetValue(inspectedObject);
            object newValue = memberManipulator.DrawMemberUi(currentValue);
            property.SetValue(inspectedObject, newValue);
        }
    }
}