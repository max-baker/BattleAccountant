using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject MenuButtons;
    public GameObject DateCashUI;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HideMenu()
    {
        GameObject[] MenuButtonsList = MenuButtons.GetComponentsInChildren<GameObject>();
    }
}
