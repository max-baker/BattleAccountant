using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransactionManage : MonoBehaviour {

    public GameObject cashDisplay;
    public GameObject TimeDisplay;
    public GameObject PlayPauseText;
    public GameObject FastForwardText;
    public GameObject PlanetHolder;

    private int cash;
    private int month;
    private int day;
    private int year;
    private bool TimePlaying = true;
    private bool FastForwarding = false;
    private float FrameTimeDelay = 1;
    private float lastTime; 

    public void Start()
    {
        cash = StaticValues.StartingCash;
        month = StaticValues.StartMonth;
        day = StaticValues.StartDay;
        year = StaticValues.StartYear;
        lastTime = Time.time;
        DisplayCash();
    }

    public void Update()
    {
        if (TimePlaying)
        {
            if (Time.time > lastTime+ FrameTimeDelay)
            {
                lastTime = Time.time;
                day++;
                DisplayTime();
                //Time Events:
                cash+=3;
                DisplayCash();
            }
        }
    }

    public void TogglePlayPause()
    {
        PlayPauseText.GetComponent<Text>().text = (TimePlaying) ? "Play" : "Pause";
        TimePlaying = !TimePlaying;
    }

    public void ToggleFastForward()
    {
        FastForwardText.GetComponent<Text>().text = (FastForwarding) ? "1X" : "2X";
        FrameTimeDelay = (FastForwarding) ? 1f : 0.5f;
        FastForwarding = !FastForwarding;
    }

    public void DisplayTime()
    {
        while (day > StaticValues.DaysInMonth(month))
        {
            day-= StaticValues.DaysInMonth(month);
            month++;
        }
        while (month > 12)
        {
            month -= 12;
            year++;
        }
        TimeDisplay.GetComponent<Text>().text = "Sol: " + month + "-" + day + '-' + year;
    }

    public void DisplayCash()
    {
        cashDisplay.GetComponent<Text>().text = "Money: " + cash;
    }

    public bool SpendCash(int amount)
    {
        if(amount> cash)
        {
            return false;
        }
        else
        {
            cash -= amount;
            return true;
        }
    }

    public bool CheckCash(int amount)
    {
        return amount < cash;
    }

    public void MakeCash(int amount)
    {
        cash+=amount;
    }

    public void PassTime(int DaysPast)
    {
        day += DaysPast;
    }

    public void ChangePlanet(string planet)
    {
        PlanetHolder.GetComponent<Text>().text = planet;
    }
}
