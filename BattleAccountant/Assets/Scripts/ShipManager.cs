using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipManager : MonoBehaviour {

    public GameObject ShipButton;
    public GameObject UICanvas;
    [HideInInspector] public List<GameObject> ShipUIList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        ShipButton.GetComponent<Button>().onClick.AddListener(DisplayShip);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisplayShip()
    {
        gameObject.GetComponent<UIManager>().HideAllMenus();
        ShipButton.GetComponent<Button>().interactable = false;
        AddBackButton();
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
