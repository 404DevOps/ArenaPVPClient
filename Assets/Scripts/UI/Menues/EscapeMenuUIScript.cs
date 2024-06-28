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
    public Object KeybindsMenuPrefab;

    public Object MainMenu;

    Object activeSubMenu;

    // Start is called before the first frame update
    void Start()
    {
        activeSubMenu = MainMenu;
        exitGameButton.onClick.AddListener(ExitGame);
        abilitiesButton.onClick.AddListener(ShowAbilitiesMenu);
        settingsButton.onClick.AddListener(ShowSettingsMenu);
        controlsButton.onClick.AddListener(ShowKeybindMenu);
    }

    public void ShowAbilitiesMenu()
    {
        CloseSubMenu();
        activeSubMenu = Instantiate(abilitiesMenuPrefab, transform);
    }
    public void ShowKeybindMenu()
    {
        CloseSubMenu();
        activeSubMenu = Instantiate(KeybindsMenuPrefab, transform);
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
