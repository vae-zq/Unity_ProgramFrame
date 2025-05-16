using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//事件传递信息的接口（主要用来父类装子类）
public interface IEventInfo { }
//包裹事件信息的类，需要参数通过泛型避免使用object类型装箱拆箱
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        //为了方便在构造函数中添加传入的委托函数
        actions += action;
    }
}
//不需要传递参数的事件信息类
public class EventInfo : IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        //为了方便在构造函数中添加传入的委托函数
        actions += action;
    }
}

/// <summary>
/// 事件中心模块，单例模式对象（基于观察者的设计模式）
/// </summary>
public class EventCenter: Singleton<EventCenter>
{
    //用于存储需要监听的各种事件的字典
    //string为事件名称（通关、怪物死亡等） UnityAction<object>为事件委托（监听这个事件 对应的委托函数们）
    //IEventInfo为父类装子类的有形参UnityAction<T>（比如怪物死亡时，传递怪物的名字，或者经验值等）和无参UnityAction
    private Dictionary<string,IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// 添加事件监听（需要传递参数的事件）
    /// </summary>
    /// <param name="eventName">事件的名字</param>
    /// <param name="action">准备用来处理事件的委托函数</param>
    public void AddEventListener<T>(string eventName, UnityAction<T> action)
    {
        //如果字典中没有这个需要监听事件名称，则添加
        if (!eventDic.ContainsKey(eventName))
        {
            eventDic.Add(eventName, new EventInfo<T>(action));
        }
        //如果字典中有这个需要监听事件的名称，则添加监听函数
        else
        {
            (eventDic[eventName] as EventInfo<T>).actions += action;
        }
    }
    /// <summary>
    /// 添加事件监听（不需要传递参数的事件）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="action"></param>
    public void AddEventListener<T>(string eventName, UnityAction action)
    {
        //如果字典中没有这个需要监听事件名称，则添加
        if (!eventDic.ContainsKey(eventName))
        {
            eventDic.Add(eventName, new EventInfo(action));
        }
        //如果字典中有这个需要监听事件的名称，则添加监听函数
        else
        {
            (eventDic[eventName] as EventInfo).actions += action;
        }
    }
    /// <summary>
    /// 事件触发（带参数的事件）
    /// </summary>
    /// <param name="eventName">哪一个名字的事件触发</param>
    public void EventTrigger<T>(string eventName,T data)
    {
        //如果字典中有这个事件名称，则执行对应的委托函数
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions?.Invoke(data);
        }
    }
    /// <summary>
    /// 事件触发（不带参数的事件）
    /// </summary>
    /// <param name="eventName"></param>
    public void EventTrigger(string eventName)
    {
        //如果字典中有这个事件名称，则执行对应的委托函数
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions?.Invoke();
        }
    }
    /// <summary>
    /// 移除事件监听（带参数的事件）
    /// </summary>
    /// <param name="eventName">需要移除的事件监听名称</param>
    /// <param name="action">需要移除的委托函数</param>
    public void RemoveEventListener<T>(string eventName, UnityAction<T> action)
    {
        //如果字典中有这个事件名称，则移除对应的委托函数
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions -= action;
        }
    }
    /// <summary>
    /// 移除事件监听（不带参数的事件）
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string eventName, UnityAction action)
    {
        //如果字典中有这个事件名称，则移除对应的委托函数
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions -= action;
        }
    }
    /// <summary>
    /// 清空所有事件监听(主要用于场景切换时)
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
