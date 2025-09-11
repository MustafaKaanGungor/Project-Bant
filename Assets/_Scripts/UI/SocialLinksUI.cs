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
            Application.OpenURL("https://www.instagram.com/woodenpillowgames/?utm_source=ig_web_button_share_sheet");
        });
        tiktok.onClick.AddListener(() =>
        {
            Application.OpenURL("https://woodenpillowgames.itch.io/tape-over");
        });
        discord.onClick.AddListener(() =>
        {
            Application.OpenURL("https://www.tiktok.com/@woodenpillow_games?is_from_webapp=1&sender_device=pc");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
