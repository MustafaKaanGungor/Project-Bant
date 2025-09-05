using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialTexts;
    [SerializeField] private int tutorialIndex = 0;
    [SerializeField] private GameObject tutorialBackground;
    [SerializeField] private bool isTutorialOn = false;

    void Start()
    {
        GameInput.Instance.OnJumpPerformed += on_jump_performed;    
    }

    private void on_jump_performed(object sender, EventArgs e)
    {
        if (tutorialIndex == 0)
        {
            ChangeTutorialStep();
        }
    }

    private void ChangeTutorialStep()
    {
        tutorialTexts[tutorialIndex].SetActive(false);
        tutorialIndex++;
        tutorialTexts[tutorialIndex].SetActive(true);
    }

    void Update()
    {

    }


}
