using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPreferences : MonoBehaviour
{

    private int soundEnabled = 1;
    private AudioSource _audio;
    private bool toggleSound = true;

    public Sprite audioOn, audioOff;

    void Start()
    {
        _audio = Object.FindObjectOfType<AudioSource>();
        if (PlayerPrefs.HasKey("SoundEnabled"))
        {
            soundEnabled = PlayerPrefs.GetInt("SoundEnabled");
        }
        InitSound();
    }

    public void InitSound ()
    {
        if (soundEnabled == 1)
        {
            SetSound(true);
        }
        else
        {
            SetSound(false);
        }
    }

    public void ToggleSound ()
    {
        toggleSound = !toggleSound;
        SetSound(toggleSound);
    }

    public void SetSound (bool sound)
    {

        if (sound)
        {
            PlayerPrefs.SetInt("SoundEnabled", 1);
            _audio.mute = false;
            transform.GetComponent<Image>().sprite = audioOn;
            transform.GetComponent<Image>().color = new Color(0.3f, 1f, 0.3f);
        }
        else
        {
            PlayerPrefs.SetInt("SoundEnabled", 0);
            _audio.mute = true;
            transform.GetComponent<Image>().sprite = audioOff;
            transform.GetComponent<Image>().color = new Color(1f, 0.3f, 0.3f);

        }

        PlayerPrefs.Save();
    }

}
