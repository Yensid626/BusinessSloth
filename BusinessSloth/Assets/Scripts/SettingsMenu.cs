using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioM;
   public void SetVolume(float volume)
    {
        audioM.SetFloat("volume", volume);
    }
}
