using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

public class AuraContainerUIHandler : MonoBehaviour
{
    public int OwnerId;
    [SerializeField] private Transform AuraGrid;

    public GameObject AuraDisplayPrefab;

    private void OnEnable()
    {
        GameEvents.OnAuraApplied.AddListener(OnAuraApplied);
        GameEvents.OnAuraExpired.AddListener(OnAuraExpired);
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
        GameEvents.OnAuraApplied.RemoveListener(OnAuraApplied);
        GameEvents.OnAuraExpired.AddListener(OnAuraExpired);
    }

    private void OnAuraExpired(int ownerId, AuraInfo aura)
    {
        if (ownerId != OwnerId)
            return;

        RemoveAura(aura.AuraInstanceId);
    }

    private void OnAuraApplied(int ownerId, AuraInfo aura)
    {
        if (ownerId != OwnerId)
            return;

        Logger.Log("On Aura Applied called.");

        AddAura(aura);
    }

    private void RemoveAura(int auraId)
    {
        for (int i = 0; i < AuraGrid.childCount; i++)
        {
            var auraDisplays = AuraGrid.GetChild(i).GetComponent<AuraDisplay>();
            if (auraDisplays.AuraInfo.AuraInstanceId == auraId)
            {
                Destroy(auraDisplays.gameObject);
            }
        }
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
