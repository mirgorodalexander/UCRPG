using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    [Title("Configurations")]
    public bool SaveOnUpdate;


    void Start()
    {
    }

    void Update()
    {
        if (SaveOnUpdate)
        {
        }
    }
}