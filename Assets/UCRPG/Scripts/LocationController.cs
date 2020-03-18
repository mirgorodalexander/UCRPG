using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class LocationController : MonoBehaviour
{
    [Title("Configurations")]
    public GameObject LocationSpawnParent;

    public float MovementSpeed;

    [Title("Controllers")]
    public WeaponController WeaponController;

    public PlayerController PlayerController;
    public EnemyController EnemyController;
    public ItemController ItemController;

    [Title("Databases")]
    public LocationDatabase LocationDatabase;

    public GameObject location;

    private GameObject StackinglocationMiddle;
    private GameObject StackinglocationEnd;

    private bool locationInitialize;

    [Title("Buttons")]
    [Button("Spawn Location", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Spawn(int locationID)
    {
        if (!locationInitialize)
        {
            PlayerController.Player.LID = 1;
            locationInitialize = true;
        }

        if (PlayerController.Player.LID != locationID)
        {
            if (EnemyController.Enemy != null)
            {
                EnemyController.Remove();
                DOVirtual.DelayedCall(0.01f, () => { EnemyController.Spawn(); });
            }

            if (location != null)
            {
                Destroy(location.gameObject);
                //Destroy(location.gameObject);
            }

            Debug.Log($"[DEBUG] - Spawning location with id \"{locationID}\".");

            if (locationID <= LocationDatabase.Locations.Count - 1)
            {

                location = Instantiate(LocationDatabase.Locations[locationID].Prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

                location.name = LocationDatabase.Locations[locationID].Name;
                location.transform.parent = LocationSpawnParent.transform;
                location.transform.localPosition = new Vector3(0, 0, 0);
                location.SetActive(true);

                PlayerController.Player.LID = locationID;
                EnemyController.LocationEnemies = LocationDatabase.Locations[locationID].EnemyID;
            }
        }
    }

    [Title("Buttons")]
    [Button("Move", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Move()
    {
        //WeaponController.Run();
        Debug.Log($"[DEBUG] - Player move begin.");
        StackinglocationMiddle = Instantiate(location, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        StackinglocationMiddle.name = "Stacking location Middle";

        StackinglocationEnd = Instantiate(location, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        StackinglocationEnd.name = "Stacking location End";

        StackinglocationMiddle.transform.SetParent(location.transform.parent.transform);
        StackinglocationEnd.transform.SetParent(location.transform.parent.transform);

        float move = Random.Range(2f, 2f);

        StackinglocationMiddle.transform.localPosition = new Vector3(0f, 0f, 0f);
        StackinglocationEnd.transform.localPosition = new Vector3(0f, 0f, move);

        location.SetActive(false);
        StackinglocationMiddle.transform
            .DOLocalMove(new Vector3(0, 0, move * -1f), MovementSpeed, false)
            .SetEase(Ease.InOutQuad);

        StackinglocationEnd.transform
            .DOMove(location.transform.position, MovementSpeed, false)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                location.SetActive(true);
                this.Remove();

                Debug.Log($"[DEBUG] - Player move end.");
                EnemyController.PlayerReady();
            });
    }

    [Button("Remove", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Remove()
    {
        DestroyImmediate(StackinglocationMiddle.gameObject);
        DestroyImmediate(StackinglocationEnd.gameObject);
    }

    void Start()
    {
        this.Spawn(PlayerController.Player.LID);
    }

    void Update()
    {
    }
}