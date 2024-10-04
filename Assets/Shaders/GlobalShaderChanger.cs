using UnityEngine;

public class GlobalShaderChanger : MonoBehaviour
{
    public Shader newShader;

    void Start()
    {
        // ���� ��� ������ ã��
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            // �� �������� ��� ��Ƽ������ ��ȸ�ϸ� ���̴� ��ü
            foreach (Material mat in renderer.materials)
            {
                mat.shader = newShader;
            }
        }
    }
}
