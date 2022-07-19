using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// ��������Hierarchy�й۲죩
/// </summary>
public class PoolData
{
    //����List�ĸ�����
    public GameObject parentObj;
    //���������
    public List<GameObject> poolList;

    //constructor
    public PoolData(GameObject obj, GameObject poolObj)
    {
        //�������� = ����obj��
        this.parentObj = new GameObject(obj.name); 
        parentObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>() { };
        PushObj(obj);
    }

    public void PushObj(GameObject obj)
    {
        poolList.Add(obj);
        obj.transform.parent = parentObj.transform;
    }

    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        //��ʾ
        obj.SetActive(true);
        //��������Ϊ�գ�������Hierarchy�й۲죩
        obj.transform.parent = null;
        return obj;
    }
}

/// <summary>
/// �����ģ��
/// </summary>
public class PoolManager : BaseManager<PoolManager>
{
    //���������
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    private GameObject poolObj;

    /// <summary>
    /// �����ö���
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetObj(string name)
    {
        GameObject obj = null;
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            obj = poolDic[name].GetObj();
        }
        else
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            //��������ָĳɳ��ӵ�����
            obj.name = name;
        }
        return obj;
    }

    /// <summary>
    /// ����涫��
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void PushObj(string name, GameObject obj)
    {
        //����صĸ��ڵ����� ��ΪPool
        if (poolObj == null)
        {
            poolObj = new GameObject("Pool");
        }
        //���ø�����ΪPool���壨������Hierarchy�й۲죩
        obj.transform.parent = poolObj.transform;

        //����
        obj.SetActive(false);
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        else
        {
            poolDic.Add(name, new PoolData(obj, poolObj));
        }
    }

    /// <summary>
    /// ��ջ����
    /// ��Ҫ���� �����л�ʱ
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
