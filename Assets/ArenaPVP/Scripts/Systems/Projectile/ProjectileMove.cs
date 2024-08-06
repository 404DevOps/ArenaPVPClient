using Logger = Assets.Scripts.Helpers.Logger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;
using GameKit.Dependencies.Utilities;
using FishNet;
using GameKit.Dependencies.Utilities.ObjectPooling.Examples;
using FishNet.Object;
using FishNet.Component.Transforming;

public class ProjectileMove : MonoBehaviour
{
    public Player Origin;
    public Player Target;
    public float MoveSpeed;

    public event Action OnCollision;
    public LayerMask LayerMask;

    private Rigidbody _rb;


    private float _currentRotateSpeed;
    private float _rotateMaxSpeed = 1400;
    private float _rotateMinSpeed = 300;
    private float _rotateIncrement = 200;
    private float _minDistance = 3f;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        transform.SetRotation(false, Origin.transform.rotation);
        _currentRotateSpeed = _rotateMinSpeed;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        MoveProjectile();
        RotateProjectile();
    }

    private void MoveProjectile()
    {
        _rb.velocity = MoveSpeed * transform.forward;

        _currentRotateSpeed += _rotateIncrement * Time.fixedDeltaTime;
        _currentRotateSpeed = Mathf.Min(_currentRotateSpeed, _rotateMaxSpeed);
        if (Math.Abs(Vector3.Distance(transform.position, Target.transform.position)) < _minDistance)
        {
            _currentRotateSpeed = _rotateMaxSpeed;
        }
    }
    private void RotateProjectile()
    {
        var direction = Target.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(direction);
        Debug.DrawRay(transform.position, direction, Color.blue);

        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _currentRotateSpeed * Time.fixedDeltaTime));

    }

    private void OnTriggerEnter(Collider other)
    {
        if (InstanceFinder.IsClientStarted)
        {
            if (other.GetComponent<Player>()?.Id == Target.Id)
            {
                //Show VFX.
                //Play Audio.
            }
        }
        if (InstanceFinder.IsServerStarted)
        {
            if (other.GetComponent<Player>()?.Id == Target.Id)
            {
                OnCollision?.Invoke();
                InstanceFinder.ServerManager.Despawn(gameObject);
            }
        }
        //Logger.Log($"Collider ({other.transform.name}): " + other.GetComponent<Player>()?.Id + ", Target.Transform: " + Target.Id);
      
    }
}
