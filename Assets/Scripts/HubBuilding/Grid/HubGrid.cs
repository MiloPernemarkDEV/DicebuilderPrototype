using UnityEngine;

namespace HubBuilding
{
    public class HubGrid
    {
        private readonly Tile[,] tiles;
        private readonly SO_HubBuildingConfig config;
        
        public HubGrid(SO_HubBuildingConfig config)
        {
            this.config = config;
            tiles = new Tile[config.GridWidth, config.GridHeight];
        }
        
        public bool CanPlace(GridLocation loc, Vector2Int itemTileCount)
        {
            for (int offsetX = 0; offsetX < itemTileCount.x; offsetX++)
            {
                for (int offsetZ = 0; offsetZ < itemTileCount.y; offsetZ++)
                {
                    int finalTileX = loc.x + offsetX;
                    int finalTileY = loc.y + offsetZ;
                    if (!IsValidGridPosition(finalTileX, finalTileY) || tiles[finalTileX, finalTileY].isActivated)
                        return false;
                }
            }
            return true;
        }

        public bool SubmitEntry(GridEntry entry, GridLocation location, Vector2Int itemTileCount)
        {
            ActivateTiles(location, itemTileCount);

            return true; 
        }

        private void ActivateTiles(GridLocation loc, Vector2Int itemTileSize)
        {
            for (int offsetX = 0; offsetX < itemTileSize.x; offsetX++)
            {
                for (int offsetY = 0; offsetY < itemTileSize.y; offsetY++)
                    tiles[loc.x + offsetX, loc.y + offsetY].isActivated = true;
            }
        }
        
        private bool IsValidGridPosition(int x, int y)
        {
            return x >= 0 && x < config.GridWidth && y >= 0 && y < config.GridHeight;
        }

    }
}

