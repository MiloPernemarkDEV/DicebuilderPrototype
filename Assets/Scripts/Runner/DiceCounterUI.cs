using System;
using TMPro;
using UnityEngine;

public class DiceCounterUI : MonoBehaviour
{
    [Serializable]
    private class DiceCounterEntry
    {
        public DiceColor color;
        public TMP_Text countText;
    }

    [SerializeField] private PlayerLanes player;
    [SerializeField] private DiceCounterEntry[] entries;
    [SerializeField] private TMP_Text coinCountText;

    private void Update()
    {
        foreach (DiceCounterEntry entry in entries)
        {
            entry.countText.text = player.GetCount(entry.color).ToString();
        }

        if (coinCountText != null)
        {
            coinCountText.text = player.Coins.ToString();
        }
    }
}