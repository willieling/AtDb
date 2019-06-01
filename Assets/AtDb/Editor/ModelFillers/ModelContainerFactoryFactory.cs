using UnityEngine;

namespace AtDb.Reader.Container
{
    public class ModelContainerFactoryFactory
    {
        private readonly ClassMaker classMaker = new ClassMaker();

        public ModelContainerFactory Create()
        {
            return new ModelContainerFactory(classMaker);
        }
    }
}