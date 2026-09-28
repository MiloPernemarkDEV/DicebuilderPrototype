using EventChannels;
using UnityEngine;

namespace HubBuilding
{
    public sealed class HubManager : MonoBehaviour
    {
        public static HubManager Instance { get; private set; }
        
        [SerializeField] private SO_HubBuildingConfig config;
        [SerializeField] private SO_EventSO_HubItemPayload placeItemEvent;
        [SerializeField] private SO_EventSO_HubItemPayload movePlacedItemEvent;
    
        public HubGrid Grid { get; private set; }
        private PlacementController controller;
        
        public void OnEnable()
        {
            placeItemEvent.OnEventTriggered += controller.ActivatePlacement;
        }

        public void OnDisable()
        {
            placeItemEvent.OnEventTriggered -= controller.ActivatePlacement;
            // GridSerializer.TestSave();
        }
    
        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Grid = new HubGrid(config);
            controller = new PlacementController(config); 
        }
        
        public void Update()
        {
            controller.Run();
        }
        
        
    }    
}
