using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState
    {
        MAIN_MENU,
        GAMEPLAY,
        PAUSE,
        LOSE,
        VICTORY
    }

    private GameState currentState = GameState.MAIN_MENU;
    public event EventHandler OnStateChanged;
    [SerializeField] private GameObject mainMenuCam;
    [SerializeField] private MMF_Player feel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnPausePerformed += on_pause_performed;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {

    }

    private void on_pause_performed(object sender, EventArgs e)
    {
        if (currentState == GameState.GAMEPLAY)
        {
            currentState = GameState.PAUSE;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
        else if (currentState == GameState.PAUSE)
        {
            currentState = GameState.GAMEPLAY;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void StartGame()
    {
        currentState = GameState.GAMEPLAY;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        GameInput.Instance.EnableKeys();
        mainMenuCam.SetActive(false);
        feel.PlayFeedbacks();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool IsPlaying()
    {
        return currentState == GameState.GAMEPLAY;
    }

    public bool IsPaused()
    {
        return currentState == GameState.PAUSE;
    }
}
