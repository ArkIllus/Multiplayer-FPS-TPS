using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 双锁double lock
public class BaseManager<T> where T : new()
{
    private static T instance;

    //保险起见：私有化构造函数
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
