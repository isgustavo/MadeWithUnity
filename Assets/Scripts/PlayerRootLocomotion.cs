using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerRootLocomotion : MonoBehaviour
{
    static readonly int HORIZONTAL_PARAMETER_HASH = Animator.StringToHash("Horizontal");
    static readonly int VERTICAL_PARAMETER_HASH = Animator.StringToHash("Vertical");
    static readonly int CROUCH_PARAMETER_HASH = Animator.StringToHash("Crouch");
    static readonly float TURN_SPEED = 5f;
    static readonly float ZOOM_TURN_SPEED = 25f;
    static readonly float FORWARD_OFFSET = .5f;

    Camera camera;
    Animator animator;
    Vector2 currentInput;
    bool isCrouch;
    bool isHoldLeftTrigger;
    
    float CurrentTurnSpeed => isHoldLeftTrigger ? ZOOM_TURN_SPEED : TURN_SPEED;

    void Start()
    {
        TryGetComponents();
        InitValues();
    }

    private void Update ()
    {
        if (currentInput.sqrMagnitude > .1f || isHoldLeftTrigger)
        {
            HandleMovementDirection();
        }

        HandleMovement(currentInput);
    }

    void TryGetComponents()
    {
        animator  = GetComponent<Animator>();
        if(animator == null)
            Debug.LogError("Player root controller animator is missing");
        
        camera = GetComponentInChildren<Camera>();
        if(camera == null)
            Debug.LogError("Player root controller camera is missing");
    }

    void InitValues()
    {
        isCrouch = false;
        isHoldLeftTrigger = false;
    }

    protected virtual void HandleMovementDirection ()
    {
        float yawCamera = camera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, yawCamera, 0f), CurrentTurnSpeed * Time.deltaTime);
    }

    protected void HandleMovement (Vector2 input)
    {
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0f;
        Vector3 cameraRight = camera.transform.right;
        cameraRight.y = 0f;

        Vector3 inputDirection = cameraForward * input.y + cameraRight * input.x; 
        if (CheckPosition(transform.position, inputDirection))
        {
            SetAnimatorMovement(input.x, input.y);
        }
        else
        {
            SetAnimatorMovement();
        }
    }

    void SetAnimatorMovement(float horizontalValue = 0f, float verticalValue = 0f)
    {
        animator.SetFloat(HORIZONTAL_PARAMETER_HASH, horizontalValue, .25f, Time.deltaTime);
        animator.SetFloat(VERTICAL_PARAMETER_HASH, verticalValue, .25f, Time.deltaTime);
    }

    protected bool CheckPosition (Vector3 position, Vector3 direction) => NavMesh.SamplePosition(position + direction * FORWARD_OFFSET, out _, .25f, NavMesh.AllAreas);

    public void OnLeftStick (InputValue value)
    {
        currentInput = value.Get<Vector2>();
    }

    public void OnLeftTrigger(InputValue value)
    {
        isHoldLeftTrigger = value.isPressed;
    }

    public void OnSouthButton(InputValue value)
    {
        if(value.isPressed)
        {
           isCrouch = !isCrouch;

           animator?.SetBool(CROUCH_PARAMETER_HASH, isCrouch);
        }
    }
}