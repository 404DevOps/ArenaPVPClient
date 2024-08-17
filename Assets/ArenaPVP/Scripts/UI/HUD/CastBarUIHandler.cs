using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ArenaLogger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

public class CastBarUIHandler : MonoBehaviour
{
    [SerializeField] GameObject CastBarParent;
    [SerializeField] Image Fill;
    [SerializeField] Image Icon;
    [SerializeField] TextMeshProUGUI CurrentCastTime;
    [SerializeField] TextMeshProUGUI MaxCastTime;
    [SerializeField] TextMeshProUGUI AbilityNameText;
    [SerializeField] Color CastbarColor;

    public AbilityBase Ability;
    public Player Player;

    private bool _isCasting;
    private float _remainingCastTime;
    private uint _currentCastId;
    private bool _newCastStarted;
    [SerializeField] private bool _isMainCastBar;

    // Start is called before the first frame update


    private void OnEnable()
    {
        GameEvents.OnPlayerInitialized.AddListener(OnPlayerInitialized);
        GameEvents.OnCastStarted.AddListener(OnCastStarted);
        GameEvents.OnCastInterrupted.AddListener(OnCastInterrupted);
        GameEvents.OnCastCompleted.AddListener(OnCastCompleted); 
    }

    private void OnPlayerInitialized(Player player)
    {
        if (_isMainCastBar) 
        {
            if(player.IsOwnedByMe)
                Player = player;
        }    
        else
        {
            Player = GetComponentInParent<NameplateUIHandler>().Player;
        }
    }

    private void OnDisable()
    {
        _isCasting = false;
        GameEvents.OnCastStarted.RemoveListener(OnCastStarted);
        GameEvents.OnCastInterrupted.RemoveListener(OnCastInterrupted);
        GameEvents.OnCastCompleted.RemoveListener(OnCastCompleted);
    }

    public void OnCastStarted(CastEventArgs args) //int ownerId, int abilityId)
    {
        if (args.OwnerId != Player.Id)
            return;

        var ability = AbilityStorage.GetAbility(args.AbilityId);
        _currentCastId = args.CastId;
        Ability = ability;
        Fill.fillAmount = 0;
        Fill.color = CastbarColor;
        Icon.sprite = Ability.AbilityInfo.Icon;
        AbilityNameText.text = Ability.AbilityInfo.Name;
        CurrentCastTime.text = "0";
        MaxCastTime.text = "/ " + Ability.AbilityInfo.CastTime.ToString();
        _remainingCastTime = Ability.AbilityInfo.CastTime;
        _isCasting = true;
        CastBarParent.SetActive(true);
        _newCastStarted = true;
    }
    public void OnCastInterrupted(AbilityCastInfo args)
    {
        if (args.OwnerId != Player.Id || args.CastId != _currentCastId)
            return;

        Fill.fillAmount = 1;
        Fill.color = Color.red;
        AbilityNameText.text = "Interrupted";
        CurrentCastTime.text = "0";
        _isCasting = false;
        _newCastStarted = false;
        StartCoroutine(SetInvisibleAfterTime(0.3f));
  
    }
    public void OnCastCompleted(CastEventArgs args)
    {
        ArenaLogger.Log($"Completed Cast with Id={args.CastId}, CurrentCastId={_currentCastId}");
        if (args.OwnerId != Player.Id || args.CastId != _currentCastId)
            return;

        _remainingCastTime = 0;
        Fill.fillAmount = 1;
        Fill.color = Color.green;
        AbilityNameText.text = "Complete";
        CurrentCastTime.text = Ability?.AbilityInfo?.CastTime.ToString("0.0");
        _newCastStarted = false;
        StartCoroutine(SetInvisibleAfterTime(0.3f));
    }

    IEnumerator SetInvisibleAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (!_newCastStarted)
        {
            _isCasting = false;
            Ability = null;
            CastBarParent.SetActive(false);
        }
    }

    void Update()
    {
        if (Ability != null)
        {
            if (_isCasting)
            {
                _remainingCastTime -= Time.deltaTime;
                if (_remainingCastTime > 0)
                {
                    var percentage = _remainingCastTime / Ability.AbilityInfo.CastTime;
                    Fill.fillAmount = 1 - percentage;
                    CurrentCastTime.text = _remainingCastTime.ToString("0.0");
                }
            }
        }
    }
}
