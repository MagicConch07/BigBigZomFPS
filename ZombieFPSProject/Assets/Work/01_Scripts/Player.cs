using UnityEngine;
using System;

namespace Work._01_Scripts
{
    public class Player : Agent
    {
        [SerializeField] private InputReader _inputReader;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void SetDead()
        {
            
        }
    }
}
