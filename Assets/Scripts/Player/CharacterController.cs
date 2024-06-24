using UnityEditorInternal;
using UnityEngine;

public class CharacterController: MonoBehaviour
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
    public float gravity = 0.18f;

    Rigidbody rb;

    //[HideInInspector]
    public bool steer;

    public Controls controls;

    [HideInInspector]
    public CameraController mainCam;
    // Start is called before the first frame update
    void Start()
    {
        var settingsGO = FindObjectOfType<PlayerSettings>();
        controls = settingsGO.Settings.Controls;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (steer)
        {
            var rotation = Input.GetAxis("Mouse X") * mainCam.CameraSpeed;
            transform.eulerAngles += new Vector3(0, rotation, 0);
        }
        if (isGrounded)
        {
            currentDirection = GetDirection();
        }

        transform.Translate(currentDirection * walkSpeed * Time.deltaTime);
        rb.velocity = Vector3.zero;
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
        var dir = new Vector3();
        if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1))
        {
            dir.z = 1f;
        }

        dir.z = Axis(controls.forwards.IsPressed() || (Input.GetKey(KeyCode.Mouse0) && steer), controls.backwards.IsPressed());
        dir.x = Axis(controls.strafeRight.IsPressed(), controls.strafeLeft.IsPressed());

        return dir;

    }
}
