using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuUIScript : MonoBehaviour
{
    public Button closeButton;
    public GameObject SkillDisplayPrefab;
    public Transform SkillGrid;

    MenuHandlerUIScript menuHandler;

    // Start is called before the first frame update
    void Start()
    {
        menuHandler = FindObjectOfType<MenuHandlerUIScript>();
        closeButton.onClick.AddListener(CloseMenu);

        AbilityManager abilityManager = FindObjectOfType<AbilityManager>();
        Player player = FindObjectsOfType<Player>().FirstOrDefault(p => p.IsOwnedByMe);

        ClearGrid();

        foreach (var ability in abilityManager.AllAbilities.Where(a => a.ClassType == player.ClassType))
        {
            var skillDisplay = Instantiate(SkillDisplayPrefab, SkillGrid);
            var holder = skillDisplay.GetComponent<AbilityUIDisplay>();
            holder.Ability = ability;
        }

    }

    public void CloseMenu()
    {
        menuHandler.CloseMenu(); ;
    }

    public void ClearGrid() 
    {
        for (int i = 0; i < SkillGrid.childCount; i++)
        {
            Destroy(SkillGrid.GetChild(i).gameObject);
        }
    }
}
