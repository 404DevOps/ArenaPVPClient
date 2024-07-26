using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResource : MonoBehaviour
{
    public float MaxResource;
    public float CurrentResource;

    public Player Player;

    public void OnResourceChanged(ResourceChangedEventArgs args) //Player player, float healthChange)
    {
        if (args.Player.Id != Player.Id)
            return;

        CurrentResource += args.ResourceChangeAmount;
        if(CurrentResource > MaxResource)
            CurrentResource = MaxResource;
    }

    private void OnEnable()
    {
        Player = GetComponent<Player>();
        MaxResource = Player.Stats.Resource;
        CurrentResource = Player.Stats.Resource;
        GameEvents.OnPlayerResourceChanged.AddListener(OnResourceChanged);
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerResourceChanged.RemoveListener(OnResourceChanged);
    }
}
