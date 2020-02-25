using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextFadeInOutEffect : MonoBehaviour
{
    public float fadeSpeed = 1.0f;
    public float alphaMax = 1.0f;
    public float alphaMin = 0.0f;
    private bool faded = false;
    private float alpha;
    private Text text;

    void Start()
    {
        alpha = 1.0f;
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (!faded)
        {
            if (alpha > alphaMin)
            {
                alpha -= fadeSpeed / 100f;
            }
            else
            {
                faded = true;
            }
        }
        else
        {
            if (alpha < alphaMax)
            {
                alpha += fadeSpeed / 100f;
            }
            else
            {
                faded = false;
            }
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }
}