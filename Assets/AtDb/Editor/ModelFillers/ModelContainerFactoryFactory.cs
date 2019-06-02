using AtDb.ModelFillers.Pool;

namespace AtDb.Reader.Container
{
    public class ModelContainerFactoryFactory
    {
        private readonly ClassMaker classMaker;
        private readonly ModelFillerTypePool fillerPool;

        public ModelContainerFactoryFactory()
        {
            classMaker = new ClassMaker();
            fillerPool = new ModelFillerTypePool(classMaker, Constants.THREADS);
        }

        public ModelContainerFactory Create()
        {
            return new ModelContainerFactory(classMaker, fillerPool);
        }
    }
}