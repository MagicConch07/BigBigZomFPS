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

    #region 속도 관련 로직

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
        // ! 이거 inputreader 바꿔라
        // TODO : inputreader 쓸데없는 참조 없애고 일관성 있게 변수 관리 => 하나에서만 하셈 여러군데 하지 말고 ㅇㅋ?
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
        // TODO : 애니메이션이나 카메라 흔들림 추가 하십쇼
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
        }
    }


    private void Update()
    { 
        IsGround = GroundCheck();
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
        _velocity =  transform.TransformDirection(_velocity);
        
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
        //플레이어는 이 함수를 안쓴다. (NavMesh기반)
    }

    public void GetKnockback(Vector3 force)
    {
        //현재는 넉백을 구현하지 않는다.
    }
    
#if UNITY_EDITOR    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down);
    }
#endif
}
