using Assets.ArenaPVP.Scripts.Helpers;
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
    public Targetable CurrentTarget;


    private List<Targetable> targetClosedList;
    private Targetable previousTarget;
    public float searchRadius = 30f;

    private Controls controls;
    private Camera mainCam;
    private Player player;
    private CameraController camController;
    private PlayerConfiguration playerSettings;
    private bool isMenuOpen;
    private bool isClickTargetLocked;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectsByType<Player>(FindObjectsSortMode.None).First(x => x.IsOwnedByMe);
        targetClosedList = new();
        mainCam = Camera.main;
        camController = mainCam.GetComponentInParent<CameraController>();
    }
    public void ReloadControls()
    {
        playerSettings = GameObject.FindGameObjectsWithTag("PlayerSettings")?[0]?.GetComponent<PlayerConfiguration>();
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
            if (CurrentTarget != null) 
            {
                CurrentTarget.Unselect();
                CurrentTarget = null;
                targetClosedList.Clear();
            }
            UIEvents.OnTargetChanged.Invoke(null);
        }
    }

    private void GetTargetUnderMouse()
    {
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            var tar = hitInfo.collider.gameObject.GetComponentInParent<Targetable>();
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
            previousTarget = CurrentTarget;
            targetClosedList.Add(target);
            CurrentTarget = target;
            target.Select();
            var player = target.GetComponent<Player>();
            UIEvents.OnTargetChanged.Invoke(player);

        }
        if (CurrentTarget != previousTarget && previousTarget != null)
        {
            previousTarget.Unselect();
        }
    }

    private Targetable GetNextTabTarget()
    {
        var possibleTargets = FindObjectsByType<Targetable>(FindObjectsSortMode.None).Where(t => PositionHelper.IsInFront(mainCam.transform, t.transform, searchRadius) && !t.isSelf).ToList();

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

   

    private Targetable GetClosestTarget(List<Targetable> possibleTargets)
    {
        var orderedTargets = possibleTargets.OrderBy(t => GetDistance(t.transform));
        var targetsInFrontOfPlayer = orderedTargets.Where(t => PositionHelper.IsInFront(player.transform, t.transform, searchRadius));
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
        UIEvents.OnAbilityDrag.AddListener(SetTargetLock);
        UIEvents.OnMainMenuOpen.AddListener(SetMenuOpen);
    }
    private void OnDisable()
    {
        GameEvents.onSettingsLoaded.RemoveListener(ReloadControls);
        UIEvents.OnAbilityDrag.RemoveListener(SetTargetLock);
        UIEvents.OnMainMenuOpen.AddListener(SetMenuOpen);
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