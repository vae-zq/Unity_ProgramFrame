using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载的管理器（单例）
/// </summary>
public class ResourcesManager:Singleton<ResourcesManager>
{
    /// <summary>
    /// 提供给外部资源同步加载的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T Load<T>(string path) where T : Object
    {
        T res = Resources.Load<T>(path);

        //如果对象是GameObject类型，可以把它实例化后，再返回出去，
        //外部只需要直接使用即可，更方便
        if (res is GameObject)
        {
            return GameObject.Instantiate(res);
        }
        else
        {
            //如果是其他类型的对象（音效、图片、文本等），则直接返回
            return res;
        }
    }

    /// <summary>
    /// 提供给外部资源异步加载的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        //因为没有继承MonoBehaviour，所以不能直接使用StartCoroutine，通过MonoManager来调用
        MonoManager.Instance.StartCoroutine(ReallyLoadAsync<T>(path, callback));
    }
    /// <summary>
    /// 内部协程异步加载资源 用于开启异步加载对应的资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        yield return request;

        if(request.asset is GameObject)
        {
            //如果对象是GameObject类型，可以把它实例化后，再返回出去，
            //外部只需要直接使用即可，更方便
            callback(GameObject.Instantiate((request.asset) as T));
        }
        else
        {
            //如果是其他类型的对象（音效、图片、文本等），则直接返回
            callback(request.asset as T);
        }
    }
}
