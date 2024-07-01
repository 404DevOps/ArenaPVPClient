using UnityEditorInternal;
using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    [SerializeField]
    public float walkSpeed = 2f;
    [SerializeField]
    public float rotationSpeed = 0.1f;

    public bool isJumping = false;
    public bool isFalling = false;
    public bool isGrounded = true;

    public float jumpSpeed = 3f;
    public float jumpDirectionalSpeed = 3f;
    public float currentFallSpeed = 4f;
    public float startFallSpeed = 0.01f;
    public float jumpHeight = 2f;
    public float groundLevel = 1f;

    public Vector3 currentDirection;
    private float rotation;
    public float gravity = 0.18f;

    Rigidbody rb;
    CharacterController characterController;

    Player player;

    //[HideInInspector]
    public bool steer;

    public Controls controls;
    private PlayerSettings settings;

    [HideInInspector]
    public CameraController mainCam;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        if (!player.IsOwnedByMe)
            return;

        settings = FindObjectOfType<PlayerSettings>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!player.IsOwnedByMe)
            return;

        if (steer)
        {
            rotation = Input.GetAxis("Mouse X") * mainCam.CameraSpeed;
            transform.eulerAngles += new Vector3(0, rotation, 0);
        }

        //todo. use character controller isGrounded
        if (isGrounded)
            currentDirection = GetDirection();
    }
    private void FixedUpdate()
    {
        if (!player.IsOwnedByMe)
            return;

        characterController.Move(currentDirection * walkSpeed * Time.deltaTime);

        HandleJump();
    }

    private void HandleJump()
    {
        if (controls.jump.IsPressed() && isGrounded)
        {
            isJumping = true;
            isGrounded = false;
        }

        if (isJumping)
        {
           
            transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime);
            transform.Translate(currentDirection * Time.deltaTime);
        }
        if (transform.position.y >= jumpHeight)
        {
            isJumping = false;
            isFalling = true;
        }
        if (isFalling)
        {
            currentFallSpeed += gravity * Time.deltaTime;
            transform.Translate(-Vector3.up * currentFallSpeed);
            transform.Translate(currentDirection * Time.deltaTime);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor" && isGrounded == false)
        {
            isFalling = false;
            isGrounded = true;
            transform.position = new Vector3(transform.position.x, groundLevel, transform.position.z);
            currentFallSpeed = startFallSpeed;
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            //if left by walking off a cliff, set falling state
            if(!isJumping)
                isFalling = true;

            isGrounded = false;
        }
    }

    public float Axis(bool pos, bool neg)
    {
        float axis = 0;

        if(pos)
            axis += 1;
        if(neg) axis -= 1;

        return axis;
    }

    private Vector3 GetDirection() 
    {
        var direction = new Vector3();
        //if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1))
        //{
        //    direction.z = 1f;
        //}
        if (controls.forwards.IsPressed() || Input.GetKey(KeyCode.Mouse0) && steer)
            direction += transform.forward;
        if (controls.backwards.IsPressed())
            direction += -transform.forward;
        if (controls.strafeLeft.IsPressed())
            direction += -transform.right;
        if (controls.strafeRight.IsPressed())
            direction += transform.right;

        //direction.z = Axis(controls.forwards.IsPressed() || (Input.GetKey(KeyCode.Mouse0) && steer), controls.backwards.IsPressed());
        //direction.x = Axis(controls.strafeRight.IsPressed(), controls.strafeLeft.IsPressed());

        return direction;

    }

    public void ReloadControlSettings() 
    {
        controls = settings.Settings.Controls;
    }

    private void OnEnable()
    {
        GameEvents.onSettingsLoaded.AddListener(ReloadControlSettings);
        UIEvents.onControlsChanged.AddListener(ReloadControlSettings);
    }
    private void OnDisable()
    {
        GameEvents.onSettingsLoaded.RemoveListener(ReloadControlSettings);
        UIEvents.onControlsChanged.RemoveListener(ReloadControlSettings);
    }
}
