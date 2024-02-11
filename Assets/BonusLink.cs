using UnityEngine;
using UnityEngine.EventSystems;

public class BonusLink : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private Cursor cursor;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        var correspondAItemDejaSelectionne = cursor.PointTowardsCorrespondingMenuItem(transform.position);
        if (correspondAItemDejaSelectionne) {
            Debug.Log(pointerEventData.pointerEnter.gameObject.tag);
        }
    }
}
