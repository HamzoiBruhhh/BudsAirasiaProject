using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float updateInterval = 1f;
    public int totalMoney = 100;
    public int totalSubs = 0;

    [Header("UI References")]
    public List<TMP_Text> moneyTextDisplays = new List<TMP_Text>();
    public List<TMP_Text> subCountTextDisplays = new List<TMP_Text>();
    public Transform shopItemsParent;
    public List<Partnership> partnerships = new List<Partnership>();

    [Header("Formatting Settings")]
    public bool useAbbreviatedNumbers = true;
    public int decimalPlaces = 1;

    private CharacterStats[] allCharacters;
    private List<ShopItem> shopItems = new List<ShopItem>();

    private void Start()
    {
        allCharacters = FindObjectsOfType<CharacterStats>();
        InitializeShopItems();
        InitializePartnerships();
        StartCoroutine(UpdateGameLoop());
        UpdateAllMoneyDisplays();
        UpdateAllSubCountDisplays();
    }

    private void InitializeShopItems()
    {
        ShopItem[] items = shopItemsParent.GetComponentsInChildren<ShopItem>(true);
        shopItems.AddRange(items);

        foreach (ShopItem item in shopItems)
        {
            item.Initialize(this);
        }
    }

    private void InitializePartnerships()
    {
        foreach (Partnership partnership in partnerships)
        {
            partnership.Initialize(this);
        }
    }

    private IEnumerator UpdateGameLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);

            foreach (CharacterStats character in allCharacters)
            {
                character.ProcessInterval();
            }

            UpdateAllMoneyDisplays();
            UpdateAllSubCountDisplays();
        }
    }

    public void UpdateTotalSubs()
    {
        totalSubs = 0;
        foreach (CharacterStats character in allCharacters)
        {
            totalSubs += character.GetCurrentSubCount();
        }
        UpdateAllSubCountDisplays();

        // Check partnerships when subs update
        foreach (Partnership partnership in partnerships)
        {
            partnership.UpdateUI();
        }
    }

    public void AddMoney(int amount)
    {
        totalMoney += amount;
        totalMoney = Mathf.Max(0, totalMoney);
        UpdateAllMoneyDisplays();
    }

    public bool CanAfford(int amount)
    {
        return totalMoney >= amount;
    }

    private void UpdateAllMoneyDisplays()
    {
        string formattedMoney = FormatNumber(totalMoney);

        foreach (TMP_Text moneyText in moneyTextDisplays)
        {
            if (moneyText != null)
            {
                moneyText.text = "Money: " + formattedMoney;
            }
        }
    }

    private void UpdateAllSubCountDisplays()
    {
        string formattedSubs = FormatNumber(totalSubs);

        foreach (TMP_Text subText in subCountTextDisplays)
        {
            if (subText != null)
            {
                subText.text = "Subs: " + formattedSubs;
            }
        }
    }

    public string FormatNumber(int number)
    {
        if (!useAbbreviatedNumbers) return number.ToString("N0");

        string suffix = "";
        float dividedNumber = number;

        if (number >= 1000000000)
        {
            suffix = "B";
            dividedNumber = number / 1000000000f;
        }
        else if (number >= 1000000)
        {
            suffix = "M";
            dividedNumber = number / 1000000f;
        }
        else if (number >= 1000)
        {
            suffix = "K";
            dividedNumber = number / 1000f;
        }

        if (!string.IsNullOrEmpty(suffix))
        {
            return dividedNumber.ToString("N" + decimalPlaces) + suffix;
        }

        return number.ToString("N0");
    }
}