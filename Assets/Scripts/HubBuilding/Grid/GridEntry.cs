using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace HubBuilding
{
    [Serializable]
    public class GridEntry
    {
        public string itemId;
        public GridPosition position;
        public Vector2Int itemTileSize; 

        public GridEntry(GridPosition position, string itemId, Vector2Int itemTileSize)
        {
            this.itemId = itemId;
            this.position = position;
            this.itemTileSize = itemTileSize;
        }
    }
}