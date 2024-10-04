using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SCPE;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class PostEffectController : MonoBehaviour
{
    [SerializeField] private Volume _volume;
     private RadialBlur _radialBlur;
     private SpeedLines _speedLines;
     private Danger _danger;
     private Blur _blur;
    // Start is called before the first frame update
    void Start()
    {
        _volume = GetComponent<Volume>();

        _volume.profile.TryGet(out _radialBlur);
        _volume.profile.TryGet(out _speedLines);
        _volume.profile.TryGet(out _danger);
        _volume.profile.TryGet(out _blur);
    }

    // Update is called once per frame
    void Update()
    {
       // radialBlur.amount.value = 
    }

    
    public void RushPostEffect(float radiaBlur, float speedLines, bool isRush)
    {
        if(isRush)
        {
        _radialBlur.amount.value = radiaBlur;
        _speedLines.intensity.value= speedLines;
        }
        else
        {
            StartCoroutine(DecraseRushEffect());
        }
    }
    public void GetDamage()
    {
        StartCoroutine(DecreaseDamage());
    }

    IEnumerator DecraseRushEffect()
    {
        float value = _radialBlur.amount.value;
        while (value>0)
        {
            _radialBlur.amount.value -= 0.02f;
            _speedLines.intensity.value -=0.02f;

            yield return new WaitForSeconds( 0.05f );
        }
    }

    IEnumerator DecreaseDamage()
    {
        _danger.intensity.value = 0.7f;
        _blur.amount.value = 1.4f;
        
        while(_danger.intensity.value > 0)
        {
            _danger.intensity.value -=0.1f;
            _blur.amount.value -= 0.2f;
            yield return new WaitForSeconds(0.08f);
        }

    }
}
