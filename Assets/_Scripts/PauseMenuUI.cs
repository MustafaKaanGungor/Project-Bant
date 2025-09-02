using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject settingsMenu;

    private void Awake()
    {
        retryButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
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
    
    private void OnDestroy() {
        GameManager.Instance.OnStateChanged -= on_state_changed;
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
