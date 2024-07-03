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


    private float _currentRotateSpeed;
    private float _rotateMaxSpeed = 1000;
    private float _rotateMinSpeed = 400;
    private float _rotateIncrement = 200;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        transform.SetRotation(false, Origin.rotation);
        _currentRotateSpeed = _rotateMinSpeed;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = MoveSpeed * transform.forward;

        _currentRotateSpeed += _rotateIncrement * Time.deltaTime;
        _currentRotateSpeed = Mathf.Min(_currentRotateSpeed, _rotateMaxSpeed);
        RotateProjectile();
    }

    private void RotateProjectile()
    {
        var direction = Target.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(direction);
        Debug.DrawRay(transform.position, direction, Color.blue);

        //if (_currentDistance < _minDistance)
        //    _rb.rotation = rotation;
        //else
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
