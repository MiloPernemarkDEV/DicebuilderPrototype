using UnityEngine;

namespace HubBuilding
{
    public struct HubGridLocation
    {
        private const float TileWidth = 1.0f;
        private const float TileHeight = 0.5f; 
        private const int OriginX = 150;
        private const int OriginY = 150;
        
        
        public int x; 
        public int y;

        public HubGridLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector3 ToWorldCoords()
        {
            return ToWorldCoords(x, y);
        }

        public Vector3 FootprintCenter(int sizeX, int sizeY)
        {
            float centerX = x + (sizeX - 1) * 0.5f;
            float centerY = y + (sizeY - 1) * 0.5f;
            return ToWorldCoords(centerX, centerY);
        }

        public static Vector3 FootprintWorldScale(int sizeX, int sizeY, float height)
        {
            float worldX = (sizeX + sizeY) * (TileWidth * 0.5f);
            float worldZ = (sizeX + sizeY) * (TileHeight * 0.5f);
            return new Vector3(worldX, height, worldZ);
        }

        private static Vector3 ToWorldCoords(float gridX, float gridY)
        {
            float shiftedX = gridX - OriginX;
            float shiftedY = gridY - OriginY;
            float worldX = (shiftedX - shiftedY) * (TileWidth * 0.5f);
            float worldZ = (shiftedX + shiftedY) * (TileHeight * 0.5f);
            return new Vector3(worldX, 0f, worldZ);
        }

        public static HubGridLocation FromWorldCoords(Vector3 worldPos)
        {
            float cartX = worldPos.x / TileWidth;
            float cartZ = worldPos.z / TileHeight;

            int gridX = Mathf.FloorToInt(cartZ + cartX) + OriginX;
            int gridY = Mathf.FloorToInt(cartZ - cartX) + OriginY;

            return new HubGridLocation(gridX, gridY);
        }
    
        public Vector2 ToVector2() => new Vector2(x, y);

        public void ChangeLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
