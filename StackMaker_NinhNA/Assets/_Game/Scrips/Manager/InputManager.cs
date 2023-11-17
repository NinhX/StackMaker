using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action<Direct> OnSwipe;
    private Vector3 mouseDown;
    private Vector3 mouseUp;

    public enum Direct {
        Forward,
        Back,
        Right,
        Left,
        None
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseUp = Input.mousePosition;
            Direct direct = GetDirect();
            OnSwipe?.Invoke(direct);
        }
    }

    private Direct GetDirect()
    {
        float deltaX = mouseUp.x - mouseDown.x;
        float deltaY = mouseUp.y - mouseDown.y;
        float deltaXAbs = Mathf.Abs(deltaX);
        float deltaYAbs = Mathf.Abs(deltaY);

        if (deltaXAbs < 1 && deltaYAbs < 1) 
        {
            return Direct.None;
        }
        else if (deltaXAbs > deltaYAbs)
        {
            return deltaX > 0 ? Direct.Right : Direct.Left;
        }
        else
        {
            return deltaY > 0 ? Direct.Forward : Direct.Back;
        }
    }
}
