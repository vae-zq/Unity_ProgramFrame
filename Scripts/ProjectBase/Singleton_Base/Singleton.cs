using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
/// <typeparam name="T">����Լ��Ϊ�����޲ι������캯���������޷�new()���������������޲ι��죩</typeparam>
public class Singleton<T> where T : new()
{
    private static T instance;
    //���ʵ��Ϊ�գ��򴴽�һ���µ�ʵ��
    public static T Instance => instance ?? (instance = new T());
}
