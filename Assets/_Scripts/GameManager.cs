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

    }

    private void Update()
    {

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
}
