using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransactionManage : MonoBehaviour {

    public GameObject cashDisplay;
    private int cash = 1500;

    public void MakeMoney()
    {
        cash += 100;
        cashDisplay.GetComponent<Text>().text = "Money: " + cash;
    }

}
