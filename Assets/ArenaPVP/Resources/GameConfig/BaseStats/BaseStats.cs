using FishNet.CodeGenerating;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStats", menuName = "Stats/BaseStats")]
public class BaseStats : ScriptableObject
{
    public float AttackPower;
    public float Spellpower;
    public float Armor;
    public float SpellResistance;
    public float RessourceRegenerationRate; 
    public float MovementSpeed;
    public float Health;
    public float Ressource;
    public float MaxStamina;
    public float MaxShield;
    public float StaminaRegenerationRate;
}
[System.Serializable]
public struct StatSnapshot
{
    public float AttackPower;
    public float Spellpower;
    public float MovementSpeed;
    public float ResourceRegenRate;
    public float SpellResistance;
    public float Armor;
    public float MaxHealth;
    public float MaxResource;
    public float MaxStamina;
    public float MaxShield;
    public float StaminaRegenerationRate;
}



