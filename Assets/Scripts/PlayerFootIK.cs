using UnityEngine;

public class PlayerFootIK : MonoBehaviour
{
    static readonly float HIGHT_FROM_GROUND = 1.14f;
    static readonly float DOWN_DISTANCE = 1.5f;
    //static readonly LayerMask ENVIROMENT_LAYER = 1 << 9;
    static readonly float PELVIS_OFFSET = 0f;
    static readonly float PELVIS_UP_AND_DOWN_SPEED = .25f;
    static readonly float FEET_TO_IK_POSITION_SPEED = .5f;
    static readonly string LEFT_FOOT_CURVE = "LeftFootCurve";
    static readonly string RIGHT_FOOT_CURVE = "RightFootCurve";

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
        AdjustFootTarget(ref rightFootPosition, HumanBodyBones.RightFoot);
        AdjustFootTarget(ref leftFootPosition, HumanBodyBones.LeftFoot);

        SetFootIKPositionAndRotation(rightFootPosition, ref rightFootIKPosition, ref rightFootIKRotation);
        SetFootIKPositionAndRotation(leftFootPosition, ref leftFootIKPosition, ref leftFootIKRotation);
    }

    private void OnAnimatorIK(int layerIndex) 
    {
        MovePelvisHeight();

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat(RIGHT_FOOT_CURVE));
        MoveFeetToIKPosition(AvatarIKGoal.RightFoot, rightFootIKPosition, rightFootIKRotation, ref lastRightFootPositionY);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat(LEFT_FOOT_CURVE));
        MoveFeetToIKPosition(AvatarIKGoal.LeftFoot, leftFootIKPosition, leftFootIKRotation, ref lastLeftFootPositionY);
    }

    void MoveFeetToIKPosition(AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastFootPositionY)
    {
        Vector3 targetIKPosition = animator.GetIKPosition(foot);

        if(positionIKHolder != Vector3.zero)
        {
            targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
            positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

            lastFootPositionY = Mathf.Lerp(lastFootPositionY, positionIKHolder.y, FEET_TO_IK_POSITION_SPEED);
            targetIKPosition.y += lastFootPositionY;

            targetIKPosition = transform.TransformPoint(targetIKPosition);

            animator.SetIKRotation(foot, rotationIKHolder);
        }

        animator.SetIKPosition(foot, targetIKPosition);   

    }

    void MovePelvisHeight()
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

    private void SetFootIKPositionAndRotation(Vector3 footPosition, ref Vector3 footIKPosition, ref Quaternion footIKRotation)
    {
        if(Physics.Raycast(footPosition, footPosition.normalized + Vector3.down * (DOWN_DISTANCE + HIGHT_FROM_GROUND), out RaycastHit hit))
        {
            footIKPosition = footPosition;
            footIKPosition.y = hit.point.y + PELVIS_OFFSET;
            footIKRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;

        } else 
        {
            footIKPosition = Vector3.zero;
        }
    }

    private void AdjustFootTarget(ref Vector3 footPosition, HumanBodyBones footBodyBone)
    {
        footPosition = animator.GetBoneTransform(footBodyBone).position;
        footPosition.y = transform.position.y + HIGHT_FROM_GROUND;
    }


}
