using AtDb.Reader;
using System;
using System.Collections.Generic;

namespace AtDb.ModelFillers.Pool
{
    public class ModelFillerTypePool
    {
        private readonly Dictionary<Type, ModelFillerPool> modelPools = new Dictionary<Type, ModelFillerPool>();
        private readonly ClassMaker classMaker;

        public ModelFillerTypePool(ClassMaker classMaker, int preAllocateCount)
        {
            this.classMaker = classMaker;
            InitializeDictionary(preAllocateCount);
        }

        public AbstractModelFiller GetFiller<T>() where T : AbstractModelFiller
        {
            Type type = (typeof(T));
            ModelFillerPool pool = modelPools[type];
            T filler = pool.Get<T>();
            return filler;
        }

        private void InitializeDictionary(int preallocateCount)
        {
            Type type = typeof(DirectModelFiller);
            InitializeEntry(preallocateCount, type);
            type = typeof(ListModelFiller);
            InitializeEntry(preallocateCount, type);
            type = typeof(DictionaryModelFiller);
            InitializeEntry(preallocateCount, type);
        }

        private void InitializeEntry(int preallocateCount, Type type)
        {
            modelPools.Add(type, new ModelFillerPool(type, preallocateCount, classMaker));
        }
    }
}