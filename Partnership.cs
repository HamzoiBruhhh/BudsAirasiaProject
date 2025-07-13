using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Partnership : MonoBehaviour
{
    [Header("Settings")]
    public string partnershipName;
    [TextArea] public string description;
    public int requiredSubs;
    public float moneyBoost = 1.5f;
    public float subBoost = 1.2f;

    [Header("UI References")]
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text requirementText;
    public TMP_Text boostText;
    public Image backgroundImage;

    [Header("Colors")]
    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.green;

    private bool isActive = false;
    private GameManager gameManager;

    public void Initialize(GameManager manager)
    {
        gameManager = manager;
        UpdateUI();

        nameText.text = partnershipName;
        descriptionText.text = description;
        boostText.text = $"Boosts: Money x{moneyBoost} | Subs x{subBoost}";
    }

    public void UpdateUI()
    {
        bool canUnlock = gameManager.totalSubs >= requiredSubs;

        requirementText.text = $"{gameManager.FormatNumber(requiredSubs)} subs";
        backgroundImage.color = canUnlock ? unlockedColor : lockedColor;

        // Automatically activate when requirements met
        if (canUnlock && !isActive)
        {
            ActivateBoost();
        }
    }

    private void ActivateBoost()
    {
        isActive = true;

        // Apply boosts to all characters
        CharacterStats[] allCharacters = FindObjectsOfType<CharacterStats>();
        foreach (CharacterStats character in allCharacters)
        {
            character.moneyGeneratePerInterval = Mathf.RoundToInt(character.moneyGeneratePerInterval * moneyBoost);
            character.subGeneratePerInterval = Mathf.RoundToInt(character.subGeneratePerInterval * subBoost);
        }

        Debug.Log($"Partnership '{partnershipName}' activated!");
    }
}