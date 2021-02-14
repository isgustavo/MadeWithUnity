using System;
using UnityEngine;

public class PlayerInput
{
    public Vector2 LeftStickInput;
    public Vector2 RightStickInput;

    public Vector3 InputDirection;

    bool isEastButtonPressed;
    float eastButtonTimePressed;
    public bool IsEastButtonPressed
    {
        get
        {
            return isEastButtonPressed;
        }
        set
        {
            if (isEastButtonPressed == value)
                return;

            isEastButtonPressed = value;
            eastButtonTimePressed = Time.realtimeSinceStartup;
            OnEastButtonChanged?.Invoke(isEastButtonPressed);
        }
    }

    public bool IsEastButtonLongPressed => Time.realtimeSinceStartup - eastButtonTimePressed > .5f;

    bool isSouthButtonPressed;
    float southButtonTimePressed;
    public bool IsSouthButtonPressed
    {
        get
        {
            return isSouthButtonPressed;
        }
        set
        {
            if (isSouthButtonPressed == value)
                return;

            isSouthButtonPressed = value;
            southButtonTimePressed = Time.realtimeSinceStartup;
            OnSouthButtonChanged?.Invoke(isSouthButtonPressed);
        }
    }

    public event Action<bool> OnEastButtonChanged;
    public event Action<bool> OnSouthButtonChanged;
}
