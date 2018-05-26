using System.Collections;
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

        public MechData()
        {
            age = (int)Random.Range(1, 20);
            model = StaticValues.GenerateMechModel();
            name = StaticValues.GenerateMechName();
        }

        public string OutputMechString()
        {
            return name+" : "+model + " : " + age + " Years Old";
        }
    }

    private List<MechData> CurrentMechs;
    private int MechIndex;
    public GameObject MechButton;
    public GameObject MechContainer;
    public GameObject UICanvas;
    public GameObject MechImage1;
    public GameObject MechImage2;
    public GameObject MechImage3;
    public GameObject MechImageBG;
    public GameObject MechDataHolder;
    [HideInInspector] public List<GameObject> MechHolderUIList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        CurrentMechs = GenerateStartingMechs(5);
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
        print(CurrentMechs[MechIndex].OutputMechString());
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
        HideMechButton.transform.localPosition = new Vector3(120, -220, 0);
        HideMechButton.GetComponent<Button>().onClick.AddListener(DisplayMechs);
        MechHolderUIList.Add(HideMechButton);
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
        HideMechButton.transform.localPosition = new Vector3(120, -220, 0);
        HideMechButton.GetComponent<Button>().onClick.AddListener(HideMechs);
        MechHolderUIList.Add(HideMechButton);
    }
}
