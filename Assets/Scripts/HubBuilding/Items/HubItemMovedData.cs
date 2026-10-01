namespace HubBuilding
{
    public class HubItemMovedData
    {
        public SO_HubItem Item;
        public GridPosition OldPosition;

        public HubItemMovedData(SO_HubItem item,  GridPosition oldPosition)
        {
            Item = item;
            OldPosition = oldPosition;
        }
    }
}