using AtDb.ModelFillers;
using System;

namespace AtDb.Reader.Container
{
    public class ModelDataContainer
    {
        private readonly ClassMaker classMaker;
        private object model;
        private TableDataContainer tableData;

        public string DataString { get; private set; }

        public ModelDataContainer(ClassMaker classMaker, object model, TableDataContainer tableData)
        {
            this.classMaker = classMaker;
            this.model = model;
            this.tableData = tableData;

            FillModelWithData();
        }

        private void FillModelWithData()
        {
            AbstractModelFiller modelFiller;

            //todo pool modelFillers?
            switch (tableData.metadata.Style)
            {
                case DataStyle.Direct:
                    modelFiller = new DirectModelFiller(classMaker, model, tableData);
                    break;
                case DataStyle.List:
                    modelFiller = new ListModelFiller(classMaker, model, tableData);
                    break;
                case DataStyle.Dictionary:
                    modelFiller = new DictionaryModelFiller(classMaker, model, tableData);
                    break;
                default:
                    //todo better error checking
                    throw new Exception();
            }

            //todo move model and table data arguments to fill function
            modelFiller.Fill();
            string json = modelFiller.DumpDataToJson();
            DataString = json;
        }
    }
}