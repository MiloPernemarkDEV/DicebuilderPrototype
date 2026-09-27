using System;
using UnityEngine;

namespace HubBuilding
{
    public class HubBuildingManager : MonoBehaviour
    {
        public static HubBuildingManager Instance { get; private set; }
        
        [SerializeField] private SO_HubBuildingConfig config;
    
        public HubBuildingGrid Grid { get; private set; }
        private HubBuildingController controller;
        
        // For testing!
        [SerializeField] private SO_HubItem debugItem;
        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 200, 40), "Start Placement"))
            {
                OnStartPlacement(debugItem);
            }
        }
        //
    
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Grid = new HubBuildingGrid(config.GridWidth, config.GridHeight);
            controller = new HubBuildingController(config); 
        }

        public void Update()
        {
            controller.Tick();
        }

        public void OnStartPlacement(SO_HubItem item)
        {
            controller.Activate(item);
        }
    }    
}
