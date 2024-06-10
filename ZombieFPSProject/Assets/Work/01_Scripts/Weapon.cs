using DG.Tweening;
using System.Collections;
using Cinemachine;
using ObjectPooling;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _muzzle;
    
    [SerializeField] private Transform _casingTrm;
    
    [SerializeField] private float _knockbackPower = 1f;
    
    [SerializeField] private CinemachineVirtualCamera _virCam;
    private CinemachineBasicMultiChannelPerlin _perlin;

    [SerializeField] private Transform _gun;

    public float rayDistance = 10f;
    public float _fireRate = 0.2f;

    private bool _isAttack = false;

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
        // Todo : ChangeInput System
        if (Input.GetKey(KeyCode.Mouse0) && _isAttack == false)
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
            ZombieHit zombieHit = PoolManager.Instance.Pop(PoolingType.ZombieHit) as ZombieHit;
            zombieHit.transform.position = hitInfo[0].point;
                
            if(hitInfo[0].collider.TryGetComponent<IDamageable>(out IDamageable health))
            {
                int damage = _owner.Stat.GetDamage(); //주인의 데미지
                //float knockbackPower = 3f; // todo : 나중에 스탯으로부터 가져와야 해.
                health.ApplyDamage(damage, hitInfo[0].point, hitInfo[0].normal, _knockbackPower, _owner, DamageType.Range);
            }
        }
        _isAttack = true;

        //! Cam 
        //PerlinCam(1, 1);
        Recoil();
        
        CreateBullet();
        
        yield return new WaitForSeconds(_fireRate);

        //! Cam
        //PerlinCam();
        
        _isAttack = false;
    }

    [Header("Gun Settings")] 
    public float DG_Duration = 0.5f;
    public int DG_Vibrato = 10;

    private void Recoil()
    {
        _gun.transform.DOShakeRotation(DG_Duration, new Vector3(-7, 0, 0), DG_Vibrato, 0f, false);
    }

    private void PerlinCam(int amplitude = 0, int frequency = 0)
    {
        _perlin.m_AmplitudeGain = amplitude;
        _perlin.m_FrequencyGain = frequency;
    }

    private void CreateBullet()
    {
        //? 이거 뭔가 꺼림직해 너무 불편해 코드 재활용 해야 돼
        Bullet bulletObj = PoolManager.Instance.Pop(PoolingType.Bullet) as Bullet;
        if (bulletObj != null) PosToTarget(bulletObj.transform, _muzzle);

        CasingBullet casingObj = PoolManager.Instance.Pop(PoolingType.CasingBullet) as CasingBullet;
        if (casingObj != null) PosToTarget(casingObj.transform, _casingTrm);
    }

    private void PosToTarget(Transform target, Transform pos)
    {
        target.position = pos.position;
        target.rotation = pos.rotation;
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_camRay.origin, _camRay.direction * rayDistance);
    }
    #endif
}

