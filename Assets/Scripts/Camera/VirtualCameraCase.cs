using Cinemachine;
using UnityEngine;

[System.Serializable]
public class VirtualCameraCase
{
    [SerializeField] CameraType cameraType;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    public CameraType CameraType => cameraType;
    public CinemachineVirtualCamera VirtualCamera => virtualCamera;
}