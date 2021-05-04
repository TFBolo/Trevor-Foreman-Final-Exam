using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public static string userName;
    public static float gameTime;
    public static int lives;

    public InputField playerName;
    public Slider sliderTime;
    public Dropdown dropdownLives;
    public Text timeText;

    private void Start()
    {
        userName = "";
        gameTime = 60;
        lives = 1;
        timeText.text = sliderTime.value.ToString();
    }

    public void ChangeName()
    {
        userName = playerName.text;
    }

    public void ChangeTime()
    {
        gameTime = sliderTime.value;
        timeText.text = sliderTime.value.ToString();
    }

    public void ChangeLives()
    {
        if (dropdownLives.value == 0)
        {
            lives = 1;
        }
        else
        {
            lives = dropdownLives.value;
        }
    }
}
