using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;

namespace AtDb.Reader
{
    public class ClassMaker
    {
        private readonly Dictionary<string, Type> assemblyTypes;
        private readonly Assembly mainAssembly;

        public ClassMaker()
        {
            const string UNITY_RUNTIME_ASSEMBLY = "Assembly-CSharp";

            mainAssembly = Assembly.Load(UNITY_RUNTIME_ASSEMBLY);
            Type[] types = mainAssembly.GetExportedTypes();

            assemblyTypes = new Dictionary<string, Type>();
            foreach (Type type in types)
            {
                assemblyTypes.Add(type.Name, type);
            }
        }

        public object MakeClass(string className)
        {
            Type type = GetClasstype(className);
            ObjectHandle objectHandle = Activator.CreateInstance(mainAssembly.FullName, type.FullName) as ObjectHandle;
            object classInstance = objectHandle.Unwrap();

            return classInstance;
        }

        private Type GetClasstype(string className)
        {
            return assemblyTypes[className];
        }
    }
}