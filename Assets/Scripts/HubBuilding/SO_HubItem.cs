using System.Collections.Generic;
using UnityEngine;

namespace HubBuilding
{
    [CreateAssetMenu(fileName = "SO_HubItem", menuName = "HubBuilding/SO_HubItem", order = 0)]
    public class SO_HubItem : ScriptableObject
    {
        [SerializeField] private string hubItemId; 
        [SerializeField] private GameObject normalPrefab;
        [SerializeField] private GameObject translucentPrefab; 
        [SerializeField] private Vector2Int tileSize; 
        [SerializeField] private bool hasSlot; 
        [SerializeField] private List<Transform> slotTransforms;
        
        public string ID => hubItemId;
        public GameObject NormalPrefab => normalPrefab;
        public GameObject TranslucentPrefab => translucentPrefab;
        public Vector2Int TileSize => tileSize;
        public bool HasSlot => hasSlot;
        public List<Transform> SlotTransforms => slotTransforms;
    } 
}
