using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public TextMeshPro tmp;
    
    void Start()
    {
        Application.targetFrameRate = 60;

    }

    
    void Update()
    {
        //sneaky exit
        if (Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        if (Input.anyKeyDown && !isMouseButtonClick())
        {
            Debug.Log(Input.inputString);
        }


    }

    private bool isMouseButtonClick()
    {
        return (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2));
    }
}
