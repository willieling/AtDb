namespace AtDb.Reader.Container
{
    public class ModelDataContainer
    {
        public readonly object data;
        public readonly TableMetadata metadata;

        public ModelDataContainer(object data, TableMetadata metadata)
        {
            this.data = data;
            this.metadata = metadata;


            //FillModelWithData();
        }

        //private void FillModelWithData()
        //{
        //    //todo move this functionality outisde the class
        //    //create a factory to make ModelDataContainer

        //    //todo pool modelFillers?
        //    switch (tableData.metadata.Style)
        //    {
        //        case DataStyle.Direct:
        //            modelFiller = new DirectModelFiller(classMaker, model, tableData);
        //            break;
        //        case DataStyle.List:
        //            modelFiller = new ListModelFiller(classMaker, model, tableData);
        //            break;
        //        case DataStyle.Dictionary:
        //            modelFiller = new DictionaryModelFiller(classMaker, model, tableData);
        //            break;
        //        default:
        //            //todo better error checking
        //            throw new Exception();
        //    }

        //    //todo move model and table data arguments to fill function
        //    modelFiller.Fill();
        //}
    }
}