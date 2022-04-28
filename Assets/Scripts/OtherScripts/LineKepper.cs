using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineKepper : MonoBehaviour
{
    public GameObject rodTip;

    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] positions = { transform.position, rodTip.transform.position };
        lr.SetPositions(positions);
    }
}
