using UnityEngine;

public class CharacterProneProvider : MonoBehaviour
{
    public static Vector3 CHARACTER_CONTROLLER_PRONE_CENTER = new Vector3(0f, .24f, 0f);
    public static float CHARACTER_CONTROLLER_PRONE_HEIGHT = .32f;

    public bool IsProne { get; set; } = false;
    bool inputRead = false;

    CharacterRootLocomotion rootLocomotion;

    private void Start ()
    {
        rootLocomotion = GetComponentInParent<CharacterRootLocomotion>();
        rootLocomotion.PlayerInput.OnEastButtonChanged += OnInputChanged;
    }

    private void OnDisable ()
    {
        if (rootLocomotion != null)
            rootLocomotion.PlayerInput.OnEastButtonChanged -= OnInputChanged;
    }

    void OnInputChanged (bool value)
    {
        if (value == true)
        {
            if (rootLocomotion.IsProne == true)
            {
                inputRead = true;
                if (CanGetUp())
                {
                    SetProne(false);
                }
            }
        } else
        {
            inputRead = false;
        }

    }

    private void Update ()
    {
        if(inputRead == false && rootLocomotion.PlayerInput.IsEastButtonPressed && rootLocomotion.PlayerInput.IsEastButtonLongPressed && IsProne == false)
            SetProne(true);
    }

    void SetProne (bool value)
    {
        IsProne = value;
        if (IsProne)
        {
            rootLocomotion.CharacterController.height = CHARACTER_CONTROLLER_PRONE_HEIGHT;
            rootLocomotion.CharacterController.center = CHARACTER_CONTROLLER_PRONE_CENTER;

        }
        else
        {
            rootLocomotion.CharacterController.height = CharacterRootLocomotion.CHARACTER_CONTROLLER_HEIGHT;
            rootLocomotion.CharacterController.center = CharacterRootLocomotion.CHARACTER_CONTROLLER_CENTER;
        }

        rootLocomotion.CharacterAnimator?.SetBool(AnimatorUtils.PRONE_PARAMETER_HASH, IsProne);
    }

    bool CanGetUp()
    {
        Debug.DrawRay(rootLocomotion.CharacterVisual.position, rootLocomotion.CharacterVisual.up * CharacterCrouchProvider.CHARACTER_CONTROLLER_CROUCH_HEIGHT, Color.black);
        return Physics.Raycast(rootLocomotion.CharacterVisual.position, rootLocomotion.CharacterVisual.up, CharacterCrouchProvider.CHARACTER_CONTROLLER_CROUCH_HEIGHT, LayerMaskUtils.GROUND) == false;
    }
}
