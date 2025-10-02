using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private List<CinemachineCamera> cameraList = new List<CinemachineCamera>();

    private CinemachineCamera activeCam;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void SwitchCamera(int cameraIndex) {
        activeCam = cameraList[cameraIndex];
        foreach (CinemachineCamera c in cameraList) {
            if (c == activeCam) {
                c.Priority = 10;
            } else {
                c.Priority = 0;
            }
        }
    }


}
