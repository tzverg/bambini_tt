using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Text ScoreL;
    public GameC gameC;

    public static bool gameOver;

    private static int scoreValue;
    private static bool changeScoreValue;

    void Awake()
    {
        gameC = FindObjectOfType<GameC>();
        scoreValue = 0;
        changeScoreValue = true;

        gameOver = false;
        GameC.paused = false;

        if (!gameC)
        {
            Debug.LogError("GameC object not found");
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitButton()
    {
        #if UNITY_ANDROID || UNITY_STANDALONE_WIN
        Application.Quit();
        #endif
    }

    public static void AddSoresNum(int scoreV)
    {
        scoreValue = scoreValue + scoreV;
        changeScoreValue = true;
    }

    private void Update()
    {
        if (changeScoreValue)
        {
            ScoreL.text = scoreValue.ToString();
            changeScoreValue = false;
        }
        if (gameOver)
        {
            gameC.GameOver();
        }
    }
}