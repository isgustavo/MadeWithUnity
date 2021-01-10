using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandIK : MonoBehaviour
{
    readonly int RIGHT_HAND = 0;
    readonly int LEFT_HAND = 1;
    readonly Vector3 HAND_ROTATION_UP = new Vector3(0, 1f, 0f);

    [SerializeField]
    float bodyDistanceFromWall = .8f;
    [SerializeField]
    float bodyVerticalOffset = 1.38f;
    [SerializeField]
    float bodyAngleRotationOffset = 20f;

    float[] weightLerpValue = new float[2] {0f, 0f};
    Vector3[] handPosition = new Vector3[2] {Vector3.zero, Vector3.zero};
    Vector3[] handIKPosition = new Vector3[2] {Vector3.zero, Vector3.zero};
    Quaternion[] handIKRotation = new Quaternion[2] {Quaternion.identity, Quaternion.identity};

    Animator animator;
    IPlayerInventory playerInventory;
    InventoryItem currentItem;
    bool isHoldLeftTrigger = false;
    LayerMask environmentLayerMask = 1 << 6;

    void Start()
    {
        TryGetComponents();
    }

    void TryGetComponents()
    {
        animator  = GetComponent<Animator>();
        if(animator == null)
            Debug.LogError("Player HandleIk animator is missing");

        playerInventory = GetComponent<IPlayerInventory>();
        if(playerInventory == null)
            Debug.LogError("Player aim controller inventory is missing");

        playerInventory.OnCurrentItemChanged += OnCurrentItemChanged;
    }

    private void FixedUpdate() 
    {
        FixedUpdateHand(RIGHT_HAND);
        FixedUpdateHand(LEFT_HAND);
    }

    private void OnAnimatorIK(int layerIndex) 
    {
        OnAnimatorIKHand(RIGHT_HAND, AvatarIKGoal.RightHand, AvatarIKHint.RightElbow);
        OnAnimatorIKHand(LEFT_HAND, AvatarIKGoal.LeftHand, AvatarIKHint.LeftElbow);
    }

    void FixedUpdateHand(int handIndex)
    {
        if(CanHandleIK(handIndex))
        {
            TryNearWall(handIndex);
        }
    }

    bool CanHandleIK(int handIndex)
    {
        return isHoldLeftTrigger == false && handIndex != currentItem?.handIndex;
    }

    void TryNearWall(int handIndex)
    {
        Vector3 direction = handIndex == 0 ? transform.right : -transform.right;
        direction = Quaternion.AngleAxis(bodyAngleRotationOffset, Vector3.up) * direction;

        Vector3 offset = handIndex == 0 ? -transform.right * .05f : transform.right * .05f;

        // Debug.DrawLine(transform.position + Vector3.up * bodyVerticalOffset, 
        //                 (transform.position + Vector3.up * bodyVerticalOffset) + direction * bodyDistanceFromWall, Color.green);

        if(Physics.Raycast(transform.position + Vector3.up * bodyVerticalOffset, direction, out RaycastHit hit, bodyDistanceFromWall, environmentLayerMask))
        {
            handIKPosition[handIndex] = hit.point + offset;
            handIKRotation[handIndex] = Quaternion.LookRotation(HAND_ROTATION_UP, hit.normal);
        } 
        else 
        {
            handIKPosition[handIndex] = Vector3.zero;
            handIKRotation[handIndex] = Quaternion.identity;
        }
    }

    void OnAnimatorIKHand(int handIndex, AvatarIKGoal avatarHand, AvatarIKHint avatarElbow)
    {
        if(CanHandleIK(handIndex) && handIKPosition[handIndex] != Vector3.zero)
        {
            weightLerpValue[handIndex] = Mathf.Clamp(weightLerpValue[handIndex] + Time.fixedDeltaTime, 0f, 1f);
            
            animator.SetIKPositionWeight(avatarHand, Mathf.Lerp(0, 1, weightLerpValue[handIndex]));
            animator.SetIKRotationWeight(avatarHand, Mathf.Lerp(0, 1, weightLerpValue[handIndex]));
            //animator.SetIKHintPositionWeight(avatarElbow, Mathf.Lerp(0, 1, weightLerpValue[handIndex]));

            animator.SetIKPosition(avatarHand, handIKPosition[handIndex]); 
            animator.SetIKRotation(avatarHand, handIKRotation[handIndex]);   
            //animator.SetIKHintPosition(avatarElbow, -Vector3.up);
            
           
        } else 
        {
            weightLerpValue[handIndex] = Mathf.Clamp(weightLerpValue[handIndex] - Time.fixedDeltaTime, 0f, 1f);

            animator.SetIKPositionWeight(avatarHand, Mathf.Lerp(0, 1, weightLerpValue[handIndex]));
            animator.SetIKRotationWeight(avatarHand, Mathf.Lerp(0, 1, weightLerpValue[handIndex]));
            //animator.SetIKHintPositionWeight(avatarElbow, Mathf.Lerp(0, 1, weightLerpValue[handIndex]));
        }
    }

    void OnCurrentItemChanged(InventoryItem item)
    {
        currentItem = item;
    }

    public void OnLeftTrigger(InputValue value)
    {
        isHoldLeftTrigger = value.isPressed;
    }
}
