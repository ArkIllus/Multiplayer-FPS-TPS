using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �¼�����
/// </summary>
public class EventCenter : BaseManager<EventCenter>
{
    //key �¼�������
    //value �������¼��Ķ�Ӧί�к���
    //<object>�����κ����͵Ķ��������ȱ����ֵ���ͻ�װ��
    private Dictionary<string, UnityAction<object>> eventDic = new Dictionary<string, UnityAction<object>>();

    //����¼�����
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

    //�Ƴ��¼�����
    public void RemoveEventListener(string name, UnityAction<object> action)
    {
        if (eventDic.ContainsKey(name))
        {
            eventDic[name] -= action;
        }
    }

    //�¼�����
    public void EventTrigger(string name, object info)
    {
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name](info);
            eventDic[name].Invoke(info);
        }
    }

    //����¼�����
    //��Ҫ���ڳ����л�ʱ
    public void Clear()
    {
        eventDic.Clear();
    }
}
