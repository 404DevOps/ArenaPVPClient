using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour
{
    public Button exitGameButton;
    public Button abilitiesButton;
    public Button settingsButton;
    public Button controlsButton;
    public Button closeButton;

    public Object abilitiesMenuPrefab;
    public Object settingsMenuPrefab;
    public Object KeybindsMenuPrefab;
    public Object MainMenuPrefab;

    Object activeSubMenu;

    // Start is called before the first frame update
    void Start()
    {
        exitGameButton.onClick.AddListener(ExitGame);
        abilitiesButton.onClick.AddListener(ShowAbilitiesMenu);
        settingsButton.onClick.AddListener(ShowSettingsMenu);
        controlsButton.onClick.AddListener(ShowKeybindMenu);
        closeButton.onClick.AddListener(CloseEscapeMenu);
    }

    public void ShowAbilitiesMenu()
    {
        OpenSubMenu(abilitiesMenuPrefab);
    }
    public void ShowKeybindMenu()
    {
        OpenSubMenu(KeybindsMenuPrefab);
    }
    public void ShowSettingsMenu()
    {
        OpenSubMenu(settingsMenuPrefab);
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
    private void OpenSubMenu(Object subMenuPrefab)
    {
        UIEvents.onOpenSubMenu.Invoke(subMenuPrefab);
    }


    private void CloseSubMenu()
    {
        OpenSubMenu(MainMenuPrefab);
    }

    public void CloseEscapeMenu()
    {
        UIEvents.onCloseMainMenu.Invoke();
    }
}
