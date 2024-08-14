using Assets.Scripts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
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
    private Color _defaultBorderColor;
    private Color _defaultIconColor;

    private float _flashTime = 0.05f;
    private float _flashTimePassed = 0f;
    private bool _flashTimerStarted = false;
    private bool _isOnCooldown = false;

    private Player _player;
    private AbilityUIDisplay _abilityDisplay;
    public Action<int, AbilityBase> OnAbilityChanged;

    private float _remainingCooldown = 0;

    private void Update()
    {
        if (_player == null)
            return;

        if (KeyBind.IsKeyUp())
        {
            var target = FindObjectOfType<TargetingSystem>().CurrentTarget?.GetComponent<Player>();

            FlashActionSlot();
            if (Ability != null)
            {
                Ability.TryUseAbility(_player, target);
                _flashTime = Ability.AbilityInfo.CastTime - _flashTimePassed;
            }
        }
        ResetActionSlotFlash();
        ShowCooldown(_player.Id);
    }

    private void StartCooldown(int ownerId, int abilityId)
    {
        if (_player.Id == ownerId && Ability.Id == abilityId)
        {
            _isOnCooldown = true;
            Swipe.gameObject.SetActive(true);
            CooldownText.gameObject.SetActive(true);
            _remainingCooldown = Ability.AbilityInfo.Cooldown;
        }
    }
    private void ShowCooldown(int ownerId)
    {
        if (Ability == null)
        {
            Swipe.gameObject.SetActive(false);
            CooldownText.gameObject.SetActive(false);
            return;
        }

        if (_isOnCooldown)
        {
            var identifier = new AbilityCooldownInfo(ownerId, Ability.Id);
            _remainingCooldown -= Time.deltaTime;

            if (CooldownManager.Instance.Contains(identifier) && _remainingCooldown > 0)
            {
                CooldownText.text = (Mathf.Ceil(_remainingCooldown)).ToString();
                var cdPercentage = _remainingCooldown / Ability.AbilityInfo.Cooldown;
                Swipe.fillAmount = cdPercentage;
            }
            else
            {
                _isOnCooldown = false;
            }
        }
        if (!_isOnCooldown)
        {
            _remainingCooldown = 0;
            Swipe.fillAmount = 0;
            Swipe.gameObject.SetActive(false);
            CooldownText.gameObject.SetActive(false);
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        var dragHandler = eventData.pointerDrag.GetComponent<AbilityUIDragHandler>();
        var droppedAbility = eventData.pointerDrag.GetComponent<AbilityUIDisplay>();

        Ability = droppedAbility.Ability;
        _abilityDisplay.Ability = Ability;
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
            Icon.color = _defaultIconColor;
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
        GetComponent<AbilityUIDragHandler>().enabled = Ability != null ? !PlayerConfiguration.Instance.Settings.LockActionBars : false;
    }
    private void FlashActionSlot()
    {
            Border.color = Color.yellow;
            _flashTimePassed = 0;
            _flashTimerStarted = true;
    }
    private void ResetActionSlotFlash()
    {
        if (_flashTimerStarted)
        {
            _flashTimePassed += Time.deltaTime;
            if (_flashTimePassed >= _flashTime)
            {
                Border.color = _defaultBorderColor;
                _flashTimerStarted = false;
            }
        }
    }

    public void InitializeSlot(Player player)
    {
        _player = player;
        _abilityDisplay = GetComponent<AbilityUIDisplay>();
        if (Ability != null)
        {
            _abilityDisplay.Ability = Ability;
            GameEvents.OnCooldownStarted.AddListener(StartCooldown);
        }
        KeyBindText.text = HelperMethods.GetKeyBindNameShort(KeyBind.primary);
        _defaultBorderColor = Border.color;
        _defaultIconColor = Icon.color;
        SetBarLock();
        SetIcon();
        Swipe.fillAmount = 0;
    }

    public void OnDisable()
    {
        GameEvents.OnCooldownStarted.RemoveListener(StartCooldown);
    }

    public void OnMouseUp(PointerEventData eventData)
    {
        FlashActionSlot();
    }
}
