using UnityEngine;
using UnityEngine.InputSystem;

namespace HubBuilding
{
    public class PlacementController
    {
        private readonly SO_HubBuildingConfig config; 
        private readonly Camera camera;

        private bool placementIsActive;
        private bool wasJustActivated;
        private bool movePlacedItemIsActive;
        
        private Vector2Int itemTileCount;
        private GameObject previewPrefab;
        private GameObject normalPrefab;
        private GameObject previewObject;
        private PreviewMaterial previewMat;
        private SO_HubItem currentItem;
        private bool hasResetOldTiles;
        private GridPosition oldPos;


        public PlacementController(SO_HubBuildingConfig config)
        {
            this.config = config;
            camera = Camera.main;
        }

        public void ActivatePlacement(SO_HubItem currentItem)
        {
            placementIsActive = true;
            wasJustActivated = true;
            this.currentItem = currentItem;
            
            itemTileCount = currentItem.TileCount;
            normalPrefab = currentItem.NormalPrefab;
            previewObject = currentItem.TranslucentPrefab;
        }
        
        public void ActivateMovePlacedItem(SO_HubItem currentItem, GridPosition oldPos)
        {
            movePlacedItemIsActive = true;
            wasJustActivated = true;
            this.currentItem = currentItem;
            
            itemTileCount = currentItem.TileCount;
            normalPrefab = currentItem.NormalPrefab;
            previewObject = currentItem.TranslucentPrefab;
            this.oldPos = oldPos;
        }
        
        public void Run()
        {
            if (!placementIsActive)
            {
                return;
            }

            if (!camera || Mouse.current == null)
            {
                return;
            }

            if (movePlacedItemIsActive)
            {
                MovePlacedItem(oldPos);
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
            var position = GridPosition.FromWorldCoords(hitInfo.point);
            var footprintCenter = position.FootprintCenter(itemTileCount.x, itemTileCount.y);

            if (!previewPrefab)
            {
                previewPrefab = UnityEngine.Object.Instantiate(previewObject, footprintCenter, Quaternion.identity);
                previewMat = new PreviewMaterial(previewPrefab);
            }

            footprintCenter = AdjustPivot(footprintCenter, hitInfo);
            previewPrefab.transform.position = footprintCenter;

            var canPlace = HubManager.Instance.Grid.CanPlace(position, itemTileCount);
            previewMat.SetColor(canPlace ? config.ValidPlacementColor : config.InvalidPlacementColor);

            if (!InputUtils.WasPressedThisFrame() || !canPlace) return;

            SubmitAndInstantiate(new GridEntry(
                position, 
                currentItem.ID, 
                currentItem.TileCount), 
                footprintCenter
            );
            
            ResetState();
            Object.Destroy(previewPrefab);
        }

        private void MovePlacedItem(GridPosition oldPos)
        {
            if (!hasResetOldTiles)
            {
                HubManager.Instance.Grid.ResetActiveTiles(oldPos);
                hasResetOldTiles = true;
            }
            HandlePlacement();
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
            placementIsActive = false;
            currentItem = null;
        }

        private void SubmitAndInstantiate(GridEntry entry, Vector3 footprintCenter)
        {
            if (HubManager.Instance.Grid.SubmitEntry(entry)) 
            { 
                Object.Instantiate(normalPrefab, footprintCenter, Quaternion.identity);
            }
        }
    }
}