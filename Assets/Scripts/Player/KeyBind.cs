using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyBind
{
    public KeyCode[] primary = new KeyCode[2], secondary;
    public bool IsPressed()
    {
        bool primaryPressed = false, secondaryPressed = false;

        foreach (KeyCode keyCode in primary)
        {
            if (!Input.GetKey(keyCode))
            {
                primaryPressed = false;
                break;
            }
            primaryPressed = true;
        }

        foreach (KeyCode keyCode in secondary)
        {          
            if (!Input.GetKey(keyCode))
            {
                secondaryPressed = false;
                break;
            }
            secondaryPressed = true;
        }

        return primaryPressed || secondaryPressed;
    }

    public bool IsKeyDown()
    {
        bool primaryPressed = false, secondaryPressed = false;

        foreach (KeyCode keyCode in primary)
        {
            if (!Input.GetKeyDown(keyCode))
            {
                primaryPressed = false;
                break;
            }
            primaryPressed = true;
        }

        foreach (KeyCode keyCode in secondary)
        {
            if (!Input.GetKeyDown(keyCode))
            {
                secondaryPressed = false;
                break;
            }
            secondaryPressed = true;
        }

        return primaryPressed || secondaryPressed;
    }
}
