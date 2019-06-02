using AtDb.ModelFillers;
using AtDb.ModelFillers.Pool;
using System;

namespace AtDb.Reader.Container
{
    public class ModelContainerFactory
    {
        private readonly ClassMaker classMaker;
        private readonly ModelFillerTypePool fillerPool;

        public ModelContainerFactory(ClassMaker classMaker, ModelFillerTypePool fillerPool)
        {
            this.classMaker = classMaker;
            this.fillerPool = fillerPool;
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
            AbstractModelFiller modelFiller = GetModelFiller(tableData.metadata.Style);
            modelFiller.Fill(model, tableData);
            return model;
        }

        private AbstractModelFiller GetModelFiller(DataStyle style)
        {
            AbstractModelFiller modelFiller;
            switch (style)
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
                    //todo better error checking
                    throw new Exception();
            }

            return modelFiller;
        }
    }
}