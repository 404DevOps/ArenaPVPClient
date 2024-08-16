using FishNet;
using FishNet.Transporting;
using FishNet.Transporting.Tugboat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionStarter : MonoBehaviour
{
    [SerializeField]
    Tugboat _tugboat;

    private void OnEnable()
    {
        InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;
    }
    private void OnDisable()
    {
        InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;
    }

    private void OnClientConnectionState(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Stopping)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        if (TryGetComponent(out Tugboat _t))
        {
            _tugboat = _t;
        }
        else
        {
            Debug.LogError("Could not get Tugboat.");
        }


        if (ParrelSync.ClonesManager.IsClone())
        {
            _tugboat.StartConnection(false);
        }
        else
        {
            _tugboat.StartConnection(true);
        }
#endif
    }
}
