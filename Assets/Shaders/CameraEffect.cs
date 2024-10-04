using UnityEngine;

[ExecuteInEditMode]
public class CameraEffect : MonoBehaviour
{
    // Ŀ���� ���̴��� ����� ��Ƽ������ ���⿡ �Ҵ�
    public Material effectMaterial;

    // ī�޶��� �������� �̹����� ȿ���� ����
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (effectMaterial != null)
        {
            // Ŀ���� ���̴��� ����� ȭ�鿡 ���̴� ��� ������Ʈ�� ȿ���� ����
            Graphics.Blit(source, destination, effectMaterial);
            Debug.Log("custom");
        }
        else
        {
            // ���̴��� ������ �׳� �⺻ ������
            Graphics.Blit(source, destination);
            Debug.Log("default");
        }
    }
}
