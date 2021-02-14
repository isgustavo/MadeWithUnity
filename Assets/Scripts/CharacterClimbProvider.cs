using UnityEngine;

public class CharacterClimbProvider : MonoBehaviour
{
    public bool IsClimb { get; set; } = false;
    bool canClimb;

    Collider objectToClimb;

    CharacterRootLocomotion rootLocomotion;

    private void Start ()
    {
        rootLocomotion = GetComponentInParent<CharacterRootLocomotion>();
        rootLocomotion.PlayerInput.OnSouthButtonChanged += OnSouthButtonChanged;
    }

    private void OnDisable ()
    {
        if (rootLocomotion != null)
            rootLocomotion.PlayerInput.OnSouthButtonChanged -= OnSouthButtonChanged;
    }

    RaycastHit raycastHit;
    private void OnSouthButtonChanged (bool value)
    {
        if (value == false)
            return;

        if (IsClimb || rootLocomotion.IsVault)
            return;

        objectToClimb = rootLocomotion.GetMoveRaycastHitObject();

        if (objectToClimb == null)
        {
            if (rootLocomotion.PlayerInput.InputDirection != Vector3.zero)
            {
                return;
            } else
            {
                rootLocomotion.CanMove(rootLocomotion.CharacterVisual.forward, raycastHit: ref raycastHit);

                if (raycastHit.collider == null)
                    return;
                else
                    objectToClimb = raycastHit.collider;
            }
        }

        canClimb = CanClimb(rootLocomotion.PlayerInput.InputDirection, rootLocomotion.CharacterVisual.right, rootLocomotion.CharacterVisual.position, rootLocomotion.CharacterController.stepOffset, rootLocomotion.CharacterController.radius);

        if (canClimb)
        {
            Physics.IgnoreCollision(rootLocomotion.CharacterController, objectToClimb, true);

            canClimb = true;
            rootLocomotion.CharacterAnimator?.SetBool(AnimatorUtils.CLIMB_PARAMETER_HASH, canClimb);
        }
    }

    bool CanClimb (Vector3 inputDirection, Vector3 characterRight, Vector3 characterVisualPosition, float characterVisualStepOffset, float characterVisualRadius)
    {
       
        Debug.DrawLine(characterVisualPosition + Vector3.up * 1f, characterVisualPosition + Vector3.up * 1f + inputDirection * (characterVisualRadius + .5f), Color.black);
        Debug.DrawLine(characterVisualPosition + Vector3.up * 1f + inputDirection * (characterVisualRadius + .5f), characterVisualPosition + Vector3.up * 1f + inputDirection * (characterVisualRadius + .5f) + Vector3.down * .5f, Color.black);

        Debug.DrawLine(characterVisualPosition + Vector3.up * 1f + characterRight * (characterVisualRadius), characterVisualPosition + Vector3.up * 1f + characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f), Color.black);
        Debug.DrawLine(characterVisualPosition + Vector3.up * 1f + characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f), characterVisualPosition + Vector3.up * 1f + characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f) + Vector3.down * .5f, Color.black);

        Debug.DrawLine(characterVisualPosition + Vector3.up * 1f - characterRight * (characterVisualRadius), characterVisualPosition + Vector3.up * 1f - characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f), Color.black);
        Debug.DrawLine(characterVisualPosition + Vector3.up * 1f - characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f), characterVisualPosition + Vector3.up * 1f - characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f) + Vector3.down * .5f, Color.black);

        if ((Physics.Raycast(characterVisualPosition + Vector3.up * 1f, inputDirection, characterVisualRadius + .5f, LayerMaskUtils.ENVIRONMENT | LayerMaskUtils.GROUND) == false
            && Physics.Raycast(characterVisualPosition + Vector3.up * 1f + characterRight * (characterVisualRadius), inputDirection, characterVisualRadius + .5f, LayerMaskUtils.ENVIRONMENT | LayerMaskUtils.GROUND) == false
            && Physics.Raycast(characterVisualPosition + Vector3.up * 1f - characterRight * (characterVisualRadius), inputDirection, characterVisualRadius + .5f, LayerMaskUtils.ENVIRONMENT | LayerMaskUtils.GROUND) == false) == true)
        {
            return (Physics.Raycast(characterVisualPosition + Vector3.up * 1f + inputDirection * (characterVisualRadius + 1f), Vector3.down, .5f, LayerMaskUtils.GROUND) == true 
                && Physics.Raycast(characterVisualPosition + Vector3.up * 1f - characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + 1f), Vector3.down, .5f, LayerMaskUtils.GROUND) == true
                && Physics.Raycast(characterVisualPosition + Vector3.up * 1f + characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + 1f), Vector3.down, .5f, LayerMaskUtils.GROUND) == true) == true;
        }

        return false;
    }
    
    public void OnCharacterClimbtEnd ()
    {
        IsClimb = false;
        Physics.IgnoreCollision(rootLocomotion.CharacterController, objectToClimb, false);
        rootLocomotion.CharacterAnimator.SetBool(AnimatorUtils.CLIMB_PARAMETER_HASH, IsClimb);
    }
}
