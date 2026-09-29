using UnityEngine;

namespace HubBuilding
{
    public sealed class PreviewMaterial
    {
        private static readonly int BaseColor = Shader.PropertyToID("_Color");

        private readonly Renderer[] renderers;
        private readonly MaterialPropertyBlock propertyBlock;

        public PreviewMaterial(GameObject preview, Material previewMaterialTemplate)
        {
            renderers = preview.GetComponentsInChildren<Renderer>();
            propertyBlock = new MaterialPropertyBlock();

            foreach (var renderer in renderers)
            {
                renderer.sharedMaterial = previewMaterialTemplate;
            }
        }

        public void SetColor(Color color)
        {
            propertyBlock.SetColor(BaseColor, color);

            foreach (var renderer in renderers)
            {
                renderer.SetPropertyBlock(propertyBlock);
            }
        }
        
        public void ClearPropertyBlock()
        {
            foreach (var renderer in renderers)
            {
                renderer.SetPropertyBlock(null);
            }
        }
    }
}