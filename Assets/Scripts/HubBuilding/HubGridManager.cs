using UnityEngine;

namespace HubBuilding
{
    public class HubGridManager : MonoBehaviour
    {
        public static HubGridManager Instance { get; private set; }
        [SerializeField] private int gridWidth; 
        [SerializeField] private int gridHeight;
    
        public HubBuildingGrid Grid { get; private set; }
    
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Grid = new HubBuildingGrid(gridWidth, gridHeight);
        }
    }    
}
