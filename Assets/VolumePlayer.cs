using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumePlayer : MonoBehaviour
{
    AudioSource audiosource;

    void Awake() => audiosource = GetComponent<AudioSource>(); 
    void Update() => audiosource.volume = GameOptions.Volume; 
}
