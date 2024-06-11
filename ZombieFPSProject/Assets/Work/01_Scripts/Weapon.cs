using System;
using DG.Tweening;
using System.Collections;
using Cinemachine;
using ObjectPooling;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Player Valuse
    
    private Agent _owner;
    [SerializeField] private InputReader _inputReader;
    
    #endregion

    #region Gun Values

    [Header("Gun Setting")]
    [SerializeField] private Transform _gun;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Transform _casingTrm;
    [SerializeField] private float _knockbackPower = 1f;
    // todo : 이거는 플레이어 스탯으로 빼자
    public float rayDistance = 50f;
    public float _fireRate = 0.12f;
    
    [Header("Gun Tween")] 
    public float gunRotationDuration = 0.12f;
    public float endDuration = 0.3f;
    public float gunTweenBackDuration = 0.1f;
    public float gun_power = -5;
    public float recoilPosPower = -0.07f;
    private Sequence _gunSequence;
    
    [Header("Muzzle Tween")]
    public Vector3 muzzle_Str;
    public int muzzle_Vibrato;
    public float muzzle_duration;
    private Sequence _muzzleSequence;
    
    private bool _isAttack = false;
    #endregion
    
    #region Camera Valuse
    [Header("Cam Settings")]
    [SerializeField] private CinemachineVirtualCamera _virCam;
    private CinemachineBasicMultiChannelPerlin _perlin;

    [Header("Cam Tween Settings")] 
    public float camTweenDuration = 0.05f;
    public float DG_ShakePositionDuration = 0.5f;
    public float power = -3;
    public int DG_Vibrato = 10;
    public Vector3 Cam_Strength = Vector3.one;
    private Sequence _CamSequence;
    
    private Ray _camRay;
    #endregion

    #region  Event
    public event Action OnFireFlame; 

    #endregion
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
            if (zombieHit != null) zombieHit.transform.position = hitInfo[0].point;

            if(hitInfo[0].collider.TryGetComponent<IDamageable>(out IDamageable health))
            {
                int damage = _owner.Stat.GetDamage(); // Onwer Damage
                health.ApplyDamage(damage, hitInfo[0].point, hitInfo[0].normal, _knockbackPower, _owner, DamageType.Range);
            }
        }
        _isAttack = true;

        //! Cam 
        //PerlinCam(1, 1);
        //CamTween();
        Recoil();
        MuzzleTween();
        
        CreateBullet();
        
        yield return new WaitForSeconds(_fireRate);

        //! Cam
        //PerlinCam();
        
        _isAttack = false;
    }
    
    private void MuzzleTween()
    {
        _muzzleSequence = DOTween.Sequence()
            .Append(_muzzle.DOShakeRotation(muzzle_duration, muzzle_Str, muzzle_Vibrato, 1, false));
    }
    
    private void CamTween()
    {
        _CamSequence = DOTween.Sequence()
            .Append(_virCam.transform.DOLocalRotate(new Vector3(power, 0, 0), camTweenDuration, RotateMode.Fast))
            .Join(_virCam.transform.DOShakePosition(DG_ShakePositionDuration, Cam_Strength, DG_Vibrato, 1, false))
            .OnComplete(() =>
            {
                _virCam.transform.DOLocalRotate(new Vector3(0, 0, 0), camTweenDuration, RotateMode.Fast);
            });
    }
    
    private void Recoil()
    {
        _gunSequence = DOTween.Sequence()
            .Append(_gun.DOLocalRotate(new Vector3(gun_power, 0, 0), gunRotationDuration).SetEase(Ease.Linear))
            .Join(_gun.DOLocalMoveZ(recoilPosPower, gunTweenBackDuration).SetEase(Ease.InOutQuad))
            .Append(_gun.DOLocalMoveZ(0, gunTweenBackDuration).SetEase(Ease.InOutQuad))
            .OnComplete(() =>
            {
                _gun.DOLocalRotate(new Vector3(0, 0, 0), endDuration);
            });
    }

    private void PerlinCam(int amplitude = 0, int frequency = 0)
    {
        _perlin.m_AmplitudeGain = amplitude;
        _perlin.m_FrequencyGain = frequency;
    }

    private void CreateBullet()
    {
        PoolObjTargetToPos(PoolingType.Bullet, _muzzle);
        PoolObjTargetToPos(PoolingType.CasingBullet, _casingTrm);
        OnFireFlame?.Invoke();
    }
    
    private void PoolObjTargetToPos (PoolingType poolType, Transform target)
    {
        PoolableMono obj = PoolManager.Instance.Pop(poolType);
        if (obj != null) TargetToPos(obj.transform, target);
    }

    private void TargetToPos(Transform pos, Transform target)
    {
        pos.position = target.position;
        pos.rotation = target.rotation;
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_camRay.origin, _camRay.direction * rayDistance);
    }
    #endif
}

