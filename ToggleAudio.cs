using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField] private AudioSource targetAudioSource;

    public void MuteTogle (bool muted)
    {
        if (muted)
        {
            targetAudioSource.volume = 0;

        }
        else
        {
            targetAudioSource.volume = 1;
        }    

    }
        
}
