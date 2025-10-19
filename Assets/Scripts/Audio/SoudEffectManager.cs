using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoudEffectManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioclip;
    private void Start()
    {
        audioSource.Play();
    }

    public void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = audioclip;
            audioSource.Play();
        }
    }


}
