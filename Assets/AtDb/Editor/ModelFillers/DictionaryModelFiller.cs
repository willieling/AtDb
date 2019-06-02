using AtDb.Reader;
using AtDb.Reader.Container;
using NPOI.SS.UserModel;
using System;
using System.Collections;

namespace AtDb.ModelFillers
{
    public class DictionaryModelFiller : CollectionModelFiller
    {
        public DictionaryModelFiller(ClassMaker classMaker) : base(classMaker)
        {
        }

        public override void Fill(object model, TableDataContainer tableData)
        {
            base.Fill(model, tableData);
            IDictionary dictionary = CreateCollection<IDictionary>();

            foreach (IRow row in tableData.rawData)
            {
                BaseDataElement listElement = CreateDictionaryValue();
                currentDataObject = listElement;

                foreach (AttributeDefinition attribute in tableData.attributes)
                {
                    SetValue(attribute, row);
                
                }

                dictionary.Add(currentDataObject.uid, currentDataObject);
            }

            SetModelCollectionMember(dictionary);
        }

        private BaseDataElement CreateDictionaryValue()
        {
            Type elementType = collectionInfo.SecondGenericType;
            BaseDataElement genericObject = CreateGenericElement(elementType);
            return genericObject;
        }
    }
}
