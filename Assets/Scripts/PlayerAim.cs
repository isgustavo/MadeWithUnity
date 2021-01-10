using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerAim : MonoBehaviour
{
    static readonly int AIM_PARAMETER_HASH = Animator.StringToHash("Aim");
    static readonly int AIM_SHOULDER_X_OFFSET_HASH = Animator.StringToHash("ShoulderXOffset");
    static readonly string AIM_LAYER_ANIMATOR_NAME = "Aiming Layer";
    static readonly int FIELD_OF_VIEW_NORMAL = 40;
    static readonly int FIELD_OF_VIEW_ZOOM = 25;
    static readonly float UNAIM_BODY_ANGLE = 85.5f;
    static readonly float OFFSET_VALUE = .35f;
    static readonly float OFFSET_RIGHT_AIM_VALUE = .5f;
    static readonly float OFFSET_LEFT_AIM_VALUE = -.5f;

    Camera camera;
    CinemachineFreeLook freeLookCameraComponent;
    CinemachineCameraOffset freeLookCameraOffset;
    IPlayerInventory playerInventory;
    InventoryItem currentItem;

    Animator animator;
    int aimLayerAnimatorIndex;

    bool isReadyToFire;
    bool isHoldLeftTrigger = false;
    bool isRightShoulder = true;

    float OffsetAimValue => isRightShoulder ? OFFSET_RIGHT_AIM_VALUE : OFFSET_LEFT_AIM_VALUE;

    float initialShoulderLerpXOffset;
    float finalShoulderLerpXOffset;
    float currentShoulderLerpDuration;

    float initialFieldOfViewLerpValue;
    float finalFieldOfViewLerpValue;
    float currentFieldOfViewDuration;

    void Start()
    {
        TryGetComponents();
        InitValues();
    }

    private void Update() 
    {
        HandleAimAngle();
        HandleFieldOfView();
        HandleCameraOffset();
    }

    private void OnDestroy() 
    {
        if(playerInventory != null)
            playerInventory.OnCurrentItemChanged -= OnCurrentItemChanged;
    }

    void TryGetComponents()
    {
        playerInventory = GetComponent<IPlayerInventory>();
        if(playerInventory == null)
            Debug.LogError("Player aim controller inventory is missing");

        playerInventory.OnCurrentItemChanged += OnCurrentItemChanged;

        animator  = GetComponent<Animator>();
        if(animator == null)
            Debug.LogError("Player aim controller animator is missing");

        aimLayerAnimatorIndex = animator.GetLayerIndex(AIM_LAYER_ANIMATOR_NAME);

        camera = GetComponentInChildren<Camera>();
        if(camera == null)
            Debug.LogError("Player aim controller camera is missing"); 

        freeLookCameraComponent = GetComponentInChildren<CinemachineFreeLook>();
        if(freeLookCameraComponent == null)
            Debug.LogError("Player aim controller freeLookCameraComponent is missing"); 

        freeLookCameraOffset = GetComponentInChildren<CinemachineCameraOffset>();
        if(freeLookCameraOffset == null)
            Debug.LogError("Player aim controller freeLookCameraOffset is missing"); 
    }

    void InitValues()
    {
        isReadyToFire = false;

        initialShoulderLerpXOffset = freeLookCameraOffset.m_Offset.x;
        finalShoulderLerpXOffset = freeLookCameraOffset.m_Offset.x;
        currentShoulderLerpDuration = 0f;

        initialFieldOfViewLerpValue = FIELD_OF_VIEW_NORMAL;
        finalFieldOfViewLerpValue = FIELD_OF_VIEW_NORMAL;
        currentFieldOfViewDuration = 0f;
    }

    void HandleAimAngle()
    {
        if (isHoldLeftTrigger)
        {
            animator.SetFloat("AimAngle", Vector3.SignedAngle(camera.transform.forward, Vector3.up, -camera.transform.right), .25f, Time.deltaTime);
        } else 
        {
            animator.SetFloat("AimAngle", UNAIM_BODY_ANGLE, .25f, Time.deltaTime);
        }
    }

    void HandleFieldOfView()
    {
        currentFieldOfViewDuration += Time.deltaTime * 2.5f;
        freeLookCameraComponent.m_Lens.FieldOfView = Mathf.Lerp(initialFieldOfViewLerpValue, finalFieldOfViewLerpValue, currentFieldOfViewDuration);
    }

    void HandleCameraOffset()
    {
        currentShoulderLerpDuration += Time.deltaTime * 2.5f;
        freeLookCameraOffset.m_Offset.x = Mathf.Lerp(initialShoulderLerpXOffset, finalShoulderLerpXOffset, currentShoulderLerpDuration);
        animator.SetFloat(AIM_SHOULDER_X_OFFSET_HASH, freeLookCameraOffset.m_Offset.x);
    }

    public void OnLeftTrigger(InputValue value)
    {
        isHoldLeftTrigger = value.isPressed;
        if(isHoldLeftTrigger)
        {
            if(currentItem != null)
                Aim(currentItem.itemID);
        }
        else
        {
           UnAim(); 
        }
    }

    public void OnWestButton(InputValue value)
    {
        if(value.isPressed && isReadyToFire)
        {
           isRightShoulder = !isRightShoulder;

           SetCurrentShoulderLerpValues(freeLookCameraOffset.m_Offset.x, OffsetAimValue);
        }
    }

    void Aim(int itemID)
    {
        animator.SetInteger(AIM_PARAMETER_HASH, itemID);
        
        SetCurrentFieldOfViewLerpValues(freeLookCameraComponent.m_Lens.FieldOfView, FIELD_OF_VIEW_ZOOM);
        SetCurrentShoulderLerpValues(freeLookCameraOffset.m_Offset.x, OffsetAimValue);

        isReadyToFire = true;
    }

    void UnAim()
    {
        animator.SetInteger(AIM_PARAMETER_HASH, 0);
        
        SetCurrentFieldOfViewLerpValues(freeLookCameraComponent.m_Lens.FieldOfView, FIELD_OF_VIEW_NORMAL);
        SetCurrentShoulderLerpValues(freeLookCameraOffset.m_Offset.x, OFFSET_VALUE);
        
        isReadyToFire = false;
    }

    void SetCurrentShoulderLerpValues(float initialValue, float finalValue)
    {
        initialShoulderLerpXOffset = initialValue;
        finalShoulderLerpXOffset = finalValue;
        currentShoulderLerpDuration = 0f;
    }


    void SetCurrentFieldOfViewLerpValues(float initialValue, float finalValue)
    {
        initialFieldOfViewLerpValue = initialValue;
        finalFieldOfViewLerpValue = finalValue;
        currentFieldOfViewDuration = 0f;
    }

    void OnCurrentItemChanged(InventoryItem item)
    {
        currentItem = item;
        if(isHoldLeftTrigger && currentItem != null)
            Aim(item.itemID);
        else 
            UnAim();
    }


}

