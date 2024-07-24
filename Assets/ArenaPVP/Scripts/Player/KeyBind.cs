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
        if (!Input.anyKey)
            return false;

        foreach (KeyCode keyCode in primary)
        {
            if (!Input.GetKey(keyCode) && keyCode != KeyCode.None)
            {
                primaryPressed = false;
                break;
            }
            else if(keyCode != KeyCode.None)
            {
                primaryPressed = true;
            }
        }

        foreach (KeyCode keyCode in secondary)
        {
            if (!Input.GetKey(keyCode) && keyCode != KeyCode.None)
            {
                secondaryPressed = false;
                break;
            }
            else if (keyCode != KeyCode.None)
            {
                secondaryPressed = true;
            }
        }

        return primaryPressed || secondaryPressed;
    }
    public bool IsKeyUp()
    {
        bool primaryPressed = false, secondaryPressed = false;

        foreach (KeyCode keyCode in primary)
        {
            if (!Input.GetKeyUp(keyCode) && keyCode != KeyCode.None)
            {
                primaryPressed = false;
                break;
            }
            else if (keyCode != KeyCode.None)
            {
                primaryPressed = true;
            }
        }

        foreach (KeyCode keyCode in secondary)
        {
            if (!Input.GetKeyUp(keyCode) && keyCode != KeyCode.None)
            {
                secondaryPressed = false;
                break;
            }
            else if (keyCode != KeyCode.None)
            {
                secondaryPressed = true;
            }
        }

        return primaryPressed || secondaryPressed;
    }
    public bool IsKeyDown()
    {
        if (!Input.anyKey)
            return false;

        bool primaryPressed = false, secondaryPressed = false;

        foreach (KeyCode keyCode in primary)
        {
            if (!Input.GetKeyDown(keyCode) && keyCode != KeyCode.None)
            {
                primaryPressed = false;
                break;
            }
            else if (keyCode != KeyCode.None)
            {
                primaryPressed = true;
            }
        }

        foreach (KeyCode keyCode in secondary)
        {
            if (!Input.GetKeyDown(keyCode) && keyCode != KeyCode.None)
            {
                secondaryPressed = false;
                break;
            }
            else if (keyCode != KeyCode.None)
            {
                secondaryPressed = true;
            }
        }

        return primaryPressed || secondaryPressed;
    }
}
