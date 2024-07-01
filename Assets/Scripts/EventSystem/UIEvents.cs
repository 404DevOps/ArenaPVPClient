using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIEvents
{
    //Abilities & ActionSlots
    public static readonly Event<bool> onAbilityDrag = new Event<bool>();

    //Menu Events
    public static readonly Event<bool> onMainMenuOpen = new Event<bool>();
    public static readonly Event onCloseSubMenu = new Event();
    public static readonly Event<Object> onOpenSubMenu = new Event<Object>();
    public static readonly Event onCloseMainMenu = new Event();
    public static readonly Event onNewKeyBindInputSelected = new Event();
    public static readonly Event onSettingsSaved = new Event();
    public static readonly Event onKeyBindsChanged = new Event();
    public static readonly Event onControlsChanged = new Event();
}
