using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBarUIController : MonoBehaviour
{
    public GameObject Actionbar1;
    public GameObject Actionbar2;

    public GameObject actionSlotPrefab;

    private PlayerSettings playerSettings;
    // Start is called before the first frame update
    void Start()
    {
        playerSettings = FindObjectOfType<PlayerSettings>();

        int i = 0;
        foreach (var keybind in playerSettings.Settings.Controls.Abilities)
        {
            var parentTransform = i < 5 ? Actionbar1.transform : Actionbar2.transform;
            var gO = Instantiate(actionSlotPrefab, parentTransform);
            var slot = gO.GetComponent<ActionSlot>();
            slot.Id = i;
            slot.KeyBind = keybind;
            gO.SetActive(true);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
