using Assets.Scripts.Enums;
using Assets.Scripts.Helpers;
using OpenCover.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Connect;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeybindInputHandler : MonoBehaviour, IPointerClickHandler
{
    public Image box;
    public TextMeshProUGUI keyBindText;
    public KeyBindType KeybindType;
    GeneralSettings settings;
    public string actionToRebind;

    private float flashDuration = 3f;
    private float Timer = 0f;
    private float delay = 0.1f;

    [HideInInspector] public bool IsInputListenerActive;
    [HideInInspector] public bool isAbilityKeybind = false;

    private KeyCode[] pressedKeys = new KeyCode[2];


    // Start is called before the first frame update
    void Start()
    {
        settings = FindObjectOfType<PlayerConfiguration>().Settings;
        SetKeyBindText();
    }

    public void SetKeyBindText()
    {
        settings = GameObject.FindGameObjectsWithTag("PlayerSettings")?[0]?.GetComponent<PlayerConfiguration>().Settings;
        if (int.TryParse(actionToRebind, out int slot))
        {
            SetPrimarySecondaryText(settings.Controls.AbilityKeybinds[slot]);
        }
        else
        {
            var fieldInfo = typeof(Controls).GetField(actionToRebind);
            if (fieldInfo.FieldType == typeof(KeyBind))
            {
                KeyBind kb = (KeyBind)fieldInfo.GetValue(settings.Controls);
                SetPrimarySecondaryText(kb);
            }
        }
    }

    private void SetPrimarySecondaryText(KeyBind kb)
    {
        if (KeybindType == KeyBindType.Primary)
            keyBindText.text = HelperMethods.GetKeyBindNameShort(kb.primary);
        else
            keyBindText.text = HelperMethods.GetKeyBindNameShort(kb.secondary);
    }


    // Update is called once per frame
    void Update()
    {
        if (IsInputListenerActive)
        {
            Timer += Time.deltaTime;
            if (Timer < delay)
                return;
            
            if (Timer >= flashDuration || (pressedKeys[0] != KeyCode.None && Input.GetKeyUp(pressedKeys[0])))
            {
                SaveNewKeybind(pressedKeys);
                ResetBox();
            }
            if (Input.anyKey)
            {
                var codes = Enum.GetValues(typeof(KeyCode)) as KeyCode[];
                foreach (var code in codes)
                {
                    if (Input.GetKey(KeyCode.Escape)) 
                    {
                        ResetBox();
                        return;
                    }
                    if (Input.GetKey(KeyCode.Backspace))
                    {
                        ResetBox();
                        SaveNewKeybind(pressedKeys);
                        return;
                    }
                    if (Input.GetKey(code) && IsAllowedKey(code))
                    {
                        if(pressedKeys[0] == KeyCode.None)
                            pressedKeys[0] = code;
                        else if(code != pressedKeys[0])
                        {
                            pressedKeys[1] = code;
                            Debug.Log(pressedKeys);
                            SaveNewKeybind(pressedKeys);
                            ResetBox();
                            break;
                        }    
                    }
                }
            }
        }


    }

    private bool IsAllowedKey(KeyCode code)
    {
        List<KeyCode> keys = new List<KeyCode>() { KeyCode.Escape, KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.LeftWindows, KeyCode.RightWindows, KeyCode.Backspace };
        return !keys.Contains(code);
    }

    public void SaveNewKeybind(KeyCode[] keys)
    {
        if (int.TryParse(actionToRebind, out int slot))
        {
            if (KeybindType == KeyBindType.Primary)
                settings.Controls.AbilityKeybinds[slot].primary = keys;
            else
                settings.Controls.AbilityKeybinds[slot].secondary = keys;

            SetPrimarySecondaryText(settings.Controls.AbilityKeybinds[slot]);
            UIEvents.OnKeyBindsChanged.Invoke();
        }
        else { 
            var fieldInfo = typeof(Controls).GetField(actionToRebind);
            if (fieldInfo.FieldType == typeof(KeyBind)) 
            {
                KeyBind kb = (KeyBind)fieldInfo.GetValue(settings.Controls);
                if (KeybindType == KeyBindType.Primary)
                    kb.primary = keys;
                else
                    kb.secondary = keys;

                fieldInfo.SetValueOptimized(settings.Controls, kb);
                SetPrimarySecondaryText(kb);
                UIEvents.OnControlsChanged.Invoke();
            }
        }
        settings.SaveSettingsToFile();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIEvents.OnNewKeyBindInputSelected.Invoke();
        IsInputListenerActive = true;
        Timer = 0;
        box.color = Color.yellow;
    }

    public void ResetBox()
    {
        box.color = Color.white;
        IsInputListenerActive = false;
        pressedKeys = new KeyCode[2];
    }
}
