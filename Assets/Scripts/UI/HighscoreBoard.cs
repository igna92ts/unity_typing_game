using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreBoard : MonoBehaviour {
    [System.Serializable]
    private class Highscores {
        public List<HighscoreEntry> highscoreList;
        public Highscores() {
            highscoreList = new List<HighscoreEntry>();
        }
    }

    public Transform entryContainer;
    public Transform entryTemplate;
    List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>();
    List<Transform> renderedEntries = new List<Transform>();
    string scorePrefsKey = "coreTable";

    public void DrawScoreTable() {
        entryTemplate.gameObject.SetActive(false);
        var highscores = JsonUtility.FromJson<Highscores>(PlayerPrefs.GetString(scorePrefsKey));
        if (highscores != null) {
            for (int i = 0; i < highscores.highscoreList.Count; i++) {
                AddHighscoreEntry(highscores.highscoreList[i], i);
            }
        }
    }

    public void SaveHighscore(int score, string name) {
        var highscores = JsonUtility.FromJson<Highscores>(PlayerPrefs.GetString(scorePrefsKey));
        if (highscores == null) {
            highscores = new Highscores();
        }
        highscores.highscoreList.Add(new HighscoreEntry { score = score, name = name });
        highscores.highscoreList = highscores.highscoreList.OrderBy(entry => entry.score).Reverse().ToList();
        var json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString(scorePrefsKey, json);
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
    public void AddHighscoreEntry(HighscoreEntry entry, int index) {
        Transform entryTransform;
        if (renderedEntries.Count > index && renderedEntries[index] != null) {
            entryTransform = renderedEntries[index];
        } else {
            entryTransform = Instantiate(entryTemplate, entryContainer); 
            renderedEntries.Add(entryTransform);
        }
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("pos").GetComponent<TMPro.TextMeshProUGUI>().text = GetRankText(index + 1); // to avoid 0
        entryTransform.Find("score").GetComponent<TMPro.TextMeshProUGUI>().text = entry.score.ToString();
        entryTransform.Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = entry.name;

        entryTransform.gameObject.GetComponent<Image>().enabled = index % 2 == 0 ? true : false;
    }
}
