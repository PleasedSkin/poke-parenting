using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CommandMenuPanel : MonoBehaviour
{

    [SerializeField]
    private Cursor cursor;

    public GameObject menuItemLinkPrefab;
    public GameObject detailedMenuItemLinkPrefab;
    public GameObject returnLinkPrefab;
    public GameObject malusLinkPrefab;
    public GameObject bonusLinkPrefab;


    private BehaviorStateEnum currentBehaviorState;

    private string currentDetailedMenuType;




    void OnEnable() {
        cursor.ResetPositionForPanel(gameObject);  
        EventManager.ChangeTopLevelCommandMenuRequired += ChangeTopLevelCommandMenu;
        EventManager.ChangeDetailedCommandMenuRequired += ChangeDetailedCommandMenu;
        EventManager.ReturnRequired += ReturnToPreviousMenu;
    }

    private void DeleteChildren() {
        if (transform.childCount > 0) {
            foreach (Transform child in transform) {
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
        }
    }



    private void ChangeTopLevelCommandMenu(string behaviorLabel) {
        DeleteChildren();

        // On instancie les nouveaux enfants
        string[] names = Resources.LoadAll<MetaComportementScriptableObject>("ScriptableObjects/" + behaviorLabel).Select(obj => obj.label).ToArray();

		foreach (string name in names)
		{
            var menuItemLink = Instantiate(menuItemLinkPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            var behaviorState = BehaviorStateUtils.DICO_CORRESPONDANCE_LABEL_BEHAVIOR.GetValueOrDefault(behaviorLabel);
            currentBehaviorState = behaviorState;

            menuItemLink.GetComponent<MenuItemLink>().SetBehaviorState(behaviorState);
            menuItemLink.GetComponent<MenuItemLink>().SetLabel(name);
            menuItemLink.GetComponent<MenuItemLink>().SetCursor(cursor);
            menuItemLink.transform.SetParent(transform, false);
		}

        AddReturnLink();
        
        cursor.ResetPositionForPanel(gameObject); // Pour qu'il remette à jour ses positions
    }

    private void ChangeDetailedCommandMenu(BehaviorStateEnum behaviorStateEnum, string detailedMenuType) {
        currentDetailedMenuType = detailedMenuType;
    
        DeleteChildren();

        var comportementsSo = Resources.LoadAll<ComportementScriptableObject>("ScriptableObjects/" + BehaviorStateUtils.DICO_CORRESPONDANCE_BEHAVIOR_LABEL.GetValueOrDefault(behaviorStateEnum) + "/" + detailedMenuType);

		foreach (var comportementSo in comportementsSo)
		{
            // Debug.Log(comportementSo.label);
            var detailedMenuItemLink = Instantiate(detailedMenuItemLinkPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            detailedMenuItemLink.GetComponent<DetailedMenuItemLink>().SetBehaviorState(behaviorStateEnum);
            detailedMenuItemLink.GetComponent<DetailedMenuItemLink>().SetComportementScriptableObject(comportementSo);
            detailedMenuItemLink.GetComponent<DetailedMenuItemLink>().SetCursor(cursor);
            detailedMenuItemLink.transform.SetParent(transform, false);
		}

        AddReturnLink();

        cursor.ResetPositionForPanel(gameObject); // Pour qu'il remette à jour ses positions
    }

    private void AddReturnLink() {
        var returnLink = Instantiate(returnLinkPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        returnLink.GetComponent<ReturnLink>().SetCursor(cursor);
        returnLink.transform.SetParent(transform, false);
    }

    private void ReturnToPreviousMenu() {
        if (currentDetailedMenuType != null) {
            ChangeTopLevelCommandMenu(BehaviorStateUtils.DICO_CORRESPONDANCE_BEHAVIOR_LABEL.GetValueOrDefault(currentBehaviorState));
            currentDetailedMenuType = null;
        } else {
            DeleteChildren();

            ResurrectBonusMalusLinks();
            cursor.ResetPositionForPanel(gameObject); // Pour qu'il remette à jour ses positions
        }
    }

    private void ResurrectBonusMalusLinks() {

        Action<GameObject> CreationLambda = prefab => {
            var bonuMalusLink = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            bonuMalusLink.GetComponent<BonusMalusLink>().SetCursor(cursor);
            bonuMalusLink.transform.SetParent(transform, false);
        };

        //CreationLambda(malusLinkPrefab);
        CreationLambda(bonusLinkPrefab);
    }


    void OnDisable() 
    {
        EventManager.ChangeTopLevelCommandMenuRequired -= ChangeTopLevelCommandMenu;
        EventManager.ChangeDetailedCommandMenuRequired -= ChangeDetailedCommandMenu;
        EventManager.ReturnRequired -= ReturnToPreviousMenu;
    }
}
