using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeManager : MonoSingleton<VolumeManager>
{
    private Volume _globalVolume;

    private void Awake()
    {
        _globalVolume = Instance.transform.GetComponent()
    }   

    public void HitImage()
    {
        
    }
}
