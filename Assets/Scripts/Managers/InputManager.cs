using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private Vector2 oldTouchPos;
    private Vector2 currentTouchPos;

    private float tolerance = 20;

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

    public RotateDirection GetTouchGesture()
    {
        if (Input.GetMouseButtonDown(0))
        {
            oldTouchPos = currentTouchPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            currentTouchPos = Input.mousePosition;
        }

        if (currentTouchPos.sqrMagnitude != oldTouchPos.sqrMagnitude)
        {
            if ((currentTouchPos.x - oldTouchPos.x) > tolerance)
            {
                print("Right");
                return RotateDirection.Right;
            }
            else if ((currentTouchPos.x - oldTouchPos.x) < -tolerance)
            {
                print("Left");
                return RotateDirection.Left;
            }
            else if ((currentTouchPos.y - oldTouchPos.y) > tolerance)
            {
                print("Up");
                return RotateDirection.Up;
            }
            else if ((currentTouchPos.y - oldTouchPos.y) < -tolerance)
            {
                print("Down");
                return RotateDirection.Down;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            oldTouchPos = currentTouchPos = Vector2.zero;
        }

        return RotateDirection.None;
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
}
