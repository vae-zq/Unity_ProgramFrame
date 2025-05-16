using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//�¼�������Ϣ�Ľӿڣ���Ҫ��������װ���ࣩ
public interface IEventInfo { }
//�����¼���Ϣ���࣬��Ҫ����ͨ�����ͱ���ʹ��object����װ�����
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        //Ϊ�˷����ڹ��캯������Ӵ����ί�к���
        actions += action;
    }
}
//����Ҫ���ݲ������¼���Ϣ��
public class EventInfo : IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        //Ϊ�˷����ڹ��캯������Ӵ����ί�к���
        actions += action;
    }
}

/// <summary>
/// �¼�����ģ�飬����ģʽ���󣨻��ڹ۲��ߵ����ģʽ��
/// </summary>
public class EventCenter: Singleton<EventCenter>
{
    //���ڴ洢��Ҫ�����ĸ����¼����ֵ�
    //stringΪ�¼����ƣ�ͨ�ء����������ȣ� UnityAction<object>Ϊ�¼�ί�У���������¼� ��Ӧ��ί�к����ǣ�
    //IEventInfoΪ����װ��������β�UnityAction<T>�������������ʱ�����ݹ�������֣����߾���ֵ�ȣ����޲�UnityAction
    private Dictionary<string,IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// ����¼���������Ҫ���ݲ������¼���
    /// </summary>
    /// <param name="eventName">�¼�������</param>
    /// <param name="action">׼�����������¼���ί�к���</param>
    public void AddEventListener<T>(string eventName, UnityAction<T> action)
    {
        //����ֵ���û�������Ҫ�����¼����ƣ������
        if (!eventDic.ContainsKey(eventName))
        {
            eventDic.Add(eventName, new EventInfo<T>(action));
        }
        //����ֵ����������Ҫ�����¼������ƣ�����Ӽ�������
        else
        {
            (eventDic[eventName] as EventInfo<T>).actions += action;
        }
    }
    /// <summary>
    /// ����¼�����������Ҫ���ݲ������¼���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="action"></param>
    public void AddEventListener<T>(string eventName, UnityAction action)
    {
        //����ֵ���û�������Ҫ�����¼����ƣ������
        if (!eventDic.ContainsKey(eventName))
        {
            eventDic.Add(eventName, new EventInfo(action));
        }
        //����ֵ����������Ҫ�����¼������ƣ�����Ӽ�������
        else
        {
            (eventDic[eventName] as EventInfo).actions += action;
        }
    }
    /// <summary>
    /// �¼����������������¼���
    /// </summary>
    /// <param name="eventName">��һ�����ֵ��¼�����</param>
    public void EventTrigger<T>(string eventName,T data)
    {
        //����ֵ���������¼����ƣ���ִ�ж�Ӧ��ί�к���
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions?.Invoke(data);
        }
    }
    /// <summary>
    /// �¼������������������¼���
    /// </summary>
    /// <param name="eventName"></param>
    public void EventTrigger(string eventName)
    {
        //����ֵ���������¼����ƣ���ִ�ж�Ӧ��ί�к���
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions?.Invoke();
        }
    }
    /// <summary>
    /// �Ƴ��¼����������������¼���
    /// </summary>
    /// <param name="eventName">��Ҫ�Ƴ����¼���������</param>
    /// <param name="action">��Ҫ�Ƴ���ί�к���</param>
    public void RemoveEventListener<T>(string eventName, UnityAction<T> action)
    {
        //����ֵ���������¼����ƣ����Ƴ���Ӧ��ί�к���
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions -= action;
        }
    }
    /// <summary>
    /// �Ƴ��¼������������������¼���
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string eventName, UnityAction action)
    {
        //����ֵ���������¼����ƣ����Ƴ���Ӧ��ί�к���
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions -= action;
        }
    }
    /// <summary>
    /// ��������¼�����(��Ҫ���ڳ����л�ʱ)
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
