using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class LocationController : MonoBehaviour
{
    [Title("Configurations")]
    public float MovementSpeed;

    public GameObject Ground;

    private GameObject StackingGroundMiddle;
    private GameObject StackingGroundEnd;

    [Title("Buttons")]
    [Button("Move", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Move()
    {
        Debug.Log($"[DEBUG] - Player move begin.");
        StackingGroundMiddle = Instantiate(Ground, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        StackingGroundMiddle.name = "Stacking Ground Middle";

        StackingGroundEnd = Instantiate(Ground, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        StackingGroundEnd.name = "Stacking Ground End";

        StackingGroundMiddle.transform.SetParent(Ground.transform.parent.transform);
        StackingGroundEnd.transform.SetParent(Ground.transform.parent.transform);

        float move = Random.Range(2f, 2f);

        StackingGroundMiddle.transform.localPosition = new Vector3(0f, 0f, 0f);
        StackingGroundEnd.transform.localPosition = new Vector3(0f, 0f, move);

        Ground.SetActive(false);
        StackingGroundMiddle.transform
            .DOLocalMove(new Vector3(0, 0, move * -1f), MovementSpeed, false)
            .SetEase(Ease.InOutQuad);

        StackingGroundEnd.transform
            .DOMove(Ground.transform.position, MovementSpeed, false)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                Ground.SetActive(true);
                this.Remove();
            });

        Debug.Log($"[DEBUG] - Player move end.");
    }

    [Button("Remove", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Remove()
    {
        DestroyImmediate(StackingGroundMiddle.gameObject);
        DestroyImmediate(StackingGroundEnd.gameObject);
    }

    void Start()
    {
    }

    void Update()
    {
    }
}