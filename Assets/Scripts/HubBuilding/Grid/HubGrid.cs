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

        public void ResetActiveTiles(GridPosition pos)
        {
            if (!IsActiveGridPosition(pos))
            {
                return; 
            }
            grid[pos.x, pos.y].IsActive = false;
        }
        
        public bool CanPlace(GridPosition pos, Vector2Int itemTileCount)
        {
            for (int offsetX = 0; offsetX < itemTileCount.x; offsetX++)
            {
                for (int offsetZ = 0; offsetZ < itemTileCount.y; offsetZ++)
                {
                    int finalTileX = pos.x + offsetX;
                    int finalTileY = pos.y + offsetZ;
                    if (!IsValidGridPosition(finalTileX, finalTileY) || grid[finalTileX, finalTileY].IsActive)
                        return false;
                }
            }
            return true;
        }
        
        public bool SubmitEntry(GridEntry entry)
        {
            ActivateTiles(entry, entry.position, entry.itemTileSize, out var finalItemGridPos);
            
            GridSerializer.AddEntry(entry);
            return true; 
        }

        public SO_HubItem GetItemFromGrid(GridPosition pos)
        {
            if (IsActiveGridPosition(pos))
            {
                return ItemLookup.Instance.Get(grid[pos.x, pos.y].ItemId);
            }

            return null; 
        }

        /* public void LoadGrid(GridSerializer.GridEntryList gridList)
        {
            foreach (var entry in gridList.entries)
            {
                SubmitEntry(entry); 
            }
        }
        */

        private void ActivateTiles(GridEntry entry, GridPosition pos, Vector2Int itemTileSize, out GridPosition finalItemGridPos)
        {
            finalItemGridPos = new GridPosition();
            
            for (int offsetX = 0; offsetX < itemTileSize.x; offsetX++)
            {
                for (int offsetY = 0; offsetY < itemTileSize.y; offsetY++)
                {
                    int finalTileX = pos.x + offsetX;
                    int finalTileY = pos.y + offsetY;
                    
                    grid[finalTileX, finalTileY].IsActive = true;
                    finalItemGridPos.x = finalTileX;
                    finalItemGridPos.y = finalTileY; 
                }
            }
        }
        
        private bool IsValidGridPosition(int x, int y)
        {
            return x >= 0 && x < config.GridWidth && y >= 0 && y < config.GridHeight;
        }

        private bool IsActiveGridPosition(GridPosition pos)
        {
            return grid[pos.x , pos.y].IsActive;
        }
    }
}

