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
    bool IsEnableGestureHand();

    void DoAction();
    /*
    // Set flag of VR mode.
    void SetMount();
    */

    MountType _mountType
    { get; set; }

    GestureType _gestureType
    { get; set; }
   
    Controller _leap_controller
    { get; set; }

    HandList Hands
    { get; set; }

    Frame _lastFrame
    { get; set; }

    UseArea _useArea
    { get; set; }

    bool _isChecked
    { get; set; }

    UsingHand _usingHand
    { get; set; }

}

