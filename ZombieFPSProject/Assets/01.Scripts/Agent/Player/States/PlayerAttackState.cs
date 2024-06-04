using System;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private int _comboCounter = 0;
    private float _lastAttackTime;
    private float _comboWindow = 0.4f; // 키를 누른이후 다시 키를 누르기까지 대기시간
    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    private Coroutine _delayCoroutine = null;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, string boolName) : base(player, stateMachine, boolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        bool comboCounterOver = _comboCounter > 2;
        bool comboWindowExhaust = Time.time >= _lastAttackTime + _comboWindow;
        if(comboCounterOver || comboWindowExhaust)
        {
            _comboCounter = 0;
        }
        _player.currentComboCounter = _comboCounter;
        _player.AnimatorCompo.speed = _player.attackSpeed;
        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);

        float movePower = _player.attackMovement[_comboCounter];

        float delayTime = 0.2f;

        _delayCoroutine = _player.StartDelayCallback(delayTime, () =>
        {
            _player.MovementCompo.StopImmediately();
        });
    }


    public override void Exit()
    {
        ++_comboCounter;
        _lastAttackTime = Time.time;
        _player.AnimatorCompo.speed = 1f;

        if(_delayCoroutine != null)
        {
            _player.StopCoroutine(_delayCoroutine);
        }
        base.Exit();
    }
    

    public override void UpdateState()
    {
        base.UpdateState();
        if(_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void AnimationFinishTrigger()
    {
        _endTriggerCalled = true;
    }
}
