using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponent : MonoBehaviour
{
    public static UIComponent instance;
    public AudioClip clickClip;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        myAudioSource = GetComponent<AudioSource>();
    }

   public void ClickSound()
    {
        myAudioSource.PlayOneShot(clickClip);
    }
}
