using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ActionBarUIHandler : MonoBehaviour
{
    public GameObject Actionbar1;
    public GameObject Actionbar2;
    public GameObject ActionSlotPrefab;

    private string actionBarMappingPath;
    private ActionBarMapping actionBarMapping;
    private Entity _player;

    public void OnEnable()
    {
        //UIEvents.OnSettingsLoaded.AddListener(InitializeActionBars);
        UIEvents.OnKeyBindsChanged.AddListener(RebuildActionBars);
        ClientEvents.OnEntityInitialized.AddListener(OnPlayerInitialized);
    }

    private void OnPlayerInitialized(Entity player)
    {
        if (player.IsOwnedByMe)
        {
            _player = player;
            actionBarMappingPath = Application.persistentDataPath + "/ActionBarMapping_" + _player.ClassType.ToString() + ".json";
            InitializeActionBars();
            ClientEvents.OnEntityInitialized.RemoveListener(OnPlayerInitialized);
        }
    }

    public void OnDisable()
    {
        //UIEvents.OnSettingsLoaded.RemoveListener(RebuildActionBars);
        UIEvents.OnKeyBindsChanged.RemoveListener(RebuildActionBars);
    }

    public void InitializeActionBars() 
    {
        LoadActionBarMapping();
        RebuildActionBars();
    }
    public void RebuildActionBars()
    { 
        ClearBars();

        int i = 0;
        foreach (var keybind in PlayerConfiguration.Instance.Settings.Controls.AbilityKeybinds)
        {
            var parentTransform = i < 5 ? Actionbar1.transform : Actionbar2.transform;

            var gO = new GameObject();
            gO.SetActive(false);

            var actionSlotGo = Instantiate(ActionSlotPrefab, gO.transform);
            var slot = actionSlotGo.GetComponent<ActionSlot>();
            slot.Id = i;
            slot.KeyBind = keybind;
            slot.Ability = GetAbilityForSlot(i);
            slot.OnAbilityChanged = OnAbilityChanged; //register event
            actionSlotGo.transform.SetParent(parentTransform, false);
            slot.InitializeSlot(_player);
            actionSlotGo.SetActive(true);
            i++;
            Destroy(gO);
        }
    }
    private void ClearBars()
    {
        for (int i = 0; i < Actionbar1.transform.childCount; i++)
        {
            Destroy(Actionbar1.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < Actionbar2.transform.childCount; i++)
        {
            Destroy(Actionbar2.transform.GetChild(i).gameObject);
        }
    }
    private AbilityBase GetAbilityForSlot(int slot)
    {
        //TODO: add default mapping per class.
        if (actionBarMapping != null)
            return actionBarMapping.GetSlot(slot);
        else
            return null;
    }
    private void OnAbilityChanged(int slot, AbilityBase ability)
    {
        Debug.Log("ActionBarsUIHandler:OnAbilityChanged called.");
        actionBarMapping.AddOrUpdateSlot(slot, ability);
        SaveActionBarMapping();
    }

    #region ActionBar Load/Save

    private void LoadActionBarMapping()
    {
        if (!File.Exists(actionBarMappingPath))
        {
            LoadDefaultMapping();
            SaveActionBarMapping();
        }
        else
        {

            var json = File.ReadAllText(actionBarMappingPath);
            var mapping = JsonUtility.FromJson<ActionBarMapping>(json);
            actionBarMapping = mapping;
        }
    }

    private void SaveActionBarMapping()
    {
        var str = JsonUtility.ToJson(actionBarMapping);
        File.WriteAllText(actionBarMappingPath, str);
    }

    private void LoadDefaultMapping()
    {
        actionBarMapping = new ActionBarMapping();
        int i = 0;
        foreach (var ability in AbilityStorage.AllAbilities.Where(a => a.AbilityInfo.ClassType == _player.ClassType))
        {
            actionBarMapping.AddOrUpdateSlot(i, ability);
        }
    }

    #endregion
}
