namespace HubBuilding
{
    // This class answers two questions:
    // "Is this rectangle of tiles free?" 
    // and "Mark this rectangle of tiles as taken" 
    public class HubBuildingGrid
    {
        private readonly int width;
        private readonly int height;

        // Sets every tile to false 
        private readonly bool[,] occupied;

        public HubBuildingGrid(int width, int height)
        {
            this.width = width;
            this.height = height;
            occupied = new bool[width, height];
        }

        public bool IsValidGridPosition(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        public bool CanPlace(int startX, int startZ, int tileCountX, int tileCountZ)
        {
            for (int offsetX = 0; offsetX < tileCountX; offsetX++)
            {
                for (int offsetZ = 0; offsetZ < tileCountZ; offsetZ++)
                {
                    int tileX = startX + offsetX;
                    int tileZ = startZ + offsetZ;
                    if (!IsValidGridPosition(tileX, tileZ) || occupied[tileX, tileZ])
                        return false;
                }
            }
            return true;
        }

        public void SetOccupied(int startX, int startZ, int tileCountX, int tileCountZ)
        {
            for (int offsetX = 0; offsetX < tileCountX; offsetX++)
            {
                for (int offsetZ = 0; offsetZ < tileCountZ; offsetZ++)
                    occupied[startX + offsetX, startZ + offsetZ] = true;
            }
        }
    }
}

