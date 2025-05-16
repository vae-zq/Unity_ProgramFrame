using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��Դ���صĹ�������������
/// </summary>
public class ResourcesManager:Singleton<ResourcesManager>
{
    /// <summary>
    /// �ṩ���ⲿ��Դͬ�����صķ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T Load<T>(string path) where T : Object
    {
        T res = Resources.Load<T>(path);

        //���������GameObject���ͣ����԰���ʵ�������ٷ��س�ȥ��
        //�ⲿֻ��Ҫֱ��ʹ�ü��ɣ�������
        if (res is GameObject)
        {
            return GameObject.Instantiate(res);
        }
        else
        {
            //������������͵Ķ�����Ч��ͼƬ���ı��ȣ�����ֱ�ӷ���
            return res;
        }
    }

    /// <summary>
    /// �ṩ���ⲿ��Դ�첽���صķ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        //��Ϊû�м̳�MonoBehaviour�����Բ���ֱ��ʹ��StartCoroutine��ͨ��MonoManager������
        MonoManager.Instance.StartCoroutine(ReallyLoadAsync<T>(path, callback));
    }
    /// <summary>
    /// �ڲ�Э���첽������Դ ���ڿ����첽���ض�Ӧ����Դ
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        yield return request;

        if(request.asset is GameObject)
        {
            //���������GameObject���ͣ����԰���ʵ�������ٷ��س�ȥ��
            //�ⲿֻ��Ҫֱ��ʹ�ü��ɣ�������
            callback(GameObject.Instantiate((request.asset) as T));
        }
        else
        {
            //������������͵Ķ�����Ч��ͼƬ���ı��ȣ�����ֱ�ӷ���
            callback(request.asset as T);
        }
    }
}
