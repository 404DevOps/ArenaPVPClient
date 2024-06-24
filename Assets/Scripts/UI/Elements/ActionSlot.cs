using Assets.Scripts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ActionSlot : MonoBehaviour, IDropHandler
{
    public int Id;
    public Image Icon;
    public Image Border;
    public TextMeshProUGUI KeyBindText;
    public Ability Ability;
    public KeyBind KeyBind;

    private float FlashTime = 0.05f;
    private float TimePassed = 0f;
    private bool timerStarted = false;


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped: " + eventData.pointerDrag.name);
        var dragHandler = eventData.pointerDrag.GetComponent<SkillDragHandler>();

        Icon.sprite = eventData.pointerDrag.GetComponentInChildren<Image>().sprite;
        Icon.color = Color.white;

        if (dragHandler.duplicate)
            Destroy(dragHandler.duplicate);
    }

    private void Update()
    {
        if (KeyBind.IsPressed())
        {
            Debug.Log($"Actionslot {Id} pressed");
            Border.color = Color.yellow;
            TimePassed = 0;
            timerStarted = true;
        }

        if (timerStarted)
        {
            TimePassed += Time.deltaTime;
            if (TimePassed >= FlashTime)
            {
                Border.color = Color.white;
                timerStarted = false;
            }
        }
       
    }

    private void Awake()
    {
        KeyBindText.text = HelperMethods.GetKeyBindNameShort(KeyBind.primary);
    }

   
}
