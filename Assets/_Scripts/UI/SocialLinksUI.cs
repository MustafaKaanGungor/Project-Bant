using UnityEngine;
using UnityEngine.UI;

public class SocialLinksUI : MonoBehaviour
{
    [SerializeField] private Button instagram;
    [SerializeField] private Button tiktok;
    [SerializeField] private Button discord;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instagram.onClick.AddListener(() =>
        {
            Application.OpenURL("");
        });
        tiktok.onClick.AddListener(() =>
        {
            Application.OpenURL("");
        });
        discord.onClick.AddListener(() =>
        {
            Application.OpenURL("");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
