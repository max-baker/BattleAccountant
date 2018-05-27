using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriorityCycler : MonoBehaviour {
    private int priorityIndex = 0;
    private string[] Priorities={"Caution", "No Prisoners", "At All Costs"};

	// Use this for initialization
	void Start () {
        gameObject.GetComponentInChildren<Text>().text = Priorities[priorityIndex];
	}
	
	public void CyclePriority()
    {
        priorityIndex = (priorityIndex + 1) % Priorities.Length;
        gameObject.GetComponentInChildren<Text>().text = Priorities[priorityIndex];
    }

    public string GetPriority()
    {
        return Priorities[priorityIndex];
    }
}
