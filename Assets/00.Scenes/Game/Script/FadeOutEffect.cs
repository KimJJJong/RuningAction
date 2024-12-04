using UnityEngine;

public class FadeOutEffect : MonoBehaviour
{
    public float fadeDuration = 2f; // 페이드 지속 시간
    private float fadeTimer = 0f;
    private bool isFading = false;
    private bool fadeOut = true; // true면 페이드 아웃, false면 페이드 인

    private Renderer[] renderers;
    private Material[] materials;

    void Start()
    {
        // 하위의 모든 Renderer 컴포넌트 가져오기
        renderers = GetComponentsInChildren<Renderer>();

        // 모든 메터리얼 수집
        var materialList = new System.Collections.Generic.List<Material>();
        foreach (var renderer in renderers)
        {
            materialList.AddRange(renderer.materials);
        }
        materials = materialList.ToArray();
    }

void Update()
{
    if (isFading)
    {
        fadeTimer += Time.deltaTime;
        float t = fadeTimer / fadeDuration;
        float alpha = fadeOut ? Mathf.Lerp(1, 0, t) : Mathf.Lerp(0, 1, t);

        // 알파 값 업데이트
        foreach (var mat in materials)
        {
            if (mat.HasProperty("_Color"))
            {
                Color color = mat.color;
                color.a = alpha;
                mat.color = color;
            }
        }

        // 페이드 완료 시
        if (fadeTimer >= fadeDuration)
        {
            isFading = false;
            fadeTimer = 0f;

            // 자동 페이드 전환 (옵션)
            if (fadeOut)
            {
                // 페이드 아웃이 끝나면 페이드 인 시작
                StartFadeIn();
            }
            else
            {
                // 페이드 인이 끝난 경우 다른 동작을 수행 가능
                Debug.Log("Fade In Completed");
            }
        }
    }
}


    public void StartFadeOut()
    {
        Debug.Log("페이드 아웃");
        fadeOut = true;
        fadeTimer = 0f;
        isFading = true;
    }

    public void StartFadeIn()
    {
        fadeOut = false;
        fadeTimer = 0f;
        isFading = true;
    }
}
