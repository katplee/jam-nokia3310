using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    private float dailyGrade;

    [SerializeField] private List<Image> lifeBars;
    [SerializeField] private Sprite full;

    private void Start()
    {
        TaskManager.RefreshGrade += RefreshLifeBar;
    }

    private void OnDestroy()
    {
        TaskManager.RefreshGrade -= RefreshLifeBar;
    }

    private void RefreshLifeBar()
    {
        int percentage = Mathf.FloorToInt(dailyGrade / 10);

        for (int i = 0; i < percentage; i++)
        {
            lifeBars[i].sprite = full;
        }
    }

    public void SetDailyGrade(float dailyGrade)
    {
        this.dailyGrade = dailyGrade;
    }
}
