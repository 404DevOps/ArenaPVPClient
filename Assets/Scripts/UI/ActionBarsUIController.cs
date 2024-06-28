using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarsUIController : MonoBehaviour
{
    public GameObject Actionbar1;
    public GameObject Actionbar2;

    public GameObject actionSlotPrefab;

    private PlayerSettings playerSettings;
    // Start is called before the first frame update
    void Start()
    {
        playerSettings = FindObjectOfType<PlayerSettings>();
        RebuildActionBars();
    }

    public void RebuildActionBars()
    { 
        ClearBars();

        int y = 0;
        foreach (var keybind in playerSettings.Settings.Controls.Abilities)
        {
            var parentTransform = y < 5 ? Actionbar1.transform : Actionbar2.transform;
            var gO = Instantiate(actionSlotPrefab, parentTransform);
            var slot = gO.GetComponent<ActionSlot>();
            slot.Id = y;
            slot.KeyBind = keybind;
            gO.SetActive(true);
            y++;
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
}
