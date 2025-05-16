using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 通过单例 MonoManager 来管理MonoController，可以实现全局唯一性、对其进行封装
/// 提供给外部添加协程的方法
/// 提供给外部添加和移除帧更新事件的方法
/// </summary>
public class MonoManager : Singleton<MonoManager>
{
    private MonoController monoController;

    //继承自单例基类，初次为null，会自动new T()，所以会进该构造函数
    public MonoManager()
    {
        //只会new一次，保证了MonoController对象其唯一性
        GameObject obj = new GameObject("MonoController");
        monoController = obj.AddComponent<MonoController>();
    }

    /// <summary>
    /// 给外部提供的添加帧更新事件的方法
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        monoController.AddUpdateListener(action);
    }
    /// <summary>
    /// 给外部提供的移除帧更新事件的方法
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action)
    {
        monoController.RemoveUpdateListener(action);
    }

    /// <summary>
    /// 给外部提供的添加协程的方法（通过 Coroutine 对象）
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return monoController.StartCoroutine(routine);
    }
    /// <summary>
    /// 给外部提供的添加协程的方法（通过方法名）
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return monoController.StartCoroutine(methodName, value);
    }
    /// <summary>
    /// 给外部提供的添加协程的方法（通过方法名）
    /// </summary>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName)
    {
        return monoController.StartCoroutine(methodName);
    }

    /// <summary>
    /// 停止协程的方法（通过 Coroutine 对象）
    /// </summary>
    /// <param name="coroutine">要停止的协程对象</param>
    public void StopCoroutine(Coroutine coroutine)
    {
        monoController.StopCoroutine(coroutine);
    }

    /// <summary>
    /// 停止协程的方法（通过方法名）
    /// </summary>
    /// <param name="methodName">要停止的协程方法名</param>
    public void StopCoroutine(string methodName)
    {
        monoController.StopCoroutine(methodName);
    }

    /// <summary>
    /// 停止所有协程的方法
    /// </summary>
    public void StopAllCoroutines()
    {
        monoController.StopAllCoroutines();
    }

}
