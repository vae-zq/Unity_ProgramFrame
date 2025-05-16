using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Canvas�µĲ㼶
/// </summary>
public enum E_UI_Layer
{
    Bottom,
    Middle,
    Top,
    System
}
/// <summary>
/// �ṩ���ⲿ��������UI�Ĺ���������������Ϸ��������UI Panel��
/// </summary>
public class UIManager : Singleton<UIManager>
{
    //�洢�������������ֵ䣨key��������ƣ�value�������󣬸���װ���ࣩ
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    //�����ṩ��Canvas���󣬷������ⲿʹ��
    public RectTransform canvas;
    //Canvas�µĸ����㼶���������������Ⱦ�㼶���������Ӷ���
    //���ײ㡢�в㡢���㡢ϵͳ�㣩
    private Transform Bottom_Layer;
    private Transform Middle_Layer;
    private Transform Top_Layer;
    private Transform System_Layer;


    public UIManager()
    {
        //�ڹ��캯���д���Canvas���Զ�����Դ�������ﴴ���ˣ����¼�ϵͳ�������¼���
        GameObject canvasObj= ResourcesManager.Instance.Load<GameObject>("UI/Canvas");
        canvas = canvasObj.transform as RectTransform;
        //Canvas�����Ƴ��л�����ʱ�����Դﵽ����ط�����
        GameObject.DontDestroyOnLoad(canvasObj);

        //�ҵ�����
        Bottom_Layer = canvas.Find("Bottom_Layer");
        Middle_Layer = canvas.Find("Middle_Layer");
        Top_Layer = canvas.Find("Top_Layer");
        System_Layer = canvas.Find("System_Layer");

        //�¼�����ϵͳҲ���ñ��Ƴ�
        GameObject eventSystem = ResourcesManager.Instance.Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(eventSystem);
    }

    /// <summary>
    /// ��ʾ���
    /// </summary>
    /// <typeparam name="T">���ű�����</typeparam>
    /// <param name="panelPath">�����</param>
    /// <param name="layer">��ʾ����һ��</param>
    /// <param name="callback">�����Ԥ���崴���ɹ�����Ҫִ�е��߼�</param>
    public void ShowPanel<T>(string panelPath,E_UI_Layer layer=E_UI_Layer.Middle,UnityAction<T> callback=null)where T:BasePanel
    {
        //����ֵ����Ѿ����ڸ���壬��ֱ�ӷ���
        if (panelDic.ContainsKey(panelPath))
        {
            panelDic[panelPath].ShowPanel();
            //�ص�panel���ⲿʹ��
            if (callback != null)
            {
                callback(panelDic[panelPath] as T);
            }
            //����Ѿ����ڣ�ֱ�ӷ��أ������ظ�����
            return;
        }

        //�첽������壬��ֹ�����Դ����
        ResourcesManager.Instance.LoadAsync<GameObject>(panelPath, (panelObj) =>
        {
            //�õ�Panel�ĸ�����㼶
            Transform fatherTransform = Bottom_Layer;
            switch (layer)
            {
                case E_UI_Layer.Bottom:
                    fatherTransform = Bottom_Layer;
                    break;
                case E_UI_Layer.Middle:
                    fatherTransform = Middle_Layer;
                    break;
                case E_UI_Layer.Top:
                    fatherTransform = Top_Layer;
                    break;
                case E_UI_Layer.System:
                    fatherTransform = System_Layer;
                    break;
            }
            //����Panel
            panelObj.transform.SetParent(fatherTransform, false);
            panelObj.transform.localPosition = Vector3.zero;
            panelObj.transform.localScale = Vector3.one;
            (panelObj.transform as RectTransform).offsetMax = Vector3.zero;
            (panelObj.transform as RectTransform).offsetMin = Vector3.zero;

            //�õ�Ԥ�������ϵ����ű�
            T panel = panelObj.GetComponent<T>();
            panel.ShowPanel();

            //�ص�panel���ⲿʹ��
            if (callback != null)
            {
                callback(panel);
            }
            //�������ӵ��ֵ���
            panelDic.Add(panelPath, panel);
        });
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="panelPath"></param>
    public void HidePanel(string panelPath)
    {
        if(panelDic.ContainsKey(panelPath))
        {
            panelDic[panelPath].HidePanel();
            //�������
            GameObject.Destroy(panelDic[panelPath].gameObject);
            //���ֵ����Ƴ�
            panelDic.Remove(panelPath);
        }
    }

    /// <summary>
    /// �õ�ĳһ���Ѿ���ʾ����壬�����ⲿ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetPanel<T>(string panelPath) where T : BasePanel
    {
        //�ж��ֵ����Ƿ���ڸ����
        if (panelDic.ContainsKey(panelPath))
        {
            //������ڸ���壬�򷵻ظ����
            return panelDic[panelPath] as T;
        }
        else
        {
            //��������ڸ���壬�򷵻�null
            return null;
        }
    }

    /// <summary>
    /// �õ�ĳһ��ĸ�����
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bottom:
                return this.Bottom_Layer;
            case E_UI_Layer.Middle:
                return this.Middle_Layer;
            case E_UI_Layer.Top:
                return this.Top_Layer;
            case E_UI_Layer.System:
                return this.System_Layer;
            default:
                return null;
        }
    }

    /// <summary>
    /// �ṩ���ⲿΪָ��UI����Զ�����¼��������ȣ�
    /// </summary>
    /// <param name="UIName">��Ҫ��������¼���UI��</param>
    /// <param name="eventTriggerType">�¼�����</param>
    /// <param name="action">�¼���Ӧ����</param>
    public static void AddCustomEventListener(UIBehaviour UIName,EventTriggerType eventTriggerType,UnityAction<BaseEventData> action)
    {
        //�õ�UIName���ϵ��¼����������
        EventTrigger eventTrigger = UIName.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = UIName.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventTriggerType;
        entry.callback.AddListener(action);
        eventTrigger.triggers.Add(entry);
    }
}
