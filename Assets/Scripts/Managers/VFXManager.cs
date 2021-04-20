using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    public GameObject particle;

    private int timer = 0;
    private void Start()
    {
        if(!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayFX(int row, int col)
    {
        timer = 0;
        DeathParticle(row, col);
        InvokeRepeating("PlayParticle", 0, 2);
    }

    private void DeathParticle(int row , int col)
    {
        particle.transform.position = new Vector3(col, 0, row);
    }

    private void PlayParticle()
    {
        particle.SetActive(true);
        if(timer>1)
        {
            particle.SetActive(false);
            CancelInvoke("PlayParticle");
        }
        timer++;
    }

}
