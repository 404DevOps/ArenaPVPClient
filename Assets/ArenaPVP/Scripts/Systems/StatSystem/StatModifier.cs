using Assets.ArenaPVP.Scripts.Models.Enums;
using System;
using System.Threading;
using UnityEngine;

[Serializable]
public class StatModifier : IDisposable
{
    [HideInInspector] public int SourceAuraId;
    public event Action<StatModifier> OnDispose = delegate { };

    [SerializeField] StatType _statType;
    [SerializeField] OperatorType _operatorType;
    [SerializeField] float _value;


    public void Handle(object sender, StatQuery query)
    {
        if (query.StatType == _statType)
        {
            switch (_operatorType)
            {
                case OperatorType.Multiply: query.Value = query.Value * _value; break;
                case OperatorType.Divide: query.Value = query.Value / _value; break;
                case OperatorType.Add: query.Value = query.Value += _value; break;
            }
        }
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
}