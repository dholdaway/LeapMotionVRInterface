using System;
using System.Collections.Generic;
using System.Text;
using Leap;

//��� ����ó ���� Ŭ�������� ��� �޾ƾ��� �������̽� Ŭ�����̴�.
public interface IGesture
{
    bool SetConfig();
    void CheckGesture();
    void UnCheck();
    int AnyHand();
    
    Controller _leap_controller
    {
        get;
        set;
    }

    Frame lastFrame
    {
        get;
        set;
    }

    bool isChecked
    {
        get;
        set;
    }

    bool isRight
    {
        get;
        set;
    }

    bool isPlaying
    {
        get;
        set;
    }
}

