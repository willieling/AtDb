using AtDb.ErrorSystem;
using AtDb.ModelFillers;
using AtDb.ModelFillers.Pool;

namespace AtDb.Reader.Container
{
    public class ModelContainerFactory : IErrorLogger
    {
        private readonly ClassMaker classMaker;
        private readonly ModelFillerTypePool fillerPool;

        public ErrorLogger ErrorLogger { get; private set; }


        public ModelContainerFactory(ClassMaker classMaker, ModelFillerTypePool fillerPool)
        {
            this.classMaker = classMaker;
            this.fillerPool = fillerPool;
            ErrorLogger = new ErrorLogger();
        }

        public ModelDataContainer Create(TableDataContainer tableData)
        {
            object model = ConvertDataToObject(tableData);

            ModelDataContainer container = new ModelDataContainer(model, tableData.metadata);
            return container;
        }

        private object ConvertDataToObject(TableDataContainer tableData)
        {
            object model = classMaker.MakeClass(tableData.metadata.ClassName);
            AbstractModelFiller modelFiller = GetModelFiller(tableData);
            if (modelFiller != null)
            {
                modelFiller.Fill(model, tableData);
                ErrorLogger.CopyNotices(modelFiller);
            }

            return model;
        }

        private AbstractModelFiller GetModelFiller(TableDataContainer tableData)
        {
            AbstractModelFiller modelFiller;
            switch (tableData.metadata.Style)
            {
                case DataStyle.Direct:
                    modelFiller = fillerPool.GetFiller<DirectModelFiller>();
                    break;
                case DataStyle.List:
                    modelFiller = fillerPool.GetFiller<ListModelFiller>();
                    break;
                case DataStyle.Dictionary:
                    modelFiller = fillerPool.GetFiller<DictionaryModelFiller>();
                    break;
                default:
                    modelFiller = null;
                    ErrorLogger.AddError("Cannot fill model for {0}", tableData.metadata.TableName);
                    break;
            }

            return modelFiller;
        }
    }
}