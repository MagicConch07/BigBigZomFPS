using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using ObjectPooling;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayer;
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
        _camRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
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
        RaycastHit[] hitInfo = new RaycastHit[3];
        int hit = Physics.RaycastNonAlloc(_camRay, hitInfo, rayDistance, _enemyLayer);
        if (hit >= 1)
        {
            Debug.Log(hitInfo[0]);
        }
        _isAttack = true;

        _perlin.m_AmplitudeGain = 1;
        _perlin.m_FrequencyGain = 1;

        Bullet bulletObj = PoolManager.Instance.Pop(PoolingType.Bullet_5_56x45) as Bullet;
        
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
        Gizmos.DrawRay(_camRay.origin, _camRay.direction * rayDistance);
    }
}

