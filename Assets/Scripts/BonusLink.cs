using UnityEngine;
using UnityEngine.EventSystems;

public class BonusLink : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private Cursor cursor;

    [SerializeField] protected string label;

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
}
