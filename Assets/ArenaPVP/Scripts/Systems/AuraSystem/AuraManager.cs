using FishNet.Editing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class AuraManager : MonoBehaviour
{

    private static AuraManager _instance;
    public static AuraManager Instance => _instance;

    private Dictionary<int, List<AuraInfo>> _playerAurasDict = new Dictionary<int, List<AuraInfo>>();

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        CheckAndUpdateAuraDurations();
    }

    private void CheckAndUpdateAuraDurations()
    {
        foreach (var entry in _playerAurasDict)
        {
            for (int i = 0; i < entry.Value.Count; i++)
            {
                var auraInfo = entry.Value[i];
                if (auraInfo.AppliedTime + auraInfo.Aura.Duration <= +Time.time)
                {
                    RemoveAura(entry.Key, i);
                    i--; //avoid skipping indexes when removing an item.
                }
                else
                {
                    auraInfo.ExpiresInSec = Mathf.CeilToInt(auraInfo.AppliedTime + auraInfo.Aura.Duration);
                }
            }
        }
    }

    private void RemoveAura(int playerId, int index)
    {
        var entry = _playerAurasDict[playerId];
        GameEvents.OnAuraExpired.Invoke(playerId, entry[index].AuraId);
        entry[index].Aura.Fade();
        entry.RemoveAt(index);
    }
    public void AddAura(int ownerId, int targetId, AuraBase aura)
    {
        var auraInfo = new AuraInfo(IdentifierService.GetAuraId(), ownerId, targetId, aura);

        if (_playerAurasDict.ContainsKey(targetId))
        {
            var entry = _playerAurasDict[targetId];
            var auraIndex = entry.FindIndex(a => a.Aura.Name == aura.Name && a.AppliedById == ownerId);
            //aura already applied by this owner, refresh duration
            if (auraIndex >= 0)
            {
                entry[auraIndex].AppliedTime = Time.time;
            }
            else 
            {
                entry.Add(auraInfo);
                GameEvents.OnAuraApplied.Invoke(targetId, auraInfo.AuraId);
            }
        }
        else 
        {
            _playerAurasDict.Add(targetId, new List<AuraInfo> { auraInfo });
            GameEvents.OnAuraApplied.Invoke(targetId, auraInfo.AuraId);
        }
    }

    public List<AuraInfo> GetAuraInfosForPlayer(int playerId)
    {
        if (_playerAurasDict.ContainsKey(playerId))
        {
            return _playerAurasDict[(playerId)];
        }
        return new List<AuraInfo>();
    }
    public AuraInfo GetAuraInfo(int playerId, int auraId)
    {
        if (_playerAurasDict.ContainsKey(playerId))
        {
            if (_playerAurasDict[playerId].Any(a => a.AuraId == auraId))
            {
                return _playerAurasDict[playerId].First(a => a.AuraId == auraId);
            }
            return AuraInfo.Null;
        }
        return AuraInfo.Null;
    }

    /// <summary>
    /// Returns Remaining Duration for given PlayerId and AuraId combination. Returns 0 if Aura is expired.
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="auraId"></param>
    /// <returns></returns>
    public float GetRemainingAuraDuration(int playerId, int auraId)
    {
        if (_playerAurasDict.ContainsKey(playerId))
        {
            if (!_playerAurasDict[playerId].Any(a => a.AuraId == auraId))
                return 0;
            else
            {
                int index = _playerAurasDict[playerId].FindIndex(a => a.AuraId == auraId);
                var auraInfo = _playerAurasDict[playerId][index];
                float remainingDuration = (auraInfo.AppliedTime + auraInfo.Aura.Duration) - Time.time;
                if (remainingDuration > 0)
                {
                    return remainingDuration;
                }
                else
                {
                    RemoveAura(playerId, index);
                    return 0;
                }
            }
        }
        return 0;
    }
}

[Serializable]
public class AuraInfo
{
    public AuraInfo(int id, int appliedById, int appliedToId, AuraBase aura) 
    {
        AuraId = id;
        Aura = aura;
        AppliedById = appliedById;
        AppliedTime = Time.time;
        ExpiresInSec = Mathf.CeilToInt(aura.Duration);
        AppliedToId = appliedToId;
    }
    public int AuraId;
    public AuraBase Aura;
    public float AppliedTime;
    public int AppliedById;
    public int AppliedToId;
    public int ExpiresInSec;

    public static AuraInfo Null = null;
}
