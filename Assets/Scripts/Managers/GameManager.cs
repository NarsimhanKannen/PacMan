using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region VARIABLES
    public static GameManager Instance;

    public GameObject gameoverPanel;
    public GameObject winnerPanel;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI progressText;

    private GameObject player;

    private int health = 3;

    private bool playerAlive = true;
    private bool gameOver = false;

    #endregion

    #region INITIALIZATION
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        gameOver = false;
        ResetHealth();
    }
    #endregion

    #region PLAYER HEALTH FUNCTIONS
    private void ResetHealth()
    {
        health = 3;
        SetHealthText(health);
        SetPlayerAliveState(true);
        gameoverPanel.SetActive(false);
    }

    public void ReducePlayerHealth()
    {
        health--;
        SetHealthText(health);
        CheckPlayerHealth();
    }

    private void CheckPlayerHealth()
    {
        if (health < 1)
        {
            StartCoroutine(GameOver());
            SetPlayerAliveState(false);
        }
    }
    private void SetHealthText(int health)
    {
        healthText.text = "Lives: " + health;
    }

    #endregion

    #region PLAYER ALIVE 
    public void KillPlayer()
    {
        health = 0;
        SetHealthText(health);
        CheckPlayerHealth();
        SetPlayerAliveState(false);
    }

    private void SetPlayerAliveState(bool val)
    {
        playerAlive = val;
    }

    public bool isPlayerAlive()
    {
        return playerAlive;
    }

    #endregion

    #region PLAYER OBJECT
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public GameObject GetPlayer()
    {
        if (this.player != null)
            return player;
        return null;
    }
    #endregion

    public void SetProgressBar(float value)
    {
        progressText.text = "Progress: " + Mathf.FloorToInt(value) + "/100%";
    }

    private IEnumerator GameOver()
    {
        Vector3 pos = GetPlayer().GetComponent<PacManBehaviour>().GetCurrentPos();
        VFXManager.Instance.PlayFX((int)pos.z, (int)pos.x);
        yield return new WaitForSeconds(0.5f);
        gameoverPanel.SetActive(true);
    }

    private IEnumerator PlayerWon()
    {
        yield return new WaitForSeconds(0.5f);
        winnerPanel.SetActive(true);
    }

    private void Update()
    {
        if (LevelManager.Instance.GetPercentageFilled() > 80 && !gameOver)
        {
            gameOver = true;
            StartCoroutine(PlayerWon());
        }
    }
}
