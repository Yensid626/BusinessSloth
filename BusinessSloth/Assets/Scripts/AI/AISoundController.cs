using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISoundController : MonoBehaviour
{
    // Start is called before the first frame update
    public string soundFile = "FootStep4";
    internal AudioSource sound;
    internal Entity381 entity;

    void Start()
    {
        entity = gameObject.GetComponent<Entity381>();
        SetupAudio();
    }

    // Update is called once per frame
    public void Tick(float dt)
    {
        sound.volume = entity.speed / entity.maxSpeed;
    }

    void SetupAudio()
    {
        sound = gameObject.AddComponent<AudioSource>();
        sound.clip = Resources.Load("Audio/" + soundFile, typeof(AudioClip)) as AudioClip;
        //sound.mute = true;
        sound.volume = 1;
        sound.loop = true;
        sound.spatialBlend = 1f;
        sound.Play();
    }
}
