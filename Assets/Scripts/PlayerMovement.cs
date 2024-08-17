using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI debugText;
    [SerializeField] private Vector2 camSens;
    [SerializeField] private Transform camParent;
    
    [SerializeField] private float gravity = -10.0f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float accel = 0.85f;
    [SerializeField] private float deaccel = 2.0f;
    [SerializeField] public float pSpeed = 10f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float jumpControl = 0.8f;
    [SerializeField] private float airAccelMult = 0.5f;
    [SerializeField] private float airDeccelMult = 0.25f;

    
    private Rigidbody rb;
    
    // received inputs
    public Vector3 moveDir;
    private bool jumpPressed = false;
    private Vector3 camRotateDir;


    // misc internal variables

    // replacing with transform.forward
    // public Vector3 forward {get; private set;}

    // values updated at the start of GeneralPhysiccs each physics tick
    public bool isGrounded {get; private set;}
    public bool isWalled {get; private set;}

    // layer 6 is nocollide layer
    int layerMask = 1 << 6;

    private PlayerScaleManager scaleManager;
    private PlayerAnimation animManager;

    private void Awake() {
        scaleManager = GetComponent<PlayerScaleManager>();
        animManager = GetComponent<PlayerAnimation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }


    void FixedUpdate()
    {
        // camera logic
        camParent.eulerAngles = new Vector3(camParent.eulerAngles.x + camRotateDir.y * camSens.y, camParent.eulerAngles.y + camRotateDir.x * camSens.x, 0f);
        // camParent.eulerAngles = new Vector3(camRotateDir.y * camSens.y, camRotateDir.x * camSens.x, 0f);

        GeneralPhysics();
        UpdateDebugText();
    }

    void UpdateDebugText()
    {
        if (debugText != null)
        {
            debugText.text = "";
            debugText.text += "Grounded: " + isGrounded + "\n";
            debugText.text += "Walled: " + isWalled + "\n";
            debugText.text += "DotV: " + Vector3.Dot(rb.velocity, transform.forward) + "\n";
        }
    }

    public bool IsWalking(){
        return Vector3.Dot(rb.velocity, transform.forward) > 0.01;
    }

    private void GeneralPhysics()
    {
        // raycast to check if on ground or wall; updates isGrounded & isWalled. raycasts called once per update only
        isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1.05f * scaleManager.currentScale, ~layerMask);
        isWalled = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.forward, 0.55f, ~layerMask) && Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), transform.forward, 0.55f, ~layerMask);

        // player move/run logic
        // get forward direction based on cam, temporarily remove x rotation
        Vector3 oldCamAngles = camParent.eulerAngles;
        camParent.eulerAngles = new Vector3(0f, camParent.eulerAngles.y + camRotateDir.x * -camSens.x, 0f);

        transform.forward = moveDir.normalized;

        // max speed and accelearation are both scaled by current player scale
        float targetSpeed;
        float acceleration;
        if (moveDir != Vector3.zero) {
            targetSpeed = moveSpeed * scaleManager.currentScale;
            acceleration = accel * scaleManager.currentScale;
            if (!isGrounded) acceleration *= airAccelMult;
        } else {
            targetSpeed = 0;
            acceleration = deaccel * scaleManager.currentScale;
            if (!isGrounded) acceleration *= airAccelMult;
        }

        // point final velocity in facing direction
        Vector3 targetVelocity = transform.forward * targetSpeed;
        // accelerate towards the target velocity with respect to fixed delta time
        rb.velocity = Vector3.MoveTowards(rb.velocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        camParent.eulerAngles = oldCamAngles;

        // gravity
        if (!isGrounded) rb.velocity += Vector3.up * gravity * scaleManager.currentScale;
        // else rb.velocity.y = 0;

        // jump logic
        if (isGrounded && jumpPressed)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpHeight * scaleManager.currentScale, rb.velocity.z);
            animManager.PlayJumpAnimation();
            jumpPressed = false;
        }

        if (rb.velocity.y > 0 && !jumpPressed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * jumpControl, rb.velocity.z);
        }
    }

    // Set the move direction relative to the current camera.
    public void SetMoveDirection(float x, float z) {
        // Vector3 transformedDir = camParent.TransformDirection(new Vector3(x, 0, z));
        // transformedDir.y = 0;
        // moveDir = transformedDir;

        moveDir = new Vector3(x, 0, z);

    }

    public void SetCamRotateDirection(Vector3 direction) {
        camRotateDir = direction;
    }

    public void JumpPress() {
        jumpPressed = true;
    }
}
