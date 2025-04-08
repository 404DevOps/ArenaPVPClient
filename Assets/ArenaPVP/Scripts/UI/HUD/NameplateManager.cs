using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Logger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

public class NameplateManager : MonoBehaviour
{
    [SerializeField] private Transform _canvas;
    [SerializeField] private GameObject _nameplatePrefab;

    [SerializeField] private float offsetY = 50;
    [SerializeField] private float smoothMoveSpeed = 0.2f;

    Dictionary<Entity, Transform> _playerNameplates = new Dictionary<Entity, Transform>();
    private CameraController _camController;

    [SerializeField] private float _maxScale = 0.8f;
    [SerializeField] private float _minScale = 0.5f;
    [SerializeField] private float _maxDistance = 50f;
    [SerializeField] private float _currentScale = 0.8f;

    private bool _firstLoad = true;

    void Awake()
    {
        _camController = Camera.main.GetComponentInParent<CameraController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (var playerNameplate in _playerNameplates)
        {
            var playerPos = playerNameplate.Key.transform.position;
            var screenpos = Camera.main.WorldToScreenPoint(playerPos);
            StartCoroutine(SmothMoveNameplate(playerNameplate.Value.transform, new Vector3(screenpos.x, screenpos.y + offsetY)));
        }
    }

    private IEnumerator SmothMoveNameplate(Transform nameplate, Vector3 targetPos)
    {
        float newScale = 0f;
        float elapsed = 0f;
        while (elapsed < smoothMoveSpeed)
        {
            if (nameplate == null)
                yield break;

            elapsed += Time.deltaTime;
            nameplate.position = Vector3.Lerp(nameplate.position, targetPos, elapsed / smoothMoveSpeed);

            newScale = GetDistancePercentage();
            _currentScale = nameplate.transform.localScale.y;

            var lerpScale = Mathf.Lerp(_currentScale, newScale, elapsed / smoothMoveSpeed);
            nameplate.localScale = new Vector3(lerpScale, lerpScale);

            yield return null;
        }

        if (nameplate != null)
        {
            nameplate.position = targetPos;
            nameplate.localScale = new Vector3(newScale, newScale);
        }
    }
    private float GetDistancePercentage()
    {
        var distancePercentage = _camController.currentDistance / _maxDistance;
        if (distancePercentage < _minScale)
            distancePercentage = _minScale;
        if(distancePercentage > _maxScale)
            distancePercentage = _maxScale;

        return distancePercentage;
    }
    private void OnEntityInitialized(Entity player)
    {
        if (player.IsOwnedByMe && !PlayerConfiguration.Instance.Settings.ShowPlayerNameplate) 
            return;
        
        InstantiateNamePlate(player);
    }

    void InstantiateNamePlate(Entity player)
    {
        var go = new GameObject();
        go.SetActive(false);

        var nameplate = Instantiate(_nameplatePrefab, go.transform);
        var nameplateHandler = nameplate.GetComponent<NameplateUIHandler>();
        nameplateHandler.Player = player;

        var castBarHandler = nameplate.GetComponentInChildren<CastBarUIHandler>();
        castBarHandler.Player = player;

        nameplate.transform.SetParent(_canvas);
        nameplate.SetActive(true);
        Destroy(go);

        _playerNameplates[player] = nameplate.transform;
    }

    public void OnSettingsLoaded()
    {
        if (!_firstLoad)
        {
            //adjust to current settings
            if (PlayerConfiguration.Instance.Settings.ShowPlayerNameplate)
            {
                if (!_playerNameplates.Any(np => np.Key.IsOwnedByMe))
                {
                    var player = FindObjectsOfType<Entity>().First(p => p.IsOwnedByMe);
                    OnEntityInitialized(player);
                }
            }
            else
            {
                if (_playerNameplates.Any(np => np.Key.IsOwnedByMe))
                {
                    var playerNameplate = _playerNameplates.First(np => np.Key.IsOwnedByMe);
                    Destroy(playerNameplate.Value.gameObject);
                    _playerNameplates.Remove(playerNameplate.Key);
                } 
            }
        }
        _firstLoad = false;
    }

    public void OnEnable()
    {
        ClientEvents.OnEntityInitialized.AddListener(OnEntityInitialized);
        UIEvents.OnSettingsLoaded.AddListener(OnSettingsLoaded);
    }

    public void OnDisable()
    {
        ClientEvents.OnEntityInitialized.RemoveListener(OnEntityInitialized);
        UIEvents.OnSettingsLoaded.RemoveListener(OnSettingsLoaded);
    }
}
