using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player Instance {get; private set;}

    [SerializeField] LevelData levelData;
    [SerializeField] TMPro.TextMeshProUGUI debugText;
    GameObject objectsBuilt;

    private Rigidbody rb;
    private BuildableManager buildableManager;
    // private Collider collider;
    // [SerializeField] private InputActionAsset actionAsset;
    private Vector3 playerVelocity;
    private bool groundedLastFrame;
    [SerializeField] private Vector2 camSens;
    [SerializeField] private Transform camParent;
    
    [SerializeField] private float gravity = -10.0f;
    [SerializeField] private float startSpeed = 5f;
    [SerializeField] private float accel = 0.85f;
    [SerializeField] private float deaccel = 2.0f;
    [SerializeField] public float pSpeed = 10f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float jumpControl = 0.8f;
    [SerializeField] private float airAccelMult = 0.5f;
    [SerializeField] private float airDeccelMult = 0.25f;

    [SerializeField] private GameObject buildPreviewObject;
    [SerializeField] private BuildPreview buildPreview;

    [SerializeField] private GameObject playerModel;
    
    private Vector3 moveDir;
    private Vector3 normalizedMoveDir;
    private Vector3 camRotateDir;
    private Vector3 dashDirection;
    private Vector3 forward;
    private bool jumpPressed = false;
    [System.NonSerialized] public float dotVel;
    private float surfaceDot = -2;
    private float surfaceDotLastFrame;
    private bool walledLastFrame;
    private Collision currentCollision;

    // velocity not controlled by player direction or max speed
    private Vector3 addedVel;

    // layer 6 is nocollide layer
    int layerMask = 1 << 6;


    [Space]
    [SerializeField] private Animator anim;
    [SerializeField] private AnimationClip walkingAnim, idleAnim, inAirAnim, placingAnim,jumpingAnim;


    public bool inCautionZone {get; set;}

    private void Awake() {
        Instance = this;
        inCautionZone = false;
    }

    // Start is called before the first frame update
    void Start(){

        rb = GetComponent<Rigidbody>();
        buildableManager = GetComponent<BuildableManager>();

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        if (!levelData) {
            levelData = FindObjectOfType<LevelData>();
        }

        if (!camParent) {
            camParent = GameObject.Find("CamParent").transform;
        }

        objectsBuilt = GameObject.Find("PlayerObjectsSpawned");

        if(levelData.availableBuildables.Length == 0){
            buildPreviewObject.gameObject.SetActive(false);
        }

        inCautionZone = false;

        //ResetPlayer();
    }


    // Update is called once per frame
    void Update()
    {
        if (debugText != null)
        {
            debugText.text = "";
            debugText.text += "Grounded: " + IsGrounded() + "\n";
            debugText.text += "Walled: " + IsWalled() + "\n";
            debugText.text += "DotV: " + Vector3.Dot(playerVelocity, forward) + "\n";
            debugText.text += "DotV Actual: " + dotVel + "\n";
            debugText.text += "Add: " + addedVel + "\n";
            debugText.text += "DotS: " + surfaceDot + "\n";
        }

        anim.SetBool("isWalking",IsWalking());
        anim.SetBool("isGrounded",IsGrounded());

        if(anim.GetCurrentAnimatorClipInfoCount(0) > 0 && anim.GetCurrentAnimatorClipInfo(0) != null && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == inAirAnim.name && IsGrounded()){
            anim.Play(idleAnim.name);
        }
        // if(IsGrounded() && IsWalking()){
        //     anim.Play(walkingAnim.name);
        // }

    }

    public void ResetPlayer(){
        transform.position = levelData.spawnPoint.transform.position;
        transform.rotation = levelData.spawnPoint.transform.rotation;
        levelData.playerScale = 1;
        
        while(objectsBuilt.transform.childCount > 0) {
            DestroyImmediate(objectsBuilt.transform.GetChild(0).gameObject);
        }

        for(int i = 0; i < levelData.availableBuildables.Length; i++){
            levelData.amounts[i] = levelData.startAmounts[i];
            buildableManager.buildUI.Refresh(i);
        }

        for(int i = 0; i < levelData.collectablesNeeded; i++){
            Debug.Log(i);
            levelData.collectiblesParent.transform.GetChild(i).gameObject.SetActive(true);
        }

        levelData.collectablesFound = 0;
        levelData.UpdateCollectibleText();

        if(levelData.availableBuildables.Length == 0){
            buildPreviewObject.gameObject.SetActive(false);
        }
        else{
            buildableManager.disabled = false;
            buildPreviewObject.SetActive(true);
        }

        foreach(PressurePlate p in levelData.pressurePlates){
            p.pressed = 0;
        }

        foreach(MovingPlate p in levelData.movingPlates){
            p.ResetPosition();
        }

        inCautionZone = false;
    }

    public void OnResetLevel(InputValue value)
    {
        ResetPlayer();
    }


    void FixedUpdate()
    {
        playerVelocity = rb.velocity;

        // camera logic
        camParent.eulerAngles = new Vector3(
        camParent.eulerAngles.x + camRotateDir.y * camSens.y * PlayerPrefs.GetFloat("mouseSensMult", 1f) * (PlayerPrefs.GetInt("invertMouseY") == 0 ? 1 : -1), 
        camParent.eulerAngles.y + camRotateDir.x * camSens.x * PlayerPrefs.GetFloat("mouseSensMult", 1f) * (PlayerPrefs.GetInt("invertMouseX") == 0 ? 1 : -1), 
        0f
        );
        // camParent.eulerAngles = new Vector3(camRotateDir.y * camSens.y, camRotateDir.x * camSens.x, 0f);

        GeneralPhysics();

        // end of frame logic
        // 'frame' in this case actually means each physics sim tick... oops
        surfaceDotLastFrame = surfaceDot;
        groundedLastFrame = IsGrounded();
        walledLastFrame = IsWalled();
        rb.velocity = playerVelocity + addedVel;
    }

    private bool IsWalking()
    {
        return Vector3.Dot(playerVelocity, forward) > 0.01;
    }

    private void GeneralPhysics()
    {
        // player move/run logic
        // get forward direction based on cam, temporarily remove x rotation
        Vector3 oldCamAngles = camParent.eulerAngles;
        camParent.eulerAngles = new Vector3(0f, camParent.eulerAngles.y + camRotateDir.x * -camSens.x, 0f);
        // camParent.eulerAngles = new Vector3(0f, camRotateDir.x * camSens.x, 0f);
        if (moveDir != Vector3.zero) 
        {
            normalizedMoveDir = Vector3.Normalize(moveDir);
            forward = camParent.TransformDirection(normalizedMoveDir);
            dotVel = Mathf.Max(startSpeed, dotVel + accel * (IsGrounded() ? 1f : airAccelMult));
        }

        // set position of build preview.
        buildPreviewObject.transform.position = new Vector3(
            transform.position.x + camParent.transform.forward.x * 2f * levelData.playerScale,
            transform.position.y,
            transform.position.z + camParent.transform.forward.z * 2f * levelData.playerScale
        );
        buildPreview.SnapToGround();
        if (forward != Vector3.zero) buildPreviewObject.transform.forward = camParent.transform.forward;

        camParent.eulerAngles = oldCamAngles;
        playerVelocity.x = (forward.x * dotVel) * levelData.playerScale;
        playerVelocity.z = (forward.z * dotVel) * levelData.playerScale;

        // top speed logic
        if (dotVel > pSpeed)
        {
            dotVel -= accel;
        }

        if (moveDir != Vector3.zero) playerModel.transform.forward = forward;
        else dotVel = Mathf.Max(dotVel - deaccel * (IsGrounded() ? 1f : airDeccelMult), 0f);

        addedVel = new Vector3(Mathf.Max(Mathf.Abs(addedVel.x) - deaccel * (IsGrounded() ? 1f : airDeccelMult), 0f) * Mathf.Sign(addedVel.x), 0f, Mathf.Max(Mathf.Max(Mathf.Abs(addedVel.z) - deaccel * (IsGrounded() ? 1f : airDeccelMult), 0f) * Mathf.Sign(addedVel.z)));

        // gravity
        if (!IsGrounded()) 
        {
            playerVelocity.y += gravity * (levelData.playerScale*0.6f + 0.4f);
        }
        else playerVelocity.y = 0;

        // jump logic
        if (IsGrounded() && jumpPressed)
        {
            playerVelocity.y = jumpHeight * (levelData.playerScale*0.6f + 0.4f);
        }

        if (playerVelocity.y > 0 && !jumpPressed)
        {
            playerVelocity.y *= jumpControl;
        }
    }

    public void OnJump(InputValue value)
    {

        anim.Play(jumpingAnim.name);

        float v = value.Get<float>();

        if(IsGrounded()){AudioManager.Instance.PlaySFX("jump");}

        // 1 when pressed, 0 when not
        jumpPressed = (v != 0f);

    }

    public void OnPause(InputValue value)
    {
        float v = value.Get<float>();

        if (v != 0f) FindObjectsByType<PauseMenu>(FindObjectsInactive.Include, FindObjectsSortMode.None)[0].gameObject.SetActive(true);
    }

    public void OnBuild(InputValue value)
    {
        anim.Play(placingAnim.name);

        float v = value.Get<float>();

        AudioManager.Instance.PlaySFX("build");

        if (v != 0f) buildableManager.PlaceBuildable(objectsBuilt.transform);
    }

    public void OnSelectBuildable(InputValue value)
    {
        float v = value.Get<float>();

        if (v != 0f) buildableManager.CycleBuildableSelection();
    }

    public void OnChangeScale(InputValue value)
    {
        if (inCautionZone) return;

        float v = value.Get<float>();

        // Debug.Log("Mouse scroll: " + v);
        if (v != 0)
        {
            levelData.playerScale = Mathf.Clamp(levelData.playerScale + v / 400, levelData.scaleMin, levelData.scaleMax);
        }
    }

    public void OnMove(InputValue value)
    {
        // Debug.Log("OnMove");
        Vector2 v = value.Get<Vector2>();
        moveDir = new Vector3(v.x, 0, v.y);

    }

    public void OnCamRotate(InputValue value)
    {
        // Debug.Log("OnMove");
        Vector2 v = value.Get<Vector2>();
        camRotateDir = new Vector3(v.x*2.5f, v.y*2.5f, 0);
    }

    // TODO have constants reflect size of collider 
    // TODO switch to CapsuleCast
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1.05f * levelData.playerScale, ~layerMask);
    }

    private bool IsWalled()
    {
        return Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1 * levelData.playerScale, transform.position.z), playerModel.transform.forward, 0.55f * levelData.playerScale, ~layerMask) 
        && Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), playerModel.transform.forward, 0.55f * levelData.playerScale, ~layerMask);
    }

    void OnCollisionEnter(Collision collision)
    {
        currentCollision = collision;
    }

    void OnCollisionStay(Collision collision)
    {
        surfaceDot = Vector3.Dot(transform.up, collision.contacts[0].normal);
    }

    void OnCollisionExit(Collision collision)
    {
        surfaceDot = 2;
        currentCollision = null;
        // inWallclimb = false;
    }

}
