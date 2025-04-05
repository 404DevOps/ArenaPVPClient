using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArenaLogger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

public class AuraContainerUIHandler : MonoBehaviour
{
    public int OwnerId;
    [SerializeField] private Transform AuraGrid;

    public GameObject AuraDisplayPrefab;

    private void OnEnable()
    {
        ClientEvents.OnAuraApplied.AddListener(OnAuraApplied);
        ClientEvents.OnAuraExpired.AddListener(OnAuraExpired);
    }

    public void InitializeGrid(int ownerId, List<AuraInfo> auras)
    {
        ResetGrid();
        OwnerId = ownerId;
        foreach (var aura in auras)
        {
            AddAura(aura);
        }
    }
    private void ResetGrid()
    {
        for (int i = 0; i < AuraGrid.childCount; i++)
        {
            Destroy(AuraGrid.GetChild(i).gameObject);
        }
    }

    private void OnDisable()
    {
        ClientEvents.OnAuraApplied.RemoveListener(OnAuraApplied);
        ClientEvents.OnAuraExpired.RemoveListener(OnAuraExpired);
    }

    private void OnAuraExpired(int ownerId, AuraInfo aura)
    {
        if (ownerId != OwnerId)
            return;

        RemoveAuraDisplay(aura.AuraInstanceId);
    }

    private void OnAuraApplied(int ownerId, AuraInfo aura)
    {
        if (ownerId != OwnerId)
            return;

        ArenaLogger.Log("On Aura Applied called.");

        AddAura(aura);
    }

    private void RemoveAuraDisplay(int auraId)
    {
        bool auraFoundAndRemoved = false;
        for (int i = 0; i < AuraGrid.childCount; i++)
        {
            var auraDisplays = AuraGrid.GetChild(i).GetComponent<AuraDisplay>();
            if (auraDisplays.AuraInfo.AuraInstanceId == auraId)
            {
                Destroy(auraDisplays.gameObject);
                auraFoundAndRemoved |= true;
            }
        }

        ArenaLogger.Log("Tried remove expired Aura: Success = " + auraFoundAndRemoved);
    }

    private void AddAura(AuraInfo auraInfo)
    {
        var gO = new GameObject();
        gO.SetActive(false);
        var auraDisplayGo = Instantiate(AuraDisplayPrefab, gO.transform);
        var display = auraDisplayGo.GetComponent<AuraDisplay>();
        display.AuraInfo = auraInfo;
        auraDisplayGo.transform.SetParent(AuraGrid);
        Destroy(gO);
    }
}
