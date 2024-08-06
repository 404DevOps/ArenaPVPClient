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
    public float searchRadius = 30f;


    private List<Targetable> _targetClosedList;
    private Targetable _previousTarget;

    private Controls _controls;
    private Camera _mainCam;
    private Player _player;
    private CameraController _camController;
    private PlayerConfiguration _playerSettings;
    private bool _isMenuOpen;
    private bool _isClickTargetLocked;

    // Start is called before the first frame update
    void OnPlayerInitialized(Player player)
    {
        if (player.IsOwnedByMe)
        {
            _player = player;
            _targetClosedList = new();
            _mainCam = Camera.main;
            _camController = _mainCam.GetComponentInParent<CameraController>();
        }
    }
    public void ReloadControls()
    {
        _playerSettings = PlayerConfiguration.Instance;
        _controls = _playerSettings.Settings.Controls;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null)
            return;
        if (_isMenuOpen)
            return;


        //targetEnemy with tab
        if (_controls.targetNext.IsKeyDown())
        {
            //target nearest to 
            var nextTarget = GetNextTabTarget();
            DoSelectTarget(nextTarget, false);
        }
        //targetSelf
        if (_controls.targetSelf.IsKeyDown())
        {
            var self = FindObjectsOfType<Targetable>().Where(t => t.IsSelf).First();
            DoSelectTarget(self, true);
        }
        //target with leftMouse
        if (!_isClickTargetLocked) {
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
                _targetClosedList.Clear();
            }
            UIEvents.OnTargetChanged.Invoke(null);
        }
    }

    private void GetTargetUnderMouse()
    {
        var ray = _mainCam.ScreenPointToRay(Input.mousePosition);
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
            !_camController.isDragging &&
            (_camController.State == CameraState.None || _camController.State == CameraState.Rotate) &&
            (_camController.previousState != CameraState.Run || _camController.previousState == CameraState.Steer);
    }

    private void DoSelectTarget(Targetable target, bool clearClosedList)
    {
        if (clearClosedList)
        {
            _targetClosedList.Clear();
        }
        if (target != null)
        {
            _previousTarget = CurrentTarget;
            _targetClosedList.Add(target);
            CurrentTarget = target;
            target.Select();
            var player = target.GetComponent<Player>();
            UIEvents.OnTargetChanged.Invoke(player);

        }
        if (CurrentTarget != _previousTarget && _previousTarget != null)
        {
            _previousTarget.Unselect();
        }
    }

    private Targetable GetNextTabTarget()
    {
        var possibleTargets = FindObjectsByType<Targetable>(FindObjectsSortMode.None).Where(t => PositionHelper.IsInFront(_mainCam.transform, t.transform, searchRadius) && !t.IsSelf).ToList();

        if (_targetClosedList.Count >= possibleTargets.Count)
            _targetClosedList.Clear();

        var untappedTargets = possibleTargets.Where(t => !_targetClosedList.Contains(t)).ToList();
        if (!untappedTargets.Any())
        {
            _targetClosedList.Clear();
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
        _targetClosedList.Clear();
        return null;
    }

    private Targetable GetClosestTarget(List<Targetable> possibleTargets)
    {
        var orderedTargets = possibleTargets.OrderBy(t => GetDistance(t.transform));
        var targetsInFrontOfPlayer = orderedTargets.Where(t => PositionHelper.IsInFront(_player.transform, t.transform, searchRadius));
        if(targetsInFrontOfPlayer.Any())
            return targetsInFrontOfPlayer.First();
        else if(orderedTargets.Any())
            return orderedTargets.First();
        else
            return null;
    }

    private float GetDistance(Transform transform)
    { 
        var screenPointOfTransform = _mainCam.WorldToViewportPoint(transform.position);
        screenPointOfTransform.z = 0; //remove distance from camera as parameter
        return Vector3.Distance(screenPointOfTransform, new Vector3(0.5f, 0.5f, 0));
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerInitialized.AddListener(OnPlayerInitialized);
        UIEvents.OnSettingsLoaded.AddListener(ReloadControls);
        UIEvents.OnAbilityDrag.AddListener(SetTargetLock);
        UIEvents.OnMainMenuOpen.AddListener(SetMenuOpen);
    }
    private void OnDisable()
    {
        UIEvents.OnSettingsLoaded.RemoveListener(ReloadControls);
        UIEvents.OnAbilityDrag.RemoveListener(SetTargetLock);
        UIEvents.OnMainMenuOpen.AddListener(SetMenuOpen);
    }
    private void SetTargetLock(bool isLock)
    {
        _isClickTargetLocked = isLock;
    }
    private void SetMenuOpen(bool isOpen)
    {
        _isMenuOpen = isOpen;  
    }
}