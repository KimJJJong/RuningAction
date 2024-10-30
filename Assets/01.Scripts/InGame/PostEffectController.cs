using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SCPE;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class PostEffectController : MonoBehaviour
{
    new Camera camera;

    [SerializeField] float shakeDuration = 1f;
    [SerializeField] float shakeMagnitude = 0.5f;
    [SerializeField] private Volume _volume;
    private RadialBlur _radialBlur;
    private SpeedLines _speedLines;
    private Danger _danger;
    private Blur _blur;

    void Start()
    {
        camera = GetComponent<Camera>();

        _volume = GetComponent<Volume>();

        _volume.profile.TryGet(out _radialBlur);
        _volume.profile.TryGet(out _speedLines);
        _volume.profile.TryGet(out _danger);
        _volume.profile.TryGet(out _blur);
    }

    public void RushPostEffect(float radiaBlur, float speedLines, bool isRush)
    {
        if (isRush)
        {
            _radialBlur.amount.value = radiaBlur;
            _speedLines.intensity.value = speedLines;
        }
        else
        {
            StartCoroutine(DecraseRushEffect());
        }
    }

    public void GetDamage()
    {
        StartCoroutine(DecreaseDamage());
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            camera.transform.position += (Vector3)Random.insideUnitCircle * shakeMagnitude;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator DecraseRushEffect()
    {
        float value = _radialBlur.amount.value;
        while (value > 0)
        {
            _radialBlur.amount.value -= 0.01f;
            _speedLines.intensity.value -= 0.02f;

            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator DecreaseDamage()
    {
        _danger.intensity.value = 0.7f;

        while (_danger.intensity.value > 0)
        {
            _danger.intensity.value -= 0.1f;
            yield return new WaitForSeconds(0.08f);
        }

    }
}
