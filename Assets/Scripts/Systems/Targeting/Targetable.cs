using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [SerializeField]
    public bool isSelf = false;
    public void Start()
    {
        isSelf = GetComponent<PlayerMovement>() != null;
    }

    public void Select()
    {
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }
    public void Unselect()
    {
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
