using UnityEngine;
using UnityEngine.UI;

public class SubscriptionManager : MonoBehaviour
{
    public float totalSubscriptions = 0;
    public float totalMoney = 0;

    [Header("UI References")]
    public Text subCountText;
    public Text moneyCountText;

    [Header("Update Interval")]
    public float updateInterval = 1.0f; // Update every second
    private float timer = 0f;

    private CharacterBehavior[] characters;

    private void Start()
    {
        characters = FindObjectsOfType<CharacterBehavior>();
        UpdateUI();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= updateInterval)
        {
            timer = 0f;
            ProcessAllCharacters();
            UpdateUI();
        }
    }

    private void ProcessAllCharacters()
    {
        foreach (CharacterBehavior character in characters)
        {
            character.ProcessInterval();
        }
    }

    public void AddSubscriptions(float amount)
    {
        totalSubscriptions += amount;
        if (totalSubscriptions < 0) totalSubscriptions = 0;
    }

    public void AddMoney(float amount)
    {
        totalMoney += amount;
        if (totalMoney < 0) totalMoney = 0;
    }

    public void DeductMoney(float amount)
    {
        totalMoney -= amount;
        if (totalMoney < 0) totalMoney = 0;
    }

    private void UpdateUI()
    {
        if (subCountText != null)
            subCountText.text = "Subs: " + totalSubscriptions.ToString("F0");

        if (moneyCountText != null)
            moneyCountText.text = "$: " + totalMoney.ToString("F0");
    }
}