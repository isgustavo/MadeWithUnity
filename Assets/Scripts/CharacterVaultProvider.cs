using System;
using UnityEngine;

public class CharacterVaultProvider : MonoBehaviour
{
    public bool IsVault { get; set; } = false;
    bool canVault;

    Collider objectToVault;

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

        if (IsVault || rootLocomotion.IsClimb)
            return;

        objectToVault = rootLocomotion.GetMoveRaycastHitObject();

        if (objectToVault == null)
        {
            if (rootLocomotion.PlayerInput.InputDirection != Vector3.zero)
            {
                return;
            }
            else
            {
                rootLocomotion.CanMove(rootLocomotion.CharacterVisual.forward, raycastHit: ref raycastHit);

                if (raycastHit.collider == null)
                    return;
                else
                    objectToVault = raycastHit.collider;
            }
        }

        canVault = CanVault(rootLocomotion.PlayerInput.InputDirection, rootLocomotion.CharacterVisual.right, rootLocomotion.CharacterVisual.position, rootLocomotion.CharacterController.stepOffset, rootLocomotion.CharacterController.radius);

        if (canVault)
        {
            Physics.IgnoreCollision(rootLocomotion.CharacterController, objectToVault, true);

            IsVault = true;
            rootLocomotion.CharacterAnimator?.SetBool(AnimatorUtils.VAULT_PARAMETER_HASH, IsVault);
        }
    }

    bool CanVault (Vector3 inputDirection, Vector3 characterRight, Vector3 characterVisualPosition, float characterVisualStepOffset, float characterVisualRadius)
    {
        Debug.DrawLine(characterVisualPosition + Vector3.up * 1f, characterVisualPosition + Vector3.up * 1f + inputDirection * (characterVisualRadius + .5f), Color.red);
        Debug.DrawLine(characterVisualPosition + Vector3.up * 1f + inputDirection * (characterVisualRadius + .5f), characterVisualPosition + Vector3.up * 1f + inputDirection * (characterVisualRadius + .5f) + Vector3.down * .5f, Color.red);

        Debug.DrawLine(characterVisualPosition + Vector3.up * 1f + characterRight * (characterVisualRadius), characterVisualPosition + Vector3.up * 1f + characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f), Color.red);
        Debug.DrawLine(characterVisualPosition + Vector3.up * 1f + characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f), characterVisualPosition + Vector3.up * 1f + characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f) + Vector3.down * .5f, Color.red);

        Debug.DrawLine(characterVisualPosition + Vector3.up * 1f - characterRight * (characterVisualRadius), characterVisualPosition + Vector3.up * 1f - characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f), Color.red);
        Debug.DrawLine(characterVisualPosition + Vector3.up * 1f - characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f), characterVisualPosition + Vector3.up * 1f - characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f) + Vector3.down * .5f, Color.red);

        if ((Physics.Raycast(characterVisualPosition + Vector3.up * 1.1f, inputDirection, characterVisualRadius + .5f, LayerMaskUtils.ENVIRONMENT | LayerMaskUtils.GROUND) == false
            && Physics.Raycast(characterVisualPosition + Vector3.up * 1.1f + characterRight * (characterVisualRadius), inputDirection, characterVisualRadius + .5f, LayerMaskUtils.ENVIRONMENT | LayerMaskUtils.GROUND) == false
            && Physics.Raycast(characterVisualPosition + Vector3.up * 1.1f - characterRight * (characterVisualRadius), inputDirection, characterVisualRadius + .5f, LayerMaskUtils.ENVIRONMENT | LayerMaskUtils.GROUND) == false) == true)
        {
            return (Physics.Raycast(characterVisualPosition + Vector3.up * 1.1f + inputDirection * (characterVisualRadius + .5f), Vector3.down, .5f, LayerMaskUtils.GROUND) == false
                && Physics.Raycast(characterVisualPosition + Vector3.up * 1.1f - characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f), Vector3.down, .5f, LayerMaskUtils.GROUND) == false
                && Physics.Raycast(characterVisualPosition + Vector3.up * 1.1f + characterRight * (characterVisualRadius) + inputDirection * (characterVisualRadius + .5f), Vector3.down, .5f, LayerMaskUtils.GROUND) == false) == true;
        }

        return false;
    }

    public void OnCharacterVaultEnd ()
    {
        IsVault = false;
        Physics.IgnoreCollision(rootLocomotion.CharacterController, objectToVault, false);
        rootLocomotion.CharacterAnimator?.SetBool(AnimatorUtils.VAULT_PARAMETER_HASH, IsVault);
    }

}
