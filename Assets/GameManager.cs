using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Runtime.InteropServices;


public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI textMeshPro;
    public TextMeshProUGUI textMeshProSuccess;
    public TextMeshProUGUI textMeshProFailure;

    public Canvas gameCanvas;
    public Canvas endGameCanvas;

    //todo-ck refactor these names to be clearer tmp
    public TextMeshProUGUI textElapsedTime;
    public TextMeshProUGUI textStreak;
    public TextMeshProUGUI textFailures;
    public TextMeshProUGUI textCopied2;

    private char[] charArray;
    private int charArraySize;
    private int successCount = 0;
    private bool wordCompleted = false;
    private bool canType = true;

    private List<string> wordList;


    //stats
    private int keystrokeStreak = 0;
    private int keystrokeStreakMax = 0;
    private int failures = 0;
    private float elapsedTime = 0f;

    private int level = 1; //todo-ck i can prob find a better way to do this...

    private static string wordList3Char = "word-list-3char";
    private static string wordList4Char = "word-list-4char";
    private static string wordList5Char = "word-list-5char";




    Boolean gameFinished = false;

    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);


    private void ResetProperties()
    {
        successCount = 0;
        wordCompleted = false;
        textMeshProSuccess.text = "";
    }

    private void ResetStats()
    {
        keystrokeStreakMax = 0;
        keystrokeStreak = 0;
        failures = 0;
        elapsedTime = 0;
        gameFinished = false;
    }

    void Start()
    {
        Application.targetFrameRate = 60;

        //Debug.Log(textMeshPro.text);
        string textMeshArray = textMeshPro.text.ToLower(); //todo-ck maybe not the best soln here
        charArray = textMeshArray.ToCharArray();
        charArraySize = textMeshArray.Length;


        AssignWordList(wordList3Char);
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
            wordCompleted = true;
            //todo-ck logic to change to the next word and reset.
            ChangeWord();
        }

        //check on keystroke if we typed the right letter
        if (Input.anyKeyDown && !IsMouseButtonClick() && canType && !gameFinished) 
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
                    textMeshProSuccess.text = textMeshProSuccess.text + theChar.ToString().ToUpper();
                    successCount++;
                    CalculateLongestStreak();

                } else
                {
                    failures++;
                    keystrokeStreak = 0;
                    StartCoroutine(ShowTextTemporarily());
                    
                }
            }
        }

        if (!gameFinished)
        {
            elapsedTime += Time.deltaTime;
        }

        if (Input.anyKeyDown && !IsMouseButtonClick() && canType && gameFinished)
        {
            StartNewGame();
        }
    }

    private void CalculateLongestStreak()
    {
        keystrokeStreak++;
        if (keystrokeStreak > keystrokeStreakMax)
        {
            keystrokeStreakMax = keystrokeStreak;
        } 
    }

    private IEnumerator ShowTextTemporarily()
    {
        canType = false;
        textMeshProFailure.enabled = true;
        yield return new WaitForSeconds(0.25f);
        textMeshProFailure.enabled = false;
        canType = true;
    }

    //todo-ck refactor copied method
    private IEnumerator ShowCopiedTextTemporarily()
    {
        textCopied2.enabled = true;
        yield return new WaitForSeconds(0.5f);
        textCopied2.enabled = false;
    }

    private bool IsMouseButtonClick()
    {
        return (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2));
    }

    private void ChangeWord()
    {
        if (wordList.Count == 0)
        {
            if (level == 1) //todo-ck we need to refactor this out.
            {
                level++;
                AssignWordList(wordList4Char);
                ChangeWord(); //todo-ck this is also not great, need to refactor out
            } else if (level == 2)
            {
                level++;
                AssignWordList(wordList5Char);
                ChangeWord(); //todo-ck this needs to be refactored out
            } else if (level >= 3) //fyi greater or equal, dont forget
            {
                EndGame();
            }
        } else
        {
            string nextWord = wordList[0];
            wordList.RemoveAt(0);

            textMeshPro.text = nextWord;
            string textMeshArray = textMeshPro.text.ToLower(); //todo-ck maybe not the best soln here
            charArray = textMeshArray.ToCharArray();
            charArraySize = textMeshArray.Length;
            ResetProperties();
        }
 
    }



    private void AssignWordList(string fileName)
    {
        wordList = new List<string>();
        successCount = 0; //todo-ck spaghetti
        foreach (string line in LoadLinesFromFile(fileName))
        {
            wordList.Add(line.Trim().ToUpper());
        }
    }

    private string[] LoadLinesFromFile(string fileName)
    {
        TextAsset textWordList = Resources.Load<TextAsset>(fileName);
        if (textWordList != null)
        {
            string[] lines = textWordList.text.Split('\n');
            return lines;
        }
        return null;
    }

    private void EndGame()
    {
        gameFinished = true;
        gameCanvas.enabled = false;
        endGameCanvas.enabled = true;
        //Debug.Log("Time: " + elapsedTime + " seconds");
        //Debug.Log("Keystroke Streak: " + keystrokeStreakMax);
        //Debug.Log("Missed Keys: " + failures);

        textElapsedTime.text = "Time: " + elapsedTime + " seconds";
        textStreak.text = "Longest Keystroke Streak: " + keystrokeStreakMax;
        textFailures.text = "Total Missed Keys: " + failures;
    }
    
    private void StartNewGame()
    {
        ResetStats();
        AssignWordList("word-list-3char"); //todo-ck refactor repeated code, youll know.
        ChangeWord();
        gameCanvas.enabled = true;
        endGameCanvas.enabled = false;
    }

    public void CopyText()
    {
        StartCoroutine(ShowCopiedTextTemporarily());
        //Debug.Log("Time: " + elapsedTime + " seconds");
        //Debug.Log("Keystroke Streak: " + keystrokeStreakMax);
        //Debug.Log("Missed Keys: " + failures);
        string textToCopy = "Time: " + elapsedTime + " seconds   " + Environment.NewLine 
            + "Keystroke Streak: " + keystrokeStreakMax + "   " + Environment.NewLine
            + "Total Missed Keys: " + failures;
        //GUIUtility.systemCopyBuffer = textToCopy; //ck-not working in webgl
        SetText(textToCopy);
    }

    //public void CopyToClipboardExternal(string text)
    //{
    //    Application.ExternalEval($"copyTextToClipboard(\"{text}\")");
    //}

    public static void SetText(string text)
    {
#if UNITY_WEBGL && UNITY_EDITOR == false
            CopyToClipboard(text);
#else
        GUIUtility.systemCopyBuffer = text;
#endif
    }
}
