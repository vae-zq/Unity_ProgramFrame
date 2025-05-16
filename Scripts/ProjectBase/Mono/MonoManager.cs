using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ͨ������ MonoManager ������MonoController������ʵ��ȫ��Ψһ�ԡ�������з�װ
/// �ṩ���ⲿ���Э�̵ķ���
/// �ṩ���ⲿ��Ӻ��Ƴ�֡�����¼��ķ���
/// </summary>
public class MonoManager : Singleton<MonoManager>
{
    private MonoController monoController;

    //�̳��Ե������࣬����Ϊnull�����Զ�new T()�����Ի���ù��캯��
    public MonoManager()
    {
        //ֻ��newһ�Σ���֤��MonoController������Ψһ��
        GameObject obj = new GameObject("MonoController");
        monoController = obj.AddComponent<MonoController>();
    }

    /// <summary>
    /// ���ⲿ�ṩ�����֡�����¼��ķ���
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        monoController.AddUpdateListener(action);
    }
    /// <summary>
    /// ���ⲿ�ṩ���Ƴ�֡�����¼��ķ���
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action)
    {
        monoController.RemoveUpdateListener(action);
    }

    /// <summary>
    /// ���ⲿ�ṩ�����Э�̵ķ�����ͨ�� Coroutine ����
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return monoController.StartCoroutine(routine);
    }
    /// <summary>
    /// ���ⲿ�ṩ�����Э�̵ķ�����ͨ����������
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return monoController.StartCoroutine(methodName, value);
    }
    /// <summary>
    /// ���ⲿ�ṩ�����Э�̵ķ�����ͨ����������
    /// </summary>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName)
    {
        return monoController.StartCoroutine(methodName);
    }

    /// <summary>
    /// ֹͣЭ�̵ķ�����ͨ�� Coroutine ����
    /// </summary>
    /// <param name="coroutine">Ҫֹͣ��Э�̶���</param>
    public void StopCoroutine(Coroutine coroutine)
    {
        monoController.StopCoroutine(coroutine);
    }

    /// <summary>
    /// ֹͣЭ�̵ķ�����ͨ����������
    /// </summary>
    /// <param name="methodName">Ҫֹͣ��Э�̷�����</param>
    public void StopCoroutine(string methodName)
    {
        monoController.StopCoroutine(methodName);
    }

    /// <summary>
    /// ֹͣ����Э�̵ķ���
    /// </summary>
    public void StopAllCoroutines()
    {
        monoController.StopAllCoroutines();
    }

}
