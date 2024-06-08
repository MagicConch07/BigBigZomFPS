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
    [SerializeField] private InputReader _inputReader;
    
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _muzzle;
    
    [SerializeField] private GameObject _casingPrefab;
    [SerializeField] private Transform _casingPos;
    
    [SerializeField] private float _knockbackPower = 1f;
    
    [SerializeField] private CinemachineVirtualCamera _virCam;
    private CinemachineBasicMultiChannelPerlin _perlin;

    public float rayDistance = 10f;
    public float _fireRate = 0.2f;

    private bool _isAttack = false;
    private bool _isTest = false;

    private Ray _camRay;

    private Agent _owner;
    
    public void InitCaster(Agent agent)
    {
        _owner = agent;
    }
    
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

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Shoot()
    {
        RaycastHit[] hitInfo = new RaycastHit[3];
        int hit = Physics.RaycastNonAlloc(_camRay, hitInfo, rayDistance, _enemyLayer);
        if (hit >= 1)
        {
            if(hitInfo[0].collider.TryGetComponent<IDamageable>(out IDamageable health))
            {
                int damage = _owner.Stat.GetDamage(); //주인의 데미지
                //float knockbackPower = 3f; // todo : 나중에 스탯으로부터 가져와야 해.
                health.ApplyDamage(damage, hitInfo[0].point, hitInfo[0].normal, _knockbackPower, _owner, DamageType.Range);
            }
        }
        _isAttack = true;

        PerlinCam(1, 1);
        
        CreateBullet();
        
        yield return new WaitForSeconds(_fireRate);

        PerlinCam();
        
        _isAttack = false;
    }

    private void PerlinCam(int amplitude = 0, int frequency = 0)
    {
        _perlin.m_AmplitudeGain = amplitude;
        _perlin.m_FrequencyGain = frequency;
    }

    private void CreateBullet()
    {
        Bullet bulletObj = PoolManager.Instance.Pop(PoolingType.Bullet_5_56x45) as Bullet;
        
        // 라이더 왈 캐싱이 더 빠르다
        bulletObj.transform.position = _muzzle.position;
        bulletObj.transform.rotation = _muzzle.rotation;

        GameObject casingObj = Instantiate(_casingPrefab);
        casingObj.transform.position = _casingPos.position;
        casingObj.transform.rotation = _casingPos.rotation;
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_camRay.origin, _camRay.direction * rayDistance);
    }
    #endif
}

