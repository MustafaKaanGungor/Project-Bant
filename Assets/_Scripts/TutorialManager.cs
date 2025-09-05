using System;
using System.Collections.Generic;
using CMF;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialTextsKeyboard;
    [SerializeField] private List<GameObject> tutorialTextsJoystick;

    [SerializeField] private int tutorialIndex = 0;
    [SerializeField] private GameObject tutorialBackground;
    [SerializeField] private bool isTutorialOn = false;
    [SerializeField] private bool isJoystick = false;
    [SerializeField] private bool isWPressed = false;
    [SerializeField] private bool isSPressed = false;
    [SerializeField] private bool isAPressed = false;
    [SerializeField] private bool isDPressed = false;
    private Vector2 movementInput = Vector2.zero;


    void Start()
    {
        GameInput.Instance.OnJumpPerformed += on_jump_performed;
        GameInput.Instance.OnFirePerformed += on_fire_performed;

        if (Joystick.current != null)
        {
            isJoystick = true;
            tutorialTextsJoystick[0].SetActive(true);
        }
        else
        {
            isJoystick = false;
            tutorialTextsKeyboard[0].SetActive(true);
        }
    }

    private void on_fire_performed(object sender, EventArgs e)
    {
        if (tutorialIndex == 2)
        {
            ChangeTutorialStep();
        }
    }

    private void on_jump_performed(object sender, EventArgs e)
    {
        if (tutorialIndex == 1)
        {
            ChangeTutorialStep();
        }
    }

    private void ChangeTutorialStep()
    {
        if (tutorialIndex <= tutorialTextsKeyboard.Count - 2)
        {
            if (isJoystick)
            {
                tutorialTextsJoystick[tutorialIndex].SetActive(false);
                tutorialIndex++;
                tutorialTextsJoystick[tutorialIndex].SetActive(true);
            }
            else
            {
                tutorialTextsKeyboard[tutorialIndex].SetActive(false);
                tutorialIndex++;
                tutorialTextsKeyboard[tutorialIndex].SetActive(true);
            }
        }
    }

    void Update()
    {
        if (tutorialIndex == 0)
        {
            movementInput = GameInput.Instance.GetMovementVector();

            if (movementInput.x >= 0.9)
            {
                isAPressed = true;
            }
            if (movementInput.x <= -0.9)
            {
                isDPressed = true;
            }
            if (movementInput.y >= 0.9)
            {
                isWPressed = true;
            }
            if (movementInput.y <= -0.9)
            {
                isSPressed = true;
            }

            if (isWPressed && isAPressed && isDPressed)
            {
                ChangeTutorialStep();
            }
        }
    }


}
