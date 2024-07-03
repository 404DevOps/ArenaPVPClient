using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AuraBase : ScriptableObject
{
    public string Name;
    public string Description;
    public float Duration;
    public Sprite Icon;
    public AuraType AuraType;

    protected abstract void Apply(object owner, object target);
    protected abstract void Fade(object owner, object target);
}