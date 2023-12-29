using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI textMeshPro;
    public TextMeshProUGUI textMeshProSuccess;
    private char[] charArray;
    private int charArraySize;
    private int successCount = 0;
    private bool wordCompleted = false;

    private List<string> wordList;
    
    
    
    void Start()
    {
        Application.targetFrameRate = 60;

        //Debug.Log(textMeshPro.text);
        string textMeshArray = textMeshPro.text.ToLower(); //todo-ck maybe not the best soln here
        charArray = textMeshArray.ToCharArray();
        charArraySize = textMeshArray.Length;

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

        //check if we're successfully compelted all letters!
        if (successCount == charArraySize && !wordCompleted)
        {
            Debug.Log("SUCCESSFULLY COMPLETED WORD!");
            wordCompleted = true;
            //todo-ck logic to change to the next word and reset.
            changeWord();
        }

        //check on keystroke if we typed the right letter
        if (Input.anyKeyDown && !isMouseButtonClick())
        {

            if (successCount < charArraySize)
            {
                char[] tempArray = Input.inputString.ToCharArray();
                char theChar = '\0';                
                if (tempArray.Length>0)
                {
                    theChar = tempArray[0];
                } 


                if (theChar != '\0' && theChar == charArray[successCount])
                {
                    Debug.Log("Success");
                    textMeshProSuccess.text = textMeshProSuccess.text + theChar.ToString().ToUpper();
                    successCount++;

                    //Debug.Log("Success Count: " + successCount);
                    //Debug.Log("Char Array Size:" + charArraySize);

                    //todo-ck logic to change the color of the chars

                } else
                {
                    Debug.Log("Fail"); 
                }
            }


        }
    }

    private bool isMouseButtonClick()
    {
        return (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2));
    }

    private void changeWord()
    {
        textMeshPro.text = "DOG";
        string textMeshArray = textMeshPro.text.ToLower(); //todo-ck maybe not the best soln here
        charArray = textMeshArray.ToCharArray();
        charArraySize = textMeshArray.Length;
        resetProperties();
    }

    private void resetProperties()
    {
        successCount = 0;
        wordCompleted = false;
        textMeshProSuccess.text = "";
    }

}
