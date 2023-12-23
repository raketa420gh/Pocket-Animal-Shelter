using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineBrain _cameraBrain;
    [SerializeField] CameraType _firstCamera;
    [Space] 
    [SerializeField] private VirtualCameraCase[] _virtualCameras = new VirtualCameraCase[3];

    private readonly int _activeCameraPriority = 100;
    private readonly int _deactiveCameraPriority = 0;
    private Dictionary<CameraType, int> _virtualCamerasLink;
    private Camera _mainCamera;
    private Transform _mainTarget;
    private VirtualCameraCase _activeVirtualCamera;

    public Camera MainCamera => _mainCamera;
    public Transform MainTarget => _mainTarget;
    
    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        _virtualCamerasLink = new Dictionary<CameraType, int>();
        
        for (int i = 0; i < _virtualCameras.Length; i++)
            _virtualCamerasLink.Add(_virtualCameras[i].CameraType, i);
        
        _cameraBrain.enabled = false;
        EnableCamera(_firstCamera);
    }

    public void SetMainTarget(Transform target)
    {
        _mainTarget = target;
        _cameraBrain.enabled = false;

        for (int i = 0; i < _virtualCameras.Length; i++)
        {
            _virtualCameras[i].VirtualCamera.Follow = _mainTarget;
            _virtualCameras[i].VirtualCamera.LookAt = _mainTarget;
        }

        _cameraBrain.transform.position = target.position;
        _cameraBrain.enabled = true;
    }

    public VirtualCameraCase GetCamera(CameraType cameraType)
    {
        return _virtualCameras[_virtualCamerasLink[cameraType]];
    }

    public void EnableCamera(CameraType cameraType)
    {
        if (_activeVirtualCamera != null && _activeVirtualCamera.CameraType == cameraType)
            return;
        
        if (!_cameraBrain.enabled)
            _cameraBrain.enabled = true;

        for (int i = 0; i < _virtualCameras.Length; i++)
            _virtualCameras[i].VirtualCamera.Priority = _deactiveCameraPriority;

        _activeVirtualCamera = _virtualCameras[_virtualCamerasLink[cameraType]];
        _activeVirtualCamera.VirtualCamera.Priority = _activeCameraPriority;
    }
}