using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMenuPanel : MonoBehaviour
{

    [SerializeField]
    private Cursor cursor;

    void OnEnable() {
        cursor.SetPanel(this.gameObject);    
    }
}
