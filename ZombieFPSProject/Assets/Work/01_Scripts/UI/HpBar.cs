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

    public int healPower = 25;
    
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
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            print("Heal");
            HealHpBar();
        }
    }

    public void HealHpBar()
    {
        _currentHp += healPower;
        if (_currentHp >= MAX_HP) _currentHp = MAX_HP;
        _microBar.UpdateBar(_currentHp, false, UpdateAnim.Heal);
    }

    public void DamageHpBar()
    {
        _currentHp -= _damagePower;
        if (_currentHp <= 0) _currentHp = 0;
        _microBar.UpdateBar(_currentHp, false, UpdateAnim.Damage);
    }
}
