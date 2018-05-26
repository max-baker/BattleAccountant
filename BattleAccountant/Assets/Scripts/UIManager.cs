using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HideAllMenus()
    {
        gameObject.GetComponent<CharacterManager>().HideCrew();
        gameObject.GetComponent<MechManager>().HideMechs();
        gameObject.GetComponent<MissionManager>().HideMissions();
    }
}
