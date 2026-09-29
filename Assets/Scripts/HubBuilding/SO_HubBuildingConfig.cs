using UnityEngine;

namespace HubBuilding
{
    [CreateAssetMenu(fileName = "SO_HubBuildingConfig", menuName = "HubBuilding/SO_HubBuildingConfig", order = 0)]
    public class SO_HubBuildingConfig : ScriptableObject
    {
        [Header("Placement Settings")]
        [SerializeField] private float maxDistanceRaycast;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private Color validPlacementColor; 
        [SerializeField] private Color invalidPlacementColor;
        [SerializeField] private float movePlacedItemHoldTime;
        [SerializeField] private Material previewMaterial;
        
        [Header("Grid Settings")]
        [SerializeField] private int gridWidth; 
        [SerializeField] private int gridHeight;
        
        public float MaxDistanceRaycast => maxDistanceRaycast;
        public LayerMask GroundLayerMask => groundLayerMask;
        public Color ValidPlacementColor => validPlacementColor;
        public Color InvalidPlacementColor => invalidPlacementColor;
        public int GridWidth => gridWidth;
        public int GridHeight => gridHeight;
        public float  MovePlacedItemHoldTime => movePlacedItemHoldTime;
        public Material PreviewMaterial => previewMaterial;
        // public Material  GridMaterial => gridMaterial;
    }
}