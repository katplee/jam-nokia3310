using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager>
{
    public static event Action<int> DayChanged;
    public static event Action<int> HourChanged;
    public static event Action WeekChanged;

    [SerializeField] private Image hour_tens;
    [SerializeField] private Image hour_ones;
    [SerializeField] private Image min_tens;
    [SerializeField] private Image min_ones;
    [SerializeField] private Image day_first_letter;
    [SerializeField] private Image day_second_letter;

    [SerializeField] List<Sprite> numbers = new List<Sprite>();
    [SerializeField] List<Sprite> letters = new List<Sprite>();

    //Controlling the time
    private float tickTime = 1f;
    [SerializeField] private float runningTime;
    [SerializeField] private float tickEndTime;
    [SerializeField] private int gameToRealLifeConv = 5; //1 game hour = 5 real life seconds
    [SerializeField] private int hourOnesTicks = 0; //restarts every 5 seconds
    [SerializeField] private int hourTensTicks = 0; //counts up 10
    [SerializeField] private int dayTicks = 0;

    private void Start()
    {
        tickEndTime = runningTime + tickTime;
    }

    private void Update()
    {
        runningTime += Time.deltaTime;

        if (runningTime >= tickEndTime)
        {
            ChangeTime();
            RestartTimer();
        }
    }

    private void ChangeTime()
    {
        hourOnesTicks++;
        ChangeMin();
    }

    private void RestartTimer()
    {
        runningTime = Time.time;
        tickEndTime = runningTime + tickTime;
    }

    private void ChangeDay()
    {
        dayTicks++;

        if (dayTicks > 6)
        {
            WeekChanged?.Invoke();
            dayTicks = 0;
        }

        DayChanged?.Invoke(dayTicks);

        switch (dayTicks)
        {
            case 0:
                day_first_letter.sprite = letters[ChangeCharToInt('S')];
                day_second_letter.sprite = letters[ChangeCharToInt('U')];
                break;

            case 1:
                day_first_letter.sprite = letters[ChangeCharToInt('M')];
                day_second_letter.sprite = letters[ChangeCharToInt('O')];
                break;

            case 2:
                day_first_letter.sprite = letters[ChangeCharToInt('T')];
                day_second_letter.sprite = letters[ChangeCharToInt('U')];
                break;

            case 3:
                day_first_letter.sprite = letters[ChangeCharToInt('W')];
                day_second_letter.sprite = letters[ChangeCharToInt('E')];
                break;

            case 4:
                day_first_letter.sprite = letters[ChangeCharToInt('T')];
                day_second_letter.sprite = letters[ChangeCharToInt('H')];
                break;

            case 5:
                day_first_letter.sprite = letters[ChangeCharToInt('F')];
                day_second_letter.sprite = letters[ChangeCharToInt('R')];
                break;

            case 6:
                day_first_letter.sprite = letters[ChangeCharToInt('S')];
                day_second_letter.sprite = letters[ChangeCharToInt('A')];
                break;

            default:
                break;
        }
    }

    private void ChangeHourTens()
    {
        if (hourTensTicks == 24)
        {
            hourTensTicks = 0;
            ChangeDay();
        }

        hour_tens.sprite = numbers[Mathf.FloorToInt(hourTensTicks / 10)];
    }

    private void ChangeHourOnes()
    {
        if (hourOnesTicks == gameToRealLifeConv) { hourOnesTicks = 0; }

        hourTensTicks++;

        if (hourTensTicks % 10 == 0 || hourTensTicks == 24) { ChangeHourTens(); }

        HourChanged?.Invoke(hourTensTicks);

        hour_ones.sprite = numbers[hourTensTicks % 10];
    }

    private void ChangeMin()
    {
        switch (hourOnesTicks % gameToRealLifeConv)
        {
            case 0:
                //Combination @mod0 (00)
                min_tens.sprite = numbers[0];
                min_ones.sprite = numbers[0];
                break;

            case 1:
                //Combination @mod1 (12)
                min_tens.sprite = numbers[1];
                min_ones.sprite = numbers[2];
                break;

            case 2:
                //Combination @mod2 (24)
                min_tens.sprite = numbers[2];
                min_ones.sprite = numbers[4];
                break;

            case 3:
                //Combination @mod3 (36)
                min_tens.sprite = numbers[3];
                min_ones.sprite = numbers[6];
                break;

            case 4:
                //Combination @mod4 (48)
                min_tens.sprite = numbers[4];
                min_ones.sprite = numbers[8];
                break;

            default:
                break;
        }

        if (hourOnesTicks == 5) { ChangeHourOnes(); }
    }

    private int ChangeCharToInt(char c)
    {
        return ((int)c % 32) - 1;
    }
}
