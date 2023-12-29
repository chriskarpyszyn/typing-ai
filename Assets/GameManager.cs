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


        AssignWordList();
        ChangeWord();

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
            ChangeWord();
        }

        //check on keystroke if we typed the right letter
        if (Input.anyKeyDown && !IsMouseButtonClick())
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

    private bool IsMouseButtonClick()
    {
        return (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2));
    }

    private void ChangeWord()
    {

        string nextWord = wordList[0];
        wordList.RemoveAt(0);

        textMeshPro.text = nextWord;
        string textMeshArray = textMeshPro.text.ToLower(); //todo-ck maybe not the best soln here
        charArray = textMeshArray.ToCharArray();
        charArraySize = textMeshArray.Length;
        ResetProperties();
    }

    private void ResetProperties()
    {
        successCount = 0;
        wordCompleted = false;
        textMeshProSuccess.text = "";
    }

    private void AssignWordList()
    {
        wordList = new List<string>();
        foreach (string line in LoadLinesFromFile())
        {
            wordList.Add(line.Trim().ToUpper());
        }
    }

    private string[] LoadLinesFromFile()
    {
        TextAsset textWordList = Resources.Load<TextAsset>("word-list");
        if (textWordList != null)
        {
            string[] lines = textWordList.text.Split('\n');
            return lines;
        }
        return null;
    }
}
