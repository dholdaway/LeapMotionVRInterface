using UnityEngine;
using System.Collections;
using Leap;


public class Circle_Gesture : MonoBehaviour, IMultiStepCheckGesture 
{
    public int _isClockwise;
    protected Vector _normal;

    public float _progress;
    
    protected Pointable _pointable;
    protected float _radius;
    protected CircleGesture _circle_gesture = null;

    
    protected GestureList _gestures;
    protected FingerList _fingers;
    protected float _startProgress;
    protected float _endProgress;
    public Gesture.GestureState _state;

    //-------------------------------------------------

    public int _useDirection = 0;
    public float _minProgress;

    public MountType mt;
    //------------------------------------------------


    public HandList Hands
    { get; set; }

    public bool _isRight
    { get; set; }

    public Controller _leap_controller
    { get; set; }

    public Frame _lastFrame
    { get; set; }

    public bool _isChecked
    { get; set; }

    public bool _isPlaying
    { get; set; }

    public int _userSide
    { get; set; }

    public bool _isHeadMount
    { get; set; }

    public GestureType gt
    { get; set; }

    public virtual void Update()
    {
        if(!_isChecked)
        {
            CheckGesture();
        }
        UnCheck();
    }


    protected int IsClockWise()
    {
        if(_circle_gesture.Pointable.Direction.AngleTo(this.GetNormal()) <= 3.14/2)
        {
            _isClockwise = 1;
        }
        else
        {
            _isClockwise = -1;
        }

        return _isClockwise;
    }

    public virtual void CheckGesture()
    {
        _lastFrame = _leap_controller.Frame(0);
        Hands = _lastFrame.Hands;
        _gestures = _lastFrame.Gestures();

        foreach(Hand hand in Hands)
        {
            foreach (Gesture gesture in _gestures)
            {
                if (gesture.Type == Gesture.GestureType.TYPE_CIRCLE)
                {
                    print("Circle Gesture");

                    _circle_gesture = new CircleGesture(gesture);
                    this.AnyHand();
                    if (!_isPlaying && (gesture.State == Gesture.GestureState.STATE_START) && WhichSide.capturedSide(hand, _userSide, _isHeadMount))
                    {
                        _isPlaying = !_isPlaying;
                        this._startProgress = _circle_gesture.Progress;
                        //print("start");
                        print("start progress : " + this._startProgress);
                    }

                    if (_isPlaying && gesture.State == Gesture.GestureState.STATE_STOP)
                    {
                        int direc = this.IsClockWise();
                        this._endProgress = _circle_gesture.Progress;
                        print("stop progress : " + this._endProgress);
                        if (this._endProgress >= this._minProgress && direc == _useDirection)
                        {
                            this._isChecked = true;
                            this._isPlaying = !this._isPlaying;
                        }
                        this._state = gesture.State;

                        break;
                    }
                    this._state = gesture.State;

                }
            }

            if (_isPlaying && _state == Gesture.GestureState.STATE_UPDATE)
            {
                int direc = this.IsClockWise();
                this._endProgress = _circle_gesture.Progress;
                print("update progress : " + this._endProgress);
                if (this._endProgress >= this._minProgress && direc == _useDirection)
                {
                    this._isChecked = true;
                    this._isPlaying = !this._isPlaying;
                }


            }


            if (_isChecked)
            {
                DoAction();
            }
        }
        
       // return isChecked;
    }

    protected virtual void DoAction()
    {
        print("�ڵ鷯 ������ �� �ڽľ�");
    }

    public virtual void UnCheck()
    {
        _isChecked = false;
    }


    public void SetMount()
    {
        if (mt == MountType.HeadMount)
        {
            _isHeadMount = true;
        }
        else
        {
            _isHeadMount = false;
        }
    }

    protected void SetGestureCondition(int direction, float progress)
    {
        gt = GestureType.circle;
        _leap_controller = ControllerSetter.SetConfig(gt);
        GesutreSetting.SetGestureCondition(this, direction, progress);      
    }

    //1�̸� ������ 0�̸� �޼�
    public bool AnyHand()
    {

        if (_circle_gesture.IsValid)
        {
            if (_circle_gesture.Hands.Rightmost.IsRight)
            {
                this._isRight = true;
            }
            else if (_circle_gesture.Hands.Leftmost.IsLeft)
            {
                this._isRight = false;
            }
            return this._isRight;
        }
        else
        {
            print("This object is not valid");
            return false;
        }
        
    }

    protected bool SetMinProgress(float times)
    {
        this._minProgress = times;
        return true;
    }

    protected Pointable GetPointable()
    {
        if(_circle_gesture != null)
        {
            _pointable = _circle_gesture.Pointable;
            return _pointable;
        }
        else
        {
            return null;
        }
        
    }

    protected float GetProgress()
    {
        if(_circle_gesture != null)
        {
            _progress = _circle_gesture.Progress;
            return _progress;
        }
        else
        {
            return -1;
        }
        
    }

    protected  Vector GetNormal()
    {
        if(_circle_gesture != null)
        {
            this._normal = _circle_gesture.Normal;
            return _normal;
        }
        else
        {
            return null;
        }
    }

    protected HandList GetHandList()
    {
        if(_circle_gesture != null)
        {
            return Hands;
        }
        else
        {
            return new HandList();
        }
        
    }

    protected FingerList GetFingerList()
    {
        if(_circle_gesture != null)
        {
            return _fingers;
        }
        else
        {
            return new FingerList();
        }
    }
}

