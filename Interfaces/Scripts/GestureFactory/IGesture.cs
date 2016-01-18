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
    bool AnyHand();
    bool WhichSide(Hand hand);
    void OnVR();

    Controller _leap_controller
    {
        get;
        set;
    }

    HandList Hands
    {
        get;
        set;
    }

    Frame _lastFrame
    {
        get;
        set;
    }

    int _userSide
    {
        get;
        set;
    }

    bool _isChecked
    {
        get;
        set;
    }

    bool _isRight
    {
        get;
        set;
    }

    bool _isVR
    {
        get;
        set;
    }

    bool _isPlaying
    {
        get;
        set;
    }
}

