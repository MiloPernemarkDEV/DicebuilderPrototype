using UnityEngine;

namespace HubBuilding
{
    public class HubBuildingMaterial
    {
        public static Material GetURPTranslucentMaterial(Color color)
        {
            Shader urpLitShader = Shader.Find("Universal Render Pipeline/Lit");

            Material mat = new Material(urpLitShader);
        
            mat.SetFloat("_Surface", 1); // Transparent
            mat.SetFloat("_Blend", 0);   // Alpha blend
        
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
        
            mat.SetColor("_BaseColor", color);
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            return mat;
        }
        
        public static void SetTranslucentMaterial(GameObject gameObject, Color color)
        {
            if (gameObject == null) return;

            foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                renderer.material = GetURPTranslucentMaterial(color);
            }
        }
    }
}