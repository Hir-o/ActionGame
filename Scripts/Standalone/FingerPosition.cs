

using Lean.Touch;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class FingerPosition
{
    private LeanFinger _finger;
    private Finger _enhancedFinger;
    private Vector2 _firstTouchScreenPosition;
    private TouchArea _screenTouchArea;

    public FingerPosition(LeanFinger finger)
    {
        _finger = finger;
        _firstTouchScreenPosition = finger.StartScreenPosition;
    }
    
    public FingerPosition(Finger finger)
    {
        _enhancedFinger = finger;
        _firstTouchScreenPosition = finger.screenPosition;
    }

    public LeanFinger Finger => _finger;
    public Finger EnhancedFinger => _enhancedFinger;
    public Vector2 FirstTouchScreenPosition => _firstTouchScreenPosition;

    public TouchArea ScreenTouchArea
    {
        get => _screenTouchArea;
        set => _screenTouchArea = value;
    }

    public bool IsActiveFinger(LeanFinger finger) => _finger.Index == finger.Index;
    public bool IsActiveFinger(Finger finger) => _enhancedFinger.index == finger.index;
}