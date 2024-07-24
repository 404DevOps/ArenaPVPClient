using Assets.ArenaPVP.Scripts.Enums;
using UnityEngine;

public static class UIEvents
{
    //Abilities & ActionSlots
    public static readonly Event<bool> OnAbilityDrag = new Event<bool>();

    //Menu Events
    public static readonly Event<bool> OnMainMenuOpen = new Event<bool>();
    public static readonly Event OnCloseSubMenu = new Event();
    public static readonly Event<Object> OnOpenSubMenu = new Event<Object>();
    public static readonly Event OnCloseMainMenu = new Event();
    public static readonly Event OnNewKeyBindInputSelected = new Event();
    public static readonly Event OnKeyBindsChanged = new Event();
    public static readonly Event OnControlsChanged = new Event();

    public static readonly Event<Player> OnTargetChanged = new Event<Player>();

    //LoadEvents
    public static readonly Event OnSettingsSaved = new Event();
    public static readonly Event OnSettingsLoaded = new Event();
    public static readonly Event OnHideTooltip = new Event();
    public static readonly Event<TooltipType, object> OnShowTooltip = new Event<TooltipType, object>(); //object = tooltip info

    public static readonly Event<string> OnShowInformationPopup = new Event<string>();
}
