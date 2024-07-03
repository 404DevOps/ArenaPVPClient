using Logger = Assets.Scripts.Helpers.Logger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;
using GameKit.Dependencies.Utilities;

public class ProjectileMove : MonoBehaviour
{
    public Transform Origin;
    public Transform Target;
    public float MoveSpeed;

    public event Action OnCollision;
    public LayerMask LayerMask;

    private Rigidbody _rb;
    private Vector3 _prediction;


    private float _maxDistance = 40;
    private float _minDistance = 3f;
    private float _rotateMaxSpeed = 999;
    private float _rotateMinSpeed = 50;
    private float _currentRotateSpeed;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        transform.SetRotation(false, Origin.rotation);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = MoveSpeed * transform.forward;
        var currentDistance = Vector3.Distance(transform.position, Target.position);

       
        if (currentDistance > _minDistance)
            _currentRotateSpeed = Mathf.Lerp(_rotateMaxSpeed, _rotateMinSpeed, currentDistance / _maxDistance);
        else
            _currentRotateSpeed = _rotateMaxSpeed;


        RotateProjectile();
    }

    private void RotateProjectile()
    {
        var direction = Target.transform.position - transform.position;
        
        var rotation = Quaternion.LookRotation(direction);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _currentRotateSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        Logger.Log($"Collider ({other.transform.name}): " + other.transform.GetInstanceID() + ", Target.Transform: " + Target.GetInstanceID());
        if (other.transform.GetInstanceID() == Target.GetInstanceID())
        {
            OnCollision?.Invoke();  
            Destroy(gameObject);
        }       
    }
}
