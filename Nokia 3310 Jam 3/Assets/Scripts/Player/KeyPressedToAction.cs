using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPressedToAction : MonoBehaviour
{
    //Will contain list of corresponding methods which contain player actions.

    public static event Action<Vector3Int, string, int> MovePlayer;

    [SerializeField] private PlayerWalkableNodeList nodeList;
    [SerializeField] private GameObject statsGO = null;

    private int currentDay = 0;

    private void Start()
    {   
        Input.ActionKeyPress += ActionCheck;
        TimeManager.DayChanged += DayChanged;
    }

    private void OnDestroy()
    {
        Input.ActionKeyPress -= ActionCheck;
        TimeManager.DayChanged -= DayChanged;
    }

    private void DayChanged(int day)
    {
        currentDay = day;
    }

    private void ActionCheck(KeyCode actionKey)
    {
        switch (actionKey)
        {
            case KeyCode.Z:
                MovePlayer?.Invoke(GoTo("Bed"), "Bed", 0);
                break;

            case KeyCode.E:
                MovePlayer?.Invoke(GoTo("Desk_Oven"), "Desk_Oven", 1);
                break;

            case KeyCode.C:
                MovePlayer?.Invoke(GoTo("Door_Slippers"), "Door_Slippers", 2);
                break;

            case KeyCode.X:
                MovePlayer?.Invoke(GoTo("Door_Slippers"), "Door_Slippers", 3);
                break;

            case KeyCode.W:
                MovePlayer?.Invoke(GoTo("Desk_Computer"), "Desk_Computer", 4);
                break;

            case KeyCode.S:
                MovePlayer?.Invoke(GoTo("Desk_Computer"), "Desk_Computer", 5);
                break;

            case KeyCode.D:
                if(currentDay == 1 || currentDay == 5)
                {
                    MovePlayer?.Invoke(GoTo("Closet_Left"), "Closet_Left", 6);
                }
                else
                {
                    MovePlayer?.Invoke(GoTo("Closet_Left"), "Closet_Left", 7);
                }
                break;

            case KeyCode.A:
                MovePlayer?.Invoke(GoTo("Stock_Room"), "Stock_Room", 8);
                break;

            case KeyCode.Q:
                MovePlayer?.Invoke(GoTo("Closet_Right"), "Closet_Right", 9);
                break;

            default:
                Debug.Log($"You just pressed {actionKey}!");
                break;
        }
    }

    private Vector3Int GoTo(string destination)
    {
        Vector3Int transform = nodeList.destinationToTransform[destination];

        return transform;
    }

    public void Testing()
    {
        //MovePlayer?.Invoke(new Vector3Int(-7, -1, 0), "Test");
    }
}
