using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Input : MonoBehaviour
{
    private List<KeyCode> codes = new List<KeyCode>()
    {
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.Z,
        KeyCode.X,
        KeyCode.C,
        KeyCode.Backspace
    };

    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float runningTime;
    [SerializeField] private float waitEndTime;

    [SerializeField] private bool writingCommand = false;
    //[SerializeField] private GameObject commandCornerGO = null;
    [SerializeField] private TMP_Text commandStringText = null;
    List<string> commandList = new List<string>();
    private string commandString = null;
    private int maxCommandLength = 1;

    public static event Action<KeyCode> ActionKeyPress;

    private void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey)
        {
            if (commandList.Any() && !writingCommand) { commandList.Clear(); }

            if (!codes.Contains(e.keyCode)) { return; }

            SetTimers(e.keyCode);

            CreateCommandString(e.keyCode);

        }
    }

    private void Update()
    {
        if (!writingCommand) { return; }

        runningTime += Time.deltaTime;
        commandStringText.text = commandString;
        
        if (!commandList.Any()) { ClearTimers(); }

        if (runningTime >= waitEndTime)
        {
            ClearTimers();

            if (commandList.Any())
            {
                SendCommand();
            }
        }        
    }


    private void CreateCommandString(KeyCode commandBit)
    {
        string stringBit = commandBit.ToString();

        if (commandList.Count == maxCommandLength && commandBit != KeyCode.Backspace) { return; }

        else if (commandBit != KeyCode.Backspace) { commandList.Add(stringBit); }

        else if (commandList.Any() && commandBit == KeyCode.Backspace) { commandList.RemoveAt(commandList.Count - 1); }

        LatestCommandString(commandList);
    }

    private void LatestCommandString(List<string> latestCommandList)
    {
        commandString = null;

        foreach (string stringBit in latestCommandList)
        {
            commandString += stringBit;
        }
    }

    private void SendCommand()
    {
        KeyCode commandKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), commandString);
        
        //Invoke the method that will perform the action.
        ActionKeyPress?.Invoke(commandKeyCode);
        
        commandList.Clear();
        commandStringText.text = null;
    }

    private void SetTimers(KeyCode commandBit)
    {
        if(commandList.Count == maxCommandLength) { return; }

        if (commandBit == KeyCode.Backspace) { return; }

        //set writing command to true
        writingCommand = true;

        //set the running time to current time
        runningTime = Time.deltaTime;

        //compute for the wait end time
        waitEndTime = runningTime + waitTime;
    }

    private void ClearTimers()
    {
        writingCommand = false;
        runningTime = 0f;
        waitEndTime = 0f;
    }
}
