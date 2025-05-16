using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// �����л�������
/// </summary>
public class SceneMgr : Singleton<SceneMgr>
{
    /// <summary>
    /// �ṩ���ⲿͬ�����س����ķ���
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="action"></param>
    public void LoadScene(string sceneName,UnityAction action)
    {
        SceneManager.LoadScene(sceneName);
        //����������ɺ�ִ�лص�
        action?.Invoke();
    }

    /// <summary>
    /// �ṩ���ⲿ�첽���س����ķ���
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="action"></param>
    public void LoadSceneAsync(string sceneName, UnityAction action)
    {
        //��Ϊû�м̳�MonoBehaviour�����Բ���ֱ��ʹ��StartCoroutine��ͨ��MonoManager������
        MonoManager.Instance.StartCoroutine(ReallyLoadSceneAsync(sceneName, action));

    }
    /// <summary>
    /// �ڲ�Э���첽���س���
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsync(string sceneName, UnityAction action)
    {
        AsyncOperation async= SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            //�¼��������ⲿ�����¼������ݽ�������ֵ���ⲿ���þ���
            EventCenter.Instance.EventTrigger("����������", async.progress);

            yield return async.progress;
        }
        //�첽���س���ʱ������������ɺ���Զ��л����ó��������Ҽ�����ɻ�Ż�ִ�к���Ĵ���
        yield return async;
        //����������ɺ�ִ�лص�
        action?.Invoke();
    }
}
