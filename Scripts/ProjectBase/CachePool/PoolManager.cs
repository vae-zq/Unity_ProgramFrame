using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * �����һ�㶼����ȡ���    ���ȡ��ʱ��Ϊ�գ����Լ�ʵ����һ����������ٴ�
 * ���������Ķ���ʱ��û�ж�Ӧ���뼯����װ�£�����Ҫ����һ��
 */
/// <summary>
/// �����ģ�飬Ψһ�Լ̳е�������(���ݳ�һ���¹�)
/// </summary>
public class PoolManager:Singleton<PoolManager>
{
    /// <summary>
    ///����������ֵ䣬stringΪ����������List<GameObject>Ϊ��Դ����
    /// </summary>
    public Dictionary<string,PoolData> poolDic = new Dictionary<string, PoolData>();
    //��Ϊ���в㼶�����ʧ��ô洢�س��еĿո����壨����ֱ�۹ۿ�����裩
    //���ʱ�����ؾͻ��Ϊ��������
    //ȡ��ʱ��ʾ�����Ͼ����ӹ�ϵ
    private GameObject poolObj;

    /// <summary>
    /// �ӳ��н�ķ���
    /// </summary>
    /// <param name="name">Ҫ���ĸ����GameObject���ƣ��ĸ����뼯��</param>
    /// <returns>�����õ���GameObject</returns>
    public void GetObj(string name,UnityAction<GameObject> callback)
    {
        //�ӳ��л�ȡ����
        //GameObject obj = null;

        //��������иö��󼯣���ӳ���ȡ��һ�����󲢷��ظ��ص�����
        //����Ҫ������ڸö��󼯣���Ҫ�����о���Ķ���û�б�ȡ�꣩
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count>0)
        {
            callback(poolDic[name].Get());
        }
        else
        {
            //���û�иö��󼯣�����Ҫ����һ���µĶ��������Է�װ��Դ�������첽���أ�
            ResourcesManager.Instance.LoadAsync<GameObject>(name, (data) =>
            {
                data.name = name;
                callback(data);
            });
            //obj = GameObject.Instantiate(Resources.Load<GameObject>(name));

            //�Ѷ������֣��ĵĺͳ��뼯����һ��������棩
            //obj.name = name;
        }
        //return obj;
    }

    /// <summary>
    /// ������еķ������ѽ��ߺ��õķŻ�����
    /// </summary>
    /// <param name="name">����ĸ����뼯</param>
    /// <param name="obj">�����GameObject</param>
    /// <returns>�����Ƿ��ɹ�</returns>
    public void SaveObj(string name,GameObject obj)
    {
        //���ڹ������������ص�GameObject
        if (poolObj == null)
        {
            //���û���򴴽�������
            poolObj = new GameObject("Pool");
        }

        //����иó��뼯�ϣ������ֱ�Ӵ�
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].Save(obj);
        }
        //���û�иó��뼯�ϣ�����Ҫ����һ�����뼯�࣬���Զ����乹�캯���д洢GameObject
        else
        {
            poolDic.Add(name, new PoolData(obj,poolObj));
        }
    }
    /// <summary>
    /// ��ջ���صķ�������Ҫ���ڳ����л�ʱ���ͷ����õ���Դ�������³����Ķ����ԣ�
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}

/// <summary>
/// ÿһ�����뼯���ࣨ���ڴ洢���ɸ������������GameObject��GameObject List��
/// </summary>
public class PoolData
{
    ///���뼯�ո����������
    public GameObject fatherObj;
    ///����װ�ص���������
    public List<GameObject> poolList;

    public PoolData(GameObject obj,GameObject poolObj)
    {
        //����һ��������뼯����Ϊ����GameObject�ĸ�����
        fatherObj = new GameObject("father_"+obj.name);
        //ͬʱ�ó��뼯Ҫ��Ϊ�ܳ��ӵ�������
        fatherObj.transform.SetParent(poolObj.transform, false);

        poolList = new List<GameObject>();

        //�½����뼯��Ҳ��Ҫ����obj������뼯
        Save(obj);
    }
    /// <summary>
    /// �ӳ��뼯�������ķ���
    /// </summary>
    /// <param name="obj"></param>
    public void Save(GameObject obj)
    {
        //ʧ��ö�����������
        obj.SetActive(false);

        //������뼯�����ø��������
        poolList.Add(obj);
        obj.transform.SetParent(fatherObj.transform, false);
    }
    /// <summary>
    /// �ӳ��뼯�н������ķ���
    /// </summary>
    /// <returns></returns>
    public GameObject Get()
    {
        GameObject obj = null;

        //ȡ�����뼯�еĵ�һ������
        obj = poolList[0];
        //���ߺ���Ҫ�Ƴ���������GameObject
        poolList.RemoveAt(0);

        //�Ͼ����ӹ�ϵ
        obj.transform.parent = null;
        //����ö���������ʾ
        obj.SetActive(true);

        return obj;
    }
}
