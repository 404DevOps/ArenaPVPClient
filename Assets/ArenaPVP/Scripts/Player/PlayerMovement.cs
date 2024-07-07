using System;
using UnityEditorInternal;
using Logger = Assets.Scripts.Helpers.Logger;
using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    #region MovementVariables

    [SerializeField]
    private float _walkSpeed = 2f;
    [SerializeField]
    private float _rotationSpeed = 0.1f;

    #endregion
    #region JumpVariables
    [SerializeField] bool _isGrounded => _characterController.isGrounded;
    [SerializeField] private float _jumpForce = 15;
    [SerializeField] private float _velocityY;
    [SerializeField] private float _gravityModifier = 5;
    #endregion
    #region References

    private Player _player;
    private float _rotation;
    private Vector3 _currentDirection;
    private CharacterController _characterController;
    private Controls _controls;
    private PlayerConfiguration _settings;
    [HideInInspector] public CameraController mainCam;

    #endregion

    //set by camera
    public bool steer;

    // Start is called before the first frame update
    void Awake()
    {
        _player = GetComponent<Player>();
        if (!_player.IsOwnedByMe)
            this.enabled = false;

        _characterController = GetComponent<CharacterController>();
        _settings = FindObjectOfType<PlayerConfiguration>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_player.IsOwnedByMe)
            return;

        HandleCameraSteer();
       
        if (_isGrounded)
            _currentDirection = GetDirection();

        ApplyGravity();
        HandleJump();
    }

    private void HandleCameraSteer()
    {
        if (steer)
        {
            _rotation = Input.GetAxis("Mouse X") * mainCam.CameraSpeed * 100;
            transform.eulerAngles += new Vector3(0, _rotation, 0) * Time.deltaTime;
        }
    }

    private void ApplyGravity()
    {
        if (!_isGrounded)
        {
            _velocityY += Physics.gravity.y * _gravityModifier * Time.deltaTime;
        }
        else if (_isGrounded && _velocityY < 0) //dont reset if jump has started
        {
            _velocityY = -1;
        }
    }

    private void FixedUpdate()
    {
      _characterController.Move(new Vector3(_currentDirection.x * _walkSpeed, _velocityY, _currentDirection.z * _walkSpeed) * Time.deltaTime);  
    }

    private void HandleJump()
    {
        if (!_isGrounded)
            return;

        if (_controls.jump.IsKeyDown())
        {
            Logger.Log("Jump Pressed");
            _velocityY += _jumpForce;
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

        if (_controls.forwards.IsPressed() || Input.GetKey(KeyCode.Mouse0) && steer)
            direction += transform.forward;
        if (_controls.backwards.IsPressed())
            direction += -transform.forward;
        if (_controls.strafeLeft.IsPressed())
            direction += -transform.right;
        if (_controls.strafeRight.IsPressed())
            direction += transform.right;

        return direction;
    }

    public void ReloadControlSettings() 
    {
        _controls = _settings.Settings.Controls;
    }

    private void OnEnable()
    {
        UIEvents.OnSettingsLoaded.AddListener(ReloadControlSettings);
        UIEvents.OnControlsChanged.AddListener(ReloadControlSettings);
    }
    private void OnDisable()
    {
        UIEvents.OnSettingsLoaded.RemoveListener(ReloadControlSettings);
        UIEvents.OnControlsChanged.RemoveListener(ReloadControlSettings);
    }
}
