using System.Collections.Generic;

// A group of dice spawned at the same time (one per lane).
// Once the player picks one, the rest of the row fades out and can't be picked.
public class DiceRow
{
    private readonly List<LaneItem> items = new List<LaneItem>();

    public bool Claimed { get; private set; }

    public void Add(LaneItem item)
    {
        items.Add(item);
    }

    public void Claim(LaneItem picked)
    {
        Claimed = true;

        foreach (LaneItem item in items)
        {
            if (item != null && item != picked)
            {
                item.MarkSkipped();
            }
        }
    }
}