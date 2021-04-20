using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatrolDirection
{
    None,
    LeftAndRight,
    UpAndDown
}

public class MonsterBehaviour : MonoBehaviour
{
    public PatrolDirection patrolDir = PatrolDirection.UpAndDown;

    public float speed;

    private int xSpeed = 0;
    private int ySpeed = 1;

    private int currentXPos;
    private int currentYPos;

    private Animator anim;
    void Start()
    {
        Init();
    }

    private void Init()
    {
        anim = GetComponent<Animator>();

        currentXPos = (int)transform.position.x;
        currentYPos = (int)transform.position.z;

        LevelManager.Instance.FillGridPosition(currentYPos, currentXPos);
        LevelManager.Instance.FillGridTag("monster", currentYPos, currentXPos);

        ChangeDirection();
        ChangeAnimation();

        InvokeRepeating("StartPatrol", 0, speed);
    }

    private void StartPatrol()
    {
        if (GameManager.Instance.isPlayerAlive())
        {
            if (LevelManager.Instance.GetGridTag(currentYPos + ySpeed, currentXPos + xSpeed) == "Player")
            {
                //ReduceHealth
                GameManager.Instance.ReducePlayerHealth();

                //In Meeting each Other in Opposite Direction
                if (isOppositeAndFacingEachother())
                {
                    GameManager.Instance.KillPlayer();
                }
            }

            else if (LevelManager.Instance.GetGridTag(currentYPos + ySpeed, currentXPos + xSpeed) == "Wall")
            {
                if (LevelManager.Instance.IsConnected(currentYPos + ySpeed, currentXPos + xSpeed))
                {
                    //Kill self
                    this.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    GameManager.Instance.ReducePlayerHealth();
                }

                ChangeDirection();
            }

            else if (LevelManager.Instance.isCellEmpty(currentYPos + ySpeed, currentXPos + xSpeed))
            {
                LevelManager.Instance.EmptyGridPosition(currentYPos, currentXPos);
                LevelManager.Instance.RemoveGridTag("monster", currentYPos, currentXPos);

                currentXPos += xSpeed;
                currentYPos += ySpeed;

                transform.position = new Vector3(currentXPos, 0, currentYPos);

                LevelManager.Instance.FillGridPosition(currentYPos, currentXPos);
                LevelManager.Instance.FillGridTag("monster", currentYPos, currentXPos);
            }

            else
            {
                LevelManager.Instance.EmptyGridPosition(currentYPos, currentXPos);
                LevelManager.Instance.RemoveGridTag("monster", currentYPos, currentXPos);

                ChangeDirection();
            }
        }
        else
        {
            CancelInvoke("StartPatrol");
        }
    }

    private void ChangeAnimation()
    {
        anim.SetFloat("x", xSpeed);
        anim.SetFloat("y", ySpeed);
    }

    private bool isOppositeAndFacingEachother()
    {
        int rot = Mathf.Abs((int)GameManager.Instance.GetPlayer().transform.localEulerAngles.y -
                                                            (int)transform.localEulerAngles.y);
        return (rot == 180 || rot == 90);
    }

    private void ChangeDirection()
    {
        if (patrolDir == PatrolDirection.UpAndDown)
        {
            for (int row = -1; row < 2; row += 2)
            {
                if (LevelManager.Instance.isCellEmpty(currentYPos + row, currentXPos + 0))
                {
                    ySpeed = row;
                    xSpeed = 0;

                    ChangeAnimation();
                    return;
                }
            }
        }

        if (patrolDir == PatrolDirection.LeftAndRight)
        {
            for (int col = -1; col < 2; col += 2)
            {
                if (LevelManager.Instance.isCellEmpty(currentYPos + 0, currentXPos + col))
                {
                    ySpeed = 0;
                    xSpeed = col;

                    ChangeAnimation();
                    return;
                }
            }
        }
    }

    public void SlowPatrol()
    {
        CancelInvoke("StartPatrol");
        InvokeRepeating("StartPatrol", 0, speed * 2);
    }

    public void ResumeNormalPatrol()
    {
        CancelInvoke("StartPatrol");
        InvokeRepeating("StartPatrol", 0, speed);
    }

    private void OnDisable()
    {
        if (VFXManager.Instance)
            VFXManager.Instance.PlayFX(currentYPos, currentXPos);
        CancelInvoke("StartPatrol");
    }
}
