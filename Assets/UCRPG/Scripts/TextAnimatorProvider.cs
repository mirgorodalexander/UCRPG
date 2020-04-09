using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;

public class TextAnimatorProvider : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextAnimatorPlayer>().ShowText(" ");
        GetComponent<TextAnimatorPlayer>().ShowText("ASSSSS");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<TextAnimatorPlayer>().ShowText("ASSSSS");
        }
    }
}
