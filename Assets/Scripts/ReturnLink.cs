using UnityEngine;

public class ReturnLink : BonusMalusLink
{
    protected override void DeclencherEvent(UnityEngine.EventSystems.PointerEventData pointerEventData) {
        EventManager.TriggerReturn();
    }
}
