using UnityEngine;
using UnityEngine.UI;

public class SocialLinksUI : MonoBehaviour
{
    [SerializeField] private Button button1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button1.onClick.AddListener(() =>
        {
            Application.OpenURL("");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
