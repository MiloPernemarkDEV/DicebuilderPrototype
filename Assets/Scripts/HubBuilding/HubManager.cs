using EventChannels;
using UnityEngine;

namespace HubBuilding
{
    public sealed class HubManager : MonoBehaviour
    {
        public static HubManager Instance { get; private set; }
        
        [SerializeField] private SO_HubBuildingConfig config;
        [SerializeField] private SO_EventSO_HubItemPayload itemPlacementEvent;
    
        public HubGrid Grid { get; private set; }
        private PlacementController controller;
        
        public void OnEnable()
        {
            itemPlacementEvent.OnEventTriggered +=  controller.Activate;
        }

        public void OnDisable()
        {
            itemPlacementEvent.OnEventTriggered -= controller.Activate;
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
