using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipManager : MonoBehaviour {

    public class ShipData
    {
        public string name;
        public int MaxCrew;
        public int MaxMechs;
        public string model;

        public ShipData()
        {
            name = "The Defiance";
            model = "Oppresor Class Dropship";
            MaxCrew = StaticValues.ShipMaxCrew;
            MaxMechs = StaticValues.ShipMaxMechs;
        }
    }

    private ShipData CurrentShip; 
    public GameObject ShipButton;
    public GameObject UICanvas;
    public GameObject ShipImageBG;
    public GameObject ShipImage;
    public GameObject ShipDetails;
    [HideInInspector] public List<GameObject> ShipUIList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        ShipButton.GetComponent<Button>().onClick.AddListener(DisplayShip);
        CurrentShip = new ShipData();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int ShipCrewLimit()
    {
        return CurrentShip.MaxCrew;
    }

    public void DisplayShip()
    {
        gameObject.GetComponent<UIManager>().HideAllMenus();
        ShipButton.GetComponent<Button>().interactable = false;
        AddBackButton();

        GameObject ShipHolder = Instantiate(ShipImageBG);
        ShipHolder.transform.position = ShipImageBG.transform.position;
        ShipUIList.Add(ShipHolder);
        GameObject ShipPicture = Instantiate(ShipImage);
        ShipPicture.transform.position = ShipImage.transform.position;
        ShipUIList.Add(ShipPicture);

        GameObject TravelButton = Instantiate(ShipButton);
        TravelButton.GetComponent<Button>().interactable = true;
        TravelButton.transform.SetParent(UICanvas.transform);
        TravelButton.GetComponentInChildren<Text>().text = "Map";
        TravelButton.transform.localScale = ShipButton.transform.localScale;
        TravelButton.transform.localPosition = new Vector3(-340, -120, 0);
        ShipUIList.Add(TravelButton);

        GameObject ShipInfo = Instantiate(ShipDetails);
        ShipInfo.SetActive(true);
        ShipInfo.transform.SetParent(UICanvas.transform);
        ShipInfo.transform.position = ShipDetails.transform.position;
        ShipInfo.transform.localScale = ShipDetails.transform.localScale;
        Text[] TextFields = ShipInfo.GetComponentsInChildren<Text>();
        foreach (Text attr in TextFields)
        {
            if (attr.gameObject.name == "ShipName")
            {
                attr.text = CurrentShip.name;
            }
            if (attr.gameObject.name == "ShipModel")
            {
                attr.text = CurrentShip.model;
            }
            if (attr.gameObject.name == "ShipMaxMechs")
            {
                attr.text = "Holds "+CurrentShip.MaxMechs + " Mechs";
            }
            if (attr.gameObject.name == "ShipMaxCrew")
            {
                attr.text = "Holds " + CurrentShip.MaxCrew + " Crew";
            }
        }
        ShipUIList.Add(ShipInfo);
    }

    public void HideShip()
    {
        ShipButton.GetComponent<Button>().interactable = true;
        foreach (GameObject elem in ShipUIList)
        {
            Destroy(elem);
        }
    }

    public void AddBackButton()
    {
        GameObject HideShipButton = Instantiate(ShipButton);
        HideShipButton.GetComponent<Button>().interactable = true;
        HideShipButton.transform.SetParent(UICanvas.transform);
        HideShipButton.GetComponentInChildren<Text>().text = "Back";
        HideShipButton.transform.localScale = ShipButton.transform.localScale;
        HideShipButton.transform.localPosition = new Vector3(120, -220, 0);
        HideShipButton.GetComponent<Button>().onClick.AddListener(HideShip);
        ShipUIList.Add(HideShipButton);
    }
}
