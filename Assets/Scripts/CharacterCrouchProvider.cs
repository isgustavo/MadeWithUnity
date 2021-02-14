using System;
using UnityEngine;

public class CharacterCrouchProvider : MonoBehaviour
{
    public static Vector3 CHARACTER_CONTROLLER_CROUCH_CENTER = new Vector3(0f, .72f, 0f);
    public static float CHARACTER_CONTROLLER_CROUCH_HEIGHT = 1.2f;

    public bool IsCrouch { get; set; } = false;
    bool inputRead = false;

    bool isProneOnPress; 

    CharacterRootLocomotion rootLocomotion;

    private void Start ()
    {
        rootLocomotion = GetComponentInParent<CharacterRootLocomotion>();
        rootLocomotion.PlayerInput.OnEastButtonChanged += OnInputChanged;
    }

    private void OnDisable ()
    {
        if(rootLocomotion != null)
            rootLocomotion.PlayerInput.OnEastButtonChanged -= OnInputChanged;
    }

    void OnInputChanged (bool value)
    {
        if(value == true)
        {
            if (IsCrouch == false)
            {
                inputRead = true;
                SetCrouch(true);
            }
        } else
        {
            if (inputRead == false && IsCrouch == true && isProneOnPress == false)
            {
                if (CanGetUp())
                {
                    SetCrouch(false);
                }
            }

            isProneOnPress = rootLocomotion.IsProne;

            inputRead = false;
        }

    }

    private void Update ()
    {
        if (inputRead == false && rootLocomotion.PlayerInput.IsEastButtonPressed && rootLocomotion.PlayerInput.IsEastButtonLongPressed && IsCrouch)
        {
            inputRead = true;
            if(isProneOnPress == true)
            {
                if (CanGetUp())
                {
                    SetCrouch(false);
                }
            }
            else
                SetCrouch(true);
        }    
    }

    void SetCrouch (bool value)
    {
        IsCrouch = value;
        if (IsCrouch)
        {
            rootLocomotion.CharacterController.height = CHARACTER_CONTROLLER_CROUCH_HEIGHT;
            rootLocomotion.CharacterController.center = CHARACTER_CONTROLLER_CROUCH_CENTER;

        }
        else
        {
            rootLocomotion.CharacterController.height = CharacterRootLocomotion.CHARACTER_CONTROLLER_HEIGHT;
            rootLocomotion.CharacterController.center = CharacterRootLocomotion.CHARACTER_CONTROLLER_CENTER;
        }

        rootLocomotion.CharacterAnimator?.SetBool(AnimatorUtils.CROUCH_PARAMETER_HASH, IsCrouch);
    }

    bool CanGetUp ()
    {
        Debug.DrawRay(rootLocomotion.CharacterVisual.position, rootLocomotion.CharacterVisual.up * CharacterRootLocomotion.CHARACTER_CONTROLLER_HEIGHT, Color.black);
        return Physics.Raycast(rootLocomotion.CharacterVisual.position, rootLocomotion.CharacterVisual.up, CharacterRootLocomotion.CHARACTER_CONTROLLER_HEIGHT, LayerMaskUtils.GROUND) == false;
    }

}
