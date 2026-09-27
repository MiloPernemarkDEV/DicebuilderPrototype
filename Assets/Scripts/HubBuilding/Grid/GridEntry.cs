using System.Collections.Generic;

namespace HubBuilding
{
    public class GridEntry
    {
        public GridEntry(GridLocation location, string itemId)
        {
           this.itemId = itemId;
           this.location = location;
        }
        private string itemId;
        GridLocation location;
    }
}