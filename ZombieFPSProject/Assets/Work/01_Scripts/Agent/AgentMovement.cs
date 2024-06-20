using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AgentMovement : MonoBehaviour, IMovement
{
    [SerializeField] private InputReader _inputReader;

    [Header("Ground Layer Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundRayDistance;

    protected Rigidbody _myRigidbody;
    private Agent _agent;

    #region �ӵ� ���� ����

    public float moveSpeed = 10f;
    public float sprintSpeed = 12f;
    public float jumpPower = 5f;
    private Vector3 _velocity;
    public Vector3 Velocity => _velocity;
    public bool IsGround { get; private set; }
    public float groundRayDistance = 1.3f;

    private float originSpeed;
    private Vector2 _movementInput;
    #endregion

    public void Initialize(Agent agent)
    {
        // ! �̰� inputreader �ٲ��
        // TODO : inputreader �������� ���� ���ְ� �ϰ��� �ְ� ���� ���� => �ϳ������� �ϼ� �������� ���� ���� ����?
        //_inputReader = 
        _myRigidbody = GetComponent<Rigidbody>();
        _agent = agent;
        originSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        _inputReader.OnJumpEvent += HandleJump;
        _inputReader.OnSprintEvent += HandleSprint;
    }

    private void OnDisable()
    {
        _inputReader.OnJumpEvent -= HandleJump;
        _inputReader.OnSprintEvent -= HandleSprint;
    }

    private void HandleSprint(bool isPress)
    {
        // TODO : �ִϸ��̼��̳� ī�޶� ��鸲 �߰� �Ͻʼ�
        if (isPress)
            moveSpeed = sprintSpeed;
        else
            moveSpeed = originSpeed;
    }

    private void HandleJump()
    {
        if (IsGround)
        {
            _myRigidbody.velocity = new Vector3(_myRigidbody.velocity.x, jumpPower, _myRigidbody.velocity.z);
            //_myRigidbody.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }


    private void Update()
    {
        IsGround = GroundCheck();
        Gravity();
    }

    public float gravityPower = 3;
    private void Gravity()
    {
        _myRigidbody.AddForce(new Vector3(0, -9.81f * gravityPower * Time.deltaTime, 0), ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        //Movement
        Move();
    }

    private void Move()
    {
        _movementInput = _inputReader.PlayerActionsInstance.Movement.ReadValue<Vector2>();

        _velocity = new Vector3(_movementInput.x, 0, _movementInput.y) * moveSpeed;
        _velocity = transform.TransformDirection(_velocity);

        _myRigidbody.velocity = new Vector3(_velocity.x, _myRigidbody.velocity.y, _velocity.z);
    }

    private bool GroundCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down * groundRayDistance, _groundRayDistance, _groundLayer);
    }

    public void StopImmediately()
    {
        _velocity = Vector3.zero;
    }

    public void SetMovement(Vector3 movement, bool isRotation = true)
    {
        // Noting
    }

    public void SetDestination(Vector3 destination)
    {
        //�÷��̾�� �� �Լ��� �Ⱦ���. (NavMesh���)
    }

    public void GetKnockback(Vector3 force)
    {
        //����� �˹��� �������� �ʴ´�.
    }

    public void Initialize(PoolAgent agent)
    {
        //! 원래 이러면 안되긴 하는데 시간이 없으니 이거는 에너미 전용
        //TODO : 민교야 분발하자 할 수 있다. 진짜 이제부터 하나씩 차근히 하는거야 한가지만 집중해
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down);
    }
#endif
}
