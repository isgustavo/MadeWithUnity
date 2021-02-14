using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterRootLocomotion : MonoBehaviour
{
    readonly float FOLLOW_ROTATION_SPEED = 1f;
    readonly float VISUAL_ROTATION_SPEED = 5f;

    readonly float MAX_ANGLE = 340;
    readonly float MIN_ANGLE = 40;

    public static Vector3 CHARACTER_CONTROLLER_CENTER = new Vector3(0f, .9f, 0f);
    public static float CHARACTER_CONTROLLER_HEIGHT = 1.7f;

    float fallingDuration;
    float lastCharacterPositionY;

    bool canRotate;
    bool isOnGround;

    public bool IsMove { get; private set; }
    public bool IsCrouch => characterCrouchProvider != null && characterCrouchProvider.IsCrouch;
    public bool IsProne => characterProneProvider != null && characterProneProvider.IsProne;
    public bool IsVault => characterVaultProvider != null && characterVaultProvider.IsVault;
    public bool IsClimb => characterClimbProvider != null && characterClimbProvider.IsClimb;

    public PlayerInput PlayerInput { get; private set; } = new PlayerInput();
    public Animator CharacterAnimator { get; private set; }
    public Camera CharacterCamera { get; private set; }
    public Transform CharacterCameraFollow { get; private set; }
    public Transform CharacterHeadFollow { get; private set; }
    public Transform CharacterVisual { get; private set; }
    public CharacterController CharacterController { get; private set; }

    CharacterCrouchProvider characterCrouchProvider;
    CharacterProneProvider characterProneProvider;
    CharacterVaultProvider characterVaultProvider;
    CharacterClimbProvider characterClimbProvider;

    RaycastHit canMoveRaycastHitResult;
    public Collider GetMoveRaycastHitObject () => canMoveRaycastHitResult.collider;

    CharacterFootIKProvider characterFootIKProvider;

    private void Start ()
    {
        CharacterAnimator = GetComponentInChildren<Animator>();
        CharacterCamera = GetComponentInChildren<Camera>();
        CharacterCameraFollow = GetComponentInChildren<CinemachineVirtualCamera>().m_Follow;
        CharacterVisual = CharacterAnimator.transform;
        CharacterController = GetComponentInChildren<CharacterController>();

        characterCrouchProvider = GetComponentInChildren<CharacterCrouchProvider>();
        characterProneProvider = GetComponentInChildren<CharacterProneProvider>();
        characterVaultProvider = GetComponentInChildren<CharacterVaultProvider>();
        characterClimbProvider = GetComponentInChildren<CharacterClimbProvider>();

        SlotRefs characterSlotsRefs = GetComponentInChildren<SlotRefs>();
        CharacterHeadFollow = characterSlotsRefs.TryGetParentWithName("Neck");
    }
    
    private void Update ()
    {
        PlayerInput.InputDirection = GetInputDirectionRelativeToCamera(PlayerInput.LeftStickInput, CharacterCameraFollow);
        IsMove = isOnGround && CanMove(PlayerInput.InputDirection, ref canMoveRaycastHitResult);

        canRotate = PlayerInput.LeftStickInput.sqrMagnitude > .1f;

        if (canRotate)
        {
            CharacterVisual.transform.rotation = Quaternion.Slerp(CharacterVisual.transform.rotation, Quaternion.Euler(0, CharacterCameraFollow.transform.rotation.eulerAngles.y, 0), VISUAL_ROTATION_SPEED * Time.deltaTime);
        }

        UpdateCameraFollowRotation(PlayerInput.RightStickInput);

        CharacterAnimator?.SetFloat(AnimatorUtils.HORIZONTAL_PARAMETER_HASH, IsMove ? PlayerInput.LeftStickInput.x : 0f, .25f, Time.deltaTime);
        CharacterAnimator?.SetFloat(AnimatorUtils.VERTICAL_PARAMETER_HASH, IsMove ? PlayerInput.LeftStickInput.y : 0f, .25f, Time.deltaTime);
        CharacterAnimator?.SetBool(AnimatorUtils.IS_GROUNDED_PARAMETER_HASH, isOnGround);
    }

    void FixedUpdate ()
    {
        if (IsGrounded() != isOnGround)
        {
            isOnGround = !isOnGround;

            if (isOnGround)
                CharacterAnimator?.SetFloat(AnimatorUtils.FALLING_DURATION_PARAMETER_HASH, fallingDuration);
            else
                fallingDuration = 0;
        }

        if (isOnGround == false)
            fallingDuration += Time.fixedDeltaTime;

    }

    void LateUpdate ()
    {
        CharacterCameraFollow.position = new Vector3(CharacterVisual.position.x, CharacterHeadFollow.position.y, CharacterVisual.position.z);

        lastCharacterPositionY = CharacterHeadFollow.position.y;
    }

    public bool CanMove (Vector3 inputDirection, ref RaycastHit raycastHit)
    {
        if (IsProne)
        {
            return true;
        } else
        {
            Debug.DrawLine(CharacterVisual.position + Vector3.up * CharacterController.stepOffset, CharacterVisual.position + Vector3.up * CharacterController.stepOffset + inputDirection * (CharacterController.radius + .5f), Color.black);
            Debug.DrawLine(CharacterVisual.position + Vector3.up * CharacterController.stepOffset + CharacterVisual.right * (CharacterController.radius), CharacterVisual.position + Vector3.up * CharacterController.stepOffset + CharacterVisual.right * (CharacterController.radius) + inputDirection * (CharacterController.radius + .5f), Color.black);
            Debug.DrawLine(CharacterVisual.position + Vector3.up * CharacterController.stepOffset - CharacterVisual.right * (CharacterController.radius), CharacterVisual.position + Vector3.up * CharacterController.stepOffset - CharacterVisual.right * (CharacterController.radius) + inputDirection * (CharacterController.radius + .5f), Color.black);
            Debug.DrawLine(CharacterVisual.position + Vector3.up * .8f, CharacterVisual.position + Vector3.up * .75f + inputDirection * (CharacterController.radius + .5f), Color.black);

            return (Physics.Raycast(CharacterVisual.position + Vector3.up * CharacterController.stepOffset, inputDirection, out raycastHit, CharacterController.radius + .5f, LayerMaskUtils.ENVIRONMENT | LayerMaskUtils.GROUND) == false
                && Physics.Raycast(CharacterVisual.position + Vector3.up * CharacterController.stepOffset + CharacterVisual.right * (CharacterController.radius), inputDirection, out raycastHit, CharacterController.radius + .5f, LayerMaskUtils.ENVIRONMENT | LayerMaskUtils.GROUND) == false
                && Physics.Raycast(CharacterVisual.position + Vector3.up * CharacterController.stepOffset - CharacterVisual.right * (CharacterController.radius), inputDirection, out raycastHit, CharacterController.radius + .5f, LayerMaskUtils.ENVIRONMENT | LayerMaskUtils.GROUND) == false
                && Physics.Raycast(CharacterVisual.position + Vector3.up * .8f, inputDirection, out raycastHit, CharacterController.radius + .5f, LayerMaskUtils.ENVIRONMENT | LayerMaskUtils.GROUND) == false) == true;

        }
    }

    Vector3 GetInputDirectionRelativeToCamera (Vector2 input, Transform cameraTransform)
    {
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f;

        return cameraForward * input.y + cameraRight * input.x;
    }

    bool IsGrounded ()
    {
        Debug.DrawLine(CharacterVisual.position + Vector3.up * CharacterController.stepOffset + transform.forward * (CharacterController.radius + .25f), CharacterVisual.position + Vector3.up * CharacterController.stepOffset + transform.forward * (CharacterController.radius + .25f) + Vector3.down * (CharacterController.stepOffset + .5f), Color.black);
        Debug.DrawLine(CharacterVisual.position + Vector3.up * CharacterController.stepOffset + -transform.forward * (CharacterController.radius + .25f), CharacterVisual.position + Vector3.up * CharacterController.stepOffset + -transform.forward * (CharacterController.radius + .25f) + Vector3.down * (CharacterController.stepOffset + .5f), Color.black);
        Debug.DrawLine(CharacterVisual.position + Vector3.up * CharacterController.stepOffset + transform.right * (CharacterController.radius + .25f), CharacterVisual.position + Vector3.up * CharacterController.stepOffset + transform.right * (CharacterController.radius + .25f) + Vector3.down * (CharacterController.stepOffset + .5f), Color.black);
        Debug.DrawLine(CharacterVisual.position + Vector3.up * CharacterController.stepOffset + -transform.right * (CharacterController.radius + .25f), CharacterVisual.position + Vector3.up * CharacterController.stepOffset + -transform.right * (CharacterController.radius + .25f) + Vector3.down * (CharacterController.stepOffset + .5f), Color.black);

        return Physics.Raycast(CharacterVisual.position + Vector3.up * CharacterController.stepOffset + transform.forward * (CharacterController.radius + .25f), Vector3.down, CharacterController.stepOffset + .5f, LayerMaskUtils.GROUND)
            || Physics.Raycast(CharacterVisual.position + Vector3.up * CharacterController.stepOffset + -transform.forward * (CharacterController.radius + .25f), Vector3.down, CharacterController.stepOffset + .5f, LayerMaskUtils.GROUND)
            || Physics.Raycast(CharacterVisual.position + Vector3.up * CharacterController.stepOffset + transform.right * (CharacterController.radius + .25f), Vector3.down, CharacterController.stepOffset + .5f, LayerMaskUtils.GROUND)
            || Physics.Raycast(CharacterVisual.position + Vector3.up * CharacterController.stepOffset + -transform.right * (CharacterController.radius + .25f), Vector3.down, CharacterController.stepOffset + .5f, LayerMaskUtils.GROUND);
    }

    void UpdateCameraFollowRotation (Vector2 input)
    {
        CharacterCameraFollow.transform.rotation *= Quaternion.AngleAxis(input.x * FOLLOW_ROTATION_SPEED, Vector3.up);
        CharacterCameraFollow.transform.rotation *= Quaternion.AngleAxis(input.y * FOLLOW_ROTATION_SPEED, Vector3.right);

        Vector3 angles = CharacterCameraFollow.transform.localEulerAngles;
        angles.z = 0;

        if (angles.x > 180 && angles.x < MAX_ANGLE)
        {
            angles.x = MAX_ANGLE;
        }
        else if (angles.x < 180 && angles.x > MIN_ANGLE)
        {
            angles.x = MIN_ANGLE;
        }

        CharacterCameraFollow.transform.localEulerAngles = angles;
    }

    public void OnLeftStick (InputValue value) => PlayerInput.LeftStickInput = value.Get<Vector2>();

    public void OnRightStick (InputValue value) => PlayerInput.RightStickInput = value.Get<Vector2>();

    public void OnSouthButton (InputValue value) => PlayerInput.IsSouthButtonPressed = value.isPressed;

    public void OnEastButton (InputValue value) => PlayerInput.IsEastButtonPressed = value.isPressed;


}
