using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private GameObject statsGO = null;

    private int sleepLevel = 0;
    private int hygieneLevel = 0;
    private int eatLevel = 0;
    private int toiletLevel = 0;
    private int taskOneLevel = 0;
    private int taskTwoLevel = 0;
    private int taskThreeLevel = 0;

    [SerializeField] private List<Image> sleepBars = null;
    [SerializeField] private List<Image> eatBars = null;
    [SerializeField] private List<Image> hygieneBars = null;
    [SerializeField] private List<Image> toiletBars = null;
    [SerializeField] private List<Image> taskOneBars = null;
    [SerializeField] private List<Image> taskTwoBars = null;
    [SerializeField] private List<Image> taskThreeBars = null;
    [SerializeField] private Sprite full = null;
    [SerializeField] private Sprite empty = null;

    private void Start()
    {
        TaskManager.IncreaseSleep += RefreshSleepBar;
        TaskManager.IncreaseEat += RefreshEatBar;
        TaskManager.IncreaseHygiene += RefreshHygieneBar;
        TaskManager.IncreaseToilet += RefreshToiletBar;
        TaskManager.IncreaseTaskOne += RefreshTaskOneBar;
        TaskManager.IncreaseTaskTwo += RefreshTaskTwoBar;
        TaskManager.IncreaseTaskThree += RefreshTaskThreeBar;
        TaskManager.ClearStats += ClearStats;
    }

    private void OnDestroy()
    {
        TaskManager.IncreaseSleep -= RefreshSleepBar;
        TaskManager.IncreaseEat -= RefreshEatBar;
        TaskManager.IncreaseHygiene -= RefreshHygieneBar;
        TaskManager.IncreaseToilet -= RefreshToiletBar;
        TaskManager.IncreaseTaskOne -= RefreshTaskOneBar;
        TaskManager.IncreaseTaskTwo -= RefreshTaskTwoBar;
        TaskManager.IncreaseTaskThree -= RefreshTaskThreeBar;
        TaskManager.ClearStats -= ClearStats;
    }

    private void ClearStats()
    {
        AutoClear(sleepBars);
        AutoClear(eatBars);
        AutoClear(hygieneBars);
        AutoClear(toiletBars);
        AutoClear(taskOneBars);
        AutoClear(taskTwoBars);
        AutoClear(taskThreeBars);        
    }

    private void AutoClear(List<Image> listToDelete)
    {
        for (int i = 0; i < listToDelete.Count; i++)
        {
            listToDelete[i].sprite = empty;
        }
        
    }

    private void RefreshTaskThreeBar()
    {
        taskThreeLevel++;

        if (taskThreeLevel > taskThreeBars.Count) { return; }

        taskThreeBars[taskThreeLevel - 1].sprite = full;
    }

    private void RefreshTaskTwoBar()
    {
        taskTwoLevel++;

        if (taskTwoLevel > taskTwoBars.Count) { return; }

        taskTwoBars[taskTwoLevel - 1].sprite = full;
    }

    private void RefreshTaskOneBar()
    {
        taskOneLevel++;

        if (taskOneLevel > taskOneBars.Count) { return; }

        taskOneBars[toiletLevel - 1].sprite = full;
    }

    private void RefreshToiletBar()
    {
        toiletLevel++;

        if (toiletLevel > toiletBars.Count) { return; }

        toiletBars[toiletLevel - 1].sprite = full;
    }

    private void RefreshHygieneBar()
    {
        hygieneLevel++;

        if (hygieneLevel > hygieneBars.Count) { return; }

        hygieneBars[hygieneLevel - 1].sprite = full;
    }

    private void RefreshEatBar()
    {
        eatLevel++;

        if (eatLevel > eatBars.Count) { return; }

        eatBars[eatLevel - 1].sprite = full;
    }

    private void RefreshSleepBar()
    {
        sleepLevel++;

        if (sleepLevel > sleepBars.Count) { return; }

        sleepBars[sleepLevel - 1].sprite = full;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            statsGO.SetActive(true);
        }
        else
        {
            statsGO.SetActive(false);
        }
    }
}


