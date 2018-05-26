using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechManager : MonoBehaviour {

    public class MechData
    {
        private string model;
        private int age;

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
            i++;
        }
        //Add Hide Mechs Button
        GameObject HideMechButton = Instantiate(MechButton);
        HideMechButton.GetComponent<Button>().interactable = true;
        HideMechButton.transform.SetParent(UICanvas.transform);
        HideMechButton.GetComponentInChildren<Text>().text = "Back";
        HideMechButton.transform.localScale = MechButton.transform.localScale;
        HideMechButton.transform.localPosition = new Vector3(120, -220, 0);
        HideMechButton.GetComponent<Button>().onClick.AddListener(HideMechs);
        MechHolderUIList.Add(HideMechButton);

    }

    public void HideMechs()
    {
        foreach (GameObject elem in MechHolderUIList)
        {
            Destroy(elem);
        }
        MechButton.GetComponent<Button>().interactable = true;
    }
}
