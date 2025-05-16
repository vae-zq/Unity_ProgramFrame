using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换管理器
/// </summary>
public class SceneMgr : Singleton<SceneMgr>
{
    /// <summary>
    /// 提供给外部同步加载场景的方法
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="action"></param>
    public void LoadScene(string sceneName,UnityAction action)
    {
        SceneManager.LoadScene(sceneName);
        //场景加载完成后执行回调
        action?.Invoke();
    }

    /// <summary>
    /// 提供给外部异步加载场景的方法
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="action"></param>
    public void LoadSceneAsync(string sceneName, UnityAction action)
    {
        //因为没有继承MonoBehaviour，所以不能直接使用StartCoroutine，通过MonoManager来调用
        MonoManager.Instance.StartCoroutine(ReallyLoadSceneAsync(sceneName, action));

    }
    /// <summary>
    /// 内部协程异步加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsync(string sceneName, UnityAction action)
    {
        AsyncOperation async= SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            //事件中心向外部发送事件，传递进度条的值，外部想用就用
            EventCenter.Instance.EventTrigger("进度条更新", async.progress);

            yield return async.progress;
        }
        //异步加载场景时，场景加载完成后会自动切换到该场景，并且加载完成会才会执行后面的代码
        yield return async;
        //场景加载完成后执行回调
        action?.Invoke();
    }
}
