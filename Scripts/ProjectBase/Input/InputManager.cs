using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ƵĹ�������������
/// </summary>
public class InputManager : Singleton<InputManager>
{
    private bool isStartInputCheck = false;
    //�ڹ��캯������ӹ���Mono��Update����=start��
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
        // ���û�п��������⣬��ֱ�ӷ���  
        if (!isStartInputCheck)
            return;

        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.D);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.J);
        CheckKeyCode(KeyCode.K);
        CheckKeyCode(KeyCode.P);
        CheckKeyCode(KeyCode.Space); // �ո��  
        CheckKeyCode(KeyCode.Escape); // ESC��  
        CheckKeyCode(KeyCode.LeftShift); // ��Shift��  
        CheckKeyCode(KeyCode.RightShift); // ��Shift��  
    }
    /// <summary>
    /// ������ⰴ������̧�� �ַ��¼����ڲ�����
    /// </summary>
    /// <param name="keyCode"></param>
    private void CheckKeyCode(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            EventCenter.Instance.EventTrigger("ĳ������",keyCode);
        }
        if (Input.GetKeyUp(keyCode))
        {
            EventCenter.Instance.EventTrigger("ĳ��̧��", keyCode);
        }
    }
}
