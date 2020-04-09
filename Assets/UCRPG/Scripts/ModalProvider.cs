using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;

public class ModalProvider : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public String TitleDefault;
    public String DescriptionDefault;

    public void Close()
    {
        Title.text = TitleDefault;
        Description.text = DescriptionDefault;
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Description.gameObject.GetComponent<TextAnimatorPlayer>().ShowText(" ");
        Description.gameObject.GetComponent<TextAnimatorPlayer>().ShowText(DescriptionDefault);
    }

    private void Awake()
    {
        TitleDefault = Title.text;
        DescriptionDefault = Description.text;
    }

    void Start()
    {
    }
    
    void Update()
    {
        
    }
}
