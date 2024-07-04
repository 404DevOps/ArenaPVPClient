using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CastManager : MonoBehaviour
{
    private static CastManager _instance;
    public static CastManager Instance => _instance;

    [SerializeField] private Dictionary<int, AbilityCastInfo> castStartedDict = new Dictionary<int, AbilityCastInfo>();
    // Start is called before the first frame update
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
        if (castStartedDict.ContainsKey(owner))
        {
            castStartedDict[owner] = castInfo;
        }
        else 
        {
            castStartedDict.Add(owner, castInfo);
        }
    }
    public AbilityCastInfo? GetCastInfo(int owner)
    {
        if (castStartedDict.ContainsKey(owner))
        {
            return castStartedDict[owner];
        }
        return null;
    }
    public bool Contains(int owner, string abilityName)
    {
        if (castStartedDict.ContainsKey(owner))
        {
            return castStartedDict[owner].AbilityName == abilityName;
        }
        return false;
    }
    public float TimeSinceCastStarted(int owner, string abilityname)
    {
        if (castStartedDict.ContainsKey(owner))
        {
            if (castStartedDict[owner].AbilityName == abilityname)
                return Time.time - castStartedDict[owner].CastStarted;
        }
        return -1f;
    }
    public void Remove(int owner)
    {
        if (castStartedDict.ContainsKey(owner))
        {
            castStartedDict.Remove(owner);
        }
    }

    public void StartCastCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
