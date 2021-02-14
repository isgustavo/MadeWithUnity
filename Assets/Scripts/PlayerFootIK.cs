using UnityEngine;

public class PlayerFootIK : MonoBehaviour
{
    readonly int RIGHT_FOOT = 0;
    readonly int LEFT_FOOT = 1;

    readonly float HIGHT_FROM_GROUND = 0.1f;
    //readonly LayerMask ENVIROMENT_LAYER = 1 << 9;
    readonly float PELVIS_OFFSET = 0f;
    readonly float PELVIS_UP_AND_DOWN_SPEED = .28f;
    readonly float FEET_TO_IK_POSITION_SPEED = .5f;

    readonly Vector3 RAYCAST_GROUND_OFFSET = Vector3.up * .25f;

    readonly string LEFT_FOOT_CURVE = "LeftFootCurve";
    readonly string RIGHT_FOOT_CURVE = "RightFootCurve";

    Vector3[] footPosition = new Vector3[2] { Vector3.zero, Vector3.zero };
    Vector3[] footIKPosition = new Vector3[2] { Vector3.zero, Vector3.zero };

    //Vector3 rightFootPosition;
    //Vector3 rightFootIKPosition;
    //Quaternion rightFootIKRotation;

    //Vector3 leftFootPosition;
    //Vector3 leftFootIKPosition;
    //Quaternion leftFootIKRotation;

    float lastPelvisPositionY;
    float lastRightFootPositionY;
    float lastLeftFootPositionY;

    Animator animator;
    LayerMask groundLayerMask = 1 << 7;

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
        FixedUpdateFoot(RIGHT_FOOT);
        FixedUpdateFoot(LEFT_FOOT);
    }

    private void OnAnimatorIK (int layerIndex)
    {
        OnPelvisAnimatorIK();

        OnAnimatorIKFoot(RIGHT_FOOT, AvatarIKGoal.RightFoot);
        OnAnimatorIKFoot(LEFT_FOOT, AvatarIKGoal.LeftFoot);

        //animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        //SetFootToIKPosition(AvatarIKGoal.RightFoot, rightFootIKPosition);

        //animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        //SetFootToIKPosition(AvatarIKGoal.LeftFoot, leftFootIKPosition);
    }


    void FixedUpdateFoot (int footIndex)
    {
        if (CanHandleIK(footIndex))
        {
            TryNearGround(footIndex);
        }
    }

    bool CanHandleIK (int footIndex)
    {
        return true;// isHoldLeftTrigger == false && handIndex != currentItem?.handIndex;
    }

    void TryNearGround (int footIndex)
    {
        Vector3 footPosition = animator.GetBoneTransform(footIndex == 0 ? HumanBodyBones.RightFoot : HumanBodyBones.LeftFoot).position;

        if (Physics.Raycast(footPosition + RAYCAST_GROUND_OFFSET, Vector3.down, out RaycastHit hit))
        {
            footIKPosition[footIndex] = footPosition;
            footIKPosition[footIndex].y = hit.point.y;

            Debug.DrawLine(footPosition + RAYCAST_GROUND_OFFSET, footIKPosition[footIndex], Color.green);
        }
        else
        {
            footIKPosition[footIndex] = Vector3.zero;
            Debug.DrawLine(footPosition + RAYCAST_GROUND_OFFSET, Vector3.zero, Color.red);
        }
    }

    void OnPelvisAnimatorIK ()
    {
        if(footIKPosition[0] == Vector3.zero || footIKPosition[1] == Vector3.zero || lastPelvisPositionY == 0)
        {
            lastPelvisPositionY = animator.bodyPosition.y;
            return;
        }

        float leftOffsetPosition = footIKPosition[1].y - transform.position.y;
        float rightOffsetPosition = footIKPosition[0].y - transform.position.y;

        float totalOffset = (leftOffsetPosition < rightOffsetPosition) ? leftOffsetPosition : rightOffsetPosition;

        Vector3 newPelvisPosition = animator.bodyPosition + Vector3.up * totalOffset;

        newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, PELVIS_UP_AND_DOWN_SPEED);

        animator.bodyPosition = newPelvisPosition;

        lastPelvisPositionY = animator.bodyPosition.y;
    }

    float[] weightLerpValue = new float[2] { 0f, 0f };

    void OnAnimatorIKFoot(int footIndex, AvatarIKGoal avatarFoot)
    {
        Vector3 targetIKPosition = animator.GetIKPosition(avatarFoot);

        if (footIKPosition[footIndex] != Vector3.zero)
        {
            targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
            Vector3 positionIK = transform.InverseTransformPoint(footIKPosition[footIndex]);

            targetIKPosition.y += positionIK.y;

            targetIKPosition = transform.TransformPoint(targetIKPosition);

            //weightLerpValue[footIndex] = Mathf.Clamp(weightLerpValue[footIndex] + Time.fixedDeltaTime, 0f, 1f);
            //float lerpValue = Mathf.Lerp(0, 1, weightLerpValue[footIndex]);

            animator.SetIKPositionWeight(avatarFoot, 1);
            animator.SetIKPosition(avatarFoot, targetIKPosition);
        }
        else
        {
            weightLerpValue[footIndex] = Mathf.Clamp(weightLerpValue[footIndex] - Time.fixedDeltaTime, 0f, 1f);
            float lerpValue = Mathf.Lerp(0, 1, weightLerpValue[footIndex]);

            animator.SetIKPositionWeight(avatarFoot, lerpValue);
        }
    }

    /*
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
        Debug.DrawRay(footPosition, Vector3.down , Color.green);
        
        Vector3 footIKPosition = Vector3.zero;
        if(Physics.Raycast(footPosition, Vector3.down, out RaycastHit hit))
        {
            footIKPosition = footPosition;
            footIKPosition.y = hit.point.y + PELVIS_OFFSET;
        } 

        return footIKPosition;
    }
    */
}
