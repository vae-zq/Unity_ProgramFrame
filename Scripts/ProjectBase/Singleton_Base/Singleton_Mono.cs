using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton_Mono<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T instance;
    //继承MonoBehaviour的单例类不能使用new()创建实例
    //只能通过拖动到对象上 或者通过AddComponent去加脚本创建实例
    //所以需要保证它的实例只能有一个
    public static T Instance=>instance;

    //防止子类需要重写Awake时，父类的Awake被覆盖导致无法获取实例，所以写成虚函数
    protected virtual void Awake()
    {
        //等于自身，所以只能被挂载一次（若挂载多次，只会等于最后一个进Awake的GameObject）
        instance= this as T;
    }
}
