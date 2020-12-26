using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera cv;

    private void Start()
    {
        cv = GetComponent<CinemachineVirtualCamera>();
        if (cv != null)
        {
            cv.m_Follow = GameManager.instance.player.transform;
        }
    }
}
