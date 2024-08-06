using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    //inputs
    KeyCode leftMouse = KeyCode.Mouse0, rightMouse = KeyCode.Mouse1;

    //camera variables
    [SerializeField] public float CameraHeight = 1.75f, CameraMaxDistance = 25;
    public float CameraMaxTilt = 90;
    public float CameraMaxSpeed;

    [Range(0, 4)]
    public float CameraSpeed = 2;

    public float camAutoPanSpeed = 30;
    public CameraState State = CameraState.None;
    public CameraState previousState = CameraState.None;
    public float currentPan, currentTilt = 10, currentDistance = 7;


    float _dragThreshold = 10f;
    [HideInInspector]
    public bool isDragging = false;
    Vector3 _startDragPosition;

    //references
    [SerializeField] private Transform _playerTransform;
    private PlayerMovement _player;
    public Transform Tilt;
    Camera mainCam;

    private bool _isCamLocked;

    // Start is called before the first frame update


    void OnPlayerInitialized(Player player)
    {
        if (player.IsOwnedByMe)
        {
            mainCam = Camera.main;
            _playerTransform = player.transform;
            _player = player.GetComponent<PlayerMovement>();
            _player.mainCam = this;

            transform.position = _playerTransform.position + Vector3.up * CameraHeight;
            transform.rotation = _playerTransform.rotation;

            Tilt.eulerAngles = new Vector3(currentTilt, transform.eulerAngles.y, transform.eulerAngles.z);
            mainCam.transform.position += Tilt.forward * -currentDistance;

            GameEvents.OnPlayerInitialized.RemoveListener(OnPlayerInitialized);
        }
    }

    public void Update()
    {
        if (_player == null)
            return;

        SetCamState();
        CameraRotation();

    }
    public void LateUpdate()
    {
        if (_player == null)
            return;

        CameraTransform();
    }
    public void SetCamState()
    {
        previousState = State;
        var newState = CameraState.None;

        if (Input.GetKey(leftMouse) && Input.GetKey(rightMouse))
            newState = CameraState.Run;
        else if (Input.GetKey(leftMouse) && !_isCamLocked)
            newState = CameraState.Rotate;
        else if (Input.GetKey(rightMouse))
            newState = CameraState.Steer;
        
        State = newState;
    }
    void CameraRotation()
    {
        if (State != CameraState.None)
        {
            if (State == CameraState.Rotate)
            {
                if (previousState == CameraState.None)
                {
                    _startDragPosition = Input.mousePosition;
                }
                if (Vector3.Distance(Input.mousePosition, _startDragPosition) > _dragThreshold)
                { 
                    isDragging = true;
                }

                currentPan += Input.GetAxis("Mouse X") * CameraSpeed;
                _player.steer = false;
            }
            else
            {
                _player.steer = true;
            } 

            currentTilt -= Input.GetAxis("Mouse Y") * CameraSpeed;
            currentTilt = Mathf.Clamp(currentTilt, -CameraMaxTilt, CameraMaxTilt);
        }
        else  //when its none
        {
            isDragging = false;
            _player.steer = false;
        }

        if (_isCamLocked)
            return;

        currentDistance -= Input.GetAxis("Mouse ScrollWheel") * 2;
        currentDistance = Mathf.Clamp(currentDistance, 0, CameraMaxDistance);
    }
    private void CameraTransform()
    {
        switch (State)
        {
            case CameraState.None: //rotate back when state is none
                var signedAngle = Vector3.SignedAngle(_player.transform.forward, transform.forward, Vector3.up);
                if (Mathf.Round(signedAngle) > 0)
                    currentPan -= camAutoPanSpeed * Mathf.Abs(signedAngle) * Time.deltaTime;
                if (Mathf.Round(signedAngle) < 0)
                    currentPan += camAutoPanSpeed * Mathf.Abs(signedAngle) * Time.deltaTime;
                Cursor.visible = true;
                break;
            case CameraState.Steer:
            case CameraState.Run:
                Cursor.visible = false;
                //rotate when start running.
                if (previousState == CameraState.Rotate && State == CameraState.Run)
                { 
                    _playerTransform.transform.eulerAngles = new Vector3(_playerTransform.eulerAngles.x, currentPan, _playerTransform.eulerAngles.z);
                }
                currentPan = _playerTransform.eulerAngles.y;
                break;
            case CameraState.Rotate:
                Cursor.visible = false;
                break;
            default:
                break;
        }

        transform.position = _playerTransform.position + Vector3.up * CameraHeight;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentPan, 0);
        Tilt.eulerAngles = new Vector3(currentTilt, transform.eulerAngles.y, 0);
        mainCam.transform.position = transform.position + Tilt.forward * -currentDistance;
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerInitialized.AddListener(OnPlayerInitialized);
        UIEvents.OnAbilityDrag.AddListener(SetCamLock);
        UIEvents.OnMainMenuOpen.AddListener(SetCamLock);
    }
    private void OnDisable()
    {
        UIEvents.OnAbilityDrag.RemoveListener(SetCamLock);
        UIEvents.OnMainMenuOpen.RemoveListener(SetCamLock);
    }

    void SetCamLock(bool isLock)
    {
        _isCamLocked = isLock;
    }
}
