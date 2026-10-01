using System;
using EventChannels;
using UnityEngine;

namespace HubBuilding
{
    public class MovePlacedItemTrigger : MonoBehaviour
    {
        [SerializeField] private SO_HubBuildingConfig config;
        [SerializeField] private SO_EventHubItemMovedPayload movePlacedItemEvent;

        private Camera camera;

        public void Start()
        {
            camera = Camera.main;
        }
        
        public void Update()
        {
            if (InputUtils.HasBeenPressedFor(config.MovePlacedItemHoldTime))
            {
                if (!InputUtils.TryGetPointerPosition(out var screenPos)) return;
                var ray = camera.ScreenPointToRay(screenPos);
                if (!Physics.Raycast(ray, out var hitInfo, config.MaxDistanceRaycast, config.GroundLayerMask)) return;

                var oldPosition = GridPosition.FromWorldCoords(hitInfo.point);
                
                movePlacedItemEvent.TriggerEvent(
                    new HubItemMovedData(
                        HubManager.Instance.Grid.GetItemFromGrid(oldPosition), 
                    oldPosition)
                );
            }
        }
    }
}