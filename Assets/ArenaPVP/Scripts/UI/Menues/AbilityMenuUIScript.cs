using Assets.ArenaPVP.Scripts.Enums;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuUIScript : MonoBehaviour
{
    public Button CloseButton;
    public GameObject AbilityDisplayPrefab;
    public Transform SkillGrid;


    // Start is called before the first frame update
    void OnEnable()
    {
        CloseButton.onClick.AddListener(CloseMenu);

        Entity player = FindObjectsByType<Entity>(FindObjectsSortMode.None).FirstOrDefault(p => p.IsOwnedByMe);

        ClearGrid();

        foreach (var ability in AbilityStorage.AllAbilities.Where(a => a.AbilityInfo.ClassType == player.ClassType || a.AbilityInfo.ClassType == CharacterClassType.None))
        {
            var abilityDisplay = Instantiate(AbilityDisplayPrefab, SkillGrid);
            var holder = abilityDisplay.GetComponent<AbilityUIDisplay>();
            holder.Ability = ability;
        }
    }

    public void CloseMenu()
    {
        UIEvents.OnCloseSubMenu.Invoke();
    }

    public void ClearGrid() 
    {
        for (int i = 0; i < SkillGrid.childCount; i++)
        {
            Destroy(SkillGrid.GetChild(i).gameObject);
        }
    }
}
