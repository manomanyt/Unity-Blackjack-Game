using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public AudioManager audioManager;

    public int walletBalance = 20000;
    public int cashBalance = 0;

    private TextMeshProUGUI walletText;
    private TextMeshProUGUI cashText;
    private Button depositButton;
    private Button withdrawButton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BlackJackScene3")
        {
            walletText = GameObject.Find("WalletBalance")?.GetComponent<TextMeshProUGUI>();
            cashText = GameObject.Find("CashBalance")?.GetComponent<TextMeshProUGUI>();
            
            depositButton = GameObject.Find("DepositButton")?.GetComponent<Button>();
            withdrawButton = GameObject.Find("WithdrawButton")?.GetComponent<Button>();

            if (depositButton != null)
            {
                depositButton.onClick.RemoveAllListeners();
                depositButton.onClick.AddListener(() => Deposit(1000));
            }

            if (withdrawButton != null)
            {
                withdrawButton.onClick.RemoveAllListeners();
                withdrawButton.onClick.AddListener(() => Withdraw(1000));
            }

            UpdateWalletUI();
        }
    }

    public void Deposit(int amount)
    {
         if (cashBalance >= amount)
        {
            cashBalance -= amount;
            walletBalance += amount;
            UpdateWalletUI();
        }
        else
        {
            Debug.Log("Not enough cash to deposit!");
        }
    }

    public void Withdraw(int amount)
    {
        if (walletBalance >= amount)
        {
            walletBalance -= amount;
            cashBalance += amount;
            UpdateWalletUI();
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void UpdateWalletUI()
    {
        if (walletText != null)
            walletText.text = "Wallet: $" + walletBalance;

        if (cashText != null)
            cashText.text = "Cash: $" + cashBalance;
    }
}
