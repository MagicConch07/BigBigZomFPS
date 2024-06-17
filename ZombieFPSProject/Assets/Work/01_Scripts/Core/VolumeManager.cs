using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : MonoSingleton<VolumeManager>
{
    
    private const float MAX_INTENSITY = 1f;
    [SerializeField] private Volume _volume;
    private VolumeProfile _volumeProfile;
    private Vignette _vignette;
    private LensDistortion _lens;
    private float _vignetteOriginIntensity;
    private bool _isHit = false;

    public float hitDuration = 0.3f;
    public float endDuration = 0.3f;
    public float lensPower = 0.3f;

    private void Awake()
    {
        _volumeProfile = _volume.profile;
        _volumeProfile.TryGet(out _vignette);
        _volumeProfile.TryGet(out _lens);
        _vignetteOriginIntensity = _vignette.intensity.value;
    }

    private void Update()
    {
        //! 왜곡 효과 추가하기
        if (Input.GetKeyDown(KeyCode.V))
        {
            HitImage();
        }
    }

    public void HitImage()
    {
        if (_isHit) return;
        StartCoroutine(HitImageCoroutine());
    }

    public IEnumerator HitImageCoroutine()
    {
        float currentTime = 0;
        float percent = hitDuration;
        
        _isHit = true;
        _vignette.color.value = Color.red;
        
        while (currentTime <= 1f)
        {
            currentTime += Time.deltaTime / percent;
            print(currentTime);
            _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, MAX_INTENSITY, currentTime);
            _lens.intensity.value = Mathf.Lerp(_lens.intensity.value, lensPower, currentTime);
            yield return null;
        }

        currentTime = 0;
        percent = endDuration;
        _vignette.color.value = Color.black;
        while (currentTime <= 1f)
        {
            currentTime += Time.deltaTime / percent;
            _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, _vignetteOriginIntensity, currentTime);
            _lens.intensity.value = Mathf.Lerp(_lens.intensity.value, 0, currentTime);
            yield return null;
        }
        _isHit = false;
    }
}
