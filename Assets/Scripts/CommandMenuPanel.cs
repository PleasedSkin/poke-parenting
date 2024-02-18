using System.IO;
using UnityEngine;

public class CommandMenuPanel : MonoBehaviour
{

    [SerializeField]
    private Cursor cursor;

    public GameObject menuItemLinkPrefab;

    public GameObject detailedMenuItemLinkPrefab;

    void OnEnable() {
        cursor.SetPanel(gameObject);  
        EventManager.ChangeTopLevelCommandMenuRequired += ChangeTopLevelCommandMenu;
        EventManager.ChangeDetailedCommandMenuRequired += ChangeDetailedCommandMenu;
    }

    private void ChangeTopLevelCommandMenu(string menuType) {
        // On supprime les enfants actuels
        if (transform.childCount > 0) {
            foreach (Transform child in transform) {
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
        }

        // On instancie les nouveaux enfants
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/ScriptableObjects/" + menuType);
		DirectoryInfo[] info = dir.GetDirectories ();

		foreach (DirectoryInfo f in info)
		{
            Debug.Log(f.Name);
            var mama = Instantiate(menuItemLinkPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            mama.GetComponent<MenuItemLink>().menuType = menuType;
            mama.GetComponent<MenuItemLink>().SetLabel(f.Name);
            mama.GetComponent<MenuItemLink>().SetCursor(cursor);
            mama.transform.SetParent(transform);
		}
        cursor.SetPanel(gameObject); // Pour qu'il remette à jour ses positions



        // plops = Resources.LoadAll("Scripts/ScriptableObjects/" + menuType, typeof(Texture2D));
        // // Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        // foreach (var t in plops)
        // {
        //     Debug.Log(t.name);
        // }

    }

    private void ChangeDetailedCommandMenu(string menuType, string detailedMenuType) {

        // TODO : des factorisations à faire

        // On supprime les enfants actuels
        if (transform.childCount > 0) {
            foreach (Transform child in transform) {
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
        }

        var comportementsSo = Resources.LoadAll<ComportementScriptableObject>("ScriptableObjects/" + menuType + "/" + detailedMenuType);

		foreach (var comportementSo in comportementsSo)
		{
            Debug.Log(comportementSo.label);
            var mama = Instantiate(detailedMenuItemLinkPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            mama.GetComponent<DetailedMenuItemLink>().menuType = menuType;
            mama.GetComponent<DetailedMenuItemLink>().SetComportementScriptableObject(comportementSo);
            mama.GetComponent<DetailedMenuItemLink>().SetCursor(cursor);
            mama.transform.SetParent(transform);
		}
        cursor.SetPanel(gameObject); // Pour qu'il remette à jour ses positions



        // plops = Resources.LoadAll("Scripts/ScriptableObjects/" + menuType, typeof(Texture2D));
        // // Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        // foreach (var t in plops)
        // {
        //     Debug.Log(t.name);
        // }

    }


    void OnDisable() 
    {
        EventManager.ChangeTopLevelCommandMenuRequired -= ChangeTopLevelCommandMenu;
        EventManager.ChangeDetailedCommandMenuRequired -= ChangeDetailedCommandMenu;
    }
}
