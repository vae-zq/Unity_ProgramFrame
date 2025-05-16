using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 音乐音效管理器
/// </summary>
public class MusicManager : Singleton<MusicManager>
{
    private AudioSource bkMusic = null;//背景音乐组件（因为背景音乐只有一个，所以挂载一个AudioSource就可以了）
    private float bkVolume = 1f; //背景音乐音量

    private GameObject soundObj = null;//音效对象（因为音效有多个，所以需要创建一个空物体来挂载多个AudioSource）
    private List<AudioSource> soundList = new List<AudioSource>(); //音效列表
    private float soundVolume = 1f; //音效音量
    private bool isSoundOpen = true;//音效开关

    public MusicManager()
    {
        //在构造函数中添加公共Mono的Update监听=在Start中（实现不继承MonoBehaviour的Update方法）
        MonoManager.Instance.AddUpdateListener(MyUpdate);
    }

    #region 背景音乐相关
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="musicName">背景音乐文件名</param>
    public void PlayBKMusic(string musicPathName)
    {
        //第一次播放背景音乐时肯定为空那么就创建AudioSource
        if (bkMusic == null)
        {
            GameObject obj = new GameObject("BKMusic");
            bkMusic = obj.AddComponent<AudioSource>();
        }

        //用音效管理器异步加载背景音乐，会在加载完成后回调资源并播放
        ResourcesManager.Instance.LoadAsync<AudioClip>(musicPathName, (clip) =>
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;//背景音乐肯定循环播放
            bkMusic.volume = bkVolume;
            bkMusic.Play();
        });
    }

    /// <summary>
    /// 改变背景音乐音量
    /// </summary>
    /// <param name="value"></param>
    public void ChangeBKVolume(float value)
    {
        //限定音量范围在0-1之间
        value = Mathf.Clamp(value, 0, 1);
        bkVolume = value;

        //如果没有播放背景音乐，则直接返回
        if (bkMusic == null)
            return;
        //如果播放背景音乐，则改变音量
        bkMusic.volume = bkVolume;
    }

    /// <summary>
    /// 暂停播放背景音乐
    /// </summary>
    public void PauseBKMusic()
    {
        //如果没有播放背景音乐，则直接返回
        if (bkMusic == null)
            return;
        //如果播放背景音乐，则暂停播放
        bkMusic.Pause();
    }
    /// <summary>
    /// 恢复播放背景音乐
    /// </summary>
    public void UnPauseBKMusic()
    {
        //如果没有播放背景音乐，则直接返回
        if (bkMusic == null)
            return;
        //如果播放背景音乐，则继续播放
        bkMusic.UnPause();
    }
    /// <summary>
    /// 关闭播放背景音乐
    /// </summary>
    public void StopBKMusic()
    {
        //如果没有播放背景音乐，则直接返回
        if (bkMusic == null)
            return;
        //如果播放背景音乐，则停止播放
        bkMusic.Stop();
    }
    #endregion


    #region 音效相关
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundName">音效文件名</param>
    public void PlaySound(string soundPathName, bool isLoop = false, UnityAction<AudioSource> callback = null)
    {
        //第一次播放音效时自动创建一个空物体来挂载音效组件
        if (soundObj == null)
        {
            soundObj = new GameObject("Sound");
            GameObject.DontDestroyOnLoad(soundObj);//音效对象不被销毁(何处都会使用)
        }

        //用音效管理器异步加载音效，会在加载完成后回调资源并播放
        AudioClip clip = ResourcesManager.Instance.Load<AudioClip>(soundPathName);
        //当加载完音效资源后，再创建音效组件使用
        AudioSource sound = soundObj.AddComponent<AudioSource>();
        sound.clip = clip;
        sound.loop = isLoop;//是否循环播放音效（可能部分音效需要循环播放）
        sound.volume = soundVolume;
        sound.mute = !isSoundOpen; // 根据全局状态设置静音（用于后面新创建音效同步音效开关）
        sound.Play();
        soundList.Add(sound);//播放完后将音效组件添加到列表中

        if (callback != null)
            callback(sound);//回调音效组件若外部想要得到该组件（比如停止播放等）
    }

    /// <summary>
    /// 改变音效音量
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundVolume(float value)
    {
        //限定音量范围在0-1之间
        value = Mathf.Clamp(value, 0, 1);
        soundVolume = value;

        //改变音效的话需要改变所有音效的音量
        foreach (AudioSource sound in soundList)
        {
            sound.volume = soundVolume;
        }
    }
    /// <summary>
    /// 打开或关闭音效
    /// </summary>
    /// <param name="isOpen"></param>
    public void OpenOrCloseSound(bool isOpen)
    {
        isSoundOpen = isOpen;
        //改变音效的话需要改变所有音效的音量
        foreach (AudioSource sound in soundList)
        {
            sound.mute = !isOpen;
        }
    }
    /// <summary>
    /// 停止播放某个音效
    /// </summary>
    /// <param name="soundName"></param>
    public void StopSound(AudioSource sound)
    {
        if (soundList.Contains(sound))
        {
            soundList.Remove(sound);//从列表中移除该音效组件
            sound.Stop();//停止播放音效
            GameObject.Destroy(sound);//停止播放音效后销毁音效组件
        }
    }
    private void MyUpdate()
    {
        //如果音效列表不为空，则遍历音效列表
        if (soundList.Count > 0)
        {
            //因为音效列表是List动态变化的（移除一个元素前移），所以需要从后往前遍历
            for (int i = soundList.Count - 1; i >= 0; i--)
            {
                //如果音效播放完毕，则从列表中移除并销毁该音效组件
                if (!soundList[i].isPlaying)
                {
                    GameObject.Destroy(soundList[i]);
                    soundList.RemoveAt(i);
                }
            }
        }
    }
    #endregion
}
