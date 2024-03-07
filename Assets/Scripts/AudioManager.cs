using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip levelUpSound;

    [SerializeField]
    private AudioClip hurtSound;

    [SerializeField]
    private AudioClip eggSound;

    private int currentLevel;


    void OnEnable() {
        EventManager.BroadcastLevel += PlayChangeLevelSound;
    }


    private void PlayChangeLevelSound(int newLevel, bool isLoadingDataContext) {
        if (!isLoadingDataContext) {
            if (newLevel == 0 && newLevel != currentLevel) {
                PlayClip(eggSound);
            } else {
                if (newLevel > currentLevel) {
                    PlayClip(levelUpSound);
                } else if (newLevel < currentLevel) {
                    PlayClip(hurtSound);
                }
            }
        }

        currentLevel = newLevel;
    }

    private void PlayClip(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.Play();
    }


}
