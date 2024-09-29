using UnityEngine;

[ExecuteInEditMode]
public class CameraCurvedEffect : MonoBehaviour
{
    public Material curvedMaterial;  // ���̴��� ����� ��Ƽ����

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // ī�޶� ������ �� �̹����� ���̴��� ���� ó��
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
