using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


}
