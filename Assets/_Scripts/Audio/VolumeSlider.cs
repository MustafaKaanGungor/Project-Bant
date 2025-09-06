using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    public Slider audioSlider;
    public AudioMixer mixer;

    public static string volumeKey = "vol";
    private float maxValue;

    public void Start() {
        maxValue = audioSlider.maxValue + 1;

        if (PlayerPrefs.HasKey(volumeKey))
            audioSlider.value = PlayerPrefs.GetFloat(volumeKey) * maxValue; //mixer.SetFloat("Master", Mathf.Log10(PlayerPrefs.GetFloat(volumeKey)) * 20);

        else {
            mixer.SetFloat("Master", Mathf.Log10(3 / maxValue * 20));
        }
    }

    public void SetMasterVolume(System.Single value) {
        Debug.Log("master volume level saved.");

        float volume = value / maxValue;

        volume = volume == 0 ? 0.01f : volume;

        mixer.SetFloat("Master", Mathf.Log10(volume) * 20); // -80 + value/5 * 100

        PlayerPrefs.SetFloat(volumeKey, volume);
        PlayerPrefs.Save();
    }
}
