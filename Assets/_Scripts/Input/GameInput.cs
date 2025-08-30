using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private BantInput inputActions;
    public event EventHandler OnJumpPerformed;
    public event EventHandler OnAimPerformed;
    public event EventHandler OnAimCanceled;
    public event EventHandler OnFirePerformed;
    public event EventHandler OnFireCanceled;
    public event EventHandler OnPausePerformed;

    private void Awake()
    {
        Instance = this;
        inputActions = new BantInput();
        inputActions.Gameplay.Enable();

        inputActions.Gameplay.Jump.performed += on_jump_performed;
        inputActions.Gameplay.Aim.performed += on_aim_performed;
        inputActions.Gameplay.Aim.canceled += on_aim_canceled;
        inputActions.Gameplay.Fire.performed += on_fire_performed;
        inputActions.Gameplay.Fire.canceled += on_fire_canceled;
        inputActions.Gameplay.Pause.performed += on_pause_performed;

        Disablekeys();
    }

    private void on_pause_performed(InputAction.CallbackContext context)
    {
        OnPausePerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Start()
    {
        PlayerMovement.Instance.OnGameEnd += on_game_ended;
    }

    private void on_game_ended(object sender, EventArgs e)
    {
        inputActions.Gameplay.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void on_aim_performed(InputAction.CallbackContext context)
    {
        OnAimPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void on_aim_canceled(InputAction.CallbackContext context)
    {
        OnAimCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void on_fire_performed(InputAction.CallbackContext context)
    {
        OnFirePerformed?.Invoke(this, EventArgs.Empty);
    }

    private void on_fire_canceled(InputAction.CallbackContext context)
    {
        OnFireCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void on_jump_performed(InputAction.CallbackContext context)
    {
        OnJumpPerformed?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        return inputActions.Gameplay.Movement.ReadValue<Vector2>();
    }

    public void Disablekeys()
    {
        inputActions.Gameplay.Disable();
    }

    public void EnableKeys()
    {
        inputActions.Gameplay.Enable();
    }
}
