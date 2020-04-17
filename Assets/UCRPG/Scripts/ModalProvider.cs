using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModalProvider : MonoBehaviour
{
    [Title("Controllers")]
    public PlayerController PlayerController;
    
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public String TitleDefault;
    public String DescriptionDefault;

    public void Close()
    {
        Title.text = TitleDefault;
        Description.text = DescriptionDefault;
        this.gameObject.SetActive(false);
        PlayerController.Player.Status = Player._Status.Waiting;
    }
    
    public void ReloadScene()
    {
        Title.text = TitleDefault;
        Description.text = DescriptionDefault;
        this.gameObject.SetActive(false);
        PlayerController.Respawn();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
