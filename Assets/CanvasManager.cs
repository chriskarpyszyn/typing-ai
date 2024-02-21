using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    public TextMeshProUGUI textTotalTime;
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateFinalScore()
    {
        
        textTotalTime.text = "Highscore: " + gm.scoreManager.GetElapsedTime();

    }
}
