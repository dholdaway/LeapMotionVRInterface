using System;
using System.Collections.Generic;
using System.Text;
using Leap;

//��� ����ó ���� Ŭ�������� ��� �޾ƾ��� �������̽� Ŭ�����̴�.
public interface IGesture
{
    /*
    // Function for setting basic option of using gesture.
    bool SetConfig();
    */
    // Function of Check gesture captured every frame.
    void CheckGesture();

    // Uncheck checked gesture flag
    void UnCheck();

    // Function of getting hand which side user used.
    bool AnyHand();

    // Set flag of VR mode.
    void SetMount();

    GestureType gt
    { get; set; }
   
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

    bool _isHeadMount
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

