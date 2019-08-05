using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreBoard : MonoBehaviour {
    public Transform entryContainer;
    public Transform entryTemplate;
    void Awake() {
        entryTemplate.gameObject.SetActive(false);

        for (int i = 0; i < 200; i++) {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer); 
            entryTransform.gameObject.SetActive(true);

            entryTransform.Find("pos").GetComponent<TMPro.TextMeshProUGUI>().text = GetRankText(i + 1); // to avoid 0
            entryTransform.Find("score").GetComponent<TMPro.TextMeshProUGUI>().text = Random.Range(0, 10000).ToString();
            entryTransform.Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = "AAA";
        }
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
}
