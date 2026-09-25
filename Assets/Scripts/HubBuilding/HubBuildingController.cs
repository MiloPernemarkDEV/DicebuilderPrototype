using UnityEngine;
using UnityEngine.InputSystem;

namespace HubBuilding
{
    public class HubBuildingController
    {
        private bool isActive;
        private readonly SO_HubBuildingConfig config; 

        private Vector2Int tileSize;
        private GameObject translucentPrefab;
        private GameObject normalPrefab;
        private bool isJustActivated;
        private Camera camera;

        private HubBuildingMaterial translucentMaterial; 

        public HubBuildingController(SO_HubBuildingConfig config)
        {
            this.config = config;
            camera = Camera.main;
        }

        public void Activate(SO_HubItem currentItem)
        {
            isActive = true;
            isJustActivated = true;
            
            tileSize = currentItem.TileSize;
            normalPrefab = currentItem.NormalPrefab;
            
            translucentPrefab = UnityEngine.Object.Instantiate(currentItem.TranslucentPrefab);
            translucentMaterial = new HubBuildingMaterial(translucentPrefab);
        }
        
        public void Tick()
        {
            if (!isActive)
            {
                return;
            }

            if (!camera || Mouse.current == null)
            {
                return;
            }
                
            HandlePlacement();
        }

        private bool WaitNextFrameOnActivation()
        {
            if (!isJustActivated)
                return false;

            isJustActivated = false;
            return true;
        }
        
        private void HandlePlacement()
        {
            if (WaitNextFrameOnActivation()) return;
            
            if (!InputUtils.TryGetPointerPosition(out var screenPos)) return;
            if (!camera) return;
            
            var ray = camera.ScreenPointToRay(screenPos);

            if (!Physics.Raycast(ray, out RaycastHit hitInfo, config.MaxDistanceRaycast, config.GroundLayerMask)) return;
            var location = HubGridLocation.FromWorldCoords(hitInfo.point);
            
            Vector3 footprintCenter = EnsurePivotPoint(location.FootprintCenter(tileSize.x, tileSize.y), hitInfo);
            translucentPrefab.transform.position = footprintCenter;

            bool canPlace = HubBuildingManager.Instance.Grid.CanPlace(location.x, location.y, tileSize.x, tileSize.y);

            translucentMaterial.SetColor(canPlace ? config.ValidPlacementColor : config.InvalidPlacementColor);

            if (InputUtils.WasPressedThisFrame() && canPlace)
            {
                HubBuildingManager.Instance.Grid.SetOccupied(location.x, location.y, tileSize.x, tileSize.y);
                var placed = UnityEngine.Object.Instantiate(normalPrefab, footprintCenter, Quaternion.identity);
                
                ApplyFootprintScale(placed);
                
                UnityEngine.Object.Destroy(translucentPrefab);
                translucentPrefab = null;
                translucentMaterial = null;
                normalPrefab = null;
                isActive = false; 
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