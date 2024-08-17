using Assets.ArenaPVP.Scripts.Enums;
using Assets.ArenaPVP.Scripts.Helpers;
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

    private float _flashDuration = 3f;
    private float _timer = 0f;
    private float _delay = 0.1f;

    [HideInInspector] public bool IsInputListenerActive;
    [HideInInspector] public bool isAbilityKeybind = false;

    private KeyCode[] _pressedKeys = new KeyCode[2];

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
            _timer += Time.deltaTime;
            if (_timer < _delay)
                return;
            
            if (_timer >= _flashDuration || (_pressedKeys[0] != KeyCode.None && Input.GetKeyUp(_pressedKeys[0])))
            {
                SaveNewKeybind(_pressedKeys);
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
                        SaveNewKeybind(_pressedKeys);
                        return;
                    }
                    if (Input.GetKey(code) && IsAllowedKey(code))
                    {
                        if(_pressedKeys[0] == KeyCode.None)
                            _pressedKeys[0] = code;
                        else if(code != _pressedKeys[0])
                        {
                            _pressedKeys[1] = code;
                            Debug.Log(_pressedKeys);
                            SaveNewKeybind(_pressedKeys);
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
        _timer = 0;
        box.color = Color.yellow;
    }

    public void ResetBox()
    {
        box.color = Color.white;
        IsInputListenerActive = false;
        _pressedKeys = new KeyCode[2];
    }
}
