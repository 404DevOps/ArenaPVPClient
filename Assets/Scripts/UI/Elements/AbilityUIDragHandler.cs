using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AbilityUIDisplay))]
public class AbilityUIDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject duplicate;
    private Canvas canvas;
    bool isClone = false;

    private CanvasGroup canvasGroup;

    RectTransform rect;
    public void OnDrag(PointerEventData eventData)
    { 
        duplicate.transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isClone)
        {
            duplicate = Instantiate(this.gameObject, canvas.transform);
            duplicate.name = "SkillDisplay Duplicate";

            var cloneRect = duplicate.GetComponent<RectTransform>();
            cloneRect.sizeDelta = rect.sizeDelta;
            duplicate.transform.position = eventData.position;

            var cloneDragHandler = duplicate.GetComponent<AbilityUIDragHandler>();
            cloneDragHandler.canvasGroup.alpha = .6f;
            cloneDragHandler.canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(duplicate);
    }

    // Update is called once per frame
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>(); 
    }
}
