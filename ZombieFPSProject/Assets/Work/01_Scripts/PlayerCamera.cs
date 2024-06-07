using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    private FPSInput.PlayerActions _inputAction;
    
    [SerializeField] private Rigidbody _myrigid;
    
    // Cam Setting
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private float cameraRotationLimit;
    private Quaternion rotationCamera;
    private Quaternion rotationCharacter;
    
    public float mouseSpeed = 1f;
    
    
    void Awake()
    {
        _inputAction = _inputReader.FPSInputInstance.Player;
    }
    
    void Start()
    {
        rotationCharacter = _myrigid.transform.localRotation;
        
        rotationCamera = transform.transform.localRotation;
    }
    
    void LateUpdate()
    {
        CameraRotation();
        
        float mouseY = _inputAction.MouseView.ReadValue<Vector2>().x * Mathf.Pow(_sensitivity, 2) * mouseSpeed;

        Quaternion rotationYaw = Quaternion.Euler(0.0f, mouseY, 0.0f);
        rotationCharacter *= rotationYaw;

        _myrigid.MoveRotation(_myrigid.rotation * rotationYaw);
    }
    
    private void CameraRotation()
    {
        float _xRotation = _inputAction.MouseView.ReadValue<Vector2>().y * Mathf.Pow(_sensitivity, 2) * mouseSpeed;

        Quaternion localRotation = transform.localRotation;
        Quaternion rotationPitch = Quaternion.Euler(-_xRotation, 0.0f, 0.0f);

        //Save rotation. We use this for smooth rotation2.
        rotationCamera *= rotationPitch;

        //Local Rotation.

        localRotation *= rotationPitch;

        localRotation = Clamp(localRotation);

        transform.localRotation = localRotation;
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
}
