using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dan;
using UnityEditor;
using UnityEngine;

namespace LeaderboardCreatorEditor
{
    public class LeaderboardCreatorWindow : EditorWindow
    {
        [System.Serializable]
        private class SavedLeaderboard
        {
            public string name, publicKey, secretKey;
            
            public SavedLeaderboard(string name, string publicKey, string secretKey)
            {
                this.name = name;
                this.publicKey = publicKey;
                this.secretKey = secretKey;
            }
        }
        
        [System.Serializable]
        private struct SavedLeaderboardList
        {
            public List<SavedLeaderboard> leaderboards;
        }
        
        private const string SAVED_LEADERBOARDS_KEY = "LEADERBOARD_CREATOR___SAVED_LEADERBOARDS";

        private static bool _isAddLeaderboardMenuOpen;
        private static string _name, _publicKey, _secretKey;
        private static Vector2 _scrollPos;

        private static SavedLeaderboardList _savedLeaderboardList;

        private static GUIStyle _titleStyle;
        
        private static LeaderboardCreatorConfig Config => Resources.Load<LeaderboardCreatorConfig>("LeaderboardCreatorConfig");

        [MenuItem("Leaderboard Creator/My Leaderboards")]
        private static void ShowWindow()
        {
            var window = GetWindow<LeaderboardCreatorWindow>();
            window.minSize = new Vector2(400, 475);
            window.titleContent = new GUIContent("Leaderboard Creator");
            window.Show();
        }

        private void OnBecameVisible()
        {
            _titleStyle = new GUIStyle
            {
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState {textColor = Color.white}
            };
            _savedLeaderboardList = GetSavedLeaderboardList();
        }

        private static SavedLeaderboardList GetSavedLeaderboardList()
        {
            var path = AssetDatabase.GetAssetPath(Config.editorOnlyLeaderboardsFile);
            var file = new System.IO.StreamReader(path);
            var json = file.ReadToEnd();
            file.Close();

            if (string.IsNullOrEmpty(json))
            {
                SaveLeaderboardList();
                return new SavedLeaderboardList {leaderboards = new List<SavedLeaderboard>()};
            }
            
            var savedLeaderboardList = JsonUtility.FromJson<SavedLeaderboardList>(json);
            return savedLeaderboardList;
        }

        private static void SaveLeaderboardList()
        {
            _savedLeaderboardList.leaderboards ??= new List<SavedLeaderboard>();
            
            var path = AssetDatabase.GetAssetPath(Config.editorOnlyLeaderboardsFile);
            var json = JsonUtility.ToJson(_savedLeaderboardList);
            
            System.IO.File.WriteAllText(path, json);
            AssetDatabase.Refresh();
        }

        private void OnGUI()
        {
            DisplayLeaderboardsMenu();

            if (!_isAddLeaderboardMenuOpen && GUILayout.Button("Enter New Leaderboard"))
                _isAddLeaderboardMenuOpen = true;
            
            if (_isAddLeaderboardMenuOpen) DisplayEnterNewLeaderboardMenu();

            if (GUILayout.Button("Save to C# Script")) 
                SaveLeaderboardsToScript();

            if (GUILayout.Button("Manage Leaderboards"))
                Application.OpenURL("https://danqzq.itch.io/leaderboard-creator");

            DrawSeparator();
            
            if (GUILayout.Button("<color=#2a9df4>Made by @danqzq</color>",
                    new GUIStyle{alignment = TextAnchor.LowerRight, richText = true}))
                Application.OpenURL("https://www.danqzq.games");
        }

