using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputVisualize : MonoBehaviour
{
    // Start is called before the first frame update
    public Image WButton;
    public Image SButton;
    public Image AButton;
    public Image DButton;
    public Image LCTRLButton;
    public Image LMBButton;
    public Image RMBButton;
    public Image SpaceButton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            WButton.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            WButton.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SButton.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            SButton.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            AButton.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            AButton.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DButton.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            DButton.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            LCTRLButton.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            LCTRLButton.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            LMBButton.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            LMBButton.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RMBButton.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            RMBButton.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpaceButton.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SpaceButton.color = Color.white;
        }

    }
}
