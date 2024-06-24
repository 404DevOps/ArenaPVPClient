using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuHandlerUIScript : MonoBehaviour
{
    public TargetingSystem targetingSystem;
    public GameObject ingameMenuPrefab;
    public Transform canvasTransform;

    public Object menuInstance;
    public bool isMenuOpen = false;

    void Start()
    {
        if (targetingSystem == null)
            throw new System.Exception("InputHandler Missing Reference for TargetingSystem");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isMenuOpen)
        {
            CloseMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && targetingSystem.currentTarget == null && !isMenuOpen)
        {
            menuInstance = Instantiate(ingameMenuPrefab, canvasTransform);
            isMenuOpen = true;
        }
    }

    public void CloseMenu()
    {
        Destroy(menuInstance);
        isMenuOpen = false;
    }
}
