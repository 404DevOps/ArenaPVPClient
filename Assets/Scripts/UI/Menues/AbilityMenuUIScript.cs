using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuUIScript : MonoBehaviour
{
    public Button closeButton;

    MenuHandlerUIScript menuHandler;

    // Start is called before the first frame update
    void Start()
    {
        menuHandler = FindObjectOfType<MenuHandlerUIScript>();
        closeButton.onClick.AddListener(CloseMenu);
    }

    public void CloseMenu()
    {
        menuHandler.CloseMenu(); ;
    }
}
