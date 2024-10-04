using UnityEngine;

public class GlobalShaderChanger : MonoBehaviour
{
    public Shader newShader;

    void Start()
    {
        // 씬의 모든 렌더러 찾기
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            // 각 렌더러의 모든 머티리얼을 순회하며 쉐이더 교체
            foreach (Material mat in renderer.materials)
            {
                mat.shader = newShader;
            }
        }
    }
}
