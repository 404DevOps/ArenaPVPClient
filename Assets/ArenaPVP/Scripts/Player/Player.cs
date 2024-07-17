using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string Name;
    public bool IsOwnedByMe;
    public CharacterClassType ClassType;

    public int Id = 0;
    // Start is called before the first frame update

    private void OnEnable()
    {
        Id = IdentifierService.GetPlayerId();
        GameEvents.OnPlayerInitialized.Invoke(this);
    }
}
