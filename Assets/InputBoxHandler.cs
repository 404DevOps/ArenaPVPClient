using Assets.Scripts.Enums;
using Assets.Scripts.Helpers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputBoxHandler : MonoBehaviour, IPointerClickHandler
{
    string action;
    public Image box;
    public TextMeshProUGUI keyBindText;
    public KeyBindType KeybindType;
    SettingsScriptableObject settings;

    public float flashDuration = 30f;
    public float Timer = 0f;
    public float delay = 0.03f;
    public bool TimerEnded;
    
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<Image>();
        keyBindText = GetComponent<TextMeshProUGUI>();
        settings = FindObjectOfType<PlayerSettings>().Settings;

        InitializeKeyBinds();
    }

    private void InitializeKeyBinds()
    {
        keyBindText.text = HelperMethods.GetKeyBindNameShort(settings.Controls.forwards.primary);
    }

    // Update is called once per frame
    void Update()
    {
        if (!TimerEnded)
        {
            Timer += Time.deltaTime;
            if (Timer < delay)
                return;
            
            if (Timer > flashDuration)
            {
                ResetBox();
            }
            if (Input.anyKey)
            {
                Debug.Log(Input.compositionString);                
            }

            //if readkey set keycode to setttings & to box (named), reset tint and set timer ended

        }


    }
    void OnGUI()
    {
        if (Input.anyKey)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                KeyCode key = e.keyCode;
                Debug.Log(key);
                if (key != null && key != KeyCode.None)
                    OnKeyPressed(key);
            }
        }
    }

    public void OnKeyPressed(KeyCode key)
    {
        switch (KeybindType)
        {
            case KeyBindType.Primary:
                Debug.Log(settings.Controls);
                settings.Controls.forwards.primary = new KeyCode[] { key };
                Debug.Log("Set Forward Control to key" + key);
                break;
            case KeyBindType.Secondary:
                settings.Controls.forwards.secondary = new KeyCode[] { key };
                break;
            default:
                break;
        }

        ResetBox();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TimerEnded = false;
        Timer = 0;
        box.color = Color.yellow;
    }

    public void ResetBox()
    {
        box.color = Color.white;
        TimerEnded = true;
    }
}
