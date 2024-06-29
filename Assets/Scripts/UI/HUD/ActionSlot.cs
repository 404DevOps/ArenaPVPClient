using Assets.Scripts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ActionSlot : MonoBehaviour, IDropHandler
{
    public int Id;
    public Image Icon;
    public Image Border;
    public TextMeshProUGUI KeyBindText;
    public CharacterAbility Ability;
    public KeyBind KeyBind;
    public PlayerSettings playerSettings;
    private Color DefaultBorderColor;

    private float FlashTime = 0.05f;
    private float TimePassed = 0f;
    private bool timerStarted = false;


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped: " + eventData.pointerDrag.name);
        var skillDragHandler = eventData.pointerDrag.GetComponent<AbilitylDragHandler>();
        var droppedAbility = eventData.pointerDrag.GetComponent<AbilityUIDisplay>();

        var display = GetComponent<AbilityUIDisplay>();
        display.Ability = droppedAbility.Ability;
        Icon.sprite = display.Ability.AbilityInfo.Icon;
        Icon.color = Color.white; //set alpha

        SetBarLock();

        Destroy(skillDragHandler.duplicate);
    }

    private void Update()
    {
        FlashActionSlot();
    }

    public void SetBarLock() 
    {
        //if no ability, no dragging, if has ability, set according to bar lock.
        GetComponent<AbilitylDragHandler>().enabled = Ability != null ? !playerSettings.Settings.LockActionBars : false;
    }

    private void FlashActionSlot()
    {
        if (KeyBind.IsPressed())
        {
            Debug.Log($"Actionslot {Id} pressed");
            Border.color = Color.yellow;
            TimePassed = 0;
            timerStarted = true;
        }

        if (timerStarted)
        {
            TimePassed += Time.deltaTime;
            if (TimePassed >= FlashTime)
            {
                Border.color = DefaultBorderColor;
                timerStarted = false;
            }
        }
    }

    private void Awake()
    {
        playerSettings = FindObjectOfType<PlayerSettings>();
        SetBarLock();

        KeyBindText.text = HelperMethods.GetKeyBindNameShort(KeyBind.primary);
        DefaultBorderColor = Border.color;
    }
}
