using System.Collections.Generic;

namespace HubBuilding
{  
    public struct GridEntry
    {
        private int itemId;
        private GridLocation location;
        public GridEntry(GridLocation location, int itemId)
        {
           this.itemId = itemId;
           this.location = location;
        }
    }
}