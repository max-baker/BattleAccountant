using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BalanceManager : MonoBehaviour {

    private bool ShowingUI = false;

    public GameObject BalanceButton;
    public GameObject UICanvas;
    public GameObject BalanceUI;
    [HideInInspector] public List<LoanData> CurrentLoans = new List<LoanData>();
    [HideInInspector] public List<GameObject> BalanceUIList = new List<GameObject>();

    public class LoanData
    {
        public int PrincipleAmount;
        public int AmountDue;
        public float InterestRate;
        public int DaysToRepayment;

        public LoanData()
        {
            PrincipleAmount = (int)Random.Range(100, 2000);
            InterestRate = Random.Range(1, 6);
            DaysToRepayment = (int)Random.Range(10, 50);
            AmountDue = (int)(PrincipleAmount * (1 + (InterestRate / 100)));
        }

    }

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
        GameObject BalanceScreenUI = Instantiate(BalanceUI, UICanvas.transform);
        BalanceScreenUI.SetActive(true);
        BalanceUIList.Add(BalanceScreenUI);
        //AddBackButton();
        ShowingUI = true;
        DisplayLoans();
    }

    private void DisplayLoans()
    {
        foreach (LoanData loan in CurrentLoans)
        {
            GameObject LoanHolder = Instantiate(BalanceUI.transform.Find("LoanHolder").gameObject, UICanvas.transform);
            LoanHolder.SetActive(true);
            LoanHolder.GetComponentInChildren<Text>().text = loan.AmountDue.ToString() + " In " + loan.DaysToRepayment.ToString() + " Days";
            //LoanTakenHolder.transform.position = new Vector3(-350, 170 - (70 * CurrentLoans.Count), 0);
            LoanHolder.transform.position = new Vector3(-6.2f, 3.1276f - 1 * (CurrentLoans.IndexOf(loan)+1), 0);
            BalanceUIList.Add(LoanHolder);
        }
    }

    public void HideBalance()
    {
        BalanceButton.GetComponent<Button>().interactable = true;
        foreach (GameObject elem in BalanceUIList)
        {
            Destroy(elem);
        }
        ShowingUI = false;
    }

    public void AddBackButton()
    {
        GameObject HideBalanceButton = Instantiate(BalanceButton);
        HideBalanceButton.GetComponent<Button>().interactable = true;
        HideBalanceButton.transform.SetParent(UICanvas.transform);
        HideBalanceButton.GetComponentInChildren<Text>().text = "Back";
        HideBalanceButton.transform.localScale = BalanceButton.transform.localScale;
        HideBalanceButton.transform.localPosition = new Vector3(120, StaticValues.BackButtonY, 0);
        HideBalanceButton.GetComponent<Button>().onClick.AddListener(HideBalance);
        BalanceUIList.Add(HideBalanceButton);
    }

    public void TakeLoan()
    {
        LoanData TakenLoan = new LoanData();
        CurrentLoans.Add(TakenLoan);
        gameObject.GetComponent<TransactionManage>().MakeCash(TakenLoan.PrincipleAmount);
        GameObject LoanTakenHolder = Instantiate(BalanceUI.transform.Find("LoanHolder").gameObject, UICanvas.transform);
        LoanTakenHolder.SetActive(true);
        BalanceUIList.Add(LoanTakenHolder);
        LoanTakenHolder.GetComponentInChildren<Text>().text = TakenLoan.AmountDue.ToString()+ " In " + TakenLoan.DaysToRepayment.ToString() + " Days";
        //LoanTakenHolder.transform.position = new Vector3(-350, 170 - (70 * CurrentLoans.Count), 0);
        LoanTakenHolder.transform.position = new Vector3(-6.2f,3.1276f  - (1 * CurrentLoans.Count), 0);
    }

    public void DecrementDays()
    {
        List<LoanData> LoansToRemove = new List<LoanData>();
        foreach(LoanData loan in CurrentLoans)
        {
            loan.DaysToRepayment--;
            if (loan.DaysToRepayment <= 0)
            {
                if (!gameObject.GetComponent<TransactionManage>().SpendCash(loan.AmountDue))
                {
                    print("Can't Repay, Insert Negative consequence");
                }
                LoansToRemove.Add(loan);
                gameObject.GetComponent<TransactionManage>().DisplayCash();
            }
        }
        foreach (LoanData oldLoan in LoansToRemove)
        {
            CurrentLoans.Remove(oldLoan);
        }
            
        if (ShowingUI)
        {
            DisplayBalance();
        }
    }
}
