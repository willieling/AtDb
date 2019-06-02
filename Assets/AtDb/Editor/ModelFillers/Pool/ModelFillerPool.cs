using AtDb.Reader;
using System;
using System.Collections.Generic;

namespace AtDb.ModelFillers.Pool
{
    public class ModelFillerPool
    {
        private readonly List<AbstractModelFiller> actives = new List<AbstractModelFiller>();
        private readonly List<AbstractModelFiller> idles = new List<AbstractModelFiller>();

        private readonly ClassMaker classMaker;

        public ModelFillerPool(Type type, int preAllocateCount, ClassMaker classMaker)
        {
            this.classMaker = classMaker;

            actives = new List<AbstractModelFiller>(preAllocateCount);
            idles = new List<AbstractModelFiller>(preAllocateCount);

            PreAllocateFillers(preAllocateCount, type);
        }

        public T Get<T>() where T : AbstractModelFiller
        {
            AbstractModelFiller filler;
            if (idles.Count > 0)
            {
                int index = idles.Count - 1;
                filler = idles[index];
                idles.RemoveAt(index);

                actives.Add(filler);
            }
            else
            {
                filler = CreateInstance(typeof(T));
                actives.Add(filler);
            }
            return filler as T;
        }

        private void PreAllocateFillers(int preAllocateCount, Type type)
        {
            for (int i = 0; i < preAllocateCount; ++i)
            {
                AbstractModelFiller filler = CreateInstance(type);
                idles.Add(filler);
            }
        }

        private AbstractModelFiller CreateInstance(Type type)
        {
            return Activator.CreateInstance(type, classMaker) as AbstractModelFiller;
        }
    }
}