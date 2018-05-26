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

        public MechData()
        {
            age = (int)Random.Range(1, 20);
            model = StaticValues.GenerateMechModel();
        }

        public string OutputMechString()
        {
            return model + " : " + age + " Years Old";
        }
    }

    private List<MechData> CurrentMechs;
    public GameObject MechButton;
    public GameObject MechContainer;
    public GameObject UICanvas;
    public GameObject MechImage1;
    public GameObject MechImage2;
    public GameObject MechImage3;
    public GameObject MechImageBG;
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
        int mechIndex = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        gameObject.GetComponent<UIManager>().HideAllMenus();
        print(CurrentMechs[mechIndex].OutputMechString());
        GameObject MechHolder = Instantiate(MechImageBG);
        MechHolder.transform.position = MechImageBG.transform.position;
        MechHolderUIList.Add(MechHolder);
        GameObject MechImageObj;
        switch (CurrentMechs[mechIndex].model)
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
        AddBackButton();
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
