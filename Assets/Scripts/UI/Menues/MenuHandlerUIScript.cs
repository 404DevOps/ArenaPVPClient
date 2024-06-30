using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuHandlerUIScript : MonoBehaviour
{
    public TargetingSystem targetingSystem;
    public GameObject MenuContainerPrefab;
    public GameObject MainMenuPrefab;
    public Transform canvasTransform;

    public GameObject menuContainer;
    private bool isMenuOpen = false;

    void Start()
    {
        if (targetingSystem == null)
            throw new System.Exception("InputHandler Missing Reference for TargetingSystem");
    }

    private void OnEnable()
    {
        UIEvents.onCloseMainMenu.AddListener(CloseMenu);
        UIEvents.onOpenSubMenu.AddListener(OpenSubMenu);
        UIEvents.onCloseSubMenu.AddListener(CloseSubMenu);
    }
    private void OnDisable()
    {
        UIEvents.onCloseMainMenu.RemoveListener(CloseMenu);
        UIEvents.onOpenSubMenu.RemoveListener(OpenSubMenu);
        UIEvents.onCloseSubMenu.RemoveListener(CloseSubMenu);
    }

    private void CloseSubMenu()
    {
        OpenSubMenu(MainMenuPrefab);
    }

    private void OpenSubMenu(Object menuPrefab)
    {
        for (int i = 0; i < menuContainer.transform.childCount; i++)
        {
            Destroy(menuContainer.transform.GetChild(i).gameObject);
        }
        Instantiate(menuPrefab, menuContainer.transform);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isMenuOpen)
        {
            CloseMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && targetingSystem.currentTarget == null && !isMenuOpen)
        {
            UIEvents.onMainMenuOpen.Invoke(true);
            menuContainer = Instantiate(MenuContainerPrefab, canvasTransform);
            Instantiate(MainMenuPrefab, menuContainer.transform);
            isMenuOpen = true;
        }
    }

    public void CloseMenu()
    {
        Destroy(menuContainer);
        UIEvents.onMainMenuOpen.InvokeDelayed(false, 5);
        isMenuOpen = false;
    }
}
