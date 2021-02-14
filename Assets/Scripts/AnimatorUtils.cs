using UnityEngine;

public static class AnimatorUtils
{
    public readonly static int HORIZONTAL_PARAMETER_HASH = Animator.StringToHash("Horizontal");
    public readonly static int VERTICAL_PARAMETER_HASH = Animator.StringToHash("Vertical");
    public readonly static int CROUCH_PARAMETER_HASH = Animator.StringToHash("IsCrouch");
    public readonly static int PRONE_PARAMETER_HASH = Animator.StringToHash("IsProne");
    public readonly static int VAULT_PARAMETER_HASH = Animator.StringToHash("IsVault");
    public readonly static int CLIMB_PARAMETER_HASH = Animator.StringToHash("IsClimb");
    public readonly static int IS_GROUNDED_PARAMETER_HASH = Animator.StringToHash("IsGrounded");
    public readonly static int FALLING_DURATION_PARAMETER_HASH = Animator.StringToHash("FallingDuration");

}
