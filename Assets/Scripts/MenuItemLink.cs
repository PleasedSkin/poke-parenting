using UnityEngine.EventSystems;


public class MenuItemLink : BonusLink
{
    
    public string menuType;

    protected override void DeclencherEvent(PointerEventData pointerEventData) {
        EventManager.TriggerChangeDetailedCommandMenu(menuType, label);
    }

}
