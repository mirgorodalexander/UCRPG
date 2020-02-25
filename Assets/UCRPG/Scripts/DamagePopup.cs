using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f);
        transform.DOLocalMoveY(120, 0.3f, false).OnComplete(() =>
        {
            gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 0.5f);
            transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f);
            transform.DOLocalMoveY(-1000, 1f, false).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }
}
