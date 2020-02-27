using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [Title("Configurations")]
    public GameObject ItemWrapper;

    public ItemDatabase ItemDatabase;
    public GameObject ItemSpawnParent;
    public GameObject ItemSpawnPoint;

    [Title("Preferences")]
    public float JumpPower;

    public float JumpDuration;

    [Title("Item Spawn Points")]
    public List<GameObject> Points;

    [Title("FX")]
    public GameObject ItemsDroppedParticles;


    [Title("Runtime")]
    public List<GameObject> Items;

    public Player Player;
    public Enemy Enemy;

    private GameObject item;
    private Tween virtualTween;

    [Title("Buttons")]
    [Button("Drop Item", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Drop()
    {
        if (Enemy != null)
        {
            virtualTween.Kill();
            this.Remove();
            List<GameObject> WorkingSpawnPoints = new List<GameObject>(Points);
            foreach (var id in Enemy.ItemID)
            {
                int rnd = Random.Range(0, WorkingSpawnPoints.Count);
                GameObject SelectedSpawnPoint = WorkingSpawnPoints[rnd];
                WorkingSpawnPoints.RemoveAt(rnd);
                foreach (var child in ItemDatabase.Items)
                {
                    if (child.ID == id)
                    {
                        float chance = child.Chance;
                        float rndchance = Random.Range(0f, 100f);
                        Debug.Log(
                            $"[DEBUG] - Trying drop item \"{child.Name} - [{child.ID}]\" with chance \"{chance}\", you throw \"{rndchance}\"");
                        if (rndchance >= 0f && rndchance <= chance)
                        {
                            item = Instantiate(
                                child.Prefab,
                                ItemSpawnPoint.transform.position,
                                ItemSpawnPoint.transform.rotation *
                                Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f))
                            ) as GameObject;

                            Item Item = item.AddComponent<Item>();

                            Item.Chance = child.Chance;
                            Item.Price = child.Price;
                            Item.Type = (Item._Type) child.Type;

                            item.name = child.Name;
                            item.transform.parent = ItemSpawnParent.transform;

                            // Adding components in runtime
                            Button bt = item.AddComponent<Button>();
                            bt.onClick.AddListener(() =>
                            {
                                Debug.Log($"[DEBUG] Taking item \"{item.name} - [{id}]\".");
                                this.AddToInventory(id);
                            });
                            Rigidbody rb = item.AddComponent<Rigidbody>();
                            rb.useGravity = true;
                            BoxCollider bc = item.AddComponent<BoxCollider>();

                            item.transform.DOScale(new Vector3(0f, 0f, 0f), 0f);
                            ItemsDroppedParticles.SetActive(true);
                            item.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), JumpDuration / 2);
                            item.transform.DOJump(SelectedSpawnPoint.transform.position, JumpPower, 1, JumpDuration,
                                    false)
                                .SetEase(Ease.Linear).OnComplete(() => { ItemsDroppedParticles.SetActive(false); });
                            Items.Add(item);

                            Debug.Log($"[DEBUG] - Item \"{child.Name}\" successful dropped by \"{Enemy.name}\"");
                        }
                    }
                }
            }

            Debug.Log($"[DEBUG] - Items will be disappear after 30 seconds.");

            virtualTween = DOVirtual.DelayedCall(30f, () => { this.Remove(); });
        }
    }

    [Button("Add To Inventory", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void AddToInventory(int ID)
    {
        Player.Inventory.Add(ID);
        if (Items[ID] != null)
        {
            Debug.Log($"[DEBUG] - Adding item \"{Items[ID].name} - [{ID}]\" to players inventory.");
            DestroyImmediate(Items[ID].gameObject);
            Items.RemoveAt(ID);
        }
    }

    [Button("Remove Item", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Remove()
    {
        virtualTween.Kill();
        if (Items.Count > 0)
        {
            foreach (var item in Items)
            {
                Debug.Log($"[DEBUG] - Removing item \"{item.name}\".");
                DestroyImmediate(item.gameObject);
            }

            Items = new List<GameObject>();
        }
    }

    void Start()
    {
        ItemsDroppedParticles.SetActive(false);
    }

    void Update()
    {
    }
}