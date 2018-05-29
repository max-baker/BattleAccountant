﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MissionManager : MonoBehaviour {

    public class MissionData
    {
        public string Faction;
        public int DaysToExpiration;
        public string Location;

        public MissionData()
        {
            DaysToExpiration = (int)Random.Range(1, 20);
            Faction = StaticValues.GenerateFaction();
            Location = StaticValues.GetRandomPlanet();
        }

        public string OutputMissionString()
        {
            return Faction + " : Expires in "  + DaysToExpiration.ToString() +" Days : On "+Location;
        }
    }

    private List<MissionData> AvailableMissions;
    private int MissionIndex;
    public GameObject MissionButton;
    public GameObject UICanvas;
    public GameObject MissionContainer;
    public GameObject MissionParams;
    [HideInInspector] public List<GameObject> MissionHolderUIList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        AvailableMissions = GenerateStartingMissions(3);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<MissionData> GenerateStartingMissions(int NumberOfMissions)
    {
        List<MissionData> StartingMissions = new List<MissionData>();
        for (int i = 0; i < NumberOfMissions; i++)
        {
            StartingMissions.Add(new MissionData());
        }
        return StartingMissions;
    }

    public void HideMissions()
    {
        foreach (GameObject elem in MissionHolderUIList)
        {
            Destroy(elem);
        }
        MissionButton.GetComponent<Button>().interactable = true;
    }

    public void DisplayMissions()
    {
        gameObject.GetComponent<UIManager>().HideAllMenus();
        MissionButton.GetComponent<Button>().interactable = false;
        int i = 0;
        foreach (MissionData mission in AvailableMissions)
        {
            GameObject MissionHolder = Instantiate(MissionContainer);
            MissionHolder.SetActive(true);
            MissionHolder.transform.SetParent(UICanvas.transform);
            MissionHolder.GetComponentInChildren<Text>().text = mission.OutputMissionString();
            MissionHolder.transform.localScale = MissionContainer.transform.localScale;
            MissionHolder.transform.localPosition = new Vector3(0, 110 - (55 * i), 0);
            MissionHolderUIList.Add(MissionHolder);
            MissionHolder.name = i.ToString();
            MissionHolder.GetComponent<Button>().onClick.AddListener(ShowMissionDetails);
            MissionHolder.name = i.ToString();
            i++;
            bool OnPlanet = (mission.Location == gameObject.GetComponent<ShipManager>().GetCurrentPlanet());
            MissionHolder.GetComponent<Button>().interactable = OnPlanet;
        }
        AddBackButton();
    }

    public void ShowMissionDetails()
    {
        MissionIndex = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        gameObject.GetComponent<UIManager>().HideAllMenus();
        MissionButton.GetComponent<Button>().interactable = false;
        GameObject MissionDetails = Instantiate(MissionParams);
        MissionDetails.SetActive(true);
        MissionDetails.transform.SetParent(UICanvas.transform);
        MissionDetails.transform.position = MissionParams.transform.position;
        MissionDetails.transform.localScale = MissionParams.transform.localScale;
        MissionHolderUIList.Add(MissionDetails);
        Text[] TextFields = MissionDetails.GetComponentsInChildren<Text>();
        foreach (Text attr in TextFields)
        {
            if (attr.gameObject.name == "MissionTitle")
            {
                attr.text = AvailableMissions[MissionIndex].OutputMissionString();
            }
        }
        Button[] Buttons = MissionDetails.GetComponentsInChildren<Button>();
        foreach (Button button in Buttons)
        {
            if (button.gameObject.name == "EmbarkButton")
            {
                button.onClick.AddListener(GoOnMission);
            }
        }
        AddBackButton();
    }

    public void AddBackButton()
    {
        GameObject HideMissionButton = Instantiate(MissionButton);
        HideMissionButton.GetComponent<Button>().interactable = true;
        HideMissionButton.transform.SetParent(UICanvas.transform);
        HideMissionButton.GetComponentInChildren<Text>().text = "Back";
        HideMissionButton.transform.localScale = MissionButton.transform.localScale;
        HideMissionButton.transform.localPosition = new Vector3(120, -220, 0);
        HideMissionButton.GetComponent<Button>().onClick.AddListener(HideMissions);
        MissionHolderUIList.Add(HideMissionButton);
    }

    public void GoOnMission()
    {
        gameObject.GetComponent<UIManager>().HideAllMenus();
        gameObject.GetComponent<AudioSource>().Play();
        AvailableMissions.RemoveAt(MissionIndex);
        int result = (int)Random.Range(1, 6); // 1 is bad, 5 is great
        print(result.ToString());
        TransactionManage transactions = gameObject.GetComponent<TransactionManage>();
        MechManager MyMechManager = gameObject.GetComponent<MechManager>();
        CharacterManager MyCharacters = gameObject.GetComponent<CharacterManager>();
        switch (result)
        {
            case 1://Diasaster
                transactions.PassTime(10);
                if (!transactions.SpendCash(1000))
                {
                    MyCharacters.KillCrew(3);
                }
                MyMechManager.DestroyMechs(3);
                break;
            case 2://Route
                transactions.PassTime(7);
                if (!transactions.SpendCash(500))
                {
                    MyCharacters.KillCrew(2);
                }
                MyMechManager.DestroyMechs(2);
                break;
            case 3://Tough Battle
                transactions.PassTime(5);
                MyMechManager.DestroyMechs(1);
                transactions.MakeCash(200);
                break;
            case 4://Victory
                transactions.PassTime(3);
                MyMechManager.DestroyMechs(1);
                transactions.MakeCash(400);
                break;
            case 5://Decisive Victory
                transactions.PassTime(3);
                transactions.MakeCash(600);
                break;
        }
    }

}
