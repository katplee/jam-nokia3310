using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;


public class TaskManager : MonoBehaviour
{
    public static event Action RefreshGrade;
    public static event Action IncreaseSleep;
    public static event Action IncreaseEat;
    public static event Action IncreaseHygiene;
    public static event Action IncreaseToilet;
    public static event Action IncreaseTaskOne;
    public static event Action IncreaseTaskTwo;
    public static event Action IncreaseTaskThree;
    public static event Action ClearStats;

    [SerializeField] PlayerDestination playerDestination = null;
    [SerializeField] LifeManager lifeManagement = null;
    [SerializeField] StatsManager statsManagement = null;

    [SerializeField] private Image firstTask;
    [SerializeField] private Image firstTask_tens;
    [SerializeField] private Image firstTask_ones;
    [SerializeField] private Image secondTask;
    [SerializeField] private Image secondTask_tens;
    [SerializeField] private Image secondTask_ones;
    [SerializeField] private Image thirdTask;
    [SerializeField] private Image thirdTask_tens;
    [SerializeField] private Image thirdTask_ones;

    [SerializeField] List<Sprite> numbers = new List<Sprite>();
    [SerializeField] List<Sprite> icons = new List<Sprite>();
    [SerializeField] Sprite n = null;
    [SerializeField] Sprite a = null;

    //Assesing the task
    [SerializeField] private int currentDay = 0;
    [SerializeField] private int currentHour = 0;

    private float dailyGrade = 0;

    private int taskOneWeight = 4;
    private int taskTwoWeight = 4;
    private int taskThreeWeight = 13;
    private int eatWeight = 3;
    private int bathWeight = 3;
    private int toiletWeight = 3;
    private int sleepWeight = 4;

    private int taskOneMax = 20;
    [SerializeField] private int taskOneCode = 0;
    [SerializeField] private float taskOneGrade = 0;

    private int taskTwoMax = 20;
    private int taskTwoCode = 0;
    [SerializeField] private float taskTwoGrade = 0;

    private int taskThreeMax = 13;
    private int taskThreeCode = 0;
    [SerializeField] private float taskThreeGrade = 0;
    private int taskThreeTime = 0;

    private int eatMax = 9;
    private int bathMax = 9;
    private int toiletMax = 9;
    private int sleepMax = 20;

    [SerializeField] private int eatGrade = 0; //1 button press
    [SerializeField] private int bathGrade = 0; //1 button press
    [SerializeField] private int toiletGrade = 0; //1 button press
    [SerializeField] private float sleepGrade = 0f;

    private void Start()
    {
        TimeManager.DayChanged += DayChanged;
        TimeManager.HourChanged += HourChanged;
        TimeManager.WeekChanged += WeekChanged;
        PlayerMovement.ReadyToAnimate += ActionBefore;
        AnimationController.AnimationDone += ActionDone;

        ConstructDailySched();
    }

    private void OnDestroy()
    {
        TimeManager.DayChanged -= DayChanged;
        TimeManager.HourChanged -= HourChanged;
        TimeManager.WeekChanged -= WeekChanged;
        PlayerMovement.ReadyToAnimate -= ActionBefore;
        AnimationController.AnimationDone += ActionDone;
    }

    private void WeekChanged()
    {

    }

    private void DayChanged(int day)
    {
        currentDay = day;

        ClearCodesGrades();

        ConstructDailySched();
    }

    private void HourChanged(int hour)
    {
        currentHour = hour;
    }

    private void ActionBefore(string destinationName)
    {
        #region Scheduled tasks
        int currentActionCode = playerDestination.GetPlayerAction();

        if ((taskThreeGrade + taskThreeWeight) <= taskThreeMax)
        {
            //check third task
            if (currentActionCode == taskThreeCode)
            {
                if (currentHour == taskThreeTime)
                {
                    taskThreeGrade += taskThreeWeight;
                }

                taskThreeGrade += taskThreeWeight / 2;

                IncreaseTaskThree?.Invoke();
            }
        }


        //on weekdays, check work
        CheckWork(currentActionCode);
        #endregion

        RefreshDailyGrade();
    }

    private void CheckWork(int currentActionCode)
    {
        if (currentDay == 0 || currentDay == 6) { return; }

        if (currentHour != 8)
        {
            if (currentActionCode == 4)
            {
                taskOneGrade -= 5;
            }
        }
    }

