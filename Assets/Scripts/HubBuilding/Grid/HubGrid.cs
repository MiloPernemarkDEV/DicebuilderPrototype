using UnityEngine;

namespace HubBuilding
{
    public class HubGrid
    {
        private readonly Tile[,] grid;
        private readonly SO_HubBuildingConfig config;
        private Vector2Int gridSize;
        
        public HubGrid(SO_HubBuildingConfig config)
        {
            this.config = config;
            grid = new Tile[config.GridWidth, config.GridHeight];
            gridSize.x = config.GridWidth;
            gridSize.y = config.GridHeight;
        }

        public void ResetTiles(GridPosition pos)
        {
            if (!IsActiveGridPosition(pos))
            {
                return; 
            }
            grid[pos.X, pos.Y].isActivated = false;
        }
        
        public bool CanPlace(GridPosition pos, Vector2Int itemTileCount)
        {
            for (int offsetX = 0; offsetX < itemTileCount.x; offsetX++)
            {
                for (int offsetZ = 0; offsetZ < itemTileCount.y; offsetZ++)
                {
                    int finalTileX = pos.X + offsetX;
                    int finalTileY = pos.Y + offsetZ;
                    if (!IsValidGridPosition(finalTileX, finalTileY) || grid[finalTileX, finalTileY].isActivated)
                        return false;
                }
            }
            return true;
        }
        
        // Entry has the id, activate outs the final tile positions then send to buffer ready to be serialized 
        // When game loads 
        public bool SubmitEntry(GridEntry entry, GridPosition pos, Vector2Int itemTileCount)
        {
            ActivateTiles(entry, pos, itemTileCount, out var finalEntryTiles);
            
            // Store entry somewhere for serialization
            return true; 
        }

        private void ActivateTiles(GridEntry entry, GridPosition pos, Vector2Int itemTileSize, out Vector2Int entryFinalTiles)
        {
            entryFinalTiles = Vector2Int.zero;
            
            for (int offsetX = 0; offsetX < itemTileSize.x; offsetX++)
            {
                for (int offsetY = 0; offsetY < itemTileSize.y; offsetY++)
                {
                    int finalTileX = pos.X + offsetX;
                    int finalTileY = pos.Y + offsetY;
                    
                    grid[finalTileX, finalTileY].isActivated = true;
                    entryFinalTiles.x = finalTileX;
                    entryFinalTiles.y = finalTileY; 
                }
            }
        }
        
        private bool IsValidGridPosition(int x, int y)
        {
            return x >= 0 && x < config.GridWidth && y >= 0 && y < config.GridHeight;
        }

        private bool IsActiveGridPosition(GridPosition pos)
        {
            return grid[pos.X , pos.Y].isActivated;
        }
    }
}

