using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

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

    private LevelManager levelManager;
    private Dictionary<int, WordListSO> levelToWordListMap;
    private List<Word> currentWordList;
    private int currentLevel;
    
    private Word currentWord;

    private int numWordsProvidedThisLevel; //numWordsCompleted
    private int specialWordRandPosition;


    //word handler prob doesnt need to even know about gamehandler
    public void Initialize(LevelManager levelManager)
    {
        currentWordList = new List<Word>();

        this.levelManager = levelManager;
        this.levelManager.OnLevelChanged += HandleLevelChanged;
        this.inputHandler.OnLetterInput += HandleLetterInput;

        InitializeLevelWordMap();
        ResetForNewLevel(levelManager.GetCurrentLevel());
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
        Debug.Log(inputChar);
        if (CheckLetter(inputChar))
        {
            Debug.Log("TRUE");
            if (IsWordCompleted())
            {
                Debug.Log("Do I get here");
                DestroyWord();
                GetAndSetNextWord();
                DrawWord();
                //level manager - word completed - event? 
            }

            //else - letter is good, word is not completed
        } else
        {
            ////typed wrong letter
            //oldGameManager.GetScoreManager().IncrementFailures();
            //oldGameManager.GetScoreManager().IncreaseScore(-1);
            //oldGameManager.GetScoreManager().ResetKeystrokeStreak();
            //oldGameManager.letterSounds?.playErrorSound();
            //StartCoroutine(oldGameManager.ShowTextTemporarily());
        }


    }

    /// <summary>
    /// Handle Level Changed Event
    /// </summary>
    /// <param name="newLevel">The new level to change to</param>
    public void HandleLevelChanged(int newLevel)
    {
        Debug.Log("Enter: HandleLevelChanged");
        ResetForNewLevel(newLevel);
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

        numWordsProvidedThisLevel++;
        if (numWordsProvidedThisLevel == specialWordRandPosition)
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
    private void ResetForNewLevel(int newLevel)
    {
        //TODO: This feels like I'm doing too much in this class. 
        //And I probably want to move some of the level logic out of the wordObject manager
        //Into the GameHandler.....
        //WordManager should just worry about getting, creating and drawing words and letters
        //And validating them...
        Debug.Log("Enter: RsetForNewLevel");
        currentLevel = newLevel; //dont need class prop?
        AssignWordList();
        GetAndSetNextWord();
        //level 1,2,3 --- all this level stuff should be in the game handler...
        if (currentLevel==1||currentLevel==2||currentLevel==3)
        {
            DrawWord();
        }
        numWordsProvidedThisLevel = 0;
        //I don't want the special wordObject to be first or last
        specialWordRandPosition = Random.Range(2, levelManager.GetWordsPerLevel()); 
    }

    public void DrawWord()
    {
        GameObject wordObject = currentWord.WithPosition(0, -0.5f, -0.2f).CreateCanvas(); //hardcoding level 1 pos
        currentWord.CreateLetterCanvas(wordObject, letterCanvas);
    }
    
    public void DestroyWord() 
    {
        // Destroy(wordGo);
        currentWord.DestroyCanvas();
        //destroy word here
    }

    /// <summary>
    /// Inserts a hardcoded wordObject from the special wordObject list for the appropriate level
    /// </summary>
    /// <returns></returns>
    private Word GetSpecialWord()
    {
        Debug.Log("Enter: Get Special Word");
        if (currentLevel <= specialWords.words.Count)
        {
            return new Word(
                specialWords.words[currentLevel - 1])
                .WithWordCanvas(wordCanvas);
        }
        else
        {
            Debug.LogWarning($"No special wordObject defined for level: {currentLevel}.");
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
        if (levelToWordListMap.TryGetValue(currentLevel, out WordListSO wordListSO))
        {
            foreach (string word in wordListSO.words)
            {
                currentWordList.Add(new Word(word));
            }
        } 
        else
        {
            Debug.LogError($"No wordObject list found for level {currentLevel}");
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
}
