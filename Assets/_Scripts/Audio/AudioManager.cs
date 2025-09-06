using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour{
    public List<AudioSource> audioSources = new List<AudioSource>();
    public GameObject audioSourcePrefab;
    public SoundEffect stickSound;
    public SoundEffect stickSound2;
    public SoundEffect stickSound3;

    public SoundEffect fireSound;

    public SoundEffect music;

    public static AudioManager instance;

    void Awake(){
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }


        DontDestroyOnLoad(this.gameObject);
    }

    public void StopAll() {

        foreach (var item in AudioManager.instance.audioSources) {
            if (item.clip == null) continue;

            item.Pause();
        }
    }

    public void ContinueAll() {

        foreach (var item in AudioManager.instance.audioSources) {
            if (item.clip == null) continue;

            item.Play();
        }
    }

    public void PlaySound(SoundEffect sound, bool playReverse = false){

        AudioSource audioSource = GetAudioSource();

        if (sound == null | audioSource == null) return;

        sound.Play(audioSource, playReverse);
    }

    public void StopSound(SoundEffect sound) {
        if (sound == null) return;

        sound.Stop();
    }

    public void StartFadeOut(SoundEffect sound, float dur) {
        if (sound == null | sound.source == null) return;

        StartCoroutine(_FadeOut(sound.source, dur));
    }

    public void PlaySoundWithDelay(SoundEffect sound, 
        float delay, bool playReverse = false){

        StartCoroutine(_PlaySoundWithDelay(sound, delay, playReverse));
    }

    private IEnumerator _PlaySoundWithDelay(SoundEffect sound, 
        float delay, bool playReverse = false) {

        yield return new WaitForSeconds(delay);
        PlaySound(sound, playReverse);
    }
    public IEnumerator _FadeOut(AudioSource audioSource, 
        float FadeTime) {

        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private AudioSource GetAudioSource() {
        for (int i = 0; i < audioSources.Count; i++) {
            if (audioSources[i].isPlaying) 
                continue;
            else 
                return audioSources[i];
        }

        AudioSource audioSource = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();
        audioSource.transform.SetParent(this.transform);
        audioSources.Add(audioSource);
        return null;
    }

}
