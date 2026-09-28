using System.Collections.Generic;

namespace HubBuilding
{  
    public struct GridEntry
    {
        public GridEntry(GridPosition position, string itemId)
        {
           ItemId = itemId;
           Position = position;
        }

        public string ItemId { get; }

        public GridPosition Position { get; set; }
    }
}