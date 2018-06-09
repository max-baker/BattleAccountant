using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class BalanceManager : MonoBehaviour {

    private bool ShowingUI = false;

    public GameObject BalanceButton;
    public GameObject UICanvas;
    public GameObject BalanceUI;
    [HideInInspector] public List<LoanData> CurrentLoans = new List<LoanData>();
    [HideInInspector] public List<StockData> CurrentStocks = new List<StockData>();
    [HideInInspector] public List<BankData> KnownBanks = new List<BankData>();
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

    public class StockData
    {
        public int InitialValue;
        public int CurrentValue;

        public StockData(int StockCost)
        {
            InitialValue = StockCost;
            CurrentValue = StockCost;
        }

        public void ProgressStock()
        {
            float OnePercentValue = CurrentValue / 100;
            int PercentChange = (int)Random.Range(-5, 10);
            CurrentValue += (int)(OnePercentValue * PercentChange);
        }

    }

    public class BankData
    {
        public int CurrentBalance;
        public string Location;

        public BankData()
        {
            CurrentBalance = 0;
            Location = StaticValues.GetRandomPlanet();
        }

        public void DepositCash(int amount)
        {
            CurrentBalance += amount;
        }

        public bool WithdrawCash(int amount)
        {
            if (CurrentBalance >= amount)
            {
                CurrentBalance -= amount;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckBalance(int amount)
        {
            if (CurrentBalance >= amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    // Use this for initialization
    void Start () {
        BalanceButton.GetComponent<Button>().onClick.AddListener(DisplayBalance);
        PopulateBanks();
    }

    private void PopulateBanks()
    {
        for(int i = 0; i < 4; i++)
        {
            KnownBanks.Add(new BankData());
        }
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
        DisplayStocks();
        DisplayBanks();
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

    public void DayPasses()
    {
        DecrementLoanDays();
        ProgressStocks();
    }

    private void DecrementLoanDays()
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

    public void BuyStock()
    {
        if (gameObject.GetComponent<TransactionManage>().SpendCash(100))
        {
            CurrentStocks.Add(new StockData(100));
            DisplayBalance();
        }
    }

    private void DisplayStocks()
    {
        foreach (StockData stock in CurrentStocks)
        {
            GameObject StockHolder = Instantiate(BalanceUI.transform.Find("StockDisplay").gameObject, UICanvas.transform);
            StockHolder.SetActive(true);
            BalanceUIList.Add(StockHolder);
            StockHolder.GetComponentInChildren<Text>().text = "Value: "+stock.CurrentValue.ToString();
            StockHolder.transform.position = new Vector3(0, 3.1276f - 1 * (CurrentStocks.IndexOf(stock) + 1), 0);
            StockHolder.GetComponentInChildren<Button>().onClick.AddListener(SellStock);
        }
    }

    public void SellStock()
    {
        int StockIndex = -(int)Mathf.Round(EventSystem.current.currentSelectedGameObject.transform.parent.position.y) +2;
        StockData StockSold = CurrentStocks[StockIndex];
        gameObject.GetComponent<TransactionManage>().MakeCash(StockSold.CurrentValue);
        CurrentStocks.Remove(StockSold);
        DisplayBalance();
    }

    private void ProgressStocks()
    {
        foreach(StockData stock in CurrentStocks)
        {
            stock.ProgressStock();
        }
    }

    private void DisplayBanks()
    {
        foreach (BankData bank in KnownBanks)
        {
            GameObject BankHolder = Instantiate(BalanceUI.transform.Find("BankDisplay").gameObject, UICanvas.transform);
            BankHolder.SetActive(true);
            BalanceUIList.Add(BankHolder);
            BankHolder.GetComponentInChildren<Text>().text = "Balance: " + bank.CurrentBalance.ToString()+"\n on "+bank.Location;
            Button WithdrawButton = BankHolder.transform.Find("Withdraw").GetComponent<Button>();
            Button DepositButton = BankHolder.transform.Find("Deposit").GetComponent<Button>();
            if (bank.Location == gameObject.GetComponent<ShipManager>().GetCurrentPlanet())
            {
                WithdrawButton.interactable = true;
                DepositButton.interactable = true;
                WithdrawButton.onClick.AddListener(BankWithdraw);
                DepositButton.onClick.AddListener(BankDeposit);
            }
            else
            {
                WithdrawButton.interactable = false;
                DepositButton.interactable = false;
            }
            BankHolder.transform.position = new Vector3(6.2f, 3.1276f - 1 * (KnownBanks.IndexOf(bank) + 1), 0);            
        }
    }

    private void BankWithdraw()
    {
        int BankIndex = -(int)Mathf.Round(EventSystem.current.currentSelectedGameObject.transform.parent.position.y) + 2;
        BankData bank = KnownBanks[BankIndex];
        if (bank.WithdrawCash(10))
        {
            gameObject.GetComponent<TransactionManage>().MakeCash(10);
        }
        DisplayBalance();
    }

    private void BankDeposit()
    {
        int BankIndex = -(int)Mathf.Round(EventSystem.current.currentSelectedGameObject.transform.parent.position.y) + 2;
        BankData bank = KnownBanks[BankIndex];
        if (gameObject.GetComponent<TransactionManage>().SpendCash(10))
        {
            bank.DepositCash(10);
        }
        DisplayBalance();
        gameObject.GetComponent<TransactionManage>().DisplayCash();
    }
}
