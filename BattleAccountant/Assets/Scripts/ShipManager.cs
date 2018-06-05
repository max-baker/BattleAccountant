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
    public GameObject Map;
    private string CurrentPlanet = "Helios";
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
        TravelButton.transform.localPosition = new Vector3(-280, -160, 0);
        TravelButton.GetComponent<Button>().onClick.AddListener(DisplayMap);
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
        HideShipButton.transform.localPosition = new Vector3(120, StaticValues.BackButtonY, 0);
        HideShipButton.GetComponent<Button>().onClick.AddListener(HideShip);
        ShipUIList.Add(HideShipButton);
    }

    public void DisplayMap()
    {
        gameObject.GetComponent<UIManager>().HideAllMenus();

        GameObject HideShipButton = Instantiate(ShipButton);
        HideShipButton.GetComponent<Button>().interactable = true;
        HideShipButton.transform.SetParent(UICanvas.transform);
        HideShipButton.GetComponentInChildren<Text>().text = "Back";
        HideShipButton.transform.localScale = ShipButton.transform.localScale;
        HideShipButton.transform.localPosition = new Vector3(120, -220, 0);
        HideShipButton.GetComponent<Button>().onClick.AddListener(DisplayShip);
        ShipUIList.Add(HideShipButton);

        ShipButton.GetComponent<Button>().interactable = false;

        GameObject MapHolder = Instantiate(Map);
        MapHolder.SetActive(true);
        MapHolder.transform.SetParent(UICanvas.transform);
        MapHolder.transform.position = Map.transform.position;
        MapHolder.transform.localScale = Map.transform.localScale;
        Button[] buttons = MapHolder.GetComponentsInChildren<Button>();
        foreach (Button attr in buttons)
        {
            if (attr.gameObject.name == "IcarusButton")
            {
                if (CurrentPlanet == "Icarus" || CurrentPlanet == "Space")
                {
                    attr.interactable = false;
                }
                else
                {
                    attr.onClick.AddListener(() => MovePlanets("Icarus"));
                    int TravelTime = StaticValues.GetDistanceBetweenPlanets(CurrentPlanet, "Icarus");
                    attr.gameObject.GetComponentInChildren<Text>().text = "Icarus: " + TravelTime + " Days";
                }
            }
            if (attr.gameObject.name == "HeliosButton")
            {
                if (CurrentPlanet == "Helios" || CurrentPlanet == "Space")
                {
                    attr.interactable = false;
                }
                else
                {
                    attr.onClick.AddListener(() => MovePlanets("Helios"));
                    int TravelTime = StaticValues.GetDistanceBetweenPlanets(CurrentPlanet, "Helios");
                    attr.gameObject.GetComponentInChildren<Text>().text = "Helios: " + TravelTime + " Days";
                }
            }
            if (attr.gameObject.name == "CerebusButton")
            {
                if (CurrentPlanet == "Cerebus" || CurrentPlanet == "Space")
                {
                    attr.interactable = false;
                }
                else
                {
                    attr.onClick.AddListener(() => MovePlanets("Cerebus"));
                    int TravelTime = StaticValues.GetDistanceBetweenPlanets(CurrentPlanet, "Cerebus");
                    attr.gameObject.GetComponentInChildren<Text>().text = "Cerebus: " + TravelTime + " Days";
                }
            }
            if (attr.gameObject.name == "KronosButton")
            {
                if (CurrentPlanet == "Kronos" || CurrentPlanet == "Space")
                {
                    attr.interactable = false;
                }
                else
                {
                    attr.onClick.AddListener(() => MovePlanets("Kronos"));
                    int TravelTime = StaticValues.GetDistanceBetweenPlanets(CurrentPlanet, "Kronos");
                    attr.gameObject.GetComponentInChildren<Text>().text = "Kronos: " + TravelTime + " Days";
                }
            }
        }
        ShipUIList.Add(MapHolder);
    }

    public void MovePlanets(string Destination)
    {
        int TravelTime = StaticValues.GetDistanceBetweenPlanets(CurrentPlanet, Destination);
        gameObject.GetComponent<TransactionManage>().TravelToPlanet(Destination, TravelTime);
        CurrentPlanet = "Space";
        gameObject.GetComponent<UIManager>().HideAllMenus();
    }

    public void ArriveAtPlanet(string planet)
    {
        CurrentPlanet = planet;
    }

    public string GetCurrentPlanet()
    {
        return CurrentPlanet;
    }
}
