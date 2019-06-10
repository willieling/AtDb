using AtDb.ErrorSystem;
using System;
using System.Reflection;

namespace AtDb.ModelFillers
{
    public class CollectionInfo
    {
        public enum CollectionInfoError
        {
            None,
            UnexpectedAmountOfMembers
        }

        private Type modelType;

        public Type CollectionType { get; private set; }
        public Type FirstGenericType { get; private set; }
        public Type SecondGenericType { get; private set; }

        public bool IsDictionary
        {
            get { return SecondGenericType == null; }
        }

        public FieldInfo CollectionField { get; private set; }
        public PropertyInfo CollectionProperty { get; private set; }

        public CollectionInfoError Error { get; private set; }

        public CollectionInfo(Type modelType)
        {
            this.modelType = modelType;
            CacheTypes();
        }

        private void CacheTypes()
        {
            CacheCollectionType();
            CacheFirstGeneric();
            CacheSecondGeneric();
        }

        private void CacheCollectionType()
        {
            const int EXEPCTED_MEMBERS = 1;
            const int FIRST_ELEMENT = 0;

            FieldInfo[] fields = modelType.GetFields(Constants.BINDING_FLAGS);
            PropertyInfo[] properties = modelType.GetProperties(Constants.BINDING_FLAGS);

            if (fields.Length + properties.Length > EXEPCTED_MEMBERS)
            {
                Error = CollectionInfoError.UnexpectedAmountOfMembers;
            }

            Type type;
            if (fields.Length > 0)
            {
                FieldInfo fieldInfo = fields[FIRST_ELEMENT];
                type = fieldInfo.FieldType;
                CollectionField = fieldInfo;
            }
            else
            {
                PropertyInfo propertyInfo = properties[FIRST_ELEMENT];
                type = propertyInfo.PropertyType;
                CollectionProperty = propertyInfo;
            }

            CollectionType = type;
        }

        private void CacheFirstGeneric()
        {
            const int FIRST_ELEMENT = 0;

            Type type = GetGenericTypeAtIndex(FIRST_ELEMENT);
            FirstGenericType = type;
        }

        private void CacheSecondGeneric()
        {
            const int SECOND_ELEMENT = 1;

            Type type = GetGenericTypeAtIndex(SECOND_ELEMENT);
            SecondGenericType = type;
        }

        private Type GetGenericTypeAtIndex(int elementIndex)
        {
            TypeInfo typeInfo = CollectionType.GetTypeInfo();
            Type genericType;
            if (elementIndex < typeInfo.GenericTypeArguments.Length)
            {
                genericType = typeInfo.GenericTypeArguments[elementIndex];
            }
            else
            {
                genericType = null;
            }

            return genericType;
        }
    }
}