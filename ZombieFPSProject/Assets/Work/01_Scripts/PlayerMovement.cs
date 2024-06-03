using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _virtualCam;
    [SerializeField] private Transform _bodyObj;
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private float cameraRotationLimit;

    private Quaternion rotationCharacter;
    private Quaternion rotationCamera;
    private FPSInput.PlayerActions _inputAction;
    private Rigidbody _myrigid;
    private bool _isCursor = false;

    public float speed = 15f;
    public float mouseSpeed = 1f;


    void Awake()
    {
        _myrigid = GetComponent<Rigidbody>();
        _inputAction = _inputReader.FPSInputInstance.Player;

        _inputReader.SettingsEvent += SettingsHandle;
        _inputReader.SprintEvent += SprintHandle;
        _inputReader.SitEvent += SitHandle;
        _inputReader.JumpEvent += JumpHandle;
    }

    void Start()
    {
        rotationCharacter = _myrigid.transform.localRotation;
        rotationCamera = _virtualCam.transform.localRotation;
    }

    void LateUpdate()
    {
        CameraRotation();

        //TODO: 카메라 움직임만 따로 클래스로
        //! 나중에 클래스로 뽑아내야지
        float mouseY = _inputAction.MouseView.ReadValue<Vector2>().x * Mathf.Pow(_sensitivity, 2) * mouseSpeed;

        Quaternion rotationYaw = Quaternion.Euler(0.0f, mouseY, 0.0f);
        rotationCharacter *= rotationYaw;

        _myrigid.MoveRotation(_myrigid.rotation * rotationYaw);
    }

    void FixedUpdate()
    {
        Vector2 movement = _inputAction.Movement.ReadValue<Vector2>();
        Vector3 moveVec = new Vector3(movement.x, 0, movement.y) * speed;

        moveVec = transform.TransformDirection(moveVec);

        _myrigid.velocity = new Vector3(moveVec.x, _myrigid.velocity.y, moveVec.z);

        //? 이건 절대 잊을 수 없지
    }

    void OnDisable()
    {
        _inputReader.SettingsEvent -= SettingsHandle;
        _inputReader.SprintEvent -= SprintHandle;
        _inputReader.SitEvent -= SitHandle;
        _inputReader.JumpEvent -= JumpHandle;
    }

    private void CameraRotation()
    {
        float _xRotation = _inputAction.MouseView.ReadValue<Vector2>().y * Mathf.Pow(_sensitivity, 2) * mouseSpeed;

        Quaternion localRotation = _virtualCam.localRotation;
        Quaternion rotationPitch = Quaternion.Euler(-_xRotation, 0.0f, 0.0f);

        //Save rotation. We use this for smooth rotation2.
        rotationCamera *= rotationPitch;

        //Local Rotation.

        localRotation *= rotationPitch;

        localRotation = Clamp(localRotation);

        _virtualCam.localRotation = localRotation;
    }

    private Quaternion Clamp(Quaternion rotation)
    {
        //TODO : 쿼터니언 이해하기
        rotation.Normalize();

        //Pitch.
        float pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rotation.x);

        //Clamp.
        pitch = Mathf.Clamp(pitch, -cameraRotationLimit, cameraRotationLimit);

        // 다시 쿼터니언으로 변환
        rotation.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

        //Return.
        return rotation;
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

    [SerializeField] private LayerMask _groundLayer;

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
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private float _rayDistance = 1.2f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Vector3.down);
    }
}
