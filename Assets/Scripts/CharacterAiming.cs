using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAiming : MonoBehaviour
{
    readonly float TURN_SPEED = 1f;

    Camera characterCamera;
    public Transform characterCameraFollow;
    public float rotationPower = 1f;
    public float rotationLerp = 0.5f;

    Vector2 currentRightStickInput;
    Vector2 currentLeftStickInput;
    //public Transform VIsual;
    Animator characterAnimator;
    Transform characterCameraFollowBody;

    CharacterController cc;

    void Start ()
    {
        characterAnimator = GetComponentInChildren<Animator>();
        characterCamera = GetComponentInChildren<Camera>();

        SlotRefs characterSlotsRefs  = GetComponentInChildren<SlotRefs>();
        characterCameraFollowBody = characterSlotsRefs.TryGetParentWithName("CharacterCameraFollow");
    }

    private void LateUpdate ()
    {
        
        characterCameraFollow.position = new Vector3(characterAnimator.transform.position.x, characterCameraFollowBody.position.y, characterCameraFollowBody.position.z);
    }

    void Update ()
    {
        //Debug.Log($"{currentRightStickInput}");
        //Debug.Log($"{currentLeftStickInput}");
        //Debug.Log($"{isHoldLeftTrigger}");
        //characterCameraFollow.eulerAngles = new Vector3(currentRightStickInput.y, currentRightStickInput.x, 0f);

        //float yawCamera = characterCameraFollow.transform.rotation.eulerAngles.y;
        //characterCameraFollow.rotation = Quaternion.Slerp(characterCameraFollow.rotation, Quaternion.Euler(0f, yawCamera, 0f), TURN_SPEED * Time.deltaTime);

        characterCameraFollow.transform.rotation *= Quaternion.AngleAxis(currentRightStickInput.x * rotationPower, Vector3.up);

        characterCameraFollow.transform.rotation *= Quaternion.AngleAxis(currentRightStickInput.y * rotationPower, Vector3.right);

        var angles = characterCameraFollow.transform.localEulerAngles;
        angles.z = 0;

        var angle = characterCameraFollow.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        characterCameraFollow.transform.localEulerAngles = angles;

        //nextRotation = Quaternion.Lerp(characterCameraFollow.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

        if (currentLeftStickInput.x == 0 && currentLeftStickInput.y == 0)
        {
            nextPosition = transform.position;

            //if (aimValue == 1)
            //{
            //    //Set the player rotation based on the look transform
            //    transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
            //    //reset the y rotation of the look transform
            //    followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            //}

            return;
        } else
        {
            characterAnimator.SetFloat("Horizontal", currentLeftStickInput.x, .25f, Time.deltaTime);
            characterAnimator.SetFloat("Vertical", currentLeftStickInput.y, .25f, Time.deltaTime);
        }

        float moveSpeed = speed / 100f;
        Vector3 position = (transform.forward * currentLeftStickInput.y * moveSpeed) + (transform.right * currentLeftStickInput.x * moveSpeed);
        nextPosition = transform.position + position;


        //Set the player rotation based on the look transform
        characterAnimator.transform.rotation = Quaternion.Slerp(characterAnimator.transform.rotation, Quaternion.Euler(0, characterCameraFollow.transform.rotation.eulerAngles.y, 0), 5f * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(0, characterCameraFollow.transform.rotation.eulerAngles.y, 0);
        //reset the y rotation of the look transform
        //characterCameraFollow.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

    }

    void UpdateRotationDirection ()
    {
        float yawCamera = characterCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, yawCamera, 0f), 5f * Time.deltaTime);
    }

    public float speed = 1f;

    public Vector3 nextPosition;
    public Quaternion nextRotation;

    public void OnLeftStick (InputValue value) => currentLeftStickInput = value.Get<Vector2>();
    public void OnRightStick (InputValue value) => currentRightStickInput = value.Get<Vector2>();
    bool isHoldLeftTrigger;
    public void OnLeftTrigger (InputValue value) => isHoldLeftTrigger = value.isPressed;
}
