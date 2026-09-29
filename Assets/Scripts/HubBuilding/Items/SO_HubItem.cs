using UnityEngine;

namespace HubBuilding
{
    [CreateAssetMenu(fileName = "SO_HubItem", menuName = "HubBuilding/SO_HubItem", order = 0)]
    public class SO_HubItem : ScriptableObject
    {
        [Header("HubBuilding")]
        [SerializeField] private string hubItemId; 
        [SerializeField] private GameObject normalPrefab;
        [SerializeField] private GameObject translucentPrefab; 
        [SerializeField] private Vector2Int tileCount; 
        [SerializeField] private bool hasSlots;
        [SerializeField] private Vector3 rotationOffset;
        [SerializeField] private Material itemMaterial;

        public string ID => hubItemId;
        public GameObject NormalPrefab => normalPrefab;
        public GameObject TranslucentPrefab => translucentPrefab;
        public Vector2Int TileCount => tileCount;
        public bool HasSlots => hasSlots;
        public Vector3 RotationOffset => rotationOffset;
        public Material ItemMaterial => itemMaterial;
        
        // [Header("Dice")]
        
    } 
}
