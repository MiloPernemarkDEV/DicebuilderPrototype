using UnityEngine;
using UnityEngine.Rendering;

namespace HubBuilding
{
    public sealed class HubBuildingMaterial
    {
        private static readonly int Surface = Shader.PropertyToID("_Surface");
        private static readonly int Blend = Shader.PropertyToID("_Blend");
        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        private readonly Renderer[] renderers;
        private readonly MaterialPropertyBlock propertyBlock;

        public HubBuildingMaterial(GameObject preview)
        {
            renderers = preview.GetComponentsInChildren<Renderer>();
            propertyBlock = new MaterialPropertyBlock();

            foreach (var renderer in renderers)
            {
                Material material = renderer.material;

                material.SetFloat(Surface, 1);
                material.SetFloat(Blend, 0);
                material.SetInt(SrcBlend, (int)BlendMode.SrcAlpha);
                material.SetInt(DstBlend, (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt(ZWrite, 0);

                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.renderQueue = (int)RenderQueue.Transparent;
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
    }
}