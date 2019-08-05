using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreBoard : MonoBehaviour {
    [System.Serializable]
    private class Highscores {
        public List<HighscoreEntry> highscoreList;
    }

    public Transform entryContainer;
    public Transform entryTemplate;
    List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>();

    public void DrawScoreTable() {
        entryTemplate.gameObject.SetActive(false);
        var highscores = JsonUtility.FromJson<Highscores>(PlayerPrefs.GetString("highscoreTable"));
        for (int i = 0; i < highscores.highscoreList.Count; i++) {
            AddHighscoreEntry(highscores.highscoreList[i], i + 1);
        }
    }

    public void SaveHighscore(int score, string name) {
        var highscores = JsonUtility.FromJson<Highscores>(PlayerPrefs.GetString("highscoreTable"));
        highscores.highscoreList.Add(new HighscoreEntry { score = score, name = name });
        highscores.highscoreList = highscores.highscoreList.OrderBy(entry => entry.score).Reverse().ToList();
        var json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    public string GetRankText(int position) {
        string rankString;
        switch(position) {
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
            default: rankString = position + "TH"; break;
        }
        return rankString;
    }
    public void AddHighscoreEntry(HighscoreEntry entry, int position) {
        Transform entryTransform = Instantiate(entryTemplate, entryContainer); 
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("pos").GetComponent<TMPro.TextMeshProUGUI>().text = GetRankText(position); // to avoid 0
        entryTransform.Find("score").GetComponent<TMPro.TextMeshProUGUI>().text = entry.score.ToString();
        entryTransform.Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = entry.name;

        entryTransform.gameObject.GetComponent<Image>().enabled = position % 2 == 0 ? true : false;
    }
}
