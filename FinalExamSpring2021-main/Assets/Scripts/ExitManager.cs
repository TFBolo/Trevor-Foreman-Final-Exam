using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ExitManager : MonoBehaviour
{
    private Transform entryTemplate;
    private Transform entryContainer;
    private List<Transform> highscoreEntryTransformList;
    public static bool scoreDelete = false;

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);
        if (!scoreDelete)
        {
            AddHighscoreEntry(MainManager.score, IntroManager.userName);
        }
        else
        {
            scoreDelete = false;
        }
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default: rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }
        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;

        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        Highscores highscores = new Highscores();
        try
        {
            string jsonString = PlayerPrefs.GetString("highscoreTable");
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
            if (highscores.highscoreEntryList.Count >= 10)
            {
                if (highscoreEntry.score > highscores.highscoreEntryList[highscores.highscoreEntryList.Count - 1].score)
                {
                    highscores.highscoreEntryList.RemoveAt(highscores.highscoreEntryList.Count - 1);
                    highscores.highscoreEntryList.Add(highscoreEntry);
                }
            }
            else
            {
                highscores.highscoreEntryList.Add(highscoreEntry);
            }

        }
        catch
        {
            List<HighscoreEntry> startList = new List<HighscoreEntry> { highscoreEntry };
            highscores = new Highscores { highscoreEntryList = startList };
        }

        string jsonScore = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", jsonScore);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }

    public void DeleteScores()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        highscores.highscoreEntryList.Clear();

        string jsonScore = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", jsonScore);
        PlayerPrefs.Save();
        scoreDelete = true;

        SceneManager.LoadScene(2);
    }

}
