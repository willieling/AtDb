using AtDb.ModelFillers.Pool;

namespace AtDb.Reader.Container
{
    public class ModelContainerFactoryFactory
    {
        private readonly ClassMaker classMaker;
        private readonly ModelFillerTypePool fillerPool;

        public ModelContainerFactoryFactory(ClassMaker classMaker)
        {
            this.classMaker = classMaker;
            fillerPool = new ModelFillerTypePool(this.classMaker, Constants.THREADS);
        }

        public ModelContainerFactory Create()
        {
            return new ModelContainerFactory(classMaker, fillerPool);
        }
    }
}