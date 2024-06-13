using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletUI : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    private List<GameObject> _bulletList;
    private int _currentBulletCount = 30;
    private void Awake()
    {
        RectTransform[] bullets = GetComponentsInChildren<RectTransform>();
        print(bullets.Length);

        for (int i = bullets.Length; i > 0; --i)
        {
            _bulletList.Add(bullets[i].gameObject);
        }
    }

    private void OnEnable()
    {
        _weapon.OnFireEvent += HandleFire;
        _weapon.OnReloadingEvent += HandleReloading;
    }

    private void OnDisable()
    {
        _weapon.OnFireEvent -= HandleFire;
        _weapon.OnReloadingEvent -= HandleReloading;
    }

    private void HandleFire()
    {
        if (_currentBulletCount <= 0) return;
        
        
    }

    private void HandleReloading()
    {
        foreach (GameObject bullet in _bulletList)
        {
            bullet.SetActive(true);
        }
    }

}
