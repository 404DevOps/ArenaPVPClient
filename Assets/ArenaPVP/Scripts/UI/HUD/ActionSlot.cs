using Assets.ArenaPVP.Scripts.Helpers;
using Assets.ArenaPVP.Scripts.Models.Enums;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private Entity _player;
    private AbilityUIDisplay _abilityDisplay;
    public Action<int, AbilityBase> OnAbilityChanged;


    private float _remainingCooldown = 0;
    private float _remainingGCD = 0;

    private TargetingSystem _targetingSystem;

    private void Start()
    {
        _targetingSystem = FindAnyObjectByType<TargetingSystem>();
    }

    private void Update()
    {
        if (_player == null)
            return;

        if (KeyBind.IsKeyUp())
        {
            var target = _targetingSystem.CurrentTarget?.GetComponent<Entity>();

            FlashActionSlot();
            if (Ability != null)
            {
                Ability.TryUseAbility(_player, target);
                _flashTime = Ability.AbilityInfo.CastTime - _flashTimePassed;
            }
        }
        UpdateCooldownTimers();
        ResetActionSlotFlash();
        ShowCooldown(_player.Id);
    }

    private void UpdateCooldownTimers()
    {
        if(_remainingCooldown > 0f)
            _remainingCooldown -= Time.deltaTime;
        if(_remainingGCD > 0f)
            _remainingGCD -= Time.deltaTime;
    }

    private void ShowCooldown(int ownerId)
    {
        if (Ability == null)
        {
            Swipe.gameObject.SetActive(false);
            CooldownText.gameObject.SetActive(false);
            return;
        }
        //give regular cooldown visualisation priority over gcd
        if (_remainingCooldown > 0 && _remainingCooldown > _remainingGCD)
        {
            var identifier = new AbilityCooldownInfo(ownerId, Ability.Id);
            if (CooldownManager.Instance.Contains(identifier) && _remainingCooldown > 0)
            {
                CooldownText.text = (Mathf.Ceil(_remainingCooldown)).ToString();
                Swipe.fillAmount = _remainingCooldown / Ability.AbilityInfo.Cooldown;
            }
        }
        //once gcd is longer than regular cooldown just show that and hide timer
        else if(_remainingGCD > 0f && !Ability.AbilityInfo.IgnoreGCD) 
        {
            CooldownText.gameObject.SetActive(false);
            Swipe.fillAmount = _remainingGCD / GCDManager.GCD_TIME;
            
        }
        if(_remainingCooldown <= 0 && _remainingGCD <= 0)
        {
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
    public void InitializeSlot(Entity player)
    {
        _player = player;
        _abilityDisplay = GetComponent<AbilityUIDisplay>();
        if (Ability != null)
        {
            _abilityDisplay.Ability = Ability;
            ClientEvents.OnCooldownStarted.AddListener(StartCooldown);
            ClientEvents.OnGCDStarted.AddListener(StartGCD);
            ClientEvents.OnCastInterrupted.AddListener(OnCastInterrupted);

        }
        KeyBindText.text = HelperMethods.GetKeyBindNameShort(KeyBind.primary);
        _defaultBorderColor = Border.color;
        _defaultIconColor = Icon.color;
        SetBarLock();
        SetIcon();
        Swipe.fillAmount = 0;
    }

    private void OnCastInterrupted(AbilityCastInfo args)
    {
        if (args.OwnerId != _player.OwnerId)
            return;

        if (args.Reason == InterruptType.Abort)
        {
            _remainingGCD = 0;
        }
    }

    private void StartCooldown(int ownerId, int abilityId)
    {
        if (_player.Id == ownerId && Ability.Id == abilityId)
        {
            Swipe.gameObject.SetActive(true);
            CooldownText.gameObject.SetActive(true);
            _remainingCooldown = Ability.AbilityInfo.Cooldown;
        }
    }
    private void StartGCD(int ownerId, float gcdDuration)
    {
        if (ownerId != _player.Id)
            return;

        Swipe.gameObject.SetActive(true);
        _remainingGCD = gcdDuration;
    }

    public void OnDisable()
    {
        ClientEvents.OnCooldownStarted.RemoveListener(StartCooldown);
        ClientEvents.OnGCDStarted.RemoveListener(StartGCD);
    }

    public void OnMouseUp(PointerEventData eventData)
    {
        FlashActionSlot();
    }


}
