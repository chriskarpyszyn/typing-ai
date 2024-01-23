using UnityEngine;

namespace Dan
{
    public class LeaderboardCreatorConfig : ScriptableObject
    {
        public TextAsset leaderboardsFile;
#if UNITY_EDITOR
        public TextAsset editorOnlyLeaderboardsFile;
#endif
    }
}