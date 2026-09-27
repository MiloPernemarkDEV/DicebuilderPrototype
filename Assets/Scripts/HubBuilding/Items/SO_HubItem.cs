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
        
        public int ID => Animator.StringToHash(hubItemId);
        public GameObject NormalPrefab => normalPrefab;
        public GameObject TranslucentPrefab => translucentPrefab;
        public Vector2Int TileCount => tileCount;
        public bool HasSlots => hasSlots;
        
        // [Header("Dice")]
        
    } 
}
