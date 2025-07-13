using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StreamerEntryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text positionText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text subCountText;
    [SerializeField] private Image backgroundImage;

    private StreamerStat streamerStat;

    public void Initialize(StreamerStat stat)
    {
        streamerStat = stat;
        nameText.text = stat.streamerName;
    }

    public void UpdateData(int position, int subCount)
    {
        positionText.text = position.ToString();
        subCountText.text = subCount.ToString("N0");

        // Optional: Highlight top streamers
        if (position <= 3)
        {
            backgroundImage.color = Color.green;
        }
        else
        {
            backgroundImage.color = Color.white;
        }
    }
}