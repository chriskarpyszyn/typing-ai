using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    /**
     * TODO
     * - GameManager one instance, does not get destroyed.
     * - ScoreManager, remove all attempts at modify UI in this class
     * - Move them to a separate CanvasManager script for each level.
     * - Leaderboard takes int, not float, need to do math 
     */

    
    //number of words taken from each list per level.
    [Header("Words Per Round")]
    public int numberWordsPerLevel = 5;

    //word texts
    public TextMeshProUGUI wrongCharXTMP;

    // game canvases
    //public Canvas gameCanvas;
    //public Canvas endGameCanvas;

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

    private string ANIMATION_STATE = "ShrinkAnimation";
    private Animator animator;
    private bool changeWordNow = false;

    private int numberOfWordsCompletedThisLevel = 0;

    Boolean gameFinished = false;

    //private string successColor = "70CF7F";
    public Color successColor = new Color(0.439f, 0.812f, 0.498f, 1f);

    public ScoreManager scoreManager;


    [SerializeField]
    private GameObject LetterParent;
    public GameObject letterPrefab;
    public float letterOffset = 2.5f;
    private List<GameObject> letterList;
    private int currentLetterPosition = 0;


    public UnityEvent<string, string> submitScoreEvent;


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
        Debug.Log("start");
        scoreManager = ScoreManager.Instance;
        if (SceneManager.GetActiveScene().buildIndex==1)
        {
            Debug.Log("Scene 1 Start()");
            letterList = new List<GameObject>();

            AssignWordList(wordList3Char);
            ChangeWord();
        }

        if (LetterParent!=null)
        {
            animator = LetterParent.GetComponent<Animator>();

        }

    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayBackgroundNoise();
    }

    public ScoreManager GetScoreManager()
    {
        return ScoreManager.Instance;
    }

    private bool IsAnimatorPlaying()
    {
        if (animator!=null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName(ANIMATION_STATE) && stateInfo.normalizedTime < 1.0f;
        }
        return false;
    }

    private void ResetAnimation()
    {
        if (animator!=null)
        {
            animator.Play(ANIMATION_STATE, 0, 0f);
            animator.Update(0);
            animator.enabled = false;
            changeWordNow = true;
        }
    }


    void Update()
    {
       if (!IsAnimatorPlaying())
        {
            ResetAnimation();
        }

        //sneaky exit
        if (Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        //check if we're successfully compelted all letters!
        if (SceneManager.GetActiveScene().buildIndex == 1 && currentLetterPosition == wordCharArraySize && !wordCompleted)
        {
            scoreManager.IncreaseScore(3);
            wordCompleted = true;
            animator.enabled = true;
            animator.Play(ANIMATION_STATE, 0, 0f);
            //todo-ck logic to change to the next word and reset.
            
        }

        //todo-ck I hate this....
        if (changeWordNow)
        {
            changeWordNow = false;
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
                    //currentLetter.transform.Find("LetterParticle").gameObject.GetComponent<ParticleSystem>().Play(); //todo-ck extract hardcoded string
                    currentLetter.GetComponentInChildren<ParticleSystem>().Play();

                    currentLetterPosition++;
                    scoreManager.IncreaseScore(2);
                    CalculateLongestStreak();

                } else
                {
                    //todo-ck extract this into an error / wrong letter method for clarity at... some.... point...
                    scoreManager.IncrementFailures();
                    scoreManager.IncreaseScore(-1);
                    scoreManager.ResetKeystrokeStreak();
                    LetterParent.GetComponent<LetterSounds>().playErrorSound();
                    StartCoroutine(ShowTextTemporarily());
                    
                }
            }
        }

        if (SceneManager.GetActiveScene().buildIndex == 1 && !gameFinished)
        {
            //scoreManager.IncreaseElapsedTime(Time.deltaTime);
            ScoreManager.Instance.IncreaseElapsedTime(Time.deltaTime);
        }
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
        //Debug.Log("Level " + level);
        //Debug.Log("Number of words completed " + numberOfWordsCompletedThisLevel);
        //Debug.Log("Number of words per level " + numberWordsPerLevel);
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
            int randomInt = new System.Random().Next(0, wordList.Count);
            string nextWord = wordList[randomInt];
            wordList.RemoveAt(randomInt);

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
        SceneManager.LoadScene(2); //todo-ck i hate having hard coded constants, breaks if I add  another scene
        DestroyGameObjectWordList();
        gameFinished = true;
        scoreManager.DisplayEndGameStats();
    }
    
    public void StartNewGame()
    {
        Debug.Log("START NEW GAME!");
        scoreManager.ResetStats();
        ResetProperties();
        numberOfWordsCompletedThisLevel = 0;
        gameFinished = false;
        SceneManager.LoadScene(1);
        level = 1; //todo-ck need a level manager.
        AssignWordList("word-list-3char"); //todo-ck refactor repeated code, youll know.
        ChangeWord();

    }

    public void SubmitScore()
    {
        //Debug.Log("SubmitScore --> Username: " + inputHighScoreName.text + " Score: " + int.Parse(textScore.text));
        // TMP_InputField inputHighScoreName;
        //LeaderboardInputField

        Debug.Log("SUBMIT SCORE FUNCTION");

        //TODO-CK-17 Refactor this out.
        GameObject inputHighScoreNameObject = GameObject.Find("LeaderboardInputField");
        if (inputHighScoreNameObject != null)
        {
            TMP_InputField inputHighScoreName = inputHighScoreNameObject.GetComponent<TMP_InputField>();
            submitScoreEvent.Invoke(inputHighScoreName.text, scoreManager.GetElapsedTimeString()); //testing the string value of score
        }
        
    }

    public void CopyText()
    {
        StartCoroutine(ShowCopiedTextTemporarily());
        //Debug.Log("Time: " + elapsedTime + " seconds");
        //Debug.Log("Keystroke Streak: " + keystrokeStreakMax);
        //Debug.Log("Missed Keys: " + failures);
        string textToCopy = "Time: " + scoreManager.GetElapsedTimeString() + " seconds   " + Environment.NewLine
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
