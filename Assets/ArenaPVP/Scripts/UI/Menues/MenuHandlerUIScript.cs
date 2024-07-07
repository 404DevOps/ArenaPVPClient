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
        UIEvents.OnCloseMainMenu.AddListener(CloseMenu);
        UIEvents.OnOpenSubMenu.AddListener(OpenSubMenu);
        UIEvents.OnCloseSubMenu.AddListener(CloseSubMenu);
    }
    private void OnDisable()
    {
        UIEvents.OnCloseMainMenu.RemoveListener(CloseMenu);
        UIEvents.OnOpenSubMenu.RemoveListener(OpenSubMenu);
        UIEvents.OnCloseSubMenu.RemoveListener(CloseSubMenu);
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
        else if (Input.GetKeyDown(KeyCode.Escape) && targetingSystem.CurrentTarget == null && !isMenuOpen)
        {
            UIEvents.OnMainMenuOpen.Invoke(true);
            menuContainer = Instantiate(MenuContainerPrefab, canvasTransform);
            Instantiate(MainMenuPrefab, menuContainer.transform);
            isMenuOpen = true;
        }
    }

    public void CloseMenu()
    {
        Destroy(menuContainer);
        UIEvents.OnMainMenuOpen.InvokeDelayed(false, 5);
        isMenuOpen = false;
    }
}
