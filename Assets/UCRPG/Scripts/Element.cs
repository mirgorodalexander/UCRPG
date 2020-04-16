using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class Element : MonoBehaviour
{
    [Title("Images")]
    public Image Icon;
    public Image CoinShadow;
    public Image CoinIcon;
    
    [Title("Texts")]
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Locked;
    public TextMeshProUGUI Opened;
    public TextMeshProUGUI Owned;

    [Title("Button")]
    public Button Button;

    [Title("Other")]
    public GameObject Prefab;
    public TextMeshProUGUI PowerValue;
    public TextMeshProUGUI SpeedValue;

    public Transform Parent;
    public bool previousState;
    public bool isFullyVisible;

    void Start()
    {
        RectTransform = this.gameObject.GetComponent<RectTransform>();
        // isFullyVisible = true;
        // previousState = false;
        // CanvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }

    private RectTransform RectTransform;

    public static void iterateChildren(CanvasGroup canvas, Transform t, bool isVisible)
    {
        if (isVisible)
        {
            if (canvas.alpha == 0)
            {
                canvas.alpha = 1;
            }
        }
        else
        {
            if (canvas.alpha == 1)
            {
                canvas.alpha = 0;
            }
        }

        // if (t.gameObject.GetComponent<Image>() != null && !t.gameObject.GetComponent<Image>().enabled != isVisible)
        // {
        //     t.gameObject.GetComponent<Image>().enabled = isVisible;
        // }
        // if (t.gameObject.GetComponent<ProceduralImage>() != null &&
        //     !t.gameObject.GetComponent<ProceduralImage>().enabled != isVisible)
        // {
        //     t.gameObject.GetComponent<ProceduralImage>().enabled = isVisible;
        // }
        // if (t.gameObject.GetComponent<TextMeshProUGUI>() != null &&
        //     !t.gameObject.GetComponent<TextMeshProUGUI>().enabled != isVisible)
        // {
        //     t.gameObject.GetComponent<TextMeshProUGUI>().enabled = isVisible;
        // }

        // foreach (Transform child in t)
        // {
        //     if (isVisible)
        //     {
        //         if (child.gameObject.AddComponent<CanvasGroup>().alpha == 0)
        //         {
        //             child.gameObject.AddComponent<CanvasGroup>().alpha = 1;
        //         }
        //     }
        //     else
        //     {
        //         if (child.gameObject.AddComponent<CanvasGroup>().alpha == 1)
        //         {
        //             child.gameObject.AddComponent<CanvasGroup>().alpha = 0;
        //         }
        //     }
        //     if (child.childCount > 0)
        //     {
        //         iterateChildren(canvas, child.transform, isVisible);
        //     }
        // }

    }

    private float delay = 0f;
    void Update()
    {
        // delay += Time.deltaTime;
        // if (delay > 0.1f)
        // {
        //     isFullyVisible = RectTransform.IsFullyVisibleFrom(Camera.allCameras[1]);
        //     if (isFullyVisible != previousState)
        //     {
        //         previousState = isFullyVisible;
        //         if(Parent.childCount > 0){
        //             Parent.gameObject.SetActive(isFullyVisible);
        //         }
        //     }
        //     delay = 0f;
        // }
    }
}