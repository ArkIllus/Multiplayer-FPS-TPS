using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件中心
/// </summary>
public class EventCenter : BaseManager<EventCenter>
{
    //key 事件的名字
    //value 监听该事件的对应委托函数
    //<object>传递任何类型的对象参数，缺点是值类型会装箱
    private Dictionary<string, UnityAction<object>> eventDic = new Dictionary<string, UnityAction<object>>();

    //添加事件监听
    public void AddEventListener(string name, UnityAction<object> action)
    {
        if (!eventDic.ContainsKey(name))
        {
            eventDic.Add(name, action);
        }
        else
        {
            eventDic[name] += action;
        }
    }

    //移除事件监听
    public void RemoveEventListener(string name, UnityAction<object> action)
    {
        if (eventDic.ContainsKey(name))
        {
            eventDic[name] -= action;
        }
    }

    //事件触发
    public void EventTrigger(string name, object info)
    {
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name](info);
            eventDic[name].Invoke(info);
        }
    }

    //清空事件中心
    //主要用在场景切换时
    public void Clear()
    {
        eventDic.Clear();
    }
}
