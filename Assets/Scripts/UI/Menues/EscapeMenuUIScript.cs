using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenuUIScript : MonoBehaviour
{
    public Button exitGameButton;
    public Button abilitiesButton;
    public Button settingsButton;
    public Button controlsButton;

    public Object abilitiesMenuPrefab;
    public Object settingsMenuPrefab;
    public Object controlsMenuPrefab;

    public Object MainMenu;

    Object activeSubMenu;

    // Start is called before the first frame update
    void Start()
    {
        activeSubMenu = MainMenu;
        exitGameButton.onClick.AddListener(ExitGame);
        abilitiesButton.onClick.AddListener(ShowAbilitiesMenu);
        settingsButton.onClick.AddListener(ShowSettingsMenu);
        controlsButton.onClick.AddListener(ShowControlsMenu);
    }

    public void ShowAbilitiesMenu()
    {
        CloseSubMenu();
        activeSubMenu = Instantiate(abilitiesMenuPrefab, transform);
    }
    public void ShowControlsMenu()
    {
        CloseSubMenu();
        activeSubMenu = Instantiate(controlsMenuPrefab, transform);
    }
    public void ShowSettingsMenu()
    {
        CloseSubMenu();
        activeSubMenu = Instantiate(settingsMenuPrefab, transform);
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    private void CloseSubMenu()
    {
        if(activeSubMenu != null)
        {
            Destroy(activeSubMenu);
        }
    }
}
