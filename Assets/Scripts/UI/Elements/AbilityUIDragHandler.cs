using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (!eventData.hovered.Any())
        {
            GetComponent<ActionSlot>()?.ResetSlot();
        }
    }

    // Update is called once per frame
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>(); 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UIEvents.onAbilityDrag.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UIEvents.onAbilityDrag.InvokeDelayed(false, 5);
    }
}
