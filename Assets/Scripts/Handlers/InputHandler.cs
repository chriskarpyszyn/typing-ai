using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    public event Action<char> OnLetterInput;
    private bool blockTyping = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown && !IsMouseButtonClick() && !blockTyping)
        {
            char inputChar = ExtractCharFromInput(Input.inputString.ToCharArray());
            if (inputChar != '\0')
            {
                OnLetterInput?.Invoke(inputChar);
            }
        }
    }

    private bool IsMouseButtonClick()
    {
        return (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2));
    }

    private char ExtractCharFromInput(char[] inputCharArray)
    {
        return inputCharArray.Length > 0 ? inputCharArray[0] : '\0';
    }

    public void SetBlockTyping(bool b)
    {
        this.blockTyping = b;
    }
}
