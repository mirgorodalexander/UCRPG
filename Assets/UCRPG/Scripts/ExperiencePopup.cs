using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ExperiencePopup : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0f).OnComplete(() =>
        {
            transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.6f);
            transform.DOLocalMoveY(200, 0.6f, false).OnComplete(() =>
            {
                gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 1f);
                DOVirtual.DelayedCall(1f, () =>
                {
                    gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 0.5f);
                    transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).OnComplete(() => { Destroy(gameObject); });
                });
            });
        });
    }
}