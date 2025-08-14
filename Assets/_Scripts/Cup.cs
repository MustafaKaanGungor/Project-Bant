using UnityEngine;
using UnityEngine.VFX;

public class Cup : MonoBehaviour
{
    [SerializeField] private GameObject brokenModel;
    [SerializeField] private GameObject tapedModel;
    [SerializeField] private VisualEffect poofEffect;
    private bool isFixed = false;

    public void GetTapedIdiot()
    {
        if (!isFixed)
        {
            brokenModel.SetActive(false);
            tapedModel.SetActive(true);
            poofEffect.Play();
            //isFixed = true;

        }
    }
}
