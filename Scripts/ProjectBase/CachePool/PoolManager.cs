using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * 缓存池一般都是先取后存    如果取的时候为空，则自己实例化一个，用完后再存
 * 如果存用完的对象时候没有对应抽屉集可以装下，则需要创建一个
 */
/// <summary>
/// 缓存池模块，唯一性继承单例基类(形容成一个衣柜)
/// </summary>
public class PoolManager:Singleton<PoolManager>
{
    /// <summary>
    ///缓存池容器字典，string为对象类名，List<GameObject>为资源集合
    /// </summary>
    public Dictionary<string,PoolData> poolDic = new Dictionary<string, PoolData>();
    //作为所有层级面板中失活不用存储回池中的空根物体（便于直观观看存与借）
    //存的时候隐藏就会变为其子物体
    //取的时显示候则会断绝父子关系
    private GameObject poolObj;

    /// <summary>
    /// 从池中借的方法
    /// </summary>
    /// <param name="name">要拿哪个类的GameObject名称（哪个抽屉集）</param>
    /// <returns>返回拿到的GameObject</returns>
    public void GetObj(string name,UnityAction<GameObject> callback)
    {
        //从池中获取对象
        //GameObject obj = null;

        //如果池中有该对象集，则从池中取出一个对象并返回给回调函数
        //不仅要满足存在该对象集，还要里面有具体的对象（没有被取完）
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count>0)
        {
            callback(poolDic[name].Get());
        }
        else
        {
            //如果没有该对象集，则需要创建一个新的对象（利用自封装资源加载器异步加载）
            ResourcesManager.Instance.LoadAsync<GameObject>(name, (data) =>
            {
                data.name = name;
                callback(data);
            });
            //obj = GameObject.Instantiate(Resources.Load<GameObject>(name));

            //把对象名字，改的和抽屉集名字一样（方便存）
            //obj.name = name;
        }
        //return obj;
    }

    /// <summary>
    /// 存进池中的方法（把借走后不用的放回来）
    /// </summary>
    /// <param name="name">存进哪个抽屉集</param>
    /// <param name="obj">存进的GameObject</param>
    /// <returns>返回是否存成功</returns>
    public void SaveObj(string name,GameObject obj)
    {
        //用于管理存进池中隐藏的GameObject
        if (poolObj == null)
        {
            //如果没有则创建空物体
            poolObj = new GameObject("Pool");
        }

        //如果有该抽屉集合，则可以直接存
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].Save(obj);
        }
        //如果没有该抽屉集合，则需要创建一个抽屉集类，后自动在其构造函数中存储GameObject
        else
        {
            poolDic.Add(name, new PoolData(obj,poolObj));
        }
    }
    /// <summary>
    /// 清空缓存池的方法，主要用于场景切换时（释放无用的资源，保持新场景的独立性）
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}

/// <summary>
/// 每一个抽屉集的类（用于存储生成父对象管理所有GameObject和GameObject List）
/// </summary>
public class PoolData
{
    ///抽屉集空父对象的名字
    public GameObject fatherObj;
    ///抽屉装载的所有物体
    public List<GameObject> poolList;

    public PoolData(GameObject obj,GameObject poolObj)
    {
        //创建一个这个抽屉集，作为所有GameObject的父物体
        fatherObj = new GameObject("father_"+obj.name);
        //同时该抽屉集要作为总池子的子物体
        fatherObj.transform.SetParent(poolObj.transform, false);

        poolList = new List<GameObject>();

        //新建抽屉集后，也需要将该obj存进抽屉集
        Save(obj);
    }
    /// <summary>
    /// 从抽屉集存进对象的方法
    /// </summary>
    /// <param name="obj"></param>
    public void Save(GameObject obj)
    {
        //失活该对象，让其隐藏
        obj.SetActive(false);

        //存进抽屉集，设置父物体管理
        poolList.Add(obj);
        obj.transform.SetParent(fatherObj.transform, false);
    }
    /// <summary>
    /// 从抽屉集中借出对象的方法
    /// </summary>
    /// <returns></returns>
    public GameObject Get()
    {
        GameObject obj = null;

        //取出抽屉集中的第一个对象
        obj = poolList[0];
        //借走后需要移除该索引的GameObject
        poolList.RemoveAt(0);

        //断绝父子关系
        obj.transform.parent = null;
        //激活该对象，让其显示
        obj.SetActive(true);

        return obj;
    }
}
