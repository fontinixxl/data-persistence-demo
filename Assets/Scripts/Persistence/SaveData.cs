using System;
using UnityEngine;

namespace Fontinixxl.Persistence
{
    [Serializable]
    public class SaveData
    {
        public string PlayerName;
        public int HighScore;
    
        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void LoadFromJson(string a_Json)
        {
            JsonUtility.FromJsonOverwrite(a_Json, this);
        }

        public void Reset()
        {
            PlayerName = string.Empty;
            HighScore = 0;
        }
    }
}
