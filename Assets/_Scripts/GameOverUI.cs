using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        retryButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
    void Start()
    {
        PlayerMovement.Instance.OnGameEnd += on_game_end;
        Hide();
    }

    private void on_game_end(object sender, EventArgs e)
    {
        Show();
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
