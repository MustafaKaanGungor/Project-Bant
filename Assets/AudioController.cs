using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UIElements.Image;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource tape1Source;
    [SerializeField] private AudioSource tape2Source;
    [SerializeField] private AudioSource tape3Source;
    [SerializeField] private AudioSource tape4Source;
    private bool isMuted = false;

    [SerializeField] private GameObject volImage;
    [SerializeField] private GameObject muteImage;
    /*[SerializeField] private UnityEngine.UI.Button button;

    void Start()
    {
        button.onClick.AddListener(() =>
        {
            MuteSound();
        });
    }*/
    public void MuteSound()
    {
        if (isMuted)
        {
            isMuted = false;
            volImage.SetActive(true);
            muteImage.SetActive(false);
            musicSource.volume = 0.5f;
            tape1Source.volume = 0.5f;
            tape2Source.volume = 0.5f;
            tape3Source.volume = 0.5f;
            tape4Source.volume = 0.5f;
        }
        else
        {
            isMuted = true;
            volImage.SetActive(false);
            muteImage.SetActive(true);
            musicSource.volume = 0;
            tape1Source.volume = 0;
            tape2Source.volume = 0;
            tape3Source.volume = 0;
            tape4Source.volume = 0;
        }

    }

}
