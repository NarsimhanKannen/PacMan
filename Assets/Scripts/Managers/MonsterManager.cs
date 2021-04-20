using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    public MonsterBehaviour[] monsters;

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

    //Controls all the monsters
    public void SlowMonsters()
    {
        foreach (var m in monsters)
        {
            m.SlowPatrol();
        }
    }

    public void PowerWearedOff()
    {
        foreach (var m in monsters)
        {
            m.ResumeNormalPatrol();
        }
    }
}
