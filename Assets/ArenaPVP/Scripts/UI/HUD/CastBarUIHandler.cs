using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void OnCastStarted(int ownerId, int abilityId)
    {
        if (ownerId != Player.Id)
            return;

        var ability = AbilityStorage.GetAbility(abilityId);

        this.Ability = ability;
        Fill.fillAmount = 0;
        Fill.color = CastbarColor;
        Icon.sprite = Ability.AbilityInfo.Icon;
        AbilityNameText.text = Ability.AbilityInfo.Name;
        CurrentCastTime.text = "0";
        MaxCastTime.text = "/ " + Ability.AbilityInfo.CastTime.ToString();
        _isCasting = true;
        CastBarParent.SetActive(true);
    }
    public void OnCastInterrupted(int ownerId)
    {
        if (ownerId != Player.Id)
            return;

        Fill.fillAmount = 1;
        Fill.color = Color.red;
        AbilityNameText.text = "Interrupted";
        CurrentCastTime.text = "0";
        StartCoroutine(SetInvisibleAfterTime(0.3f));
    }
    public void OnCastCompleted(int ownerId)
    {
        if (ownerId != Player.Id)
            return;

        Fill.fillAmount = 1;
        Fill.color = Color.green;
        AbilityNameText.text = "Complete";
        CurrentCastTime.text = Ability?.AbilityInfo?.CastTime.ToString("0.0");
        StartCoroutine(SetInvisibleAfterTime(0.3f));
    }

    IEnumerator SetInvisibleAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _isCasting = false;
        Ability = null;
        CastBarParent.SetActive(false);
    }

    void Update()
    {
        if (Ability != null)
        {
            if (_isCasting)
            {
                float remainingCastTime = CastManager.Instance.GetRemainingCastTime(Player.Id, Ability.Id);
                if (remainingCastTime > 0)
                {
                    var percentage = remainingCastTime / Ability.AbilityInfo.CastTime;
                    Fill.fillAmount = 1 - percentage;
                    CurrentCastTime.text = remainingCastTime.ToString("0.0");
                }
            }
        }
    }
}
