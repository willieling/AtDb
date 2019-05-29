using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Test : MonoBehaviour
{
    private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public;

    void Start()
    {
        const int EXEPCTED_MEMBERS = 1;

        object model = new TestObject();
        Type modelType = model.GetType();
        FieldInfo[] fields = modelType.GetFields(BINDING_FLAGS);
        PropertyInfo[] properties = modelType.GetProperties(BINDING_FLAGS);

        if (fields.Length + properties.Length > EXEPCTED_MEMBERS)
        {
            //todo error logging
        }

        FieldInfo field = fields[0];
        Type fieldType = field.FieldType;
        TypeInfo typeInfo = fieldType.GetTypeInfo();
        Type generic = typeInfo.GenericTypeArguments[0];
    }
}

public class TestObject
{
    public List<int> list;
}
