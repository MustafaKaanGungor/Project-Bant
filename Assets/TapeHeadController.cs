using UnityEngine;
using System.Collections.Generic;

public class TapeHeadController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject tapeEnd;
    private GameObject tempTape;
    public GameObject tapePartPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        tapeEnd = gameObject;
        tempTape = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            CreateNode();
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            DeleteNode();
        }
    }

    void CreateNode()
    {
        tempTape = Instantiate(tapePartPrefab, tapeEnd.transform.position, Quaternion.identity);
        tapeEnd.GetComponent<HingeJoint>().connectedBody = tempTape.GetComponent<Rigidbody>();
    }

    void DeleteNode()
    {
        
    }
}
