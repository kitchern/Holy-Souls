using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TakeDamagePost : MonoBehaviour
{
    public float intensity = 0;
    PostProcessVolume volume;
    Vignette vignette;
    public Color vignetteHurtColor;
    public Color vignetteShieldBreakColor;
    Color originalvignetteColor;
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings<Vignette>(out vignette);
        originalvignetteColor = vignette.color;

        if(!vignette)
        {
            print("error, vignette empty");
        }
        else
        {
            print("has vignette");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamageNormal()
    {
        intensity = 0.6f;
        vignette.color.Override(Color.Lerp(vignetteShieldBreakColor, originalvignetteColor, Time.deltaTime * 2));
        
    }

    public void TakeDamageShield()
    {
        intensity = 0.6f;
        vignette.color.Override(Color.Lerp(vignetteHurtColor, originalvignetteColor, Time.deltaTime * 2));
        vignette.intensity.Override(0.4f);
    
    }
}
