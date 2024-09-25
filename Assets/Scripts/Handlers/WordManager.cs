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


    public void Initialize(LevelManager levelManager)
    {
        this.levelManager = levelManager;
        this.levelManager.OnLevelChanged += HandleLevelChanged;

        InitializeLevelWordMap();
        ResetForNewLevel(levelManager.GetCurrentLevel());
    }

    public void OnDisable()
    {
        if (levelManager != null)
        {
            levelManager.OnLevelChanged -= HandleLevelChanged;
        }
    }

    /// <summary>
    /// Handle Level Changed Event
    /// </summary>
    /// <param name="newLevel">The new level to change to</param>
    public void HandleLevelChanged(int newLevel)
    {
        ResetForNewLevel(newLevel);
    }


    /// <summary>
    /// Returns a word from the wordlist.
    /// </summary>
    /// <returns></returns>
    public Word GetAndSetNextWord()
    {
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

    /// <summary>
    /// Reset properties when changing level.
    /// </summary>
    private void ResetForNewLevel(int newLevel)
    {
        currentLevel = newLevel; //dont need class prop?
        AssignWordList();
        numWordsProvidedThisLevel = 0;
        //I don't want the special word to be first or last
        specialWordRandPosition = Random.Range(2, levelManager.GetWordsPerLevel()); 
    }

    /// <summary>
    /// Inserts a hardcoded word from the special word list for the appropriate level
    /// </summary>
    /// <returns></returns>
    private Word GetSpecialWord()
    {
        if (currentLevel <= specialWords.words.Count)
        {
            return new Word(specialWords.words[currentLevel - 1], wordCanvas);
        }
        else
        {
            Debug.LogWarning($"No special word defined for level: {currentLevel}.");
            return null;
        }
    }

    /// <summary>
    /// Get's a random word from the word list.
    /// </summary>
    /// <returns></returns>
    private Word GetRegularWord()
    {
        int randomIndex = Random.Range(0, currentWordList.Count);
        Word returnWord = currentWordList[randomIndex];
        currentWordList.RemoveAt(randomIndex);
        return returnWord;
    }

    /// <summary>
    /// Assigns the appropriate list of words for the current level
    /// </summary>
    private void AssignWordList()
    {
        if (levelToWordListMap.TryGetValue(currentLevel, out WordListSO wordListSO))
        {
            foreach (string word in wordListSO.words)
            {
                currentWordList.Add(new Word(word));
            }
        } 
        else
        {
            Debug.LogError($"No word list found for level {currentLevel}");
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
