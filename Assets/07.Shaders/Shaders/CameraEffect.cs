using UnityEngine;

[ExecuteInEditMode]
public class CameraEffect : MonoBehaviour
{
    // 커스텀 쉐이더가 적용된 머티리얼을 여기에 할당
    public Material effectMaterial;

    // 카메라의 렌더링된 이미지에 효과를 적용
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (effectMaterial != null)
        {
            // 커스텀 쉐이더를 사용해 화면에 보이는 모든 오브젝트에 효과를 적용
            Graphics.Blit(source, destination, effectMaterial);
            Debug.Log("custom");
        }
        else
        {
            // 쉐이더가 없으면 그냥 기본 렌더링
            Graphics.Blit(source, destination);
            Debug.Log("default");
        }
    }
}
