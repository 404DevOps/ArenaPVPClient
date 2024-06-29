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
    private Color DefaultIconColor;

    private float FlashTime = 0.05f;
    private float TimePassed = 0f;
    private bool timerStarted = false;

    private AbilityUIDisplay abilityDisplay;

    public Action<int, CharacterAbility> OnAbilityChanged;

    private void Update()
    {
        FlashActionSlot();
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<ActionSlot>()?.Id == Id)
            return;

        var dragHandler = eventData.pointerDrag.GetComponent<AbilityUIDragHandler>();
        var droppedAbility = eventData.pointerDrag.GetComponent<AbilityUIDisplay>();

        Ability = droppedAbility.Ability;
        abilityDisplay.Ability = Ability;
        SetIcon();
        SetBarLock();
        OnAbilityChanged.Invoke(Id, Ability);

        //If dragged item is ActionSlot, move ability instead of copy
        if (eventData.pointerDrag.GetComponent<ActionSlot>() != null)
        {
            eventData.pointerDrag.GetComponent<ActionSlot>().ResetSlot();
        }

        Destroy(dragHandler.duplicate);
        
    }

    private void ResetSlot()
    {
        Ability = null;
        SetIcon();
        SetBarLock();
        OnAbilityChanged.Invoke(Id, null);
    }
    private void SetIcon()
    {
        if (Ability == null)
        {
            Icon.sprite = null;
            Icon.color = DefaultIconColor;
        }
        else 
        {
            Icon.sprite = Ability.AbilityInfo.Icon;
            Icon.color = Color.white; //set alpha
        }
    }

    public void SetBarLock() 
    {
        //if no ability, no dragging, if has ability, set according to bar lock.
        GetComponent<AbilityUIDragHandler>().enabled = Ability != null ? !playerSettings.Settings.LockActionBars : false;
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
        abilityDisplay = GetComponent<AbilityUIDisplay>();
        if (Ability != null)
            abilityDisplay.Ability = Ability;
        KeyBindText.text = HelperMethods.GetKeyBindNameShort(KeyBind.primary);
        DefaultBorderColor = Border.color;
        DefaultBorderColor = Icon.color;
        SetBarLock();
        SetIcon();
        
    }
}
