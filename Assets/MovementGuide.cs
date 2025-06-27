using UnityEngine;

public class MovementGuide : MonoBehaviour
{
    [SerializeField] public GameObject[] UIelements;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            UIelements[0].SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            UIelements[0].SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            UIelements[1].SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            UIelements[1].SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UIelements[2].SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            UIelements[2].SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UIelements[3].SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            UIelements[3].SetActive(false);
        }
    }
}
