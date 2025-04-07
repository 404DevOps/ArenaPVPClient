using FishNet.Managing.Statistic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Helpers
{
    private static Camera _camera;
    public static Camera Camera
    {
        get
        {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }
    public static void DeleteChildren(this Transform t)
    {
        while (t.childCount > 0) Object.DestroyImmediate(t.GetChild(0).gameObject);
    }

    private static Entity _localPlayer;
    public static Entity LocalPlayer => _localPlayer;

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        ClientEvents.OnEntityInitialized.AddListener(OnEntityInitialized);
    }

    private static void OnEntityInitialized(Entity entity)
    {
        if (entity.IsOwnedByMe) // Or however you identify the local player
        {
            _localPlayer = entity;
        }
    }
}

public static class WaitManager
{
    private static readonly Dictionary<float, Stack<WaitForSeconds>> _pool = new();
    private static WaitForSeconds GetWait(float time)
    {
        if (_pool.TryGetValue(time, out var stack) && stack.Count > 0)
        {
            return stack.Pop(); // Reuse if available
        }
        return new WaitForSeconds(time); // Otherwise, create a new one
    }
    private static void ReturnWait(float time, WaitForSeconds wait)
    {
        if (!_pool.ContainsKey(time))
        {
            _pool[time] = new Stack<WaitForSeconds>();
        }
        _pool[time].Push(wait); // Store back for reuse
    }
    public static IEnumerator Wait(float time)
    {
        var wait = GetWait(time);
        yield return wait;
        ReturnWait(time, wait); // Auto return after wait is done
    }



    private static WaitForFixedUpdate _waitForFixed;
    public static WaitForFixedUpdate WaitForFixed()
    {
        if (_waitForFixed == null)
            _waitForFixed = new WaitForFixedUpdate();

        return _waitForFixed;
    }
}