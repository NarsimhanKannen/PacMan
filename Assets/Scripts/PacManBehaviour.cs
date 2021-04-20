using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotateDirection
{
    None,
    Up,
    Down,
    Left,
    Right
}

public class PacManBehaviour : MonoBehaviour
{
    public RotateDirection playerDirection = RotateDirection.Up;

    public float speed;

    public int checkPosition;
    public int savedCheckPosition;

    private int spawnRowPos;
    private int spawnColPos;

    private int xSpeed = 0;
    private int ySpeed = 1;

    public int currentXPos;
    public int currentYPos;

    private Animator anim;

    void Start()
    {
        GameManager.Instance.SetPlayer(this.gameObject);

        anim = GetComponent<Animator>();

        currentXPos = (int)transform.position.x;
        currentYPos = (int)transform.position.z;

        InvokeRepeating("MoveCharacter", 0, speed);
    }

    private void MoveCharacter()
    {
        if (GameManager.Instance.isPlayerAlive())
        {
            //print(LevelManager.Instance.GetGridTag(currentYPos + ySpeed, currentXPos + xSpeed));
            if (LevelManager.Instance.GetGridTag(currentYPos + ySpeed, currentXPos + xSpeed) == "Monster")
            {
                print("Dead");
                Debug.Break();
                CancelInvoke("MoveCharacter");
            }
            else if (LevelManager.Instance.isCellEmpty(currentYPos + ySpeed, currentXPos + xSpeed))
            {
                LevelManager.Instance.RemoveGridTag("Player", currentYPos, currentXPos);
                currentXPos += xSpeed;
                currentYPos += ySpeed;

                LevelManager.Instance.FillGridTag("Player", currentYPos, currentXPos);
                transform.position = new Vector3(currentXPos, 0, currentYPos);

                PositionTracker();
            }
        }
        else
        {
            CancelInvoke("MoveCharacter");
        }
    }

    private void RotateCharacter(RotateDirection dir)
    {
        if (dir != RotateDirection.None)
        {
            switch (dir)
            {
                case RotateDirection.Up:
                    ChangeMoveDirection(0, 1);
                    ResetSavedCheckPosition();
                    playerDirection = RotateDirection.Up;
                    break;

                case RotateDirection.Down:
                    ChangeMoveDirection(0, -1);
                    ResetSavedCheckPosition();
                    playerDirection = RotateDirection.Down;
                    break;

                case RotateDirection.Left:
                    ChangeMoveDirection(-1, 0);
                    ResetSavedCheckPosition();
                    playerDirection = RotateDirection.Left;
                    break;

                case RotateDirection.Right:
                    ChangeMoveDirection(1, 0);
                    ResetSavedCheckPosition();
                    playerDirection = RotateDirection.Right;
                    break;
            }
        }
    }

    private void ChangeMoveDirection(int x, int y)
    {
        this.xSpeed = x;
        this.ySpeed = y;

        ChangeAnimation();
    }

    private void ResetSavedCheckPosition()
    {
        savedCheckPosition = 0;
    }

    private void PositionTracker()
    {
        if (playerDirection == RotateDirection.Up)
        {
            checkPosition = currentYPos;

            spawnColPos = currentXPos;
            spawnRowPos = checkPosition - 1;
        }
        if (playerDirection == RotateDirection.Down)
        {
            checkPosition = currentYPos;

            spawnColPos = currentXPos;
            spawnRowPos = checkPosition + 1;
        }

        if (playerDirection == RotateDirection.Left)
        {
            checkPosition = currentXPos;

            spawnRowPos = currentYPos;
            spawnColPos = checkPosition + 1;
        }
        if (playerDirection == RotateDirection.Right)
        {
            checkPosition = currentXPos;

            spawnRowPos = currentYPos;
            spawnColPos = checkPosition - 1;
        }

        if (checkPosition != savedCheckPosition)
        {
            //GridFiiller.Instance.CheckLastCell();
            LevelManager.Instance.SpawnWall(spawnRowPos, spawnColPos);
            GameManager.Instance.SetProgressBar(LevelManager.Instance.GetPercentageFilled());

            savedCheckPosition = checkPosition;
        }
    }

    private void ChangeAnimation()
    {
        anim.SetFloat("x", xSpeed);
        anim.SetFloat("y", ySpeed);
    }

    public Vector3 GetCurrentPos()
    {
        return new Vector3(currentXPos, 0, currentYPos);
    }

    void Update()
    {
#if UNITY_ANDROID
        RotateCharacter(InputManager.Instance.GetDirection());
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        RotateCharacter(InputManager.Instance.GetKeyboardInput());
#endif

    }

}
