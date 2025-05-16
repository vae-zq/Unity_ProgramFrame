using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// �����ࣨ��������������ҵ��Լ���������еĿؼ�����������ק�Ȳ�����
/// �ṩ���ػ�����ʾ�Լ��ķ�����������д��
/// </summary>
public class BasePanel : MonoBehaviour
{
    //ͨ������ת��ԭ�� ���洢�������Ϳؼ�����ť����ѡ�������б�ȣ�
    //����һ��UI�������Я���ü����ű���������List�洢
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
    /// ��ʾ���
    /// </summary>
    public virtual void ShowPanel()
    {
    }
    /// <summary>
    /// �������
    /// </summary>
    public virtual void HidePanel()
    {
    }
    /// <summary>
    /// �ҵ��Ӷ������Ӷ���Ķ�Ӧ�ؼ��ű�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenCompoent<T>()where T:UIBehaviour
    {
        T[] compoents = this.GetComponentsInChildren<T>();
        for (int i = 0;i < compoents.Length; i++)
        {
            string UIName = compoents[i].gameObject.name;
            //����ֵ��Ѵ��ڸ�UI������ѿؼ��ű�������List��
            if (UIDic.ContainsKey(UIName))
            {
                UIDic[UIName].Add(compoents[i]);
            }
            else
            {
                //�������ڣ������ֵ����
                UIDic.Add(compoents[i].gameObject.name, new List<UIBehaviour> { compoents[i] });
            }

            #region UI�¼�����
            if (compoents[i] is Button)
            {
                //����ť��ӵ���¼�
                (compoents[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(UIName);
                });
            }
            else if (compoents[i] is Toggle)
            {
                //����ѡ������¼�
                (compoents[i] as Toggle).onValueChanged.AddListener((isOn) =>
                {
                    OnValueChanged_Toggle(UIName, isOn);
                });
            }
            else if (compoents[i] is InputField)
            {
                //�����������¼�
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
                //������������¼�
                (compoents[i] as Slider).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged_Slider(UIName, value);
                });
            }
            else if (compoents[i] is Scrollbar)
            {
                //������������¼�
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
                //�������б�����¼�
                (compoents[i] as Dropdown).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged_Dropdown(UIName, value);
                });
            }
            #endregion
        }
    }

    /// <summary>
    /// �õ����ֶ�Ӧ��ָ���ؼ��ű�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="UIName"></param>
    /// <returns></returns>
    public T GetUI<T>(string UIName) where T:UIBehaviour
    {
        if (UIDic.ContainsKey(UIName))
        {
            //forѭ������List��
            for (int i=0; i < UIDic[UIName].Count; i++)
            {
                if (UIDic[UIName][i] is T)
                {
                    return UIDic[UIName][i] as T;
                }
            }
        }
        //û�оͷ��ؿ�
        return null;
    }

    #region �¼������鷽��������������д�߼�������Switch�жϲ�ͬUIִ�в�ͬ�߼���
    //��ť����¼�
    protected virtual void OnClick(string btnName)
    {
    }

    //��ѡ��ѡ�¼�
    protected virtual void OnValueChanged_Toggle(string togName,bool isOn)
    {
    }

    #region ����������¼�
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

    //Slider�������¼�
    protected virtual void OnValueChanged_Slider(string sliderName, float value)
    {
    }

    //Scrollbar�������¼�
    protected virtual void OnValueChanged_Scrollbar(string scrollBarName, float value)
    {
    }

    //������ͼ�¼�
    protected virtual void OnValueChanged_ScrollRect(string scrollRectName, Vector2 value)
    {
    }

    //�����б��¼�
    protected virtual void OnValueChanged_Dropdown(string dropdownName, int value)
    {
    }
    #endregion

}
