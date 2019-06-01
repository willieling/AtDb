using AtDb.Reader;
using AtDb.Reader.Container;
using System;

namespace AtDb.ModelFillers
{
    public abstract class CollectionModelFiller : AbstractModelFiller
    {
        protected readonly CollectionInfo collectionInfo;

        public CollectionModelFiller(ClassMaker classMaker, object model, TableDataContainer tableData) : base(classMaker, model, tableData)
        {
            collectionInfo = new CollectionInfo(modelType);
        }

        protected void SetModelCollectionMember(object collection)
        {
            if (collectionInfo.CollectionField != null)
            {
                collectionInfo.CollectionField.SetValue(model, collection);
            }
            else
            {
                collectionInfo.CollectionProperty.SetValue(model, collection);
            }
        }

        protected T CreateCollection<T>() where T : class
        {
            T collection = Activator.CreateInstance(collectionInfo.CollectionType) as T;
            return collection;
        }

        protected BaseDataElement CreateGenericElement(Type elementType)
        {
            BaseDataElement genericObject = Activator.CreateInstance(elementType) as BaseDataElement;
            return genericObject;
        }
    }
}