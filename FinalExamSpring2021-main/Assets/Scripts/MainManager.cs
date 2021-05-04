using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static int score;

    public int lives;
    public float endTime;
    public float gameTime;

    public Text pointsText;
    public Text livesText;
    public Text nameText;
    public Text timeText;
    public GameObject pauseMenu;

    private void Start()
    {
        score = 0;
        pointsText.text = score.ToString();
        lives = IntroManager.lives;
        livesText.text = lives.ToString();
        nameText.text = IntroManager.userName;
        gameTime = IntroManager.gameTime;
        endTime = Time.time + gameTime;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!(endTime - Time.time <= 0))
        {
            timeText.text = (endTime - Time.time).ToString("F2");
        }

        if (Input.GetKeyDown("escape"))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ScoreUp()
    {
        score++;
        pointsText.text = score.ToString();
    }

    public void ScoreDown()
    {
        score--;
        pointsText.text = score.ToString();
    }

    public void LivesUp()
    {
        lives++;
        livesText.text = lives.ToString();
    }

    public void LivesDown()
    {
        lives--;
        livesText.text = lives.ToString();
    }

    public void Unpause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();

        save.time = endTime - Time.time;
        save.name = IntroManager.userName;
        save.score = score;
        save.lives = lives;

        return save;
    }

    public void SaveGame()
    {
        Save save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            score = save.score;
            pointsText.text = score.ToString();
            lives = save.lives;
            livesText.text = lives.ToString();
            IntroManager.userName = save.name;
            nameText.text = IntroManager.userName;
            gameTime = save.time;
            endTime = Time.time + gameTime;

            Debug.Log("Game Loaded");

            Unpause();
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

    public void SaveAsJSON()
    {
        Save save = CreateSaveGameObject();
        string json = JsonUtility.ToJson(save);

        Debug.Log("Saving as JSON: " + json);
    }
}
