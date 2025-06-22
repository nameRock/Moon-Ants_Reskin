using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposeCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCameraOne;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCameraTwo;

    private float _PCDistance = 31f;
    private float _mobileDistance = 40f;

    private void Start()
    {
        if (Application.isMobilePlatform)
        {
            _cinemachineVirtualCameraOne.m_Lens.FieldOfView = _mobileDistance;
            _cinemachineVirtualCameraTwo.m_Lens.FieldOfView = _mobileDistance;
        }
        else
        {
            _cinemachineVirtualCameraOne.m_Lens.FieldOfView = _PCDistance;
            _cinemachineVirtualCameraTwo.m_Lens.FieldOfView = _PCDistance;
        }
    }
}
