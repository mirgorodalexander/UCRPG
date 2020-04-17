using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RenderController : MonoBehaviour
{
    [Title("Configurations")]
    public int FPS;
    public int VSYNC;
    public bool WarmUpAllShaders;
    public bool Logs;
    public bool ThirdPersonView;

    public Camera FirstPersonCamera;
    public Camera ThirdPersonCamera;

    void Awake()
    {
        Debug.unityLogger.logEnabled = Logs;
        QualitySettings.vSyncCount = VSYNC;
        Application.targetFrameRate = FPS;
        if(WarmUpAllShaders){
            Shader.WarmupAllShaders();
        }
    }

    void Start()
    {
        QualitySettings.vSyncCount = VSYNC;
        Application.targetFrameRate = FPS;
        if(WarmUpAllShaders){
            Shader.WarmupAllShaders();
        }

        FirstPersonCamera.gameObject.SetActive(!ThirdPersonView);
        ThirdPersonCamera.gameObject.SetActive(ThirdPersonView);
    }
}