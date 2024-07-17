using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloatingTextContainer : MonoBehaviour
{
    public Player Player;
    public float offsetY = 5f;
    public float moveSpeed = 0.5f;

    private int _maxChildCount = 9;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        var screenPos = cam.WorldToScreenPoint(Player.transform.position);
        transform.position = new Vector3(screenPos.x, screenPos.y + offsetY, screenPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        SmoothFollowPlayer();
        RemoveExcessFloatingTexts();
    }

    private void RemoveExcessFloatingTexts()
    {
        if (transform.childCount > _maxChildCount)
        {
            var texts = GetComponentsInChildren<FloatingText>().ToList();
            texts = texts.OrderByDescending(t => t._timer).ToList();

            while (transform.childCount > _maxChildCount)
            {
                DestroyImmediate(texts.First()?.transform?.parent?.gameObject);
            }
        }
    }
    private void SmoothFollowPlayer()
    {
        var worldToScreen = cam.WorldToScreenPoint(Player.transform.position);
        var newPos = new Vector3(worldToScreen.x, worldToScreen.y + offsetY, worldToScreen.z);
        transform.position = Vector3.Lerp(transform.position, newPos, moveSpeed * Time.deltaTime);
    }
}

