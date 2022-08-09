using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace CustomUtils {
    /// <summary>
    /// Serializing and Deserializing ScoreSystem to JSON
    /// </summary>
    public class SaveAndRestoreScoreList {
        public ScoreSystem[] ScoreList;

        private static readonly string PersistentPath =
            Application.persistentDataPath + Path.AltDirectorySeparatorChar + "score.json";

        /// <summary>
        /// Constructor -> instantiating the scoreList
        /// </summary>
        public SaveAndRestoreScoreList() {
            LoadScore();
        }

        /// <summary>
        /// loading the Score
        /// Updating the score if the input parameter is higher than an old value
        /// Writing the new ScoreSystem[] to the file
        /// </summary>
        /// <param name="newScore">score the player accomplished</param>
        public void SaveScore(ScoreSystem newScore) {
            LoadScore();

            for (int i = 0; i < ScoreList.Length; i++) {
                if (ScoreList[i].CurrentScore >= newScore.CurrentScore) continue;
                ScoreList[i] = newScore;
                break;
            }

            using StreamWriter writer = new StreamWriter(PersistentPath);
            Array.Sort(ScoreList);

            writer.Write(JsonConvert.SerializeObject(ScoreList));

            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// checking if the File Exists
        /// reading the file and deserializing JSON Objects to ScoreSystem()
        ///     if deserialization is null : creating a new ScoreSystem[] 
        /// </summary>
        private void LoadScore() {
            CheckFileExists();

            using StreamReader reader = new StreamReader(PersistentPath);
            string fromJson = reader.ReadToEnd();

            ScoreList = JsonConvert.DeserializeObject<ScoreSystem[]>(fromJson) ??
                        new ScoreSystem[] {new(), new(), new()};

            reader.Close();
        }

        /// <summary>
        /// creating a file under persistentPath if no file exists
        /// </summary>
        private static void CheckFileExists() {
            if (File.Exists(PersistentPath)) return;
            File.Create(PersistentPath).Close();
        }
    }
}