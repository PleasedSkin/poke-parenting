using TMPro;
using UnityEngine;

public class LevelLabel : MonoBehaviour
{
    
    private TextMeshProUGUI labelComponent;


    void OnEnable() {
        EventManager.BroadcastLevel += UpdateLabel;
    }

    void Start() {
        labelComponent = GetComponent<TextMeshProUGUI>();
    }


    private void UpdateLabel(int level) {
        labelComponent.SetText($"Niveau : {level}");
    }


    void OnDisable() {
        EventManager.BroadcastLevel -= UpdateLabel;
    }


}
