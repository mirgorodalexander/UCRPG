using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RenderController : MonoBehaviour
{
    [Title("Configurations")]
    public int FPS;
    public int VSYNC;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Shader.WarmupAllShaders();
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Shader.WarmupAllShaders();
    }
}