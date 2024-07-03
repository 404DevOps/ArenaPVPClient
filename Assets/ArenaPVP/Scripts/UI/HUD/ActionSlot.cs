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
using Logger = Assets.Scripts.Helpers.Logger;

public class ActionSlot : MonoBehaviour, IDropHandler
{
    public int Id;
    public Image Icon;
    public Image Border;
    public Image Swipe;
    public TextMeshProUGUI CooldownText;
    public TextMeshProUGUI KeyBindText;
    public AbilityBase Ability;
    public KeyBind KeyBind;
    public PlayerConfiguration playerSettings;
    private Color DefaultBorderColor;
    private Color DefaultIconColor;

    private float FlashTime = 0.05f;
    private float TimePassed = 0f;
    private bool timerStarted = false;

    private AbilityUIDisplay abilityDisplay;

    public Action<int, AbilityBase> OnAbilityChanged;

    private void Update()
    {
        var target = FindObjectOfType<TargetingSystem>().CurrentTarget;
        var player = FindObjectOfType<Player>().transform;

        if (KeyBind.IsPressed())
        {

            FlashActionSlot();
            if (Ability != null)
            {
                if (target != null)
                    Ability.TryUseAbility(player, target.transform);
                else
                    Logger.Log("No Target selected.");
            }
        }
        ResetActionSlotFlash();
        ShowCooldown(player.GetInstanceID());
    }

    private void ShowCooldown(int ownerId)
    {
        if (Ability == null)
        {
            Swipe.gameObject.SetActive(false);
            CooldownText.gameObject.SetActive(false);
            return;
        }

            Swipe.gameObject.SetActive(true);
            CooldownText.gameObject.SetActive(true);
            var identifier = new AbilityWithOwner(ownerId, Ability.AbilityInfo.Name);
            var timeSinceLastUse = CooldownManager.Instance.TimeSinceLastUse(identifier);
            var remainingCooldown = timeSinceLastUse - Ability.AbilityInfo.Cooldown;
            if (CooldownManager.Instance.Contains(identifier) && remainingCooldown < 0)
            {
                var absCd = Mathf.Abs(remainingCooldown);
                CooldownText.text = (Mathf.Ceil(absCd)).ToString();
                var cdPercentage = (absCd / Ability.AbilityInfo.Cooldown);
                Logger.Log("CD Percentage: " + cdPercentage);
                Swipe.fillAmount = cdPercentage;
            }
            else
            {
                Swipe.fillAmount = 0;
                CooldownText.gameObject.SetActive(false);
            }
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragHandler = eventData.pointerDrag.GetComponent<AbilityUIDragHandler>();
        var droppedAbility = eventData.pointerDrag.GetComponent<AbilityUIDisplay>();

        Ability = droppedAbility.Ability;
        abilityDisplay.Ability = Ability;
        SetIcon();
        SetBarLock();
        OnAbilityChanged.Invoke(Id, Ability);

        //If dragged item is ActionSlot, move ability instead of copy
        var slot = eventData.pointerDrag.GetComponent<ActionSlot>();
        if (slot != null)
        {
            if (slot.Id != Id)
                slot.ResetSlot();

            Destroy(dragHandler.Duplicate);
        }
    }
    public void ResetSlot()
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
            Border.color = Color.yellow;
            TimePassed = 0;
            timerStarted = true;
    }
    private void ResetActionSlotFlash()
    {
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
        playerSettings = FindObjectOfType<PlayerConfiguration>();
        abilityDisplay = GetComponent<AbilityUIDisplay>();
        if (Ability != null)
        {
            abilityDisplay.Ability = Ability;
        }
            
        KeyBindText.text = HelperMethods.GetKeyBindNameShort(KeyBind.primary);
        DefaultBorderColor = Border.color;
        DefaultIconColor = Icon.color;
        SetBarLock();
        SetIcon();
        Swipe.fillAmount = 0;

    }

    public void OnMouseUp(PointerEventData eventData)
    {
        FlashActionSlot();
    }
}
