using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace CustomUtils {
    public class SaveAndRestoreScoreList {
        public ScoreSystem[] scoreList;

        private static readonly string persistentPath =
            Application.persistentDataPath + Path.AltDirectorySeparatorChar + "score.json";

        public SaveAndRestoreScoreList() {
            LoadScore();
        }

        public void SaveScore(ScoreSystem newScore) {
            LoadScore();

            for (int i = 0; i < scoreList.Length; i++) {
                if (scoreList[i].currentScore >= newScore.currentScore) continue;
                scoreList[i] = newScore;
                break;
            }

            using StreamWriter writer = new StreamWriter(persistentPath);
            Array.Sort(scoreList);

            writer.Write(JsonConvert.SerializeObject(scoreList));

            writer.Flush();
            writer.Close();
        }


        private void LoadScore() {
            CheckFileExists();

            using StreamReader reader = new StreamReader(persistentPath);
            string fromJson = reader.ReadToEnd();

            scoreList = JsonConvert.DeserializeObject<ScoreSystem[]>(fromJson) ??
                        new ScoreSystem[] {new(), new(), new()};

            reader.Close();
        }

        private static void CheckFileExists() {
            if (File.Exists(persistentPath)) return;
            File.Create(persistentPath).Close();
        }
    }
}