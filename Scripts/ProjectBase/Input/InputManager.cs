using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入控制的管理器（单例）
/// </summary>
public class InputManager : Singleton<InputManager>
{
    private bool isStartInputCheck = false;
    //在构造函数中添加公共Mono的Update监听=start中
    public InputManager()
    {
        MonoManager.Instance.AddUpdateListener(MyUpdate);
    }
    public void StartOrEnd_InputCheck(bool isStart)
    {
        isStartInputCheck=isStart;
    }
    private void MyUpdate()
    {
        // 如果没有开启输入检测，则直接返回  
        if (!isStartInputCheck)
            return;

        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.D);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.J);
        CheckKeyCode(KeyCode.K);
        CheckKeyCode(KeyCode.P);
        CheckKeyCode(KeyCode.Space); // 空格键  
        CheckKeyCode(KeyCode.Escape); // ESC键  
        CheckKeyCode(KeyCode.LeftShift); // 左Shift键  
        CheckKeyCode(KeyCode.RightShift); // 右Shift键  
    }
    /// <summary>
    /// 用来检测按键按下抬起 分发事件的内部方法
    /// </summary>
    /// <param name="keyCode"></param>
    private void CheckKeyCode(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            EventCenter.Instance.EventTrigger("某键按下",keyCode);
        }
        if (Input.GetKeyUp(keyCode))
        {
            EventCenter.Instance.EventTrigger("某键抬起", keyCode);
        }
    }
}
