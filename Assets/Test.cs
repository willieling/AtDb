using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Test : MonoBehaviour
{
    private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public;

    void Start()
    {
        List<int> test = new List<int>();
        IList ilist = test;

        object obj = 5;
        ilist.Add(obj);
    }
}

public class TestObject
{
    public List<int> list;
}
