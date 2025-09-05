using System;
using UnityEngine;
using UnityEngine.UI;

public class TapeBarUI : MonoBehaviour
{
    [SerializeField] private Image bar;

    private void Start()
    {
        PlayerMovement.Instance.OnTapeAmountChange += on_tape_amount_change;
    }

    void OnDisable()
    {

        PlayerMovement.Instance.OnTapeAmountChange -= on_tape_amount_change;
    }

    void OnEnable()
    {
        PlayerMovement.Instance.OnTapeAmountChange += on_tape_amount_change;
    }

    private void on_tape_amount_change(object sender, EventArgs e)
    {
        bar.fillAmount = PlayerMovement.Instance.HowMuchTapeLeft() / 100;
    }
}
