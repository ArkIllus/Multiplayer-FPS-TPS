using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: ˫��double lock
public class BaseManager<T> where T : new()
{
    private static T instance;

    //���������˽�л����캯��
    private BaseManager()
    {

    }

    public static T GetInstance()
    {
        if(instance == null)
        {
            instance = new T();
        }
        return instance;
    }
}
