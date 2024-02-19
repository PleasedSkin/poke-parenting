using UnityEngine;
using UnityEngine.EventSystems;

public class DetailedMenuItemLink : MenuItemLink
{
    [SerializeField]
    private ComportementScriptableObject comportementScriptableObject;

    void Start() 
    {
        GetComponent<TMPro.TMP_Text>().text = $"<link>{comportementScriptableObject.label}</link>";
    }

    public void SetComportementScriptableObject(ComportementScriptableObject behaviour) {
        this.comportementScriptableObject = behaviour;
    }

    protected override void DeclencherEvent(PointerEventData pointerEventData) {
        EventManager.TriggerSelectDetailedCommandMenuItem(comportementScriptableObject.points);
    }


}
