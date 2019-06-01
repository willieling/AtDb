using AtDb.ModelFillers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtDb.Reader.Container
{
    public class ModelContainerFactory
    {
        private ClassMaker classMaker;

        public ModelContainerFactory(ClassMaker classMaker)
        {
            this.classMaker = classMaker;
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
                    modelFiller = new DirectModelFiller(classMaker);
                    break;
                case DataStyle.List:
                    modelFiller = new ListModelFiller(classMaker);
                    break;
                case DataStyle.Dictionary:
                    modelFiller = new DictionaryModelFiller(classMaker);
                    break;
                default:
                    //todo better error checking
                    throw new Exception();
            }

            return modelFiller;
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