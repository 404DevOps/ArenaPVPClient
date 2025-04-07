using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public abstract class ConditionBase : ScriptableObject
{

    public virtual bool IsTrue(Entity origin, Entity target)
    {
        return true;
    }
}