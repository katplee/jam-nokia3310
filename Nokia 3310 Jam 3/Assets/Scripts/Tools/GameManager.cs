using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{    
    private List<KeyCode> codes = new List<KeyCode>()
    {
        KeyCode.Z,
        KeyCode.X,
        KeyCode.C,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E
    };

    public static event Action<KeyCode> ActionKeyPress;

    private void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey)
        {
            if (!codes.Contains(e.keyCode)) { return; }

            ActionKeyPress?.Invoke(e.keyCode);
        }
    } 
}
