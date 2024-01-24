using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Runtime.InteropServices;


public class GameManager : MonoBehaviour
{
    private ScoreManager scoreManager;

    //number of words taken from each list per level.
    [Header("Words Per Round")]
    public int numberWordsPerLevel = 5;

    //word texts
    public TextMeshProUGUI wrongCharXTMP;

    // game canvases
    public Canvas gameCanvas;
    public Canvas endGameCanvas;

    public TextMeshProUGUI textCopied2;

    private char[] wordCharArray;
    private int wordCharArraySize;
    
    private bool wordCompleted = false;
    private bool canType = true;

    private List<string> wordList;

    private int level = 1; //todo-ck i can prob find a better way to do this...

    private static string wordList3Char = "word-list-3char";
    private static string wordList4Char = "word-list-4char";
    private static string wordList5Char = "word-list-5char";

    private int numberOfWordsCompletedThisLevel = 0;

    Boolean gameFinished = false;

    //private string successColor = "70CF7F";
    public Color successColor = new Color(0.439f, 0.812f, 0.498f, 1f);



    [SerializeField]
    private GameObject LetterParent;
    public GameObject letterPrefab;
    public float letterOffset = 2.5f;
    private List<GameObject> letterList;
    private int currentLetterPosition = 0;
     


    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);


    private void ResetProperties()
    {
        currentLetterPosition = 0;
        wordCompleted = false;
    }

 
    void Start()
    {
        Application.targetFrameRate = 60;
        scoreManager = GetComponent<ScoreManager>();

        letterList = new List<GameObject>();

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
        if (currentLetterPosition == wordCharArraySize && !wordCompleted)
        {
            scoreManager.SetScore(3);
            wordCompleted = true;
            //todo-ck logic to change to the next word and reset.
            ChangeWord();
        }

        //check on keystroke if we typed the right letter
        if (Input.anyKeyDown && !IsMouseButtonClick() && canType && !gameFinished) 
        {

            if (currentLetterPosition < wordCharArraySize)
            {
                char[] tempArray = Input.inputString.ToCharArray();
                char inputChar = '\0';                
                if (tempArray.Length>0)
                {
                    inputChar = tempArray[0];
                } 


                if (inputChar != '\0' && inputChar == wordCharArray[currentLetterPosition])
                {
                    //set color of word to success!!!
                    GameObject currentLetter = letterList[currentLetterPosition];
                    currentLetter.GetComponent<TextMeshPro>().color = successColor;

                    currentLetterPosition++;
                    scoreManager.SetScore(2);
                    CalculateLongestStreak();

                } else
                {
                    scoreManager.IncrementFailures();
                    scoreManager.SetScore(-1);
                    scoreManager.ResetKeystrokeStreak();
                    StartCoroutine(ShowTextTemporarily());
                    
                }
            }
        }

        if (!gameFinished)
        {
            scoreManager.TickElapsedTime(Time.deltaTime);
        }

        //if (Input.anyKeyDown && !IsMouseButtonClick() && canType && gameFinished)
        //{
        //    StartNewGame();
        //}
    }

    private void CalculateLongestStreak()
    {
        scoreManager.IncrementKeystrokeStreak();
    }

    private IEnumerator ShowTextTemporarily()
    {
        canType = false;
        wrongCharXTMP.enabled = true;
        yield return new WaitForSeconds(0.25f);
        wrongCharXTMP.enabled = false;
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
        Debug.Log("Level " + level);
        if (numberOfWordsCompletedThisLevel >= numberWordsPerLevel)
        {
            if (level == 1) //todo-ck we need to refactor this out.
            {
                level++;
                numberOfWordsCompletedThisLevel = 0;
                AssignWordList(wordList4Char);
                ChangeWord(); //todo-ck this is also not great, need to refactor out
                
            } else if (level == 2)
            {
                level++;
                numberOfWordsCompletedThisLevel = 0;
                AssignWordList(wordList5Char);
                ChangeWord(); //todo-ck this needs to be refactored out
                
            } else if (level >= 3) //fyi greater or equal, dont forget
            {
                EndGame();
            }
        } else
        {
            //get the next word and remove it.
            string nextWord = wordList[0];
            wordList.RemoveAt(0);

            //keep track of the number of words completed in this level
            //todo-ck move to a level manager script
            numberOfWordsCompletedThisLevel++;

            //put the characters into an array so that we can do our input checks

            wordCharArray = nextWord.ToLower().ToCharArray();
            wordCharArraySize = wordCharArray.Length;

            //destroy old list and draw the next word on the scene
            DestroyGameObjectWordList();
            CreateGameObjectWordList(wordCharArray);
            
            ResetProperties();
        }
    }
    
    //Create a list of game objects that spell a word, and draw them to screen.
    private void CreateGameObjectWordList(char[] wordCharArray)
    {
        currentLetterPosition = 0;

        float firstLetterPositionX = -6;
        foreach (char c in wordCharArray)
        {
            //worry about position in a minute
            //Vector3 position = new Vector3(firstLetterPositionX, 0.5f, 0);
            //firstLetterPositionX = firstLetterPositionX + letterOffset;
            GameObject newLetter = Instantiate(letterPrefab, new Vector3(0,0,0), Quaternion.identity);
            newLetter.transform.SetParent(LetterParent.transform, false);
            newLetter.name = "offset" + firstLetterPositionX;
            newLetter.GetComponent<TextMeshPro>().text = c.ToString().ToUpper();

            letterList.Add(newLetter);
        }
    }

    //Destroys all the game objects in the word list.
    private void DestroyGameObjectWordList()
    {
        foreach (GameObject go in letterList)
        {
            Destroy(go);
        }

        letterList.Clear();

    }

    private void AssignWordList(string fileName)
    {
        wordList = new List<string>();
        currentLetterPosition = 0; //todo-ck spaghetti
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
        DestroyGameObjectWordList();
        gameFinished = true;
        gameCanvas.enabled = false;
        endGameCanvas.enabled = true;
        scoreManager.DisplayEndGameStats();
    }
    
    private void StartNewGame()
    {
        scoreManager.ResetStats();
        gameFinished = false;
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
        string textToCopy = "Time: " + scoreManager.GetElapsedTime() + " seconds   " + Environment.NewLine
            + "Score: " + scoreManager.GetScore() + Environment.NewLine
            + "Keystroke Streak: " + scoreManager.GetKeyStrokeMax() + "   " + Environment.NewLine
            + "Total Missed Keys: " + scoreManager.GetFailures();
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
