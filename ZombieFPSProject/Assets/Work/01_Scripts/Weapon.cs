using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _casingPrefab;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Transform _casingPos;
    [SerializeField] private CinemachineVirtualCamera _virCam;
    private CinemachineBasicMultiChannelPerlin _perlin;

    public float rayDistance = 10f;
    public float _fireRate = 0.2f;

    private bool _isAttack = false;
    private bool _isTest = false;

    private Ray _camRay;

    void Awake()
    {
        _perlin = _virCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        _camRay = Camera.main.ScreenPointToRay(Vector3.forward * rayDistance);
        if (Input.GetKey(KeyCode.Mouse0) && _isAttack == false)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (_isTest == false)
                _isTest = true;
            else
                _isTest = false;
        }

        while (_isTest && _isAttack == false)
        {
            StartCoroutine(Shoot());
        }

    }

    private IEnumerator Shoot()
    {
        _isAttack = true;

        _perlin.m_AmplitudeGain = 1;
        _perlin.m_FrequencyGain = 1;

        GameObject bulletObj = Instantiate(_bulletPrefab);
        bulletObj.transform.position = _muzzle.position;
        bulletObj.transform.rotation = _muzzle.rotation;

        GameObject casingObj = Instantiate(_casingPrefab);
        casingObj.transform.position = _casingPos.position;
        casingObj.transform.rotation = _casingPos.rotation;
        yield return new WaitForSeconds(_fireRate);

        _perlin.m_AmplitudeGain = 0;
        _perlin.m_FrequencyGain = 0;
        _isAttack = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_camRay);
    }
}

