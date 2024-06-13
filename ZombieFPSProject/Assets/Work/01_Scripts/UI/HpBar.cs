using System;
using System.Collections;
using System.Collections.Generic;
using Microlight.MicroBar;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    private const int MAX_HP = 100;
    private MicroBar _microBar;
    private int _currentHp;
    private int _damagePower = 10;
    public int DamagePower { get => _damagePower; set => _damagePower = value; }
    private void Awake()
    {
        _currentHp = MAX_HP;
        _microBar = GetComponent<MicroBar>();
        _microBar.Initialize(MAX_HP);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            print("Damage");
            DamageHpBar();
        }
    }

    public void HealHpBar()
    {
        
    }

    public void DamageHpBar()
    {
        _currentHp -= _damagePower;
        if (_currentHp <= 0) _currentHp = 0;
        _microBar.UpdateBar(_currentHp, false, UpdateAnim.Damage);
    }
}
