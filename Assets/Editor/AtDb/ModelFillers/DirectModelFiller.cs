using AtDb.Reader;
using AtDb.Reader.Container;
using NPOI.SS.UserModel;

namespace AtDb.ModelFillers
{
    public class DirectModelFiller : AbstractModelFiller
    {
        public DirectModelFiller(ClassMaker classMaker, object model, TableDataContainer tableData) : base(classMaker, model, tableData)
        {
        }

        public override void Fill()
        {
            const int EXPECTED_COUNT = 1;

            currentDataObject = model as BaseDataElement;

            if (tableData.rawData.Count > EXPECTED_COUNT)
            {
                //todo error logging
            }

            IRow row = tableData.rawData[0];

            foreach (AttributeDefinition attribute in tableData.attributes)
            {
                SetValue(attribute, row);
            }
        }
    }
}
