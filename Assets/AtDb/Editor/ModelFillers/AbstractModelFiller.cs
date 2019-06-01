using AtDb.Reader;
using AtDb.Reader.Container;
using NPOI.SS.UserModel;
using System;
using System.Reflection;

namespace AtDb.ModelFillers
{
    public abstract class AbstractModelFiller
    {
        protected readonly ClassMaker classMaker;

        protected object model;
        protected TableDataContainer tableData;
        protected Type modelType;

        public BaseDataElement currentDataObject;

        public AbstractModelFiller(ClassMaker classMaker)
        {
            this.classMaker = classMaker;
        }

        public virtual void Fill(object model, TableDataContainer tableData)
        {
            this.model = model;
            this.tableData = tableData;

            modelType = model.GetType();
        }

        protected Type GetGenericTypeAtIndex(object collection, int FIRST_ARGUMENT)
        {
            Type collectionType = collection.GetType();
            TypeInfo typeInfo = collectionType.GetTypeInfo();
            Type generic = typeInfo.GenericTypeArguments[FIRST_ARGUMENT];
            return generic;
        }

        protected void SetValue(AttributeDefinition attribute, IRow row)
        {
            if (IsField(attribute))
            {
                SetFieldValue(attribute, row);
            }
            else if(IsProperty(attribute))
            {
                SetPropertyValue(attribute, row);
            }
            else
            {
                //todo error logging
            }
        }

        private bool IsField(AttributeDefinition attribute)
        {
            FieldInfo fieldInfo = GetFieldInfo(attribute.Name);
            return fieldInfo != null;
        }

        private void SetFieldValue(AttributeDefinition attribute, IRow row)
        {
            FieldInfo fieldInfo = GetFieldInfo(attribute.Name);
            object value = GetValue(attribute, row);
            fieldInfo.SetValue(currentDataObject, value);
        }

        private FieldInfo GetFieldInfo(string fieldName)
        {
            Type type = currentDataObject.GetType();
            FieldInfo fieldInfo = type.GetField(fieldName, Constants.BINDING_FLAGS);
            return fieldInfo;
        }

        private bool IsProperty(AttributeDefinition attribute)
        {
            PropertyInfo propertyIfno = GetPropertyInfo(attribute.Name);
            return propertyIfno != null;
        }

        private void SetPropertyValue(AttributeDefinition attribute, IRow row)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(attribute.Name);
            object value = GetValue(attribute, row);
            propertyInfo.SetValue(currentDataObject, value);
        }

        private PropertyInfo GetPropertyInfo(string fieldName)
        {
            Type type = currentDataObject.GetType();
            return type.GetProperty(fieldName, Constants.BINDING_FLAGS);
        }

        private object GetValue(AttributeDefinition attribute, IRow row)
        {
            object value;
            if(attribute.IsSingleValue)
            {
                value = GetSingleCellValue(attribute, row);
            }
            else
            {
                value = GetMultipleCellValue(attribute, row);
            }
            return value;
        }

        private object GetSingleCellValue(AttributeDefinition attribute, IRow row)
        {
            ICell cell = row.GetCell(attribute.StartIndex);
            object value = GetCellValue(attribute.Type, cell);
            return value;
        }

        private object GetMultipleCellValue(AttributeDefinition attribute, IRow row)
        {
            Array array = GetarrayForAttributeType(attribute);
            for (int i = attribute.StartIndex, j = 0; i <= attribute.EndIndex; ++i, ++j)
            {
                ICell cell = row.GetCell(i);
                object value = GetCellValue(attribute.Type, cell);
                array.SetValue(value, j);
            }

            return array;
        }

        private Array GetarrayForAttributeType(AttributeDefinition attribute)
        {
            Type genericType = classMaker.GetType(attribute.Type);
            Array array = Array.CreateInstance(genericType, attribute.Length);
            return array;
        }

        private Converter<object, string> ConvertObjectToT()
        {
            return (item) => (string)item;
        }

        private object GetCellValue(string type, ICell cell)
        {
            object value;
            switch(type)
            {
                case "int":
                    value = (int)cell.NumericCellValue;
                    break;
                case "long":
                    value = (long)cell.NumericCellValue;
                    break;
                case "float":
                    value = (float)cell.NumericCellValue;
                    break;
                case "double":
                    value = cell.NumericCellValue;
                    break;
                case "string":
                    value = cell.StringCellValue;
                    break;
                case "bool":
                    value = cell.BooleanCellValue;
                    break;
                default:
                    //todo log error
                    value = null;
                    break;
            }

            return value;
        }
    }
}
