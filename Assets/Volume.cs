using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public Slider volSlide;
    public static float VolumeValue = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        volSlide = GetComponent<Slider>();
        volSlide.value = GameOptions.Volume;
        volSlide.onValueChanged.AddListener(OnValueChanged);
    }

    // Update is called once per frame
    void OnValueChanged(float newValue)
    {
        GameOptions.Volume = newValue;
        GameOptions.Save();
    }
}
