using UnityEngine;

public class CharacterFootIKProvider : MonoBehaviour
{
    readonly int RIGHT_FOOT = 0;
    readonly int LEFT_FOOT = 1;
    readonly float PELVIS_UP_AND_DOWN_SPEED = .28f;
    readonly string IK_ANIMATION_TAG = "IKTag";

    Vector3[] footPosition = new Vector3[2] { Vector3.zero, Vector3.zero };
    Vector3[] footIKPosition = new Vector3[2] { Vector3.zero, Vector3.zero };

    float[] weightLerpValue = new float[2] { 0f, 0f };

    float lastPelvisPositionY;

    CharacterRootLocomotion rootLocomotion;

    LayerMask groundLayerMask;

    private void Start ()
    {
        groundLayerMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Environment");

        rootLocomotion = GetComponentInParent<CharacterRootLocomotion>();
    }

    public void FixedUpdate ()
    {
        if (CanFootIk())
        {
            UpdateFoot(RIGHT_FOOT);
            UpdateFoot(LEFT_FOOT);
        }
    }

    void OnAnimatorIK (int layerIndex)
    {
        if (CanFootIk())
        {
            OnPelvisAnimatorIK();

            OnAnimatorIKFoot(RIGHT_FOOT, AvatarIKGoal.RightFoot);
            OnAnimatorIKFoot(LEFT_FOOT, AvatarIKGoal.LeftFoot);
        }
    }

    bool CanFootIk()
    {
        return rootLocomotion.CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsTag(IK_ANIMATION_TAG);
    }

    void UpdateFoot (int footIndex)
    {
        TryGetNearGroundPosition(footIndex);
    }

    Vector3 groundPoint;

    void TryGetNearGroundPosition (int footIndex)
    {
        Vector3 footPosition = rootLocomotion.CharacterAnimator.GetBoneTransform(footIndex == 0 ? HumanBodyBones.RightFoot : HumanBodyBones.LeftFoot).position;

        if (Physics.Raycast(footPosition + Vector3.up * rootLocomotion.CharacterController.stepOffset, Vector3.down, out RaycastHit hit, rootLocomotion.CharacterController.stepOffset + .5f, groundLayerMask))
        {
            footIKPosition[footIndex] = footPosition;
            footIKPosition[footIndex].y = hit.point.y;

            Debug.DrawLine(footPosition + Vector3.up * rootLocomotion.CharacterController.stepOffset, footIKPosition[footIndex], Color.green);
        }
        else
        {
            footIKPosition[footIndex] = Vector3.zero;
        }
    }

    void OnPelvisAnimatorIK ()
    {
        if (footIKPosition[0] == Vector3.zero || footIKPosition[1] == Vector3.zero || lastPelvisPositionY == 0)
        {
            lastPelvisPositionY = rootLocomotion.CharacterAnimator.bodyPosition.y;
            return;
        }

        float leftOffsetPosition = footIKPosition[1].y - transform.position.y;
        float rightOffsetPosition = footIKPosition[0].y - transform.position.y;

        float totalOffset = (leftOffsetPosition < rightOffsetPosition) ? leftOffsetPosition : rightOffsetPosition;

        Vector3 newPelvisPosition = rootLocomotion.CharacterAnimator.bodyPosition + Vector3.up * totalOffset;

        newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, PELVIS_UP_AND_DOWN_SPEED);

        rootLocomotion.CharacterAnimator.bodyPosition = newPelvisPosition;

        lastPelvisPositionY = rootLocomotion.CharacterAnimator.bodyPosition.y;
    }

    void OnAnimatorIKFoot (int footIndex, AvatarIKGoal avatarFoot)
    {
        Vector3 targetIKPosition = rootLocomotion.CharacterAnimator.GetIKPosition(avatarFoot);

        if (footIKPosition[footIndex] != Vector3.zero)
        {
            targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
            Vector3 positionIK = transform.InverseTransformPoint(footIKPosition[footIndex]);

            targetIKPosition.y += positionIK.y;

            targetIKPosition = transform.TransformPoint(targetIKPosition);

            rootLocomotion.CharacterAnimator.SetIKPositionWeight(avatarFoot, 1);
            rootLocomotion.CharacterAnimator.SetIKPosition(avatarFoot, targetIKPosition);
        }
        else
        {
            weightLerpValue[footIndex] = Mathf.Clamp(weightLerpValue[footIndex] - Time.fixedDeltaTime, 0f, 1f);
            float lerpValue = Mathf.Lerp(0, 1, weightLerpValue[footIndex]);

            rootLocomotion.CharacterAnimator.SetIKPositionWeight(avatarFoot, lerpValue);
        }
    }


}
