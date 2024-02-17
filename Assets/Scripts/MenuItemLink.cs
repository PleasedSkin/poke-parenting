using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuItemLink : BonusLink
{
    
    [SerializeField]
    private ComportementScriptableObject comportementScriptableObject;


    void Start() 
    {
        GetComponent<TMPro.TMP_Text>().text = $"<link>{comportementScriptableObject.label}</link>";
    }


}
