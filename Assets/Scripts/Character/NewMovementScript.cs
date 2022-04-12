using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMovementScript : MonoBehaviour
{
    
    private PlayerControllerInput playerControllerInput;
    public CharacterController controller;
    public Transform cam;
    public PlayerInput playerInput;

    public float speed = 2f;
    public float turnTime = 0.1f;
    float turnVelocity;

    void Awake()
    {
        playerControllerInput = new PlayerControllerInput();
    }
    private void OnEnable()
    {
        playerControllerInput.Enable();
    }

    private void OnDisable()
    {
        playerControllerInput.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float movementInput = playerControllerInput.Land.Move.ReadValue<float>();

        Vector3 currentPos = transform.position;

        currentPos += transform.forward * speed * Time.deltaTime;
    }
    /*public void OnMove(InputValue value)
    {
        Vector3 direction = value.Get<Vector3>().normalized;
        
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }*/
}
