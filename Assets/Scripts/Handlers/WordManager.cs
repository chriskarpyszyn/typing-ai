using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordManager : MonoBehaviour
{

    [System.Serializable]
    public class LevelToWordListPair
    {
        public int level;
        public WordListSO wordList;
    }

    [SerializeField] private InputHandler inputHandler;

    [SerializeField] private List<LevelToWordListPair> levelWordListPairs;
    [SerializeField] private WordListSO specialWords;

    [SerializeField] private WordCanvas wordCanvas;
    [SerializeField] private LetterCanvas letterCanvas;
    [SerializeField] private TextMeshProUGUI TMPwrongCharX;

    private LevelManager levelManager;
    private Dictionary<int, WordListSO> levelToWordListMap;
    private List<Word> currentWordList;
    
    private Word currentWord;

    private int specialWordRandPosition;

    private AudioSource errorAudioSource;


    public void Initialize(LevelManager levelManager)
    {

        inputHandler = FindObjectOfType<InputHandler>();

        inputHandler.OnLetterInput += HandleLetterInput;
        errorAudioSource = GetComponent<AudioSource>();

        currentWordList = new List<Word>();

        this.levelManager = levelManager;
        this.levelManager.OnLevelChanged += HandleLevelChanged;
        this.levelManager.OnNextWord += HandleNextWord;
        

        InitializeLevelWordMap();
    }

    public void OnDisable()
    {
        if (levelManager != null)
        {
            levelManager.OnLevelChanged -= HandleLevelChanged;
        }
        inputHandler.OnLetterInput -= HandleLetterInput;
    }

    public void HandleLetterInput(char inputChar)
    {
        if (CheckLetter(inputChar))
        {
            Debug.Log("TRUE");
            //scoreManager.IncreaseScore(2);
            //scoreManager.IncrementKeystrokeStreak();

            if (IsWordCompleted())
            {
                Debug.Log("Do I get here");
                DestroyWord();
                levelManager.WordCompleted();
            }
        } else
        {
            ////typed wrong letter
            //oldGameManager.GetScoreManager().IncrementFailures();
            //oldGameManager.GetScoreManager().IncreaseScore(-1);
            //oldGameManager.GetScoreManager().ResetKeystrokeStreak();

            new Sounds().PlaySound(errorAudioSource, 1.2f, 0.3f);
            
            StartCoroutine(ShowTextTemporarily());
        }
    }

    public void HandleNextWord()
    {
        GetAndSetNextWord();
        DrawWord();
    }

    /// <summary>
    /// Handle Level Changed Event
    /// </summary>
    /// <param name="newLevel">The new level to change to</param>
    public void HandleLevelChanged(int newLevel)
    {
        Debug.Log("Enter: HandleLevelChanged: " + newLevel);
        ResetForNewLevel();
    }


    /// <summary>
    /// Returns a wordObject from the wordlist.
    /// </summary>
    /// <returns></returns>
    public Word GetAndSetNextWord()
    {
        Debug.Log("Enter: GetAndSetNextWord");
        if (currentWordList == null || currentWordList.Count == 0)
        {
            Debug.LogError("WordList is empty or null");
            currentWord = null;
            return currentWord;
        }

        if (levelManager.GetWordsCompletedInCurrentLevel() == specialWordRandPosition)
        {
            currentWord = GetSpecialWord() ?? GetRegularWord();
            return currentWord;
        }

        currentWord = GetRegularWord();
        return currentWord;
    }

    public bool CheckLetter(char inputChar)
    {
        return currentWord.ValidateLetter(inputChar);
    }


    public bool IsWordCompleted()
    {
        return currentWord.IsWordCompleted();
    }

    /// <summary>
    /// TODO: REFACTOR Reset properties when changing level.
    /// </summary>
    private void ResetForNewLevel()
    {
        Debug.Log("Enter: RsetForNewLevel");
        //I don't want the special wordObject to be first or last
        specialWordRandPosition = Random.Range(2, levelManager.GetWordsPerLevel());

        AssignWordList();
        GetAndSetNextWord();
        //level 1,2,3 --- all this level stuff should be in the game handler...
        int currentLevel = levelManager.GetCurrentLevel();
        if (currentLevel==1||currentLevel==2||currentLevel==3)
        {
            DrawWord();
        }
    }

    public void DrawWord()
    {
        //todo-ck extract hardcoded values, maybe.
        GameObject wordObject = currentWord.WithPosition(0, -0.5f, -0.5f).CreateCanvas(); //hardcoding level 1 pos
        currentWord.CreateLetterCanvas(wordObject, letterCanvas);
        currentWord.GetLetterAtCurrentIndex().LetterScaleIncreaseFX();
    }
    
    public void DestroyWord() 
    {
        currentWord.CanvasShrinkFX();
        currentWord.DestroyCanvas();
    }

    /// <summary>
    /// Inserts a hardcoded wordObject from the special wordObject list for the appropriate level
    /// </summary>
    /// <returns></returns>
    private Word GetSpecialWord()
    {
        Debug.Log("Enter: Get Special Word");
        if (levelManager.GetCurrentLevel() <= specialWords.words.Count)
        {
            return new Word(
                specialWords.words[levelManager.GetCurrentLevel() - 1])
                .WithWordCanvas(wordCanvas);
        }
        else
        {
            Debug.LogWarning($"No special wordObject defined for level: {levelManager.GetCurrentLevel()}.");
            return null;
        }
    }

    /// <summary>
    /// Get's a random wordObject from the wordObject list.
    /// </summary>
    /// <returns></returns>
    private Word GetRegularWord()
    {
        Debug.Log("Enter: Get Regular Word");
        int randomIndex = Random.Range(0, currentWordList.Count);
        Word returnWord = currentWordList[randomIndex].WithWordCanvas(wordCanvas);
        currentWordList.RemoveAt(randomIndex);
        return returnWord;
    }

    /// <summary>
    /// Assigns the appropriate list of words for the current level
    /// </summary>
    private void AssignWordList()
    {
        Debug.Log("Enter: AssignWordList");

        currentWordList.Clear();
        if (levelToWordListMap.TryGetValue(levelManager.GetCurrentLevel(), out WordListSO wordListSO))
        {
            foreach (string word in wordListSO.words)
            {
                currentWordList.Add(new Word(word));
            }
        } 
        else
        {
            Debug.LogError($"No wordObject list found for level {levelManager.GetCurrentLevel()}");
            currentWordList = new List<Word>();
        }
    }

    /// <summary>
    /// Sets the Level to Word List map
    /// </summary>
    private void InitializeLevelWordMap()
    {
        levelToWordListMap = new Dictionary<int, WordListSO>();
        foreach (var pair in levelWordListPairs)
        {
            levelToWordListMap[pair.level] = pair.wordList;
        }
    }

    private IEnumerator ShowTextTemporarily()
    {
        inputHandler.SetBlockTyping(true);
        TMPwrongCharX.enabled = true;
        yield return new WaitForSeconds(0.25f);
        TMPwrongCharX.enabled = false;
        inputHandler.SetBlockTyping(false);
    }
}
