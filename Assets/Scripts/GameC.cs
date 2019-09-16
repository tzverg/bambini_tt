using System.Collections.Generic;
using UnityEngine;

public class GameC : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject scorePanel;

    public static bool paused = false;
    public static List<GameObject> enemyList;

    #region Monobehaviour API

    private void Start()
    {
        SwitchView();
        SetTimeScale();
    }

    public void CheckEnemyList()
    {
        if (enemyList.Count == 0)
        {
            SwitchGameState();
        }
    }

    public void SwitchGameState()
    {
        paused = !paused;

        SwitchView();
        SetTimeScale();
    }

    public void GameOver()
    {
        paused = true;

        SwitchView();
        SetTimeScale();
    }

    private void SwitchView()
    {
        menuPanel.SetActive(paused);
        //scorePanel.SetActive(!paused);
    }

    private static void SetTimeScale()
    {
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    #endregion
}
