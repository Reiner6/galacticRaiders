using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicComponent : MonoBehaviour
{
    public AudioClip introClip;
    public AudioClip loopClip;

    public AudioSource myAudioSource;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        myAudioSource = GetComponent<AudioSource>();

        StartCoroutine(StartClips(introClip, loopClip));
    }
    /// <summary>
    /// calls music, first the intro and when it finishes goes to the loop audiolip
    /// </summary>
    public IEnumerator StartClips(AudioClip intro, AudioClip loop)
    {
        StopAllCoroutines();
        myAudioSource.Stop();
        myAudioSource.clip = intro;
        myAudioSource.Play();
        myAudioSource.loop = false;

        yield return new WaitForSeconds(intro.length);


        myAudioSource.clip = loop;
        myAudioSource.Play();
        myAudioSource.loop = true;
    }
}