using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _virtualCam;
    
    private FPSInput.PlayerActions _inputAction;
    private Rigidbody _myrigid;
    private bool _isCursor = false;
    
    public float speed = 15f;
    
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private float _rayDistance = 1.2f;
    
    [SerializeField] private LayerMask _groundLayer;
    void Awake()
    {
        _myrigid = GetComponent<Rigidbody>();
        _inputAction = _inputReader.FPSInputInstance.Player;

        _inputReader.SettingsEvent += SettingsHandle;
        _inputReader.SprintEvent += SprintHandle;
        _inputReader.SitEvent += SitHandle;
        _inputReader.JumpEvent += JumpHandle;
    }

    void OnDisable()
    {
        _inputReader.SettingsEvent -= SettingsHandle;
        _inputReader.SprintEvent -= SprintHandle;
        _inputReader.SitEvent -= SitHandle;
        _inputReader.JumpEvent -= JumpHandle;
    }

    //TODO : 이거 세팅을 따로 클래스로 뽑아내서 만들어야 함
    //! 일단 여기다가 플레이어 움직임이 아닌 세팅도 넣겠음
    private void SettingsHandle(bool isPress)
    {
        _isCursor = !isPress;
    }

    private void SprintHandle(bool isPress)
    {
        if (isPress == false)
        {
            speed = 12;
            return;
        }

        speed = 20;
    }

    private void MoveSit(bool isPress)
    {
        if (isPress == false)
        {
            _virtualCam.transform.localPosition = new Vector3(_virtualCam.transform.localPosition.x, _virtualCam.transform.localPosition.y, _virtualCam.transform.localPosition.z);
            return;
        }

        _virtualCam.transform.localPosition = new Vector3(_virtualCam.transform.localPosition.x, -_virtualCam.transform.localPosition.y, _virtualCam.transform.localPosition.z);
    }

    private void SitHandle(bool isPress)
    {
        if (isPress == false)
        {
            _virtualCam.transform.localPosition = new Vector3(_virtualCam.transform.localPosition.x, 2f, _virtualCam.transform.localPosition.z);
            return;
        }

        _virtualCam.transform.localPosition = new Vector3(_virtualCam.transform.localPosition.x, 1f, _virtualCam.transform.localPosition.z);
    }

    private void JumpHandle(bool isPress)
    {
        if (isPress == false) return;

        RaycastHit[] hits = new RaycastHit[1];
        Physics.RaycastNonAlloc(transform.position, Vector3.down, hits, _rayDistance, _groundLayer);
        int hit = Physics.RaycastNonAlloc(transform.position, Vector3.down, hits, _rayDistance, _groundLayer);
        if (hit != 0)
        {
            _myrigid.velocity = new Vector3(_myrigid.velocity.x, _jumpPower, _myrigid.velocity.z);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Vector3.down);
    }
}
