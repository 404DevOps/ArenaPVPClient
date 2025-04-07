using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaSelectorBase : ScriptableObject
{
    public abstract List<Entity> GetTargetsInArea(Entity source);

}
