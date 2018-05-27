using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BalanceManager : MonoBehaviour {

    public GameObject BalanceButton;
    public GameObject UICanvas;
    [HideInInspector] public List<GameObject> BalanceUIList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        BalanceButton.GetComponent<Button>().onClick.AddListener(DisplayBalance);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisplayBalance()
    {
        gameObject.GetComponent<UIManager>().HideAllMenus();
        BalanceButton.GetComponent<Button>().interactable = false;
        AddBackButton();
    }

    public void HideBalance()
    {
        BalanceButton.GetComponent<Button>().interactable = true;
        foreach (GameObject elem in BalanceUIList)
        {
            Destroy(elem);
        }
    }

    public void AddBackButton()
    {
        GameObject HideBalanceButton = Instantiate(BalanceButton);
        HideBalanceButton.GetComponent<Button>().interactable = true;
        HideBalanceButton.transform.SetParent(UICanvas.transform);
        HideBalanceButton.GetComponentInChildren<Text>().text = "Back";
        HideBalanceButton.transform.localScale = BalanceButton.transform.localScale;
        HideBalanceButton.transform.localPosition = new Vector3(120, -220, 0);
        HideBalanceButton.GetComponent<Button>().onClick.AddListener(HideBalance);
        BalanceUIList.Add(HideBalanceButton);
    }
}
