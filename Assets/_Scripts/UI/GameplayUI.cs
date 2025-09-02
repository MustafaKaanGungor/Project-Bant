using System;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnStateChanged += on_game_state_changed;
        Hide();
    }

    private void on_game_state_changed(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsPlaying())
        {
            Show();
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
