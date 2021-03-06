﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MechManager : MonoBehaviour {

    public class MechData
    {
        public string model;
        public int age;
        public string name;
        public List<int> SelectedWeapons;

        public MechData()
        {
            age = (int)Random.Range(1, 20);
            model = StaticValues.GenerateMechModel();
            name = StaticValues.GenerateMechName();
            SelectedWeapons = new List<int>(new int[] {0,0,0,0});
        }

        public string OutputMechString()
        {
            return name+" : "+model + " : " + age + " Years Old";
        }
    }

    public List<MechData> CurrentMechs;
    public int MechIndex;
    public GameObject MechButton;
    public GameObject MechContainer;
    public GameObject UICanvas;
    public GameObject MechImage1;
    public GameObject MechImage2;
    public GameObject MechImage3;
    public GameObject MechImageBG;
    public GameObject MechDataHolder;
    public GameObject WeaponSlot;
    [HideInInspector] public List<GameObject> MechHolderUIList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        CurrentMechs = GenerateStartingMechs(4);
        MechButton.GetComponent<Button>().onClick.AddListener(DisplayMechs);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<MechData> GenerateStartingMechs(int NumberOfMechs)
    {
        List<MechData> StartingMechs = new List<MechData>();
        for (int i = 0; i < NumberOfMechs; i++)
        {
            StartingMechs.Add(new MechData());
        }
        return StartingMechs;
    }

    public void DisplayMechs()
    {
        gameObject.GetComponent<UIManager>().HideAllMenus();
        MechButton.GetComponent<Button>().interactable = false;
        int i = 0;
        foreach (MechData mech in CurrentMechs)
        {
            GameObject MechHolder = Instantiate(MechContainer);
            MechHolder.SetActive(true);
            MechHolder.transform.SetParent(UICanvas.transform);
            MechHolder.GetComponentInChildren<Text>().text = mech.OutputMechString();
            MechHolder.transform.localScale = MechContainer.transform.localScale;
            MechHolder.transform.localPosition = new Vector3(-450 + (225 * (i % 5)), 100 - (100 * (i / 5)), 0);
            MechHolderUIList.Add(MechHolder);
            MechHolder.name = i.ToString();
            MechHolder.GetComponent<Button>().onClick.AddListener(ShowMechDetails);
            i++;
        }
        //Add Hide Mechs Button
        AddBackButton();
    }

    public void HideMechs()
    {
        foreach (GameObject elem in MechHolderUIList)
        {
            Destroy(elem);
        }
        MechButton.GetComponent<Button>().interactable = true;
    }

    public void ShowMechDetails()
    {
        MechIndex = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        gameObject.GetComponent<UIManager>().HideAllMenus();
        MechButton.GetComponent<Button>().interactable = false;
        GameObject MechHolder = Instantiate(MechImageBG);
        MechHolder.transform.position = MechImageBG.transform.position;
        MechHolderUIList.Add(MechHolder);
        GameObject MechImageObj;
        switch (CurrentMechs[MechIndex].model)
        {
            case ("Mad Cat"):
                MechImageObj = Instantiate(MechImage1);               
                break;
            case ("Rifleman"):
                MechImageObj = Instantiate(MechImage2);
                break;
            case ("Star Adder"):
                MechImageObj = Instantiate(MechImage3);
                break;
            default:
                print("model not found");
                MechImageObj = Instantiate(MechImage1);
                break;
        }
        MechImageObj.transform.position = MechImage1.transform.position;
        MechHolderUIList.Add(MechImageObj);

        GameObject MechDetails = Instantiate(MechDataHolder);
        MechDetails.SetActive(true);
        MechDetails.transform.SetParent(UICanvas.transform);
        MechDetails.transform.position = MechDataHolder.transform.position;
        MechDetails.transform.localScale = MechDataHolder.transform.localScale;
        Text[] TextFields = MechDetails.GetComponentsInChildren<Text>();
        foreach(Text attr in TextFields)
        {
            if (attr.gameObject.name == "Placeholder")
            {
                attr.text = CurrentMechs[MechIndex].name;
            }
            if (attr.gameObject.name == "MechModel")
            {
                attr.text = CurrentMechs[MechIndex].model;
            }
            if (attr.gameObject.name == "MechAge")
            {
                attr.text = CurrentMechs[MechIndex].age.ToString()+ " Years Old";
            }
        }
        MechDetails.GetComponentInChildren<InputField>().onValueChanged.AddListener(ChangeMechName);
        MechHolderUIList.Add(MechDetails);

        GameObject HideMechButton = Instantiate(MechButton);
        HideMechButton.GetComponent<Button>().interactable = true;
        HideMechButton.transform.SetParent(UICanvas.transform);
        HideMechButton.GetComponentInChildren<Text>().text = "Back";
        HideMechButton.transform.localScale = MechButton.transform.localScale;
        HideMechButton.transform.localPosition = new Vector3(120, StaticValues.BackButtonY, 0);
        HideMechButton.GetComponent<Button>().onClick.AddListener(DisplayMechs);
        MechHolderUIList.Add(HideMechButton);

        DisplayMechWeaponSlots();
    }

    private void DisplayMechWeaponSlots()
    {
        int NumberOfSlots = StaticValues.GetWeaponSlotsForMechModel(CurrentMechs[MechIndex].model);
        for(int i=0; i < NumberOfSlots; i++)
        {
            GameObject WeaponHolder = Instantiate(WeaponSlot, UICanvas.transform);
            WeaponHolder.SetActive(true);     
            WeaponHolder.transform.Find("Label").GetComponent<Text>().text = StaticValues.GetWeaponSlotsName(CurrentMechs[MechIndex].model, i) + ':';
            WeaponHolder.GetComponentInChildren<Dropdown>().ClearOptions();         
            WeaponHolder.GetComponent<WeaponSlotManager>().SlotNumber = i;
            WeaponHolder.GetComponentInChildren<Dropdown>().AddOptions(StaticValues.WeaponOptions);
            WeaponHolder.GetComponentInChildren<Dropdown>().value = CurrentMechs[MechIndex].SelectedWeapons[i];
            //WeaponHolder.transform.position = new Vector3(450, 50 - (i * 100), 0);
            WeaponHolder.transform.position = new Vector3(8, 1-(1.5f*i), 0); //Postition multiplied by 52, idk why
            WeaponHolder.transform.localScale = WeaponSlot.transform.localScale;            
            MechHolderUIList.Add(WeaponHolder);
        }
    }

    public void ChangeMechWeapon(int SlotNumber, int NewWeapon)
    {
        CurrentMechs[MechIndex].SelectedWeapons[SlotNumber] = NewWeapon;
    }

    public void ChangeMechName(string newName)
    {
        CurrentMechs[MechIndex].name = newName;
    }

    public void AddBackButton()
    {
        GameObject HideMechButton = Instantiate(MechButton);
        HideMechButton.GetComponent<Button>().interactable = true;
        HideMechButton.transform.SetParent(UICanvas.transform);
        HideMechButton.GetComponentInChildren<Text>().text = "Back";
        HideMechButton.transform.localScale = MechButton.transform.localScale;
        HideMechButton.transform.localPosition = new Vector3(120, StaticValues.BackButtonY, 0);
        HideMechButton.GetComponent<Button>().onClick.AddListener(HideMechs);
        MechHolderUIList.Add(HideMechButton);
    }

    public void DestroyMechs(int NumberOfMechs)
    {
        while (NumberOfMechs > 0)
        {
            if (CurrentMechs.Count <= 0)
            {
                print("No Mechs");
                break;
            }
            int i = (int)Random.Range(0, CurrentMechs.Count);
            CurrentMechs.RemoveAt(i);
            NumberOfMechs--;
        }
    }

    public List<string> GetMechNames()
    {
        List<string> MechNames = new List<string>();
        foreach (MechData mech in CurrentMechs)
        {
            MechNames.Add(mech.name);
        }
        return MechNames;
    }
}
