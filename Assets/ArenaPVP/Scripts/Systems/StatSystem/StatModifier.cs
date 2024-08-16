using Assets.ArenaPVP.Scripts.Models.Enums;
using System;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

[Serializable]
public class StatModifier : IDisposable
{
    [HideInInspector] public int SourceAuraId;
    public event Action<StatModifier> OnDispose = delegate { };

    [SerializeField] internal StatType _statType;
    [SerializeField] internal OperatorType _operatorType;
    [SerializeField] internal float _value;

    public float Modify(StatQuery query)
    {
        float result = 0;
        if (query.StatType == _statType)
        {
            switch (_operatorType)
            {
                case OperatorType.Multiply: result = query.BaseValue * _value; break;
                case OperatorType.Divide: result = query.BaseValue / _value; break;
                case OperatorType.Add: result = _value; break;
                case OperatorType.AddPercentage: result = (query.Value / 100) * _value; break;
                case OperatorType.SubtractPercentage: result = -((query.Value / 100) * _value); break;
            }
        }
        return result;
    }
    public void Dispose()
    {
        OnDispose.Invoke(this);
    }
}

public enum OperatorType
{
    Add,
    Multiply,
    Divide,
    AddPercentage,
    SubtractPercentage
}