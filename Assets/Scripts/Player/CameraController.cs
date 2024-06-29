using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
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

    public float camPanSpeed = 30;
    public CameraState State = CameraState.None;
    public CameraState previousState = CameraState.None;
    public float currentPan, currentTilt = 10, currentDistance = 7;


    float dragThreshold = 10f;
    [HideInInspector]
    public bool isDragging = false;
    Vector3 startDragPosition;

    //references
    [SerializeField] private Transform playerTransform;
    private CharacterController player;
    public Transform Tilt;
    Camera mainCam;

    private MenuHandlerUIScript menuHandler;


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        menuHandler = FindObjectOfType<MenuHandlerUIScript>();
        player = playerTransform.GetComponent<CharacterController>();
        player.mainCam = this;


        //initialize pos
        transform.position = playerTransform.position + Vector3.up * CameraHeight;
        transform.rotation = playerTransform.rotation;

        Tilt.eulerAngles = new Vector3(currentTilt, transform.eulerAngles.y, transform.eulerAngles.z);

        mainCam.transform.position += Tilt.forward * -currentDistance;

    }

    public void Update()
    {
        SetCamState();
        if (menuHandler.isMenuOpen && State == CameraState.Rotate)
            return;

        CameraRotation();
    }
    public void LateUpdate()
    {
        CameraTransform();
    }
    public void SetCamState()
    {
        previousState = State;
        var newState = CameraState.None;

        if (Input.GetKey(leftMouse) && Input.GetKey(rightMouse))
            newState = CameraState.Run;
        else if (Input.GetKey(leftMouse))
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
                    startDragPosition = Input.mousePosition;
                }
                if (Vector3.Distance(Input.mousePosition, startDragPosition) > dragThreshold)
                { 
                    isDragging = true;
                }

                currentPan += Input.GetAxis("Mouse X") * CameraSpeed;
                player.steer = false;
            }
            else
            {
                player.steer = true;
            } 

            currentTilt -= Input.GetAxis("Mouse Y") * CameraSpeed;
            currentTilt = Mathf.Clamp(currentTilt, -CameraMaxTilt, CameraMaxTilt);
        }
        else  //when its none
        {
            isDragging = false;
            player.steer = false;
        }


        currentDistance -= Input.GetAxis("Mouse ScrollWheel") * 2;
        currentDistance = Mathf.Clamp(currentDistance, 0, CameraMaxDistance);
    }
    private void CameraTransform()
    {
        switch (State)
        {
            case CameraState.None:
                var signedAngle = Vector3.SignedAngle(player.transform.forward, transform.forward, Vector3.up);
                if (Mathf.Round(signedAngle) > 0)
                    currentPan -= camPanSpeed * Mathf.Abs(signedAngle) * Time.deltaTime;
                if (Mathf.Round(signedAngle) < 0)
                    currentPan += camPanSpeed * Mathf.Abs(signedAngle) * Time.deltaTime;
                Cursor.visible = true;
                break;
            case CameraState.Steer:
            case CameraState.Run:
                Cursor.visible = false;
                if (previousState == CameraState.Rotate && State == CameraState.Run)
                { 
                    playerTransform.transform.eulerAngles = new Vector3(playerTransform.eulerAngles.x, currentPan, playerTransform.eulerAngles.z);
                }
                currentPan = playerTransform.eulerAngles.y;
                break;
            case CameraState.Rotate:
                Cursor.visible = false;
                break;
            default:
                break;
        }

        transform.position = playerTransform.position + Vector3.up * CameraHeight;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentPan, 0);
        Tilt.eulerAngles = new Vector3(currentTilt, transform.eulerAngles.y, 0);

        mainCam.transform.position = transform.position + Tilt.forward * -currentDistance;
    }

    
}
