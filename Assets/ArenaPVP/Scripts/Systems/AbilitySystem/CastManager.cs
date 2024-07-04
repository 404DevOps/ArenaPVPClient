using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CastManager : MonoBehaviour
{
    private static CastManager _instance;
    public static CastManager Instance => _instance;

    private Dictionary<string, AbilityCastInfo> castStartedDict = new Dictionary<string, AbilityCastInfo>();
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    public void AddOrUpdate(string owner, AbilityCastInfo castInfo)
    {
        if (castStartedDict.ContainsKey(owner))
        {
            castStartedDict[owner] = castInfo;
        }
        else 
        {
            castStartedDict.Add(owner, castInfo);
        }
    }
    public AbilityCastInfo? GetCastInfo(string owner)
    {
        if (castStartedDict.ContainsKey(owner))
        {
            return castStartedDict[owner];
        }
        return null;
    }
    public float TimeSinceCastStarted(string owner, string abilityname)
    {
        var result = 0f;
        if (castStartedDict.ContainsKey(owner))
        {
            if (castStartedDict[owner].AbilityName == abilityname)
                return castStartedDict[owner].CastStarted;
        }
        return -1f;
    }
    public void Remove(string owner)
    {
        if (castStartedDict.ContainsKey(owner))
        {
            castStartedDict.Remove(owner);
        }
    }
}
