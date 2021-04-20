using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private Vector2 fingerDown;
    private Vector2 fingerUp;

    private bool isFingerReleased = false;

    private float tolerance = 20;

    private RotateDirection dir;

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

    public RotateDirection GetDirection()
    {
        return dir;
    }

    private void SetDir(RotateDirection dir)
    {
        this.dir = dir;
    }

    private void checkSwipe()
    {
        if (verticalMove() > tolerance && verticalMove() > horizontalValMove())
        {
            if (fingerDown.y - fingerUp.y > 0)
            {
                SetDir(RotateDirection.Up);
            }
            else if (fingerDown.y - fingerUp.y < 0)
            {
                SetDir(RotateDirection.Down);
            }

            isFingerReleased = true;
            fingerUp = fingerDown;
        }

        else if (horizontalValMove() > tolerance && horizontalValMove() > verticalMove())
        {
            if (fingerDown.x - fingerUp.x > 0)
            {
                SetDir(RotateDirection.Right);
            }
            else if (fingerDown.x - fingerUp.x < 0)
            {
                SetDir(RotateDirection.Left);
            }

            isFingerReleased = true;
            fingerUp = fingerDown;
        }
        else
        {
            SetDir(RotateDirection.None);
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    public RotateDirection GetKeyboardInput()
    {
        Vector2 inp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (inp.x != 0)
        {
            return (inp.x > 0) ? RotateDirection.Right : RotateDirection.Left;
        }
        else if (inp.y != 0)
        {
            return (inp.y > 0) ? RotateDirection.Up : RotateDirection.Down;
        }

        return RotateDirection.None;
    }
    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
                isFingerReleased = false;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (!isFingerReleased)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }
        }
    }
}
