using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace HubBuilding
{
    public class HubBuildingController
    {
        private bool isActive;
        private SO_HubBuildingConfig config; 

        private Vector2Int tileSize;
        private GameObject translucentPrefab;
        private GameObject normalPrefab;

        public HubBuildingController(SO_HubBuildingConfig config)
        {
            this.config = config;
        }

        public void Activate(SO_HubItem currentItem)
        {
            isActive = true;
            tileSize = currentItem.TileSize;
            normalPrefab = currentItem.NormalPrefab;
            translucentPrefab = currentItem.TranslucentPrefab;
            
            translucentPrefab = Object.Instantiate(currentItem.TranslucentPrefab);
        }
        
        public void Tick()
        {
            if (!isActive)
            {
                return;
            }

            if (!Camera.main || Mouse.current == null)
            {
                return;
            }
                
            HandlePlacement();
        }

        private void HandlePlacement()
        {
            if (!InputUtils.TryGetPointerPosition(out var screenPos)) return;
            if (Camera.main == null) return;
            
            var ray = Camera.main.ScreenPointToRay(screenPos);

            if (!Physics.Raycast(ray, out RaycastHit hitInfo, config.MaxDistanceRaycast, config.GroundLayerMask)) return;
            var location = HubGridLocation.FromWorldCoords(hitInfo.point);
            
            Vector3 footprintCenter = EnsurePivotPoint(location.FootprintCenter(tileSize.x, tileSize.y), hitInfo);
            translucentPrefab.transform.position = footprintCenter;

            bool canPlace = HubGridManager.Instance.Grid.CanPlace(location.x, location.y, tileSize.x, tileSize.y);
            
            HubBuildingMaterial.SetTranslucentMaterial(translucentPrefab, 
                canPlace ? config.ValidPlacementColor : config.InvalidPlacementColor
            );

            if (InputUtils.WasPressedThisFrame() && canPlace)
            {
                HubGridManager.Instance.Grid.SetOccupied(location.x, location.y, tileSize.x, tileSize.y);
                var placed = Object.Instantiate(normalPrefab, footprintCenter, Quaternion.identity);
                
                ApplyFootprintScale(placed);
                
                Object.Destroy(translucentPrefab);
                translucentPrefab = null;
                normalPrefab = null;
            }
        }

        private void ApplyFootprintScale(GameObject item)
        {
            float height = item.transform.localScale.y;
            item.transform.localScale = HubGridLocation.FootprintWorldScale(
                tileSize.x, tileSize.y, height
            ); 
        }

        private Vector3 EnsurePivotPoint(Vector3 footprintCenter, RaycastHit hitInfo)
        {
            if (translucentPrefab.TryGetComponent<Renderer>(out var renderer))
            {
                float pivotToBottomOffset = renderer.localBounds.center.y - renderer.localBounds.extents.y;
                footprintCenter.y = hitInfo.point.y - pivotToBottomOffset;
            }
            else
            {
                footprintCenter.y = hitInfo.point.y + (translucentPrefab.transform.localScale.y * 0.5f);
            }

            return footprintCenter; 
        }
    }
}