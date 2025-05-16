using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承MonoBehaviour的单例类（不再需要手动挂载或添加到对象上）
/// 想用它，直接获取Instance就行了
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonAuto_Mono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    //继承MonoBehaviour的单例类不能使用new()创建实例
    //只能通过拖动到对象上 或者通过AddComponent去加脚本创建实例
    //所以需要保证它的实例只能有一个
    public static T Instance
    {
        get 
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                //设置对象的名字为脚本的名字
                obj.name = typeof(T).Name;

                //让该单例模式对象过场景不被移除，因为单例模式对象一般是存在整个游戏生命周期的
                GameObject.DontDestroyOnLoad(obj);

                //添加到对象上后返回该实例
                instance = obj.AddComponent<T>();
            }
            return instance; 
        }
    }
}