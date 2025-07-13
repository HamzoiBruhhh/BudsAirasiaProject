using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class StreamerRankingUI : MonoBehaviour
{
    [SerializeField] private GameObject streamerEntryPrefab;
    [SerializeField] private VerticalLayoutGroup rankingLayoutGroup;
    [SerializeField] private float updateInterval = 1f;

    private List<StreamerStat> streamers = new List<StreamerStat>();
    private Dictionary<string, StreamerEntryUI> uiEntries = new Dictionary<string, StreamerEntryUI>();
    private float timer;

    private void Start()
    {
        // Initialize with some sample streamers
        AddStreamer("Ninja", 5000, 1.2f);
        AddStreamer("Pokimane", 4500, 1.0f);
        AddStreamer("Shroud", 4000, 0.8f);
        AddStreamer("TimTheTatman", 3500, 0.7f);
        AddStreamer("DrLupo", 3000, 0.6f);

        // Initial UI setup
        UpdateAllUI();
    }

    private void Update()
    {
        // Update sub counts passively
        foreach (var streamer in streamers)
        {
            streamer.UpdateSubs(Time.deltaTime);
        }

        // Update UI at intervals
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0f;
            UpdateRanking();
        }
    }

    public void AddStreamer(string name, int initialSubs, float growthRate)
    {
        var newStreamer = new StreamerStat(name, initialSubs, growthRate);
        streamers.Add(newStreamer);

        // Create UI entry
        var entryObj = Instantiate(streamerEntryPrefab, rankingLayoutGroup.transform);
        var entryUI = entryObj.GetComponent<StreamerEntryUI>();
        entryUI.Initialize(newStreamer);

        uiEntries.Add(name, entryUI);
    }

    private void UpdateRanking()
    {
        // Sort streamers by sub count (descending)
        var sortedStreamers = streamers.OrderByDescending(s => s.subCount).ToList();

        // Update positions in the layout group
        for (int i = 0; i < sortedStreamers.Count; i++)
        {
            var streamer = sortedStreamers[i];
            uiEntries[streamer.streamerName].UpdateData(i + 1, streamer.subCount);

            // Move to proper position in hierarchy (Vertical Layout Group will handle positioning)
            uiEntries[streamer.streamerName].transform.SetSiblingIndex(i);
        }
    }

    private void UpdateAllUI()
    {
        var sortedStreamers = streamers.OrderByDescending(s => s.subCount).ToList();

        for (int i = 0; i < sortedStreamers.Count; i++)
        {
            var streamer = sortedStreamers[i];
            uiEntries[streamer.streamerName].UpdateData(i + 1, streamer.subCount);
            uiEntries[streamer.streamerName].transform.SetSiblingIndex(i);
        }
    }
}