    private void ActionDone()
    {
        //0: sleep
        //1: eat
        //2: hygiene
        //3: toilet
        //4: work
        //5: study
        //6: supermarket
        //7: exercise
        //8: food_prep
        //9: clean_room

        int currentActionCode = playerDestination.GetPlayerAction();

        if (currentActionCode == taskOneCode)
        {
            if ((taskOneGrade + taskOneWeight) <= taskOneMax)
            {
                taskOneGrade += taskOneWeight;
                IncreaseTaskOne?.Invoke();
            }

        }
        else if (currentActionCode == taskTwoCode)
        {
            if ((taskTwoGrade + taskTwoWeight) <= taskTwoMax)
            {
                taskTwoGrade += taskTwoWeight;
                IncreaseTaskTwo?.Invoke();
            }
        }
        else
        {
            switch (currentActionCode)
            {
                case 0:
                    if ((sleepGrade + sleepWeight) > sleepMax) { break; }
                    sleepGrade += sleepWeight;
                    IncreaseSleep.Invoke();
                    break;
                case 1:
                    if ((eatGrade + eatWeight) > eatMax) { break; }
                    eatGrade += eatWeight;
                    IncreaseEat?.Invoke();
                    break;
                case 2:
                    if ((bathGrade + bathWeight) > bathMax) { break; }
                    bathGrade += bathWeight;
                    IncreaseHygiene?.Invoke();
                    break;
                case 3:
                    if ((toiletGrade + toiletWeight) > toiletMax) { break; }
                    toiletGrade += toiletWeight;
                    IncreaseToilet?.Invoke();
                    break;

                default:
                    break;
            }
        }

        RefreshDailyGrade();
        playerDestination.ClearStatus();
    }

    private void RefreshDailyGrade()
    {
        //compute accomplishment report;
        dailyGrade = taskOneGrade + taskTwoGrade + taskThreeGrade +
            eatGrade + bathGrade + toiletGrade + sleepGrade;

        lifeManagement.SetDailyGrade(dailyGrade);

        RefreshGrade?.Invoke();
    }

    private void ConstructDailySched()
    {
        int superTime = Random.Range(17, 19);
        int prepFoodTime = Random.Range(5, 23);
        int cleanRoomTime = Random.Range(9, 18);

        //4: work
        //5: study
        //6: supermarket
        //7: exercise
        //8: food_prep
        //9: clean_room

        switch (currentDay)
        {
            case 0:
                //free heavy exercise
                taskOneCode = 7;
                firstTask.sprite = icons[taskOneCode];
                firstTask_tens.sprite = n;
                firstTask_ones.sprite = a;

                //free study
                taskTwoCode = 5;
                secondTask.sprite = icons[taskTwoCode];
                secondTask_tens.sprite = n;
                secondTask_ones.sprite = a;

                //sheduled clean room
                taskThreeCode = 9;
                thirdTask.sprite = icons[taskThreeCode];
                taskThreeTime = cleanRoomTime;
                thirdTask_tens.sprite = numbers[SetTimeTens(cleanRoomTime)];
                thirdTask_ones.sprite = numbers[SetTimeOnes(cleanRoomTime)];
                break;

            case 1:
            case 5:
                //scheduled work from 8am
                taskOneCode = 4;
                firstTask.sprite = icons[taskOneCode];
                firstTask_tens.sprite = numbers[0];
                firstTask_ones.sprite = numbers[8];

                //free study
                taskTwoCode = 5;
                secondTask.sprite = icons[taskTwoCode];
                secondTask_tens.sprite = n;
                secondTask_ones.sprite = a;

                //sheduled supermarket
                taskThreeCode = 6;
                thirdTask.sprite = icons[taskThreeCode];
                thirdTask_tens.sprite = numbers[SetTimeTens(superTime)];
                thirdTask_ones.sprite = numbers[SetTimeOnes(superTime)];
                taskThreeTime = superTime;
                break;

            case 2:
            case 3:
            case 4:
                //scheduled work from 8am
                taskOneCode = 4;
                firstTask.sprite = icons[taskOneCode];
                firstTask_tens.sprite = numbers[0];
                firstTask_ones.sprite = numbers[8];

                //free study
                taskTwoCode = 5;
                secondTask.sprite = icons[taskTwoCode];
                secondTask_tens.sprite = n;
                secondTask_ones.sprite = a;

                //free run
                taskThreeCode = 7;
                thirdTask.sprite = icons[taskThreeCode];
                thirdTask_tens.sprite = n;
                thirdTask_ones.sprite = a;
                break;

            case 6:
                //free heavy exercise
                taskThreeCode = 7;
                thirdTask.sprite = icons[taskThreeCode];
                secondTask_tens.sprite = n;
                secondTask_ones.sprite = a;

                //free study
                taskTwoCode = 5;
                secondTask.sprite = icons[taskTwoCode];
                firstTask_tens.sprite = n;
                firstTask_ones.sprite = a;

                //sheduled food prep
                taskThreeCode = 8;
                thirdTask.sprite = icons[taskThreeCode];
                thirdTask_tens.sprite = numbers[SetTimeTens(prepFoodTime)];
                thirdTask_ones.sprite = numbers[SetTimeOnes(prepFoodTime)];
                taskThreeTime = prepFoodTime;
                break;

            default:
                break;
        }
    }

    private int SetTimeTens(int number)
    {
        return Mathf.FloorToInt(number / 10);
    }

    private int SetTimeOnes(int number)
    {
        if (number < 10)
        {
            return number;
        }
        else
        {
            return number % 10;
        }
    }

    private void ClearCodesGrades()
    {
        dailyGrade = 0;

        taskOneCode = 0;
        taskOneGrade = 0;
        taskTwoCode = 0;
        taskTwoGrade = 0;
        taskThreeCode = 0;
        taskThreeGrade = 0;
        taskThreeTime = 0;

        eatGrade = 0;
        bathGrade = 0;
        toiletGrade = 0;
        sleepGrade = 0f;

        ClearStats?.Invoke();
    }
}
