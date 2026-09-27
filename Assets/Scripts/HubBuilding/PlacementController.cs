using UnityEngine;
using UnityEngine.InputSystem;

namespace HubBuilding
{
    public class PlacementController
    {
        private readonly SO_HubBuildingConfig config; 
        private readonly Camera camera;

        private bool isActive;
        private bool wasJustActivated;
        
        private Vector2Int itemTileCount;
        private GameObject previewPrefab;
        private GameObject normalPrefab;
        private GameObject previewObject;
        private PreviewMaterial previewMat;
        private SO_HubItem currentItem;

        public PlacementController(SO_HubBuildingConfig config)
        {
            this.config = config;
            camera = Camera.main;
        }

        public void Activate(SO_HubItem currentItem)
        {
            isActive = true;
            wasJustActivated = true;
            this.currentItem = currentItem;
            
            itemTileCount = currentItem.TileCount;
            normalPrefab = currentItem.NormalPrefab;
            previewObject = currentItem.TranslucentPrefab;
        }
        
        public void Run()
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
        
        private void HandlePlacement()
        {
            if (WaitNextFrameOnActivation()) return;
            if (!InputUtils.TryGetPointerPosition(out var screenPos)) return;
            if (!camera) return;
            
            Ray ray = camera.ScreenPointToRay(screenPos);

            if (!Physics.Raycast(ray, out RaycastHit hitInfo, config.MaxDistanceRaycast, config.GroundLayerMask)) return;
            GridLocation location = GridLocation.FromWorldCoords(hitInfo.point);
            Vector3 footprintCenter = location.FootprintCenter(itemTileCount.x, itemTileCount.y);

            if (!previewPrefab)
            {
                previewPrefab = UnityEngine.Object.Instantiate(previewObject, footprintCenter, Quaternion.identity);
                previewMat = new PreviewMaterial(previewPrefab);
            }

            footprintCenter = AdjustPivot(footprintCenter, hitInfo);
            previewPrefab.transform.position = footprintCenter;

            var canPlace = HubManager.Instance.Grid.CanPlace(location, itemTileCount);
            previewMat.SetColor(canPlace ? config.ValidPlacementColor : config.InvalidPlacementColor);

            if (!InputUtils.WasPressedThisFrame() || !canPlace) return;

            SubmitAndInstantiate(location, footprintCenter);
            ResetState();
            Object.Destroy(previewPrefab);
        }
        
        private Vector3 AdjustPivot(Vector3 footprintCenter, RaycastHit hitInfo)
        {
            if (previewPrefab.TryGetComponent<Renderer>(out var renderer))
            {
                float pivotToBottomOffset = renderer.localBounds.center.y - renderer.localBounds.extents.y;
                footprintCenter.y = hitInfo.point.y - pivotToBottomOffset;
            }
            else
            {
                footprintCenter.y = hitInfo.point.y + (previewPrefab.transform.localScale.y * 0.5f);
            }

            return footprintCenter; 
        }
        
        private bool WaitNextFrameOnActivation()
        {
            if (!wasJustActivated)
                return false;

            wasJustActivated = false;
            return true;
        }

        private void ResetState()
        {
            previewPrefab = null;
            previewMat = null;
            normalPrefab = null;
            isActive = false;
            currentItem = null;
        }

        private void SubmitAndInstantiate(GridLocation location, Vector3 footprintCenter)
        {
            if (HubManager.Instance.Grid.SubmitEntry(new GridEntry(location, currentItem.ID), location, itemTileCount))
            { 
                Object.Instantiate(normalPrefab, footprintCenter, Quaternion.identity);
            }
            Debug.Log($"Failed to submit {currentItem.ID} at Location: {location} with footprint: {footprintCenter}");
        }
    }
}