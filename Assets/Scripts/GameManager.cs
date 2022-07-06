using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    int score;
    int level = 1;
    int levelObjective = 10000;
    int layerCleared;
    public Text scorePoint, level_Text, layers_Text;
    float fallSpeed = 3f;
    [SerializeField] GameObject gameOver_Screen;
    bool gameOver;
    //Menu
    [SerializeField] Text[] scores_Texts;
    int levelHigh, scoreHigh, layerHigh;
    [SerializeField] GameObject highScorePanel;
    bool openHigh;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            scorePoint.text = score.ToString("D9");
            level_Text.text = level.ToString("D2");
            layers_Text.text = layerCleared.ToString("D2");
        }
    }
    public float ReadFallSpeed()
    {
        return fallSpeed;
    }
    public void SetScore(int amount)
    {
        try
        {
            score += amount;
            CalculateLevel();
            scorePoint.text = score.ToString("D9");
        }
        catch (System.Exception error)
        {
            Debug.LogError($"The error: {error}");
        }
        if (score > scoreHigh)
        {
            scoreHigh = score;
            PlayerPrefs.SetInt("score", scoreHigh);
        }
    }

    public void LayerCleared(int amount)
    {
        layerCleared += amount;
        if(amount == 1)
        {
            SetScore(400);
        }
        else if(amount == 2)
        {
            SetScore(800);
        }
        else if(amount == 3)
        {
            SetScore(1600);
        }
        else if(amount == 4)
        {
            SetScore(3200);
        }
        layers_Text.text = layerCleared.ToString("D2");
        if (layerCleared > layerHigh)
        {
            layerHigh = layerCleared;
            PlayerPrefs.SetInt("layer", layerHigh);
        }
    }

    void CalculateLevel()
    {
        if(score > levelObjective && level <= 10)
        {
            levelObjective += 10000;
            level++;
            fallSpeed -= 0.2f;
            level_Text.text = level.ToString("D2");
        }
        if (level > levelHigh)
        {
            levelHigh = level;
            PlayerPrefs.SetInt("level", levelHigh);
        }
    }

    public bool returnGameOver()
    {
        return gameOver;
    }
    public void SetGameOver()
    {
        gameOver = true;
        StartCoroutine(Over());
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    IEnumerator Over()
    {
        yield return new WaitForSeconds(1.2f);
        gameOver_Screen.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void OpenHighScore_Panel()
    {
        scoreHigh = PlayerPrefs.GetInt("score",0);
        levelHigh = PlayerPrefs.GetInt("level",0);
        layerCleared = PlayerPrefs.GetInt("layer",0);
        scores_Texts[0].text = "Highest Score: " + scoreHigh;
        scores_Texts[1].text = "Highest Level: " + levelHigh;
        scores_Texts[2].text = "Highest Layer: " + layerHigh;
        openHigh = !openHigh;
        highScorePanel.SetActive(openHigh);
    }
}
