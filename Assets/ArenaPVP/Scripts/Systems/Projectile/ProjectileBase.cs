using FishNet;
using GameKit.Dependencies.Utilities;
using System;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

public abstract class ProjectileBase : MonoBehaviour
{
    public Player Origin;
    public float MoveSpeed;

    public LayerMask LayerMask;

    internal Rigidbody _rb;
    internal bool _isInitialized;
    internal float _passedTime;

    internal float _delta;
    internal float _passedTimeDelta;
    internal int _abilityId;

    internal virtual void Initialize(int abilityId, float passedTime) 
    {
        _abilityId = abilityId;
        _passedTime = passedTime;
        _isInitialized = true;
    }
    void FixedUpdate()
    {
        if (!_isInitialized) return;

        CalculateCatchup();
        MoveProjectile();
        RotateProjectile();
    }

    private void CalculateCatchup()
    {
        _delta = Time.deltaTime;

        //See if to add on additional delta to consume passed time.
        _passedTimeDelta = 0f;
        if (_passedTime > 0f)
        {
            float step = (_passedTime * 0.08f);
            _passedTime -= step;

            /* If the remaining time is less than half a delta then
             * just append it onto the step. The change won't be noticeable. */
            if (_passedTime <= (_delta / 2f))
            {
                step += _passedTime;
                _passedTime = 0f;
            }
            _passedTimeDelta = step;
        }
    }

    internal abstract void MoveProjectile();
    internal abstract void RotateProjectile();
}
