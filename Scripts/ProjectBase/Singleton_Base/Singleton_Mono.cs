using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton_Mono<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T instance;
    //�̳�MonoBehaviour�ĵ����಻��ʹ��new()����ʵ��
    //ֻ��ͨ���϶��������� ����ͨ��AddComponentȥ�ӽű�����ʵ��
    //������Ҫ��֤����ʵ��ֻ����һ��
    public static T Instance=>instance;

    //��ֹ������Ҫ��дAwakeʱ�������Awake�����ǵ����޷���ȡʵ��������д���麯��
    protected virtual void Awake()
    {
        //������������ֻ�ܱ�����һ�Σ������ض�Σ�ֻ��������һ����Awake��GameObject��
        instance= this as T;
    }
}
