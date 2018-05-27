using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager:MonoBehaviour  {

	public class CrewMember
    {
        private int health;
        private int fatigue;
        private int reputation;
        private string name;

        public CrewMember()
        {
            health = (int)Random.Range(0,100);
            fatigue = (int)Random.Range(0,100);
            reputation = (int)Random.Range(0,100);
            name = StaticValues.GenerateFirstName();
        }

        public string OutputCrewString()
        {
            return name + " : " + health + " health : " + fatigue + " fatigue : " + reputation + " reputation";
        }
    }

    private List<CrewMember> CurrentCrew;
    public GameObject CrewButton;
    public GameObject CrewMemberContainer;
    public GameObject UICanvas;
    [HideInInspector] public List<GameObject> CrewHolderUIList = new List<GameObject>();

    public void Start()
    {
        CurrentCrew = GenerateStartingCrew(3);
        CrewButton.GetComponent<Button>().onClick.AddListener(DisplayCrew);
    }
    public void Update()
    {
    }

    public CrewMember GenerateCrewMemeber()
    {
        return new CrewMember();
    }

    public List<CrewMember> GenerateStartingCrew(int numberOfCrew)
    {
        List<CrewMember> StartingCrew = new List<CrewMember>();
        for(int i = 0; i < numberOfCrew; i++)
        {
            StartingCrew.Add(new CrewMember());
        }
        return StartingCrew;
    }

    public void DisplayCrew()
    {
        gameObject.GetComponent<UIManager>().HideAllMenus();
        CrewButton.GetComponent<Button>().interactable = false;
        int i = 0;
        foreach (CrewMember crew in CurrentCrew)
        {
            GameObject CrewHolder = Instantiate(CrewMemberContainer);
            CrewHolder.SetActive(true);
            CrewHolder.transform.SetParent(UICanvas.transform);
            CrewHolder.GetComponentInChildren<Text>().text = crew.OutputCrewString();
            CrewHolder.transform.localScale = CrewMemberContainer.transform.localScale;
            CrewHolder.transform.localPosition = new Vector3(-450 + (225*(i%5)), 100-(100*(i/5)), 0);
            CrewHolderUIList.Add(CrewHolder);
            i++;

        }
        //Add Hire Crew Button
        GameObject HireButton = Instantiate(CrewButton);
        HireButton.GetComponent<Button>().interactable = true;
        HireButton.transform.SetParent(UICanvas.transform);
        HireButton.GetComponentInChildren<Text>().text = "Hire Crewman";
        HireButton.transform.localScale = CrewMemberContainer.transform.localScale;
        HireButton.transform.localPosition = new Vector3(-80, -220, 0);
        HireButton.GetComponent<Button>().onClick.AddListener(HireCrewMember);
        CrewHolderUIList.Add(HireButton);

        //Add Hide Crew Button
        GameObject HideCrewButton = Instantiate(CrewButton);
        HideCrewButton.GetComponent<Button>().interactable = true;
        HideCrewButton.transform.SetParent(UICanvas.transform);
        HideCrewButton.GetComponentInChildren<Text>().text = "Back";
        HideCrewButton.transform.localScale = CrewMemberContainer.transform.localScale;
        HideCrewButton.transform.localPosition = new Vector3(120, -220, 0);
        HideCrewButton.GetComponent<Button>().onClick.AddListener(HideCrew);
        CrewHolderUIList.Add(HideCrewButton);
    }

    public void HideCrew()
    {
        foreach (GameObject elem in CrewHolderUIList)
        {
            Destroy(elem);
        }
        CrewButton.GetComponent<Button>().interactable = true;
    }

    public void KillCrew(int Members)
    {
        while (Members > 0)
        {
            if (CurrentCrew.Count <= 0)
            {
                print("No Crew, You Lose");
                break;
            }
            int i = (int)Random.Range(0, CurrentCrew.Count);
            CurrentCrew.RemoveAt(i);
            Members--;
        }
    }

    public void HireCrewMember()
    {
        if (this.gameObject.GetComponent<TransactionManage>().SpendCash(StaticValues.CrewCost))
        {
            if (CurrentCrew.Count < StaticValues.MaxCrew)
            {
                foreach (GameObject elem in CrewHolderUIList)
                {
                    Destroy(elem);
                }
                CurrentCrew.Add(new CrewMember());
                DisplayCrew();
            }
            else
            {
                //Handles Max Crew
                print("ALready at Max Crew");
            }
        }
        else
        {
            //Handle no money
            print("No Cash");
        }
    }

}
