using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;

    public Player Player;

    public void OnHealthChanged(Player player, float healthChange)
    {
        if (player.Id != Player.Id)
            return;

        CurrentHealth += healthChange;
        if(CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    private void OnEnable()
    {
        Player = GetComponent<Player>();
        GameEvents.OnPlayerHealthChanged.AddListener(OnHealthChanged);
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerHealthChanged.AddListener(OnHealthChanged);
    }
    void Update()
    {
        
    }
}
