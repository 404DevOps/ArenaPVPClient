using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [SerializeField]
    public bool IsSelf = false;

    public void Select()
    {
        this.gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
    }
    public void Unselect()
    {
        this.gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
    }
}
