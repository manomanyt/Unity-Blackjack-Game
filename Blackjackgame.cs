using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blackjackgame : MonoBehaviour
{
    private bool revealDealerCards = false;
    private List<string> playerCards = new List<string>();
    private List<string> dealerCards = new List<string>();
    private List<string> deck = new List<string>();

    public AudioManager audioManager;

    public GameObject cardPrefab;
    public Transform playerHandUI;
    public Transform dealerHandUI;

    public Button bet1000Button;
    public Button bet10000Button;
    private int currentBet = 0;

    public Button hitButton;
    public Button standButton;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI playerHandText;
    public TextMeshProUGUI dealerHandText;
    public TextMeshProUGUI cashText;

    public Dictionary<string, Sprite> cardSprites = new Dictionary<string, Sprite>();

    void Start()
    {
        LoadCardSprites();
        StartNewRound();
        UpdateCashUI();
    }

    void StartNewRound()
    {
        foreach (Transform child in dealerHandUI)
            Destroy(child.gameObject);
         foreach (Transform child in playerHandUI)
            Destroy(child.gameObject);

        playerCards.Clear();
        dealerCards.Clear();
        deck.Clear();
        CreateDeck();

        revealDealerCards = false;

        ShowHands();

        messageText.text = "";

        hitButton.onClick.RemoveAllListeners();
        standButton.onClick.RemoveAllListeners();

        bet1000Button.onClick.RemoveAllListeners();
        bet10000Button.onClick.RemoveAllListeners();

        bet1000Button.onClick.AddListener(() => PlaceBet(1000));
        bet10000Button.onClick.AddListener(() => PlaceBet(10000));

        hitButton.onClick.AddListener(PlayerHit);
        standButton.onClick.AddListener(PlayerStand);
    }

    public void PlaceBet(int amount)
    {
     if (MoneyManager.Instance.cashBalance >= amount)
     {
        MoneyManager.Instance.cashBalance -= amount;
        currentBet = amount;
        UpdateCashUI();

        hitButton.interactable = true;
        standButton.interactable = true;

        bet1000Button.interactable = false;
        bet10000Button.interactable = false;

        messageText.text = "Bet placed: $" + currentBet;

        playerCards.Add(DrawCard());
        dealerCards.Add(DrawCard());
        dealerCards.Add(DrawCard());

        DisplayHand(playerCards, playerHandUI);

        foreach (Transform child in dealerHandUI)
            Destroy(child.gameObject);

        for (int i = 0; i < dealerCards.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, dealerHandUI);

            if (i == 1)
            {
                newCard.GetComponent<Image>().sprite = cardSprites["CardBack"];
            }
            else
            {
                string spriteName = GetSpriteName(dealerCards[i]);
                newCard.GetComponent<Image>().sprite = cardSprites[spriteName];
            }
        }
        
        ShowHands();
        StartNewRound();
     }
     else
     {
        messageText.text = "Not enough cash to bet!";
     }
    }
    
    void UpdateCashUI()
    {
     if (cashText != null)
        cashText.text = "Cash: $" + MoneyManager.Instance.cashBalance;
    }

    void CreateDeck()
    {
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                deck.Add(rank + " of " + suit);

             if (rank == "10" || rank == "Jack" || rank == "Queen" || rank == "King" || rank == "Ace")
             {
                deck.Add(rank + " of " + suit);
             }
            }
        }
    }

    string DrawCard()
    {
        int randomIndex = Random.Range(0, deck.Count);
        string card = deck[randomIndex];
        deck.RemoveAt(randomIndex);

        if (audioManager != null)
        {
         audioManager.PlayCardSound();
        }
        
        return card;
    }

    void ShowHands()
    {
        int playerValue = CalculateHandValue(playerCards);
        int dealerValue = CalculateHandValue(dealerCards);

        playerHandText.text = "Player total: " + playerValue;
        dealerHandText.text = revealDealerCards ? "Dealer total: " + dealerValue : "Dealer total: ???";

        DisplayHand(playerCards, playerHandUI);

        foreach (Transform child in dealerHandUI)
            Destroy(child.gameObject);

        for (int i = 0; i < dealerCards.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, dealerHandUI);

            if (i == 1 && !revealDealerCards)
            {
                newCard.GetComponent<Image>().sprite = cardSprites["CardBack"];
            }
            else
            {
                string spriteName = GetSpriteName(dealerCards[i]);
                newCard.GetComponent<Image>().sprite = cardSprites[spriteName];
            }
        }
    }


    int CalculateHandValue(List<string> hand)
    {
        int total = 0;
        int aceCount = 0;

        foreach (string card in hand)
        {
            string rank = card.Split(' ')[0];

            if (rank == "Jack" || rank == "Queen" || rank == "King")
            {
                total += 10;
            }
            else if (rank == "Ace")
            {
                total += 11;
                aceCount++;
            }
            else
            {
                total += int.Parse(rank);
            }
        }

        while (total > 21 && aceCount > 0)
        {
            total -= 10;
            aceCount--;
        }

        return total;
    }

    void PlayerHit()
    {
        playerCards.Add(DrawCard());
        ShowHands();

        int playerTotal = CalculateHandValue(playerCards);
        if (playerTotal > 21)
        {
            Debug.Log("Player busts! Total: " + playerTotal);

            messageText.text = "Player busts!";

            hitButton.interactable = false;
            standButton.interactable = false;

            bet1000Button.interactable = true;
            bet10000Button.interactable = true;

            currentBet = 0;

        }
        else if (playerTotal == 21)
        {
            PlayerStand();
        }
    }

    void PlayerStand()
    {
        Debug.Log("Player stands.");

        hitButton.interactable = false;
        standButton.interactable = false;

        revealDealerCards = true;

        ShowHands();

        DealerTurn();
    }
    bool IsSoft17(List<string> dealerHand)
    {
     int total = CalculateHandValue(dealerHand);
     return total == 17 && dealerHand.Exists(card => card.Split(' ')[0] == "Ace");
    }
    void DealerTurn()
    {
        int dealerTotal = CalculateHandValue(dealerCards);
        int playerTotal = CalculateHandValue(playerCards);


        while ((dealerTotal < playerTotal && dealerTotal <= 21) || IsSoft17(dealerCards))
       {
        dealerCards.Add(DrawCard());
        dealerTotal = CalculateHandValue(dealerCards);
       }

       revealDealerCards = true;
       ShowHands();

       if (dealerTotal > 21 || playerTotal > dealerTotal)
       {
        messageText.text = "Player wins!";
        MoneyManager.Instance.cashBalance += currentBet * 2;
        audioManager.PlayWinSound();
       }
       else if (dealerTotal == playerTotal)
       {
        messageText.text = "Push! Bet returned.";
        MoneyManager.Instance.cashBalance += currentBet;
       } 
       else
       {
        messageText.text = "Dealer wins!";
       }

       hitButton.interactable = false;
       standButton.interactable = false;

       bet1000Button.interactable = true;
       bet10000Button.interactable = true;

       currentBet = 0;
       UpdateCashUI();
    }


    string GetSpriteName(string card)
    {
        string[] parts = card.Split(' ');
        string rank = parts[0];
        string suit = parts[2];

        switch (rank)
        {
            case "Ace": return suit + "1";
            case "Jack": return suit + "10.1";
            case "Queen": return suit + "10.2";
            case "King": return suit + "10.3";
            default: return suit + rank;
        }
    }

    void LoadCardSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Cards");
        foreach (Sprite s in sprites)
        {
            cardSprites[s.name] = s;
        }
    }

    void DisplayHand(List<string> hand, Transform handUI)
    {

        foreach (Transform child in handUI)
            Destroy(child.gameObject);

        foreach (string card in hand)
        {
            GameObject newCard = Instantiate(cardPrefab, handUI);
            string spriteName = GetSpriteName(card);
            newCard.GetComponent<Image>().sprite = cardSprites[spriteName];
        }
    }
}