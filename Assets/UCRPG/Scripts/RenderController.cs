using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RenderController : MonoBehaviour
{
    [Title("Configurations")]
    public int FPS;
    public int VSYNC;
    public bool Logs;

    void Awake()
    {
        Debug.unityLogger.logEnabled = Logs;
        QualitySettings.vSyncCount = VSYNC;
        Application.targetFrameRate = FPS;
        Shader.WarmupAllShaders();
    }

    void Start()
    {
        QualitySettings.vSyncCount = VSYNC;
        Application.targetFrameRate = FPS;
        Shader.WarmupAllShaders();
    }
}