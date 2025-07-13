using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemName;
    public int cost = 100;
    public int subGenerationIncrease = 1;
    public int maxPurchases = 5;
    public float costMultiplier = 1.5f;
    public CharacterStats targetCharacter; // Reference to specific character

    [Header("UI References")]
    public Button buyButton;
    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text effectText;
    public TMP_Text purchasesText;

    private GameManager gameManager;
    private int currentPurchases = 0;

    public void Initialize(GameManager manager)
    {
        gameManager = manager;
        UpdateUI();
        buyButton.onClick.AddListener(OnBuyButtonClick);
        nameText.text = itemName;
    }

    private void UpdateUI()
    {
        costText.text = "RM " + gameManager.FormatNumber(GetCurrentCost());
        effectText.text = "+" + subGenerationIncrease + " subs/sec for " + targetCharacter.name;
        purchasesText.text = currentPurchases + "/" + maxPurchases;
        buyButton.interactable = currentPurchases < maxPurchases && gameManager.CanAfford(GetCurrentCost());
    }

    private void OnBuyButtonClick()
    {
        int currentCost = GetCurrentCost();

        if (gameManager.CanAfford(currentCost) && currentPurchases < maxPurchases)
        {
            gameManager.AddMoney(-currentCost);
            currentPurchases++;

            // Only affect the target character
            if (targetCharacter != null)
            {
                targetCharacter.IncreaseSubGeneration(subGenerationIncrease);
            }

            UpdateUI();
        }
    }

    private int GetCurrentCost()
    {
        return Mathf.RoundToInt(cost * Mathf.Pow(costMultiplier, currentPurchases));
    }

    private void Update()
    {
        if (currentPurchases < maxPurchases)
        {
            buyButton.interactable = gameManager.CanAfford(GetCurrentCost());
        }
    }
}