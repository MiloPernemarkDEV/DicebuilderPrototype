using System.Collections.Generic;

namespace HubBuilding
{  
    public struct GridEntry
    {
        private string itemId;
        private GridLocation location;
        public GridEntry(GridLocation location, string itemId)
        {
           this.itemId = itemId;
           this.location = location;
        }
    }
}