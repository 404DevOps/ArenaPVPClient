using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CastManager : MonoBehaviour
{
    private static CastManager _instance;
    public static CastManager Instance => _instance;

    private Dictionary<int, AbilityCastInfo> _castStartedDict = new Dictionary<int, AbilityCastInfo>();

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    public void AddOrUpdate(int owner, string abilityName)
    {
        var castInfo = new AbilityCastInfo(abilityName);
        if (_castStartedDict.ContainsKey(owner))
        {
            _castStartedDict[owner] = castInfo;
        }
        else 
        {
            _castStartedDict.Add(owner, castInfo);
        }
    }
    public AbilityCastInfo? GetCastInfo(int owner)
    {
        if (_castStartedDict.ContainsKey(owner))
        {
            return _castStartedDict[owner];
        }
        return null;
    }
    public bool Contains(int owner, string abilityName)
    {
        if (_castStartedDict.ContainsKey(owner))
        {
            return _castStartedDict[owner].AbilityName == abilityName;
        }
        return false;
    }
    public float TimeSinceCastStarted(int owner, string abilityname)
    {
        if (_castStartedDict.ContainsKey(owner))
        {
            if (_castStartedDict[owner].AbilityName == abilityname)
                return Time.time - _castStartedDict[owner].CastStarted;
        }
        return -1f;
    }
    public void Remove(int owner)
    {
        if (_castStartedDict.ContainsKey(owner))
        {
            _castStartedDict.Remove(owner);
        }
    }

    public void StartCastCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
