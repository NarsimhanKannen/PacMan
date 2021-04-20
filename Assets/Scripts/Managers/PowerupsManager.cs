using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupsManager : MonoBehaviour
{
    public static PowerupsManager Instance;
    public GameObject powerUp;

    private bool appleActiveInGame = true;

    private int timer = 0;
    public int disableTimer = 0;
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

    public void PickedPowerUp()
    {
        timer = 0;
        CancelInvoke("RefreshPowerUp");
        InvokeRepeating("SlowPower", 0, 1);
        PowerUpState(false);
    }

    private void SlowPower()
    {
        if (timer == 0)
            MonsterManager.Instance.SlowMonsters();
        if (timer > 2)
        {
            MonsterManager.Instance.PowerWearedOff();
            CancelInvoke("SlowPower");

            disableTimer = 0;
            InvokeRepeating("RefreshPowerUp", 0, 1);
        }
        timer++;
    }

    private void PowerUpState(bool val)
    {
        powerUp.SetActive(val);
    }

    private void PowerUpPos(int row, int col)
    {
        powerUp.transform.position = new Vector3(col, 0, row);
    }

    private void SpawnPowerUp()
    {
        int row = Random.Range(1, LevelManager.Instance.GetMaxRow());
        int col = Random.Range(1, LevelManager.Instance.GetMaxCol());

        if (LevelManager.Instance.isCellEmpty(row, col))
        {
            appleActiveInGame = true;

            PowerUpState(true);
            PowerUpPos(row, col);

            disableTimer = 0;
            InvokeRepeating("RefreshPowerUp", 0, 1);
        }
        else
        {
            SpawnPowerUp();
        }
    }

    private void RefreshPowerUp()
    {
        if (disableTimer == 8)
        {
            PowerUpState(false);
            appleActiveInGame = false;
        }
        else if (disableTimer == 13)
        {
            SpawnPowerUp();
            CancelInvoke("RefreshPowerUp");
        }
        disableTimer++;
    }
    private void Start()
    {
        SpawnPowerUp();
    }

    private void Update()
    {
        if (powerUp.activeInHierarchy)
        {
            if (GameManager.Instance.GetPlayer().GetComponent<PacManBehaviour>().GetCurrentPos()
                == powerUp.transform.position && appleActiveInGame)
            {
                PickedPowerUp();
                appleActiveInGame = false;
            }
        }
    }
}
