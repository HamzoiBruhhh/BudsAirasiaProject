using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class RankingManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerEntry
    {
        public int position; // This will be calculated, not set in Inspector
        public string channelName;
        public long totalSubscribers; // Changed to long to handle very large numbers

        // Constructor for code-based creation (optional, but good practice)
        public PlayerEntry(string name, long subscribers)
        {
            channelName = name;
            totalSubscribers = subscribers;
        }

        // Default constructor for Inspector creation
        public PlayerEntry()
        {
            // Empty constructor needed for Unity to properly serialize in Inspector
        }
    }

    // This is the list you'll populate in the Inspector!
    [Header("Initial Channel Data")]
    public List<PlayerEntry> initialChannelData = new List<PlayerEntry>();

    // This list will be managed internally by the script
    private List<PlayerEntry> rankingEntriesInternal = new List<PlayerEntry>();


    [Header("UI Elements")]
    public GameObject rankingEntryPrefab;
    public Transform contentParent;


    void Start()
    {
        // First, clear any previous entries (especially useful if testing in Editor)
        rankingEntriesInternal.Clear();

        // Add all the entries from the Inspector-editable list
        foreach (PlayerEntry entry in initialChannelData)
        {
            // Ensure we're using the internal list for manipulation
            AddOrUpdateEntry(entry.channelName, entry.totalSubscribers);
        }

        // Now sort and display based on the entries added from the Inspector
        SortAndDisplayRankings();
    }

    /// <summary>
    /// Adds a new channel entry or updates an existing one's subscriber count.
    /// </summary>
    /// <param name="name">The name of the YouTube channel.</param>
    /// <param name="subscribers">The total number of subscribers.</param>
    public void AddOrUpdateEntry(string name, long subscribers) // Changed to long
    {
        PlayerEntry existingEntry = rankingEntriesInternal.FirstOrDefault(e => e.channelName == name);

        if (existingEntry != null)
        {
            existingEntry.totalSubscribers = subscribers;
        }
        else
        {
            // Use the constructor that takes arguments
            rankingEntriesInternal.Add(new PlayerEntry(name, subscribers));
        }

        // After adding/updating, it's often good to re-sort and display,
        // but in Start(), we do it once at the end.
        // If you call this method later (e.g., from a button), call SortAndDisplayRankings(); after.
    }

    /// <summary>
    /// Sorts the ranking entries by subscribers (descending) and updates their positions.
    /// Then, it refreshes the UI display.
    /// </summary>
    public void SortAndDisplayRankings()
    {
        // Sort the internal list
        rankingEntriesInternal = rankingEntriesInternal.OrderByDescending(e => e.totalSubscribers).ToList();

        // Update positions based on the new sort order
        for (int i = 0; i < rankingEntriesInternal.Count; i++)
        {
            rankingEntriesInternal[i].position = i + 1;
        }

        // Clear existing UI elements
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate new UI elements for each ranking entry
        foreach (PlayerEntry entry in rankingEntriesInternal)
        {
            GameObject rankingRow = Instantiate(rankingEntryPrefab, contentParent);

            // Get TextMeshProUGUI components from the children
            TextMeshProUGUI positionText = rankingRow.transform.Find("PositionText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI channelNameText = rankingRow.transform.Find("ChannelNameText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI subscribersText = rankingRow.transform.Find("SubscribersText")?.GetComponent<TextMeshProUGUI>();

            // Set the text for each component
            if (positionText != null) positionText.text = entry.position.ToString();
            if (channelNameText != null) channelNameText.text = entry.channelName;
            // Format subscribers with comma separators for readability
            if (subscribersText != null) subscribersText.text = entry.totalSubscribers.ToString("N0");
        }
    }

    /// <summary>
    /// Removes a channel entry by name and re-sorts/displays the rankings.
    /// </summary>
    /// <param name="channelName">The name of the channel to remove.</param>
    public void RemoveEntry(string channelName)
    {
        PlayerEntry entryToRemove = rankingEntriesInternal.FirstOrDefault(e => e.channelName == channelName);
        if (entryToRemove != null)
        {
            rankingEntriesInternal.Remove(entryToRemove);
            SortAndDisplayRankings(); // Re-sort and display after removal
        }
    }

    /// <summary>
    /// Gets the current rank (position) of a specific channel.
    /// </summary>
    /// <param name="channelName">The name of the channel to find the rank for.</param>
    /// <returns>The position of the channel (1-indexed), or -1 if not found.</returns>
    public int GetChannelRank(string channelName)
    {
        PlayerEntry entry = rankingEntriesInternal.FirstOrDefault(e => e.channelName == channelName);
        return entry != null ? entry.position : -1;
    }
}