        private static void DrawSeparator()
        {
            GUILayout.Space(10);
            var rect = EditorGUILayout.BeginHorizontal();
            Handles.color = Color.gray;
            Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        private static void DisplayLeaderboardsMenu()
        {
            GUILayout.Space(10);
            GUILayout.Label("My Leaderboards", _titleStyle);
            
            if (_savedLeaderboardList.leaderboards.Count == 0)
            {
                GUILayout.Label("You don't have any saved leaderboards.");
                return;
            }

            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(200));
            for (var i = 0; i < _savedLeaderboardList.leaderboards.Count; i++)
            {
                GUILayout.Space(10);
                GUILayout.Label("Leaderboard #" + (i + 1), EditorStyles.boldLabel);
                
                var savedLeaderboard = _savedLeaderboardList.leaderboards[i];
                savedLeaderboard.name = EditorGUILayout.TextField("Name", savedLeaderboard.name);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Copy Public Key"))
                    EditorGUIUtility.systemCopyBuffer = savedLeaderboard.publicKey;
                if (GUILayout.Button("Copy Secret Key"))
                    EditorGUIUtility.systemCopyBuffer = savedLeaderboard.secretKey;
                GUILayout.EndHorizontal();

                if (!GUILayout.Button("Forget Leaderboard"))
                    continue;

                _savedLeaderboardList.leaderboards.Remove(savedLeaderboard);
                SaveLeaderboardList();
                break;
            }

            GUILayout.EndScrollView();
        }

        private static void DisplayEnterNewLeaderboardMenu()
        {
            DrawSeparator();
            GUILayout.Label("Enter New Leaderboard", _titleStyle);

            _name      = EditorGUILayout.TextField("Name", _name);
            _publicKey = EditorGUILayout.TextField("Public Key", _publicKey);
            _secretKey = EditorGUILayout.TextField("Secret Key", _secretKey);

            if (GUILayout.Button("Add Leaderboard"))
                EnterNewLeaderboard();
            
            if (GUILayout.Button("Cancel"))
                _isAddLeaderboardMenuOpen = false;
                
            DrawSeparator();
        }

        private static void EnterNewLeaderboard()
        {
            if (string.IsNullOrEmpty(_publicKey) || string.IsNullOrEmpty(_secretKey))
            {
                EditorUtility.DisplayDialog("Leaderboard Creator Error", "Please fill all the fields.", "OK");
                return;
            }
            
            if (!ValidateLeaderboardName(_name)) return;

            _savedLeaderboardList = GetSavedLeaderboardList();
            if (_savedLeaderboardList.leaderboards.Exists(l => l.name == _name))
            {
                EditorUtility.DisplayDialog("Leaderboard Creator Error", "You already have a leaderboard with that name.", "OK");
                return;
            }
                
            _savedLeaderboardList.leaderboards.Add(new SavedLeaderboard(_name, _publicKey, _secretKey));
            SaveLeaderboardList();
                
            _name = _publicKey = _secretKey = "";
        }

        private static bool ValidateLeaderboardName(string leaderboardName)
        {
            if (string.IsNullOrEmpty(leaderboardName))
            {
                EditorUtility.DisplayDialog("Leaderboard Creator Error", "Please enter a name.", "OK");
                return false;
            }
            
            if (!Regex.IsMatch(leaderboardName, @"^[a-zA-Z0-9_]+$"))
            {
                EditorUtility.DisplayDialog("Leaderboard Creator Error", "The name can only contain alphabetical letters, numbers and underscores.", "OK");
                return false;
            }

            if (!Regex.IsMatch(leaderboardName, @"^[0-9]"))
                return true;
            
            EditorUtility.DisplayDialog("Leaderboard Creator Error", "The name cannot start with a number.", "OK");
            return false;
        }

        private static void SaveLeaderboardsToScript()
        {
            if (_savedLeaderboardList.leaderboards.Any(savedLeaderboard => !ValidateLeaderboardName(savedLeaderboard.name)))
                return;
            
            SaveLeaderboardList();

            var path = AssetDatabase.GetAssetPath(Config.leaderboardsFile);
            var file = new System.IO.StreamWriter(path);

            file.WriteLine("namespace Dan.Main");
            file.WriteLine("{");
            file.WriteLine("    public static class Leaderboards");
            file.WriteLine("    {");

            foreach (var savedLeaderboard in _savedLeaderboardList.leaderboards)
            {
                file.WriteLine($"        public static LeaderboardReference {savedLeaderboard.name} = " +
                               $"new LeaderboardReference(\"{savedLeaderboard.publicKey}\");");
            }
                
            file.WriteLine("    }");
            file.WriteLine("}");
            file.Close();
            AssetDatabase.Refresh();
        }
    }
}