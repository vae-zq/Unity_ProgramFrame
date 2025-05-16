using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例基类
/// </summary>
/// <typeparam name="T">必须约束为存在无参公共构造函数，否则无法new()（泛型类必须存在无参构造）</typeparam>
public class Singleton<T> where T : new()
{
    private static T instance;
    //如果实例为空，则创建一个新的实例
    public static T Instance => instance ?? (instance = new T());
}
