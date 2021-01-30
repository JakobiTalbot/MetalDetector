using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraShader : MonoBehaviour
{
    public Material shaderMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        shaderMaterial.mainTexture = source;
        Graphics.Blit(source, destination, shaderMaterial);
    }
}
