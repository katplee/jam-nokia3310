using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Input : MonoBehaviour
{
    public static event Action<KeyCode> ActionKeyPress;

    [SerializeField] private bool writingCommand = false;
    [SerializeField] GameObject commandCornerGO = null;
    [SerializeField] Image commandLetter = null;
    [SerializeField] List<Sprite> letters = null;
    [SerializeField] Sprite empty = null;

    private Dictionary<string, int> stringToInt = new Dictionary<string, int>()
    {
        { "Q", 0 },
        { "W", 1 },
        { "E", 2 },
        { "A", 3 },
        { "S", 4 },
        { "D", 5 },
        { "Z", 6 },
        { "X", 7 },
        { "C", 8 },
    };

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

    private bool isAnimating = false;

    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float runningTime;
    [SerializeField] private float waitEndTime;

    [SerializeField] private TMP_Text commandStringText = null;
    [SerializeField] private List<string> commandList = new List<string>();
    private string commandString = null;
    private int maxCommandLength = 1;

    private void Start()
    {
        PlayerMovement.ReadyToAnimate += IsAnimating;
        AnimationController.AnimationDone += IsNotAnimating;
    }

    private void OnDestroy()
    {
        PlayerMovement.ReadyToAnimate -= IsAnimating;
        AnimationController.AnimationDone -= IsNotAnimating;
    }

    private void IsAnimating(string destinationName)
    {
        isAnimating = true;
    }

    private void IsNotAnimating()
    {
        isAnimating = false;
    }

    private void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey)
        {
            //if there is any sort of animation on going, commands will not be accepted
            if (isAnimating) { return; }

            //if key pressed is not included in the keys allowed, return
            if (!codes.Contains(e.keyCode)) { return; }

            //before timers are set and command strings are created,
            //clear any strings left in the commandlist (if any),
            //if the player is currently not writing any commands
            if (commandList.Any() && !writingCommand) { commandList.Clear(); }

            //set timers for sending the command          
            SetTimers(e.keyCode);

            //create the command string which will dictate the destination/movement of the player
            CreateCommandString(e.keyCode);

            //show the command corner at the bottom right

            if (!commandList.Any() && !writingCommand) { return; }
            commandCornerGO.SetActive(true);
        }
    }

    private void Update()
    {
        //will only run if latest key press was a letter
        //or if character is erasing a command
        //pressing backspace as a first command will be returned
        if (!writingCommand) { return; }

        //if it is in the middle of writing a command,
        //update the timers previously set
        runningTime += Time.deltaTime;
        commandStringText.text = commandString;

        //the command corner can be seen if there is either:
        //any field in the command list (meaning that a letter was pressed)
        //or if a command is being writing (even if the command list is empty - just erased a field)
        if (commandCornerGO.activeSelf)
        {
            if (commandString == null) { commandLetter.sprite = empty; }
            else { commandLetter.sprite = letters[stringToInt[commandString]]; }
        }

        //while in the middle of writing a command, clear timers the moment the command list goes empty
        //the timers will start couting up only if the last key pressed was non-backspace
        if (!commandList.Any()) { ClearTimers(); }

        if (runningTime >= waitEndTime)
        {
            ClearTimers();

            if (commandList.Any())
            {
                SendCommand();
            }            
            ClearList();
        }
    }

    private void SetTimers(KeyCode commandBit)
    {
        //set key presses that will not affect the setting of the timer

        //if the command list has already reached the maximum
        //any key pressed will not have any effect on the timer
        //but if the command list is still accepting key presses,
        //the timer can still change to accommodate for future key presses
        if (commandList.Count == maxCommandLength) { return; }

        //if the player is erasing any mistake,
        //there is not addition/deduction to the timer
        if (commandBit == KeyCode.Backspace) { return; }

        //set writing command to true
        writingCommand = true;

        //set the running time to current time
        runningTime = Time.time;

        //compute for the wait end time
        waitEndTime = runningTime + waitTime;
    }

    private void CreateCommandString(KeyCode commandBit)
    {
        //set key presses that will affect the creation of the command string        

        //if the command list has reached the maximum, only a backspace key press will be accepted
        //note: a restriction when the command bit is not backspace and the command list is at max
        if (commandList.Count == maxCommandLength && commandBit != KeyCode.Backspace) { return; }

        //if the command list is empty, there is nothing to erase,
        //so a backspace will terminate the loop
        //note: a restriction when the command bit is backspace and the command list is empty
        else if (!commandList.Any() && commandBit == KeyCode.Backspace) { return; }

        //if there are fields inside the command list, pressing the backspace will delete the last entered field
        //note: a restriction when the command bit is backspace and the command list not empty/at max
        else if (commandList.Any() && commandBit == KeyCode.Backspace) { commandList.RemoveAt(commandList.Count - 1); }

        //if the command list is not at max, add the pressed key to the command list
        //note: a restriction when the command bit is not backspace and the command list is empty/with at least one field
        else if (commandBit != KeyCode.Backspace)
        {
            //convert the key code to string format then add to the command list            
            commandList.Add(commandBit.ToString());
        }

        UpdateCommandString(commandList);
    }

    private void UpdateCommandString(List<string> latestCommandList)
    {
        commandString = null;

        foreach (string commandBit in latestCommandList)
        {
            commandString += commandBit;
        }
    }

    private void SendCommand()
    {
        KeyCode commandKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), commandString);

        //Invoke the method that will perform the action.
        ActionKeyPress?.Invoke(commandKeyCode);
    }

    private void ClearList()
    {
        commandList.Clear();
        commandStringText.text = null;
        commandCornerGO.SetActive(false);

    }

    private void ClearTimers()
    {
        writingCommand = false;
        runningTime = 0f;
        waitEndTime = 0f;
    }
}
