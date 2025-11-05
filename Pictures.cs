using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pictures : MonoBehaviour
{

    public Image pictureUI;    
    public Sprite brokeSprite;
    public Sprite richSprite;
    public Sprite normalSprite;
    public Sprite bitbrokeSprite;
    public Sprite bitrichSprite;
    public TextMeshProUGUI stateText;
    public Button loseButton;
    public Button winButton;

    void Start()
    {
        UpdatePicture();
    }

    void UpdatePicture()
    {
        int wallet = MoneyManager.Instance.walletBalance;
        int cash = MoneyManager.Instance.cashBalance;

        if (wallet <= 0 && cash <= 0)
        {
            pictureUI.sprite = brokeSprite;
            stateText.text = "State: Homeless";
            loseButton.gameObject.SetActive(true);

        }
        else if (wallet >= 100000)
        {
            pictureUI.sprite = richSprite;
            stateText.text = "State: Wealthy";
            winButton.gameObject.SetActive(true);
        }
        else if (wallet <= 5000 && cash <= 5000)
        {
            pictureUI.sprite = bitbrokeSprite;
            stateText.text = "State: Struggling";
            loseButton.gameObject.SetActive(false);
            winButton.gameObject.SetActive(false);
        }
        else if (wallet >= 50000)
        {
            pictureUI.sprite = bitrichSprite;
            stateText.text = "State: Very Comfortable";
            loseButton.gameObject.SetActive(false);
            winButton.gameObject.SetActive(false);
        }
        else
        {
            pictureUI.sprite = normalSprite;
            stateText.text = "State: Comfortable";
            winButton.gameObject.SetActive(false);
            loseButton.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        UpdatePicture();
    }
}
