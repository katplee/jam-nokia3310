using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestination : MonoBehaviour
{
    [SerializeField] private int playerAction;

    public int GetPlayerAction()
    {
        return playerAction;
    }

    public void SetStatus(int actionCode)
    {
        playerAction = actionCode;
    }

    public void ClearStatus()
    {
        playerAction = -1;
    }
}
