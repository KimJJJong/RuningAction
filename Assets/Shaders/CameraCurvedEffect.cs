using UnityEngine;

[ExecuteInEditMode]
public class CameraCurvedEffect : MonoBehaviour
{
    public Material curvedMaterial;  // 셰이더가 적용된 머티리얼

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // 카메라 렌더링 후 이미지를 셰이더를 통해 처리
        if (curvedMaterial != null)
        {
            Graphics.Blit(source, destination, curvedMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
