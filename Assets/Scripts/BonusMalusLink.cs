using UnityEngine;
using UnityEngine.EventSystems;

public class BonusMalusLink : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private Cursor cursor;

    [SerializeField] protected string label;

    [SerializeField] protected BehaviorStateEnum behaviorState;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        var correspondAItemDejaSelectionne = cursor.PointTowardsCorrespondingMenuItem(transform.position);
        if (correspondAItemDejaSelectionne) {
            //Debug.Log(pointerEventData.pointerEnter.gameObject.tag);
            // TODO : générer liens du répertoire -> event
            DeclencherEvent(pointerEventData);
        }
    }

    protected virtual void DeclencherEvent(PointerEventData pointerEventData) {
        EventManager.TriggerChangeTopLevelCommandMenu(pointerEventData.pointerEnter.gameObject.tag);
    }

    public void SetLabel(string inputLabel) {
        label = inputLabel;
        GetComponent<TMPro.TMP_Text>().text = $"<link>{label}</link>";
    }

    public void SetCursor(Cursor aCursor) {
        cursor = aCursor;
    }

    public void SetBehaviorState(BehaviorStateEnum behaviorStateEnum) {
        behaviorState = behaviorStateEnum;
        var redColor = new Color(0.7169812f, 0.1907295f, 0.1589534f, 1f);
        var greenColor = new Color(0.2080649f, 0.4716981f, 0.1935742f, 1f);
        GetComponent<TMPro.TMP_Text>().color = behaviorState == BehaviorStateEnum.BONUS ? greenColor : redColor;
    }
}
