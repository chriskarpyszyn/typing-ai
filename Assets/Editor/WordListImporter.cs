using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WordListImporter : EditorWindow
{
    private TextAsset textFile;
    private WordListSO wordListSO;

    [MenuItem("Tools/Word List Importer")]
    public static void ShowWindow()
    {
        GetWindow<WordListImporter>("Word List Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Import Words from Text File", EditorStyles.boldLabel);

        textFile = (TextAsset)EditorGUILayout.ObjectField("Test File", textFile, typeof(TextAsset), false);
        wordListSO = (WordListSO)EditorGUILayout.ObjectField("Word List Asset", wordListSO, typeof(WordListSO), false);

        if (GUILayout.Button("Import Words"))
        {
            if (textFile != null && wordListSO != null)
            {
                ImportWords();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please assign both a Text File and a Word List asset.", "OK");
            }
        }
    }

    private void ImportWords()
    {
        string[] lines = textFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        wordListSO.words = new List<string>(lines);
        EditorUtility.SetDirty(wordListSO);
        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("Success", "Words imported successfully!", "OK");
    }
}
