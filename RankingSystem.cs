using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingSystem : MonoBehaviour
{
    [System.Serializable]
    public class Character
    {
        public string characterName;
        public int subCount;
        [HideInInspector] public GameObject uiInstance;
    }

    public List<Character> characters = new List<Character>();
    public GameObject rankEntryPrefab;
    public Transform rankingPanel;

    void Start()
    {
        // Initial setup
        UpdateRankingUI();
    }

    void Update()
    {
        // For testing - in real game you'd update based on actual game events
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Randomly change some sub counts
            foreach (var character in characters)
            {
                if (Random.Range(0, 3) == 0)
                {
                    character.subCount += Random.Range(-100, 500);
                }
            }
            UpdateRankingUI();
        }
    }

    public void UpdateRankingUI()
    {
        // Sort characters by sub count (descending)
        characters.Sort((a, b) => b.subCount.CompareTo(a.subCount));

        // Clear existing UI (except prefab)
        foreach (Transform child in rankingPanel)
        {
            if (child.gameObject != rankEntryPrefab)
                Destroy(child.gameObject);
        }

        // Create new UI entries
        for (int i = 0; i < characters.Count; i++)
        {
            var character = characters[i];
            GameObject entry = Instantiate(rankEntryPrefab, rankingPanel);
            entry.SetActive(true);

            // Set rank number (i+1 because 0-based index)
            entry.transform.Find("RankText").GetComponent<Text>().text = (i + 1).ToString();

            // Set character name
            entry.transform.Find("NameText").GetComponent<Text>().text = character.characterName;

            // Set sub count
            entry.transform.Find("SubText").GetComponent<Text>().text = character.subCount.ToString();

            character.uiInstance = entry;

            // Optional: Style top ranks differently
            if (i == 0) entry.GetComponent<Image>().color = Color.yellow; // Gold for 1st
            else if (i == 1) entry.GetComponent<Image>().color = Color.gray; // Silver for 2nd
            else if (i == 2) entry.GetComponent<Image>().color = new Color(0.8f, 0.5f, 0.2f); // Bronze for 3rd
        }
    }

    // Call this when a character's sub count changes
    public void UpdateCharacterSubs(string characterName, int newSubCount)
    {
        var character = characters.Find(c => c.characterName == characterName);
        if (character != null)
        {
            character.subCount = newSubCount;
            UpdateRankingUI();
        }
    }
}