using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Canvas下的层级
/// </summary>
public enum E_UI_Layer
{
    Bottom,
    Middle,
    Top,
    System
}
/// <summary>
/// 提供给外部用来操作UI的管理器（方便在游戏各处调用UI Panel）
/// </summary>
public class UIManager : Singleton<UIManager>
{
    //存储面板的容器——字典（key：面板名称，value：面板对象，父类装子类）
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    //对外提供的Canvas对象，方便在外部使用
    public RectTransform canvas;
    //Canvas下的各个层级，用来方便管理渲染层级和设置其子对象
    //（底层、中层、顶层、系统层）
    private Transform Bottom_Layer;
    private Transform Middle_Layer;
    private Transform Top_Layer;
    private Transform System_Layer;


    public UIManager()
    {
        //在构造函数中创建Canvas（自动在资源管理器里创建了）和事件系统（监听事件）
        GameObject canvasObj= ResourcesManager.Instance.Load<GameObject>("UI/Canvas");
        canvas = canvasObj.transform as RectTransform;
        //Canvas不被移除切换场景时，可以达到任意地方管理
        GameObject.DontDestroyOnLoad(canvasObj);

        //找到各层
        Bottom_Layer = canvas.Find("Bottom_Layer");
        Middle_Layer = canvas.Find("Middle_Layer");
        Top_Layer = canvas.Find("Top_Layer");
        System_Layer = canvas.Find("System_Layer");

        //事件监听系统也不该被移除
        GameObject eventSystem = ResourcesManager.Instance.Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(eventSystem);
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelPath">面板名</param>
    /// <param name="layer">显示在哪一层</param>
    /// <param name="callback">当面板预设体创建成功后，需要执行的逻辑</param>
    public void ShowPanel<T>(string panelPath,E_UI_Layer layer=E_UI_Layer.Middle,UnityAction<T> callback=null)where T:BasePanel
    {
        //如果字典中已经存在该面板，则直接返回
        if (panelDic.ContainsKey(panelPath))
        {
            panelDic[panelPath].ShowPanel();
            //回调panel到外部使用
            if (callback != null)
            {
                callback(panelDic[panelPath] as T);
            }
            //面板已经存在，直接返回，避免重复加载
            return;
        }

        //异步加载面板，防止面板资源过大
        ResourcesManager.Instance.LoadAsync<GameObject>(panelPath, (panelObj) =>
        {
            //得到Panel的父对象层级
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
            //设置Panel
            panelObj.transform.SetParent(fatherTransform, false);
            panelObj.transform.localPosition = Vector3.zero;
            panelObj.transform.localScale = Vector3.one;
            (panelObj.transform as RectTransform).offsetMax = Vector3.zero;
            (panelObj.transform as RectTransform).offsetMin = Vector3.zero;

            //得到预设体身上的面板脚本
            T panel = panelObj.GetComponent<T>();
            panel.ShowPanel();

            //回调panel到外部使用
            if (callback != null)
            {
                callback(panel);
            }
            //把面板添加到字典中
            panelDic.Add(panelPath, panel);
        });
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelPath"></param>
    public void HidePanel(string panelPath)
    {
        if(panelDic.ContainsKey(panelPath))
        {
            panelDic[panelPath].HidePanel();
            //销毁面板
            GameObject.Destroy(panelDic[panelPath].gameObject);
            //从字典中移除
            panelDic.Remove(panelPath);
        }
    }

    /// <summary>
    /// 得到某一个已经显示的面板，方便外部调用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetPanel<T>(string panelPath) where T : BasePanel
    {
        //判断字典中是否存在该面板
        if (panelDic.ContainsKey(panelPath))
        {
            //如果存在该面板，则返回该面板
            return panelDic[panelPath] as T;
        }
        else
        {
            //如果不存在该面板，则返回null
            return null;
        }
    }

    /// <summary>
    /// 得到某一层的父对象
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
    /// 提供给外部为指定UI添加自定义的事件（拖拉等）
    /// </summary>
    /// <param name="UIName">需要添加其他事件的UI名</param>
    /// <param name="eventTriggerType">事件类型</param>
    /// <param name="action">事件响应函数</param>
    public static void AddCustomEventListener(UIBehaviour UIName,EventTriggerType eventTriggerType,UnityAction<BaseEventData> action)
    {
        //得到UIName身上的事件触发器组件
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
