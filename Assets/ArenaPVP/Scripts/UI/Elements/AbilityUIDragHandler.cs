using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AbilityUIDisplay))]
public class AbilityUIDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject Duplicate;

    private Canvas canvas;
    private bool isClone = false;
    private CanvasGroup canvasGroup;
    private RectTransform rect;

    // Update is called once per frame
    void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    { 
        Duplicate.transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        if (!isClone)
        {
            Duplicate = Instantiate(this.gameObject, canvas.transform);
            Duplicate.name = "SkillDisplay Duplicate";

            var cloneRect = Duplicate.GetComponent<RectTransform>();
            cloneRect.sizeDelta = rect.sizeDelta;
            Duplicate.transform.position = eventData.position;

            var cloneDragHandler = Duplicate.GetComponent<AbilityUIDragHandler>();
            cloneDragHandler.canvasGroup.alpha = .6f;
            cloneDragHandler.canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(Duplicate);

        var thisSlot = GetComponent<ActionSlot>();
        if (eventData.hovered.Any() && thisSlot != null)
        {
            var hoveredSlot = eventData.hovered.FirstOrDefault(a => a.GetComponent<ActionSlot>())?.GetComponent<ActionSlot>();
            if (hoveredSlot == null || thisSlot.Id != hoveredSlot.Id)
            {
                GetComponent<ActionSlot>()?.ResetSlot();
            }
        }
        else 
        {
            GetComponent<ActionSlot>()?.ResetSlot();
        }
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UIEvents.OnAbilityDrag.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UIEvents.OnAbilityDrag.InvokeDelayed(false, 5);
    }
}
