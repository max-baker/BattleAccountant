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
    public GameObject OrbitBackground;
    public GameObject TravelBackground;

    private int cash;
    private int month;
    private int day;
    private int year;
    private bool TimePlaying = true;
    private bool FastForwarding = false;
    private float FrameTimeDelay = 2;
    private float lastTime;
    private bool traveling= false;
    private string destination;
    private int daysLeftInVoyage;

    public void Start()
    {
        cash = StaticValues.StartingCash;
        month = StaticValues.StartMonth;
        day = StaticValues.StartDay;
        year = StaticValues.StartYear;
        lastTime = Time.time;
        PlanetHolder.GetComponent<Text>().text = gameObject.GetComponent<ShipManager>().GetCurrentPlanet();
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
                DisplayCash();
                TravelProgress();
                gameObject.GetComponent<BalanceManager>().DecrementDays();
            }
        }
    }

    private void TravelProgress()
    {
        if (traveling)
        {
            daysLeftInVoyage--;
            if (daysLeftInVoyage <= 0)
            {
                traveling = false;
                PlanetHolder.GetComponent<Text>().text = destination;
                OrbitBackground.SetActive(true);
                TravelBackground.SetActive(false);
                gameObject.GetComponent<ShipManager>().ArriveAtPlanet(destination);
                gameObject.GetComponent<UIManager>().HideAllMenus();
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
        FrameTimeDelay = (FastForwarding) ? 2f : 1f;
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

    public void TravelToPlanet(string planet, int travelTime)
    {
        traveling = true;
        destination = planet;
        daysLeftInVoyage = travelTime;
        PlanetHolder.GetComponent<Text>().text = "Space";
        OrbitBackground.SetActive(false);
        TravelBackground.SetActive(true);
    }
}
