using UnityEngine;
using System;

namespace Work._01_Scripts
{
    public class Player : Agent
    {
        [SerializeField] private InputReader _inputReader;

        [SerializeField] private Weapon _weapon;

        protected override void Awake()
        {
            Transform visualTrm = transform.Find("HeadView/Visual");
            AnimatorCompo = visualTrm.GetComponent<Animator>();
            MovementCompo = GetComponent<IMovement>();
            MovementCompo.Initialize(this);

            // Todo : Add Melee Attack
            // 아직 DamageCaster는 필요없으나 나중에 근접공격을 위해 살려두기
            /*Transform damageTrm = transform.Find("DamageCaster");
            if (damageTrm != null)
            {
                DamageCasterCompo = damageTrm.GetComponent<DamageCaster>();
                DamageCasterCompo.InitCaster(this);
            }*/

            Stat = Instantiate(Stat); //자기자신 복제본으로 만들고 들어간다.
            Stat.SetOwner(this);

            HealthCompo = GetComponent<Health>();
            HealthCompo.Initialize(this);
            
            _weapon.InitCaster(this);
            
            // Todo : Cursor Lock and Visble 따로 처리
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //! 이거 진짜 분리 해야 해
        }

        public void TestHit()   
        {
            //! 나중에 처리
            //print(HealthCompo.CurrentHealth);
        }

        public override void SetDead()
        {
            //isDead = true;
            Debug.LogError("Player Die!!!");
        }
    }
}
