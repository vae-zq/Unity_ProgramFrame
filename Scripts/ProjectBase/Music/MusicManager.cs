using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ������Ч������
/// </summary>
public class MusicManager : Singleton<MusicManager>
{
    private AudioSource bkMusic = null;//���������������Ϊ��������ֻ��һ�������Թ���һ��AudioSource�Ϳ����ˣ�
    private float bkVolume = 1f; //������������

    private GameObject soundObj = null;//��Ч������Ϊ��Ч�ж����������Ҫ����һ�������������ض��AudioSource��
    private List<AudioSource> soundList = new List<AudioSource>(); //��Ч�б�
    private float soundVolume = 1f; //��Ч����
    private bool isSoundOpen = true;//��Ч����

    public MusicManager()
    {
        //�ڹ��캯������ӹ���Mono��Update����=��Start�У�ʵ�ֲ��̳�MonoBehaviour��Update������
        MonoManager.Instance.AddUpdateListener(MyUpdate);
    }

    #region �����������
    /// <summary>
    /// ���ű�������
    /// </summary>
    /// <param name="musicName">���������ļ���</param>
    public void PlayBKMusic(string musicPathName)
    {
        //��һ�β��ű�������ʱ�϶�Ϊ����ô�ʹ���AudioSource
        if (bkMusic == null)
        {
            GameObject obj = new GameObject("BKMusic");
            bkMusic = obj.AddComponent<AudioSource>();
        }

        //����Ч�������첽���ر������֣����ڼ�����ɺ�ص���Դ������
        ResourcesManager.Instance.LoadAsync<AudioClip>(musicPathName, (clip) =>
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;//�������ֿ϶�ѭ������
            bkMusic.volume = bkVolume;
            bkMusic.Play();
        });
    }

    /// <summary>
    /// �ı䱳����������
    /// </summary>
    /// <param name="value"></param>
    public void ChangeBKVolume(float value)
    {
        //�޶�������Χ��0-1֮��
        value = Mathf.Clamp(value, 0, 1);
        bkVolume = value;

        //���û�в��ű������֣���ֱ�ӷ���
        if (bkMusic == null)
            return;
        //������ű������֣���ı�����
        bkMusic.volume = bkVolume;
    }

    /// <summary>
    /// ��ͣ���ű�������
    /// </summary>
    public void PauseBKMusic()
    {
        //���û�в��ű������֣���ֱ�ӷ���
        if (bkMusic == null)
            return;
        //������ű������֣�����ͣ����
        bkMusic.Pause();
    }
    /// <summary>
    /// �ָ����ű�������
    /// </summary>
    public void UnPauseBKMusic()
    {
        //���û�в��ű������֣���ֱ�ӷ���
        if (bkMusic == null)
            return;
        //������ű������֣����������
        bkMusic.UnPause();
    }
    /// <summary>
    /// �رղ��ű�������
    /// </summary>
    public void StopBKMusic()
    {
        //���û�в��ű������֣���ֱ�ӷ���
        if (bkMusic == null)
            return;
        //������ű������֣���ֹͣ����
        bkMusic.Stop();
    }
    #endregion


    #region ��Ч���
    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="soundName">��Ч�ļ���</param>
    public void PlaySound(string soundPathName, bool isLoop = false, UnityAction<AudioSource> callback = null)
    {
        //��һ�β�����Чʱ�Զ�����һ����������������Ч���
        if (soundObj == null)
        {
            soundObj = new GameObject("Sound");
            GameObject.DontDestroyOnLoad(soundObj);//��Ч���󲻱�����(�δ�����ʹ��)
        }

        //����Ч�������첽������Ч�����ڼ�����ɺ�ص���Դ������
        AudioClip clip = ResourcesManager.Instance.Load<AudioClip>(soundPathName);
        //����������Ч��Դ���ٴ�����Ч���ʹ��
        AudioSource sound = soundObj.AddComponent<AudioSource>();
        sound.clip = clip;
        sound.loop = isLoop;//�Ƿ�ѭ��������Ч�����ܲ�����Ч��Ҫѭ�����ţ�
        sound.volume = soundVolume;
        sound.mute = !isSoundOpen; // ����ȫ��״̬���þ��������ں����´�����Чͬ����Ч���أ�
        sound.Play();
        soundList.Add(sound);//���������Ч�����ӵ��б���

        if (callback != null)
            callback(sound);//�ص���Ч������ⲿ��Ҫ�õ������������ֹͣ���ŵȣ�
    }

    /// <summary>
    /// �ı���Ч����
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundVolume(float value)
    {
        //�޶�������Χ��0-1֮��
        value = Mathf.Clamp(value, 0, 1);
        soundVolume = value;

        //�ı���Ч�Ļ���Ҫ�ı�������Ч������
        foreach (AudioSource sound in soundList)
        {
            sound.volume = soundVolume;
        }
    }
    /// <summary>
    /// �򿪻�ر���Ч
    /// </summary>
    /// <param name="isOpen"></param>
    public void OpenOrCloseSound(bool isOpen)
    {
        isSoundOpen = isOpen;
        //�ı���Ч�Ļ���Ҫ�ı�������Ч������
        foreach (AudioSource sound in soundList)
        {
            sound.mute = !isOpen;
        }
    }
    /// <summary>
    /// ֹͣ����ĳ����Ч
    /// </summary>
    /// <param name="soundName"></param>
    public void StopSound(AudioSource sound)
    {
        if (soundList.Contains(sound))
        {
            soundList.Remove(sound);//���б����Ƴ�����Ч���
            sound.Stop();//ֹͣ������Ч
            GameObject.Destroy(sound);//ֹͣ������Ч��������Ч���
        }
    }
    private void MyUpdate()
    {
        //�����Ч�б�Ϊ�գ��������Ч�б�
        if (soundList.Count > 0)
        {
            //��Ϊ��Ч�б���List��̬�仯�ģ��Ƴ�һ��Ԫ��ǰ�ƣ���������Ҫ�Ӻ���ǰ����
            for (int i = soundList.Count - 1; i >= 0; i--)
            {
                //�����Ч������ϣ�����б����Ƴ������ٸ���Ч���
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
