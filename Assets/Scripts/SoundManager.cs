using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip buttonClick, winSound, clickFailSound, buySound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonClick ()
    {
        audioSource.PlayOneShot(buttonClick);
    }

    public void PlayClickSound()
    {
        float vol = audioSource.volume;
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(winSound);
        audioSource.volume = vol;
    }

    public void PlayClickFailSound()
    {
        float vol = audioSource.volume;
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(clickFailSound);
        audioSource.volume = vol;
    }

    public void PlayBuySound()
    {
        float vol = audioSource.volume;
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(buySound);
        audioSource.volume = vol;
    }

}
