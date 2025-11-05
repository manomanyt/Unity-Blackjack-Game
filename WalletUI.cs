using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
  public TextMeshProUGUI walletText;
  public TextMeshProUGUI cashText;
  public Button depositButton;
  public Button withdrawButton;

    void Start()
    {
        depositButton.onClick.RemoveAllListeners();
        withdrawButton.onClick.RemoveAllListeners();

        depositButton.onClick.AddListener(Deposit1000);
        withdrawButton.onClick.AddListener(Withdraw1000);
        
        UpdateUI();
    }

    public void Withdraw1000()
    {
        if (MoneyManager.Instance.walletBalance >= 1000)
        {
            MoneyManager.Instance.walletBalance -= 1000;
            MoneyManager.Instance.cashBalance += 1000;
            UpdateUI();
        }
    }

    public void Deposit1000()
    {
        if (MoneyManager.Instance.cashBalance >= 1000)
        {
            MoneyManager.Instance.walletBalance += 1000;
            MoneyManager.Instance.cashBalance -= 1000;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        walletText.text = "Wallet: $" + MoneyManager.Instance.walletBalance;
        cashText.text = "Cash: $" + MoneyManager.Instance.cashBalance;
    }

}
