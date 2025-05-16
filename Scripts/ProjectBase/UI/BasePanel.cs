using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 面板基类（方便在子类快速找到自己面板下所有的控件对象，无需拖拽等操作）
/// 提供隐藏或者显示自己的方法（允许重写）
/// </summary>
public class BasePanel : MonoBehaviour
{
    //通过里氏转换原则 来存储所有类型控件（按钮、单选框、下拉列表等）
    //由于一个UI组件可能携带好几个脚本，所以用List存储
    private Dictionary<string,List<UIBehaviour>> UIDic=new Dictionary<string,List<UIBehaviour>>();

    protected virtual void Awake()
    {
        FindChildrenCompoent<Image>();
        FindChildrenCompoent<Text>();
        FindChildrenCompoent<RawImage>();
        FindChildrenCompoent<Button>();
        FindChildrenCompoent<Toggle>();
        FindChildrenCompoent<InputField>();
        FindChildrenCompoent<Slider>();
        FindChildrenCompoent<Scrollbar>();
        FindChildrenCompoent<ScrollRect>();
        FindChildrenCompoent<Dropdown>();
    }
    /// <summary>
    /// 显示面板
    /// </summary>
    public virtual void ShowPanel()
    {
    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    public virtual void HidePanel()
    {
    }
    /// <summary>
    /// 找到子对象及孙子对象的对应控件脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenCompoent<T>()where T:UIBehaviour
    {
        T[] compoents = this.GetComponentsInChildren<T>();
        for (int i = 0;i < compoents.Length; i++)
        {
            string UIName = compoents[i].gameObject.name;
            //如果字典已存在该UI名，则把控件脚本加于其List中
            if (UIDic.ContainsKey(UIName))
            {
                UIDic[UIName].Add(compoents[i]);
            }
            else
            {
                //若不存在，则在字典添加
                UIDic.Add(compoents[i].gameObject.name, new List<UIBehaviour> { compoents[i] });
            }

            #region UI事件监听
            if (compoents[i] is Button)
            {
                //给按钮添加点击事件
                (compoents[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(UIName);
                });
            }
            else if (compoents[i] is Toggle)
            {
                //给单选框添加事件
                (compoents[i] as Toggle).onValueChanged.AddListener((isOn) =>
                {
                    OnValueChanged_Toggle(UIName, isOn);
                });
            }
            else if (compoents[i] is InputField)
            {
                //给输入框添加事件
                (compoents[i] as InputField).onValueChanged.AddListener((inputText) =>
                {
                    OnValueChanged_InputField(UIName, inputText);
                });
                (compoents[i] as InputField).onEndEdit.AddListener((inputText) =>
                {
                    OnEndEdit(UIName, inputText);
                });
                (compoents[i] as InputField).onSubmit.AddListener((inputText) =>
                {
                    OnSubmit(UIName, inputText);
                });
            }
            else if (compoents[i] is Slider)
            {
                //给滑动条添加事件
                (compoents[i] as Slider).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged_Slider(UIName, value);
                });
            }
            else if (compoents[i] is Scrollbar)
            {
                //给滚动条添加事件
                (compoents[i] as Scrollbar).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged_Scrollbar(UIName, value);
                });
            }
            else if(compoents[i] is ScrollRect)
            {
                (compoents[i] as ScrollRect).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged_ScrollRect(UIName, value);
                });
            }
            else if (compoents[i] is Dropdown)
            {
                //给下拉列表添加事件
                (compoents[i] as Dropdown).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged_Dropdown(UIName, value);
                });
            }
            #endregion
        }
    }

    /// <summary>
    /// 得到名字对应的指定控件脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="UIName"></param>
    /// <returns></returns>
    public T GetUI<T>(string UIName) where T:UIBehaviour
    {
        if (UIDic.ContainsKey(UIName))
        {
            //for循环遍历List表
            for (int i=0; i < UIDic[UIName].Count; i++)
            {
                if (UIDic[UIName][i] is T)
                {
                    return UIDic[UIName][i] as T;
                }
            }
        }
        //没有就返回空
        return null;
    }

    #region 事件监听虚方法（用于子类重写逻辑，根据Switch判断不同UI执行不同逻辑）
    //按钮点击事件
    protected virtual void OnClick(string btnName)
    {
    }

    //单选框勾选事件
    protected virtual void OnValueChanged_Toggle(string togName,bool isOn)
    {
    }

    #region 输入框输入事件
    protected virtual void OnValueChanged_InputField(string inputName, string inputText)
    {

    }
    protected virtual void OnEndEdit(string inputName, string inputText)
    {
    }
    protected virtual void OnSubmit(string inputName, string inputText)
    {
    }
    #endregion

    //Slider滑动条事件
    protected virtual void OnValueChanged_Slider(string sliderName, float value)
    {
    }

    //Scrollbar滚动条事件
    protected virtual void OnValueChanged_Scrollbar(string scrollBarName, float value)
    {
    }

    //滚动视图事件
    protected virtual void OnValueChanged_ScrollRect(string scrollRectName, Vector2 value)
    {
    }

    //下拉列表事件
    protected virtual void OnValueChanged_Dropdown(string dropdownName, int value)
    {
    }
    #endregion

}
