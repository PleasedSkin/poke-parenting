using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int level = 0;
    private int starsAmount = 0;

    void OnEnable() {
        EventManager.SelectDetailedCommandMenuItem += SelectDetailedCommandMenuItem;
    }



    void OnDisable() {
        EventManager.SelectDetailedCommandMenuItem -= SelectDetailedCommandMenuItem;
    }


    
    void SelectDetailedCommandMenuItem(int levelsAmount) {
        SetLevel(levelsAmount);
    }


    private void SetLevel(int levelsAmount) {
        level += levelsAmount;
        level = Mathf.Clamp(level, 0, 100);
        EventManager.TriggerBroadcastLevel(level);
    }
 
}
