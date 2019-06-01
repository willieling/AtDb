using AtDb.Reader;
using AtDb.Reader.Container;
using NPOI.SS.UserModel;
using System;
using System.Collections;

namespace AtDb.ModelFillers
{
    public class ListModelFiller : CollectionModelFiller
    {
        public ListModelFiller(ClassMaker classMaker, object model, TableDataContainer tableData) : base(classMaker, model, tableData)
        {
        }

        public override void Fill()
        {
            IList modelList = CreateCollection<IList>();

            foreach (IRow row in tableData.rawData)
            {
                BaseDataElement listElement = CreateListElement();
                currentDataObject = listElement;

                foreach (AttributeDefinition attribute in tableData.attributes)
                {
                    SetValue(attribute, row);
                }

                modelList.Add(currentDataObject);
            }

            SetModelCollectionMember(modelList);
        }

        private BaseDataElement CreateListElement()
        {
            Type elementType = collectionInfo.FirstGenericType;
            BaseDataElement genericObject = CreateGenericElement(elementType);
            return genericObject;
        }
    }
}
