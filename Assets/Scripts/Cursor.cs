using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    // [SerializeField] private GameObject panel;
    private RectTransform itemRectTransform;
    private RectTransform selfRectTransform;

    private int currentIndex = 0;

    private List<GameObject> itemsMenu = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {

        selfRectTransform = GetComponent<RectTransform>();

        // foreach (Transform child in panel.transform)
        // {
        //     itemsMenu.Add(child.gameObject);
        // }
    }


    public void SetPanel(GameObject panel) {
        itemsMenu = new List<GameObject>();
        foreach (Transform child in panel.transform)
        {
            itemsMenu.Add(child.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % itemsMenu.Count;
            MoveCursor();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex - 1) % itemsMenu.Count;
            if (currentIndex < 0)
            {
                currentIndex = itemsMenu.Count - 1;
            }
            MoveCursor();
        }

    }

    private void MoveCursor()
    {
        itemRectTransform = itemsMenu[this.currentIndex].GetComponent<RectTransform>();
        transform.position = itemsMenu[this.currentIndex].transform.position - (new Vector3(selfRectTransform.sizeDelta.x / 2, 0, 0) * transform.localScale.x) - new Vector3(itemRectTransform.sizeDelta.x / 2, 0, 0);
    }

    public bool PointTowardsCorrespondingMenuItem(Vector3 menuItemPosition)
    {

        var resultat = false;

        for (var i = 0; i < itemsMenu.Count; i++)
        {
            if (itemsMenu[i].transform.position == menuItemPosition)
            {
                if (i == this.currentIndex) {
                    return true;
                }
                this.currentIndex = i;
                MoveCursor();
                break;
            }
        }

        return resultat;

    }

}
