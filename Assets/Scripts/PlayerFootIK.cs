using UnityEngine;

public class PlayerFootIK : MonoBehaviour
{
    readonly float HIGHT_FROM_GROUND = 0.1f;
    //readonly LayerMask ENVIROMENT_LAYER = 1 << 9;
    readonly float PELVIS_OFFSET = 0f;
    readonly float PELVIS_UP_AND_DOWN_SPEED = .28f;
    readonly float FEET_TO_IK_POSITION_SPEED = .5f;
    readonly string LEFT_FOOT_CURVE = "LeftFootCurve";
    readonly string RIGHT_FOOT_CURVE = "RightFootCurve";

    Vector3 rightFootPosition;
    Vector3 rightFootIKPosition;
    Quaternion rightFootIKRotation;

    Vector3 leftFootPosition;
    Vector3 leftFootIKPosition;
    Quaternion leftFootIKRotation;

    float lastPelvisPositionY;
    float lastRightFootPositionY;
    float lastLeftFootPositionY;

    Animator animator;

    void Start()
    {
        TryGetComponents();
    }

    void TryGetComponents()
    {
        animator  = GetComponent<Animator>();
        if(animator == null)
            Debug.LogError("Player root controller animator is missing");
    }

    private void FixedUpdate() 
    {
        rightFootPosition = GetCurrentFootPosition(HumanBodyBones.RightFoot);
        leftFootPosition = GetCurrentFootPosition(HumanBodyBones.LeftFoot);

        rightFootIKPosition = GetCurrentFootIKPosition(rightFootPosition + Vector3.up * HIGHT_FROM_GROUND);
        leftFootIKPosition = GetCurrentFootIKPosition(leftFootPosition + Vector3.up * HIGHT_FROM_GROUND);
    }

    private void OnAnimatorIK(int layerIndex) 
    {
        SetPelvisHeight();

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        SetFootToIKPosition(AvatarIKGoal.RightFoot, rightFootIKPosition);
        
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        SetFootToIKPosition(AvatarIKGoal.LeftFoot, leftFootIKPosition);
    }

    void SetPelvisHeight()
    {
        if(rightFootIKPosition == Vector3.zero || leftFootIKPosition == Vector3.zero || lastPelvisPositionY == 0)
        {
            lastPelvisPositionY = animator.bodyPosition.y;
            return;
        }

        float leftOffsetPosition = leftFootIKPosition.y - transform.position.y;
        float rightOffsetPosition = rightFootIKPosition.y - transform.position.y;

        float totalOffset = (leftOffsetPosition < rightOffsetPosition) ? leftOffsetPosition : rightOffsetPosition;

        Vector3 newPelvisPosition = animator.bodyPosition + Vector3.up * totalOffset;

        newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, PELVIS_UP_AND_DOWN_SPEED);

        animator.bodyPosition = newPelvisPosition;

        lastPelvisPositionY = animator.bodyPosition.y;
    }

    void SetFootToIKPosition(AvatarIKGoal foot, Vector3 positionIKHolder)
    {
        Vector3 targetIKPosition = animator.GetIKPosition(foot);

        if(positionIKHolder != Vector3.zero)
        {
            targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
            positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

            targetIKPosition.y += positionIKHolder.y;

            targetIKPosition = transform.TransformPoint(targetIKPosition);
        }

        animator.SetIKPosition(foot, targetIKPosition);   
    }


    private Vector3 GetCurrentFootPosition(HumanBodyBones footBodyBone)
    {
        return animator.GetBoneTransform(footBodyBone).position;
    }

    private Vector3 GetCurrentFootIKPosition (Vector3 footPosition)
    {
        //Debug.DrawRay(footPosition, Vector3.down , Color.green);
        
        Vector3 footIKPosition = Vector3.zero;
        if(Physics.Raycast(footPosition, Vector3.down, out RaycastHit hit))
        {
            footIKPosition = footPosition;
            footIKPosition.y = hit.point.y + PELVIS_OFFSET;
        } 

        return footIKPosition;
    }
}
