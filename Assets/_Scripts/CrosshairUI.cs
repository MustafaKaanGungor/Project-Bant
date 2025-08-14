using System;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    private Image crosshairImage;

    void Awake()
    {
        crosshairImage = GetComponent<Image>();
    }

    private void Start()
    {
        GameInput.Instance.OnAimPerformed += on_aim_performed;
        GameInput.Instance.OnAimCanceled += on_aim_canceled;

        gameObject.SetActive(false);
    }

    private void on_aim_performed(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
    }

    private void on_aim_canceled(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (PlayerMovement.Instance.IsLookingAtGrappleable())
        {
            crosshairImage.color = Color.white;
        }
        else
        {
            crosshairImage.color = Color.red;
        }
    }
}
