using AtDb.Reader;
using AtDb.Reader.Container;
using NPOI.SS.UserModel;

namespace AtDb.ModelFillers
{
    public class DirectModelFiller : AbstractModelFiller
    {
        public DirectModelFiller(ClassMaker classMaker) : base(classMaker)
        {
        }

        public override void Fill(object model, TableDataContainer tableData)
        {
            const int EXPECTED_COUNT = 1;

            base.Fill(model, tableData);

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
