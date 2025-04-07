using FishNet;
using GameKit.Dependencies.Utilities;
using System;
using UnityEngine;
using ArenaLogger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

public class ProjectileTargeted : ProjectileBase
{
    public Entity Target;

    private float _currentRotateSpeed;
    private float _rotateMaxSpeed = 1400;
    private float _rotateMinSpeed = 300;
    private float _rotateIncrement = 200;
    private float _minDistance = 3f;



    public void Initialize(int abilityId, Entity origin, Entity target, float passedTime)
    {
        Origin = origin;
        Target = target;

        _rb = GetComponent<Rigidbody>();
        transform.SetRotation(false, Origin.transform.rotation);
        _currentRotateSpeed = _rotateMinSpeed;

        base.Initialize(abilityId, passedTime);
    }

    internal override void MoveProjectile()
    {
        _rb.linearVelocity = MoveSpeed * transform.forward * (_delta + _passedTimeDelta);

        _currentRotateSpeed += _rotateIncrement * Time.fixedDeltaTime;
        _currentRotateSpeed = Mathf.Min(_currentRotateSpeed, _rotateMaxSpeed);
        if (Math.Abs(Vector3.Distance(transform.position, Target.transform.position)) < _minDistance)
        {
            _currentRotateSpeed = _rotateMaxSpeed;
        }
    }
    internal override void RotateProjectile()
    {
        var direction = Target.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(direction);
        Debug.DrawRay(transform.position, direction, Color.blue);

        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _currentRotateSpeed * Time.fixedDeltaTime));

    }

    private void OnTriggerEnter(Collider other)
    {
        var entity = other.GetComponentInParent<Entity>();
        if (entity.Id == Target.Id)
        {
            if (InstanceFinder.IsClientStarted)
            {
                ArenaLogger.Log($"Projectile Client hit.");
                //Show VFX.
                //Play Audio.
            }
            if (InstanceFinder.IsServerStarted)
            {
                ArenaLogger.Log($"Projectile Server hit.");
                AbilityStorage.GetAbility(_abilityId).ApplyEffectsServer(Origin,Target);
            }

            //TODO: return to a pool
            Destroy(gameObject);
        }
    }
}
