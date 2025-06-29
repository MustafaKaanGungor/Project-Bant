using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("hyeo?");
            LoadTestLevel();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadPlaygroundLevel();
        }
    }

    public void LoadTestLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadPlaygroundLevel()
    {
        SceneManager.LoadScene(0);
    }
}
