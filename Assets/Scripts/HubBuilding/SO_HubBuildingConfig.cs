using UnityEngine;

namespace HubBuilding
{
    [CreateAssetMenu(fileName = "SO_HubBuildingConfig", menuName = "HubBuilding/SO_HubBuildingConfig", order = 0)]
    public class SO_HubBuildingConfig : ScriptableObject
    {
        [SerializeField] private float maxDistanceRaycast;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private Color validPlacementColor; 
        [SerializeField] private Color invalidPlacementColor;
        
        public float MaxDistanceRaycast => maxDistanceRaycast;
        public LayerMask GroundLayerMask => groundLayerMask;
        public Color ValidPlacementColor => validPlacementColor;
        public Color InvalidPlacementColor => invalidPlacementColor;
    }
}