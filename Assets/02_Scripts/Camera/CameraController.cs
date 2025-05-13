using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cmVirtualCamera;

    [Space] [Header("Mouse properties")] 
    [SerializeField] private float mouseXSems;
    [SerializeField] private float mouseYSens;
    [Space]
    [SerializeField] private float yMaxAngle;
    [SerializeField] private float xMaxAngle;
    [SerializeField] private float lerpSpeed;

    private float _mouseX;
    private float _mouseY;
    
    private float _xRotation;
    private float _yRotation;

    private Vector3 _mousePos;
    private Quaternion _camCenter = Quaternion.Euler(20,0,0);
    private Quaternion _targetRotation;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 eulerAngles = _camCenter.eulerAngles;
            _xRotation = eulerAngles.x;
            _yRotation = eulerAngles.y;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButton(1))
        {
            RotateCamera();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cmVirtualCamera.transform.rotation =
                Quaternion.Slerp(cmVirtualCamera.transform.rotation, _camCenter, lerpSpeed * Time.deltaTime);
        }
    }
    void RotateCamera()
    {
        _mouseX = Input.GetAxisRaw("Mouse X") * mouseXSems * Time.deltaTime;
        _mouseY = Input.GetAxisRaw("Mouse Y") * mouseYSens * Time.deltaTime;
        
        _yRotation += _mouseX;
        _yRotation = Mathf.Clamp(_yRotation, -xMaxAngle, xMaxAngle);
        
        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -yMaxAngle, yMaxAngle);
        
        _targetRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        cmVirtualCamera.transform.rotation =
            Quaternion.Slerp(cmVirtualCamera.transform.rotation, _targetRotation, lerpSpeed * Time.deltaTime);
    }
}
