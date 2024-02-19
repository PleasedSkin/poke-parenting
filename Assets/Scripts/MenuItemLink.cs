using UnityEngine.EventSystems;


public class MenuItemLink : BonusMalusLink
{
    
    protected override void DeclencherEvent(PointerEventData pointerEventData) {
        EventManager.TriggerChangeDetailedCommandMenu(behaviorState, label);
    }

}
