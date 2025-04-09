using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public struct StaminaChangedEventArgs
{
    public StaminaChangedEventArgs(Entity entity, float staminaChangedAmount)
    { 
        Entity = entity;
        StaminaChangedAmount = staminaChangedAmount;
        
    }
    public Entity Entity;
    public float StaminaChangedAmount;
}

