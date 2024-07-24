using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        GameEvents.OnAuraExpired.Invoke(playerId, entry[index]);
        entry[index].Aura.Fade();
        entry.RemoveAt(index);
    }
    public int AddAura(Player source, Player target, AuraBase aura)
    {
        var auraInfo = new AuraInfo(IdentifierService.GetAuraId(), source, target, aura);

        if (_playerAurasDict.ContainsKey(target.Id))
        {
            var entry = _playerAurasDict[target.Id];
            var auraIndex = entry.FindIndex(a => a.Aura.Name == aura.Name && a.AppliedBy == source);
            //aura already applied by this owner, refresh duration
            if (auraIndex >= 0)
            {
                entry[auraIndex].AppliedTime = Time.time;
                return entry[auraIndex].AuraId;
            }
            else 
            {
                entry.Add(auraInfo);
                GameEvents.OnAuraApplied.Invoke(target.Id, auraInfo);
                return auraInfo.AuraId;
            }
        }
        else 
        {
            _playerAurasDict.Add(target.Id, new List<AuraInfo> { auraInfo });
            GameEvents.OnAuraApplied.Invoke(target.Id, auraInfo);
            return auraInfo.AuraId;
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
    public AuraInfo(int id, Player appliedById, Player appliedToId, AuraBase aura) 
    {
        AuraId = id;
        Aura = aura;
        AppliedBy = appliedById;
        AppliedTime = Time.time;
        ExpiresInSec = Mathf.CeilToInt(aura.Duration);
        AppliedTo = appliedToId;
    }
    public int AuraId;
    public AuraBase Aura;
    public float AppliedTime;
    public Player AppliedBy;
    public Player AppliedTo;
    public int ExpiresInSec;

    public static AuraInfo Null = null;
}
