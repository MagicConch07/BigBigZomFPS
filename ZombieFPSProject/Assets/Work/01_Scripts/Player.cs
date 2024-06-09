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
            base.Awake();
            _weapon.InitCaster(this);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //! 이거 진짜 분리 해야 해
        }

        public override void SetDead()
        {
            
        }
    }
}
