using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̳�MonoBehaviour�ĵ����ࣨ������Ҫ�ֶ����ػ���ӵ������ϣ�
/// ��������ֱ�ӻ�ȡInstance������
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonAuto_Mono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    //�̳�MonoBehaviour�ĵ����಻��ʹ��new()����ʵ��
    //ֻ��ͨ���϶��������� ����ͨ��AddComponentȥ�ӽű�����ʵ��
    //������Ҫ��֤����ʵ��ֻ����һ��
    public static T Instance
    {
        get 
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                //���ö��������Ϊ�ű�������
                obj.name = typeof(T).Name;

                //�øõ���ģʽ��������������Ƴ�����Ϊ����ģʽ����һ���Ǵ���������Ϸ�������ڵ�
                GameObject.DontDestroyOnLoad(obj);

                //��ӵ������Ϻ󷵻ظ�ʵ��
                instance = obj.AddComponent<T>();
            }
            return instance; 
        }
    }
}