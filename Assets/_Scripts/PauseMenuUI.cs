using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject settingsMenu;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
        settingsButton.onClick.AddListener(() =>
        {
            settingsMenu.SetActive(true);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += on_state_changed;

        Hide();
    }

    private void on_state_changed(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsPaused())
        {
            Show();
        }   
        else if (GameManager.Instance.IsPlaying())
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);

        
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        
        
    }
}
