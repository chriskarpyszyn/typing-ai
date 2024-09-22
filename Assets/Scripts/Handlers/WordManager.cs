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

    private LevelManager levelManager;
    private Dictionary<int, WordListSO> levelToWordListMap;
    private List<string> currentWordList;
    private int currentLevel;
    private int wordsProvidedThisLevel;
    private int specialWordPosition;


    private void Awake()
    {
        InitializeLevelWordMap();
    }



    public void Initialize(LevelManager levelManager)
    {
        this.levelManager = levelManager;
        this.levelManager.OnLevelChanged += HandleLevelChanged;

        InitializeLevelWordMap();
        ResetForNewLevel(levelManager.GetCurrentLevel());
    }

    /// <summary>
    /// Returns a word from the wordlist.
    /// </summary>
    /// <returns></returns>
    public string GetNextWord()
    {
        if (currentWordList == null || currentWordList.Count == 0)
        {
            Debug.LogError("WordList is empty or null");
            return string.Empty;
        }

        wordsProvidedThisLevel++;

        if (wordsProvidedThisLevel == specialWordPosition)
        {
            return GetSpecialWord() ?? GetRegularWord();
        }

        return GetRegularWord();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newLevel"></param>
    public void HandleLevelChanged(int newLevel)
    {
        ResetForNewLevel(newLevel);
    }

    /// <summary>
    /// Reset properties when changing level.
    /// </summary>
    private void ResetForNewLevel(int newLevel)
    {
        currentLevel = newLevel; //dont need class prop
        AssignWordList();
        wordsProvidedThisLevel = 0;
        //I don't want the special word to be first or last
        specialWordPosition = Random.Range(2, levelManager.GetWordsPerLevel()); 
    }

    /// <summary>
    /// Inserts a hardcoded word from the special word list for the appropriate level
    /// </summary>
    /// <returns></returns>
    private string GetSpecialWord()
    {
        if (currentLevel <= specialWords.words.Count)
        {
            return specialWords.words[currentLevel - 1];
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
    private string GetRegularWord()
    {
        int randomIndex = Random.Range(0, currentWordList.Count);
        string returnWord = currentWordList[randomIndex];
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
            currentWordList = new List<string>(wordListSO.words);
        } 
        else
        {
            Debug.LogError($"No word list found for level {currentLevel}");
            currentWordList = new List<string>();
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
