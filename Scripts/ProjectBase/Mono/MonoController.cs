using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///���й���֡�����¼�
/// </summary>
public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        //������¼���Ϊ�գ������¼�
        if (updateEvent != null)
        {
            updateEvent.Invoke();
        }
    }

    /// <summary>
    /// ���ⲿ�ṩ�����֡�����¼��ķ���
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }
    /// <summary>
    /// ���ⲿ�ṩ���Ƴ�֡�����¼��ķ���
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action)
    {
        updateEvent -= action;
    }
}
