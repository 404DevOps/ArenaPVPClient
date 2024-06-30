using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField]
    public Targetable currentTarget;
    public List<Targetable> targetClosedList;
    public Targetable previousTarget;
    public float searchRadius = 30f;

    private Controls controls;
    private Camera mainCam;
    private CameraController camController;
    private PlayerSettings playerSettings;
    private bool isMenuOpen;
    private bool isClickTargetLocked;

    // Start is called before the first frame update
    void Start()
    {
       
        mainCam = Camera.main;
        camController = mainCam.GetComponentInParent<CameraController>();
    }
    public void ReloadControls()
    {
        playerSettings = FindObjectOfType<PlayerSettings>();
        controls = playerSettings.Settings.Controls;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMenuOpen)
            return;

        //targetEnemy with tab
        if (controls.targetNext.IsKeyDown())
        {
            //target nearest to 
            var nextTarget = GetNextTabTarget();
            DoSelectTarget(nextTarget, false);
        }
        //targetSelf
        if (controls.targetSelf.IsKeyDown())
        {
            var self = FindObjectsOfType<Targetable>().Where(t => t.isSelf).First();
            DoSelectTarget(self, true);
        }
        //target with leftMouse
        if (!isClickTargetLocked) {
            if (Input.GetKeyUp(KeyCode.Mouse0) && MouseNotUsedByCam())
            {
                Debug.Log("GetTarget");
                GetTargetUnderMouse();
            } 
        }
        //removeTarget
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentTarget != null) 
            {
                currentTarget.Unselect();
                currentTarget = null;
                targetClosedList.Clear();
            }
        }
    }

    private void GetTargetUnderMouse()
    {
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            var tar = hitInfo.collider.gameObject.GetComponent<Targetable>();
            if (tar != null)
            {
                DoSelectTarget(tar, true);
            }
        }
    }

    private bool MouseNotUsedByCam()
    {
        return
            !camController.isDragging &&
            (camController.State == CameraState.None || camController.State == CameraState.Rotate) &&
            (camController.previousState != CameraState.Run || camController.previousState == CameraState.Steer);
    }

    private void DoSelectTarget(Targetable target, bool clearClosedList)
    {
        if (clearClosedList)
        {
            targetClosedList.Clear();
        }
        if (target != null)
        {
            previousTarget = currentTarget;
            targetClosedList.Add(target);
            currentTarget = target;
            target.Select();

        }
        if (currentTarget != previousTarget && previousTarget != null)
        {
            previousTarget.Unselect();
        }
    }

    private Targetable GetNextTabTarget()
    {
        var possibleTargets = FindObjectsByType<Targetable>(FindObjectsSortMode.None).Where(t => IsInFront(t, TargetInFrontMode.Camera) && !t.isSelf).ToList();

        if (targetClosedList.Count >= possibleTargets.Count)
            targetClosedList.Clear();

        var untappedTargets = possibleTargets.Where(t => !targetClosedList.Contains(t)).ToList();
        if (!untappedTargets.Any())
        {
            targetClosedList.Clear();
        }
        else 
        {
            possibleTargets = untappedTargets;
        }

        if (possibleTargets.Any())
        {
            var closest = GetClosestTarget(possibleTargets);
            if (closest != null)
            {
                return closest;
            }
            else { 
            }
        }

        Debug.Log("None found, default Target returned.");
        targetClosedList.Clear();
        return null;
    }

    private bool IsInFront(Targetable target, TargetInFrontMode mode )
    {
        Transform transform;
        switch (mode)
        {
            case TargetInFrontMode.Camera:
                transform = mainCam.transform;
                break;
            case TargetInFrontMode.Player:
                transform = this.transform;
                break;
            default:
                throw new Exception("TargetingSystem.IsInFront() needs InFrontMode Parameter");
        }

        var directionToTarget = transform.position - target.transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        float distance = directionToTarget.magnitude;
        return Mathf.Abs(angle) > 90 && distance < searchRadius;
    }

    private Targetable GetClosestTarget(List<Targetable> possibleTargets)
    {
        var orderedTargets = possibleTargets.OrderBy(t => GetDistance(t.transform));
        var targetsInFrontOfPlayer = orderedTargets.Where(t => IsInFront(t, TargetInFrontMode.Player));
        if(targetsInFrontOfPlayer.Any())
            return targetsInFrontOfPlayer.First();
        else if(orderedTargets.Any())
            return orderedTargets.First();
        else
            return null;
    }

    private float GetDistance(Transform transform)
    { 
        var screenPointOfTransform = mainCam.WorldToViewportPoint(transform.position);
        screenPointOfTransform.z = 0; //remove distance from camera as parameter
        return Vector3.Distance(screenPointOfTransform, new Vector3(0.5f, 0.5f, 0));
    }

    private void OnEnable()
    {
        GameEvents.onSettingsLoaded.AddListener(ReloadControls);
        UIEvents.onAbilityDrag.AddListener(SetTargetLock);
        UIEvents.onMainMenuOpen.AddListener(SetMenuOpen);
    }
    private void OnDisable()
    {
        GameEvents.onSettingsLoaded.RemoveListener(ReloadControls);
        UIEvents.onAbilityDrag.RemoveListener(SetTargetLock);
        UIEvents.onMainMenuOpen.AddListener(SetMenuOpen);
    }
    private void SetTargetLock(bool isLock)
    {
        isClickTargetLocked = isLock;
    }
    private void SetMenuOpen(bool isOpen)
    {
        isMenuOpen = isOpen;  
    }
}