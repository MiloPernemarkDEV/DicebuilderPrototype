namespace HubBuilding
{
    public class HubBuildingGrid
    {
        private readonly int width;
        private readonly int height;

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

        public bool CanPlace(int x, int y, int sizeX, int sizeY)
        {
            for (int ix = 0; ix < sizeX; ix++)
            {
                for (int iy = 0; iy < sizeY; iy++)
                {
                    int gx = x + ix;
                    int gy = y + iy;
                    if (!IsValidGridPosition(gx, gy) || occupied[gx, gy])
                        return false;
                }
            }

            return true;
        }

        public void SetOccupied(int x, int y, int sizeX, int sizeY)
        {
            for (int ix = 0; ix < sizeX; ix++)
            {
                for (int iy = 0; iy < sizeY; iy++)
                    occupied[x + ix, y + iy] = true;
            }
        }
    }
}

