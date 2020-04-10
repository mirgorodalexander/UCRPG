using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemController : MonoBehaviour
{
    [Title("Configurations")]
    public GameObject ItemWrapper;

    public ItemDatabase ItemDatabase;
    public GameObject ItemSpawnParent;
    public GameObject ItemSpawnPoint;
    
    [Title("Variables")]
    public GlobalVariableInt Coins;

    [Title("Controllers")]
    public MessageController MessageController;
    public PlayerController PlayerController;
    public MenuController MenuController;
    
    [Title("Preferences")]
    public float JumpPower;

    public float JumpDuration;

    [FormerlySerializedAs("Points")] [Title("Item Spawn SpawnPoints")]
    public List<GameObject> SpawnPoints;

    [Title("Item Take SpawnPoints")]
    public List<GameObject> TakePoints;

    [Title("FX")]
    public GameObject ItemsDroppedParticles;
    public GameObject ItemsDroppedGlowParticles;

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
            List<GameObject> WorkingSpawnPoints = new List<GameObject>(SpawnPoints);
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
                            
                            GameObject particle = Instantiate(ItemsDroppedGlowParticles, item.transform.position,item.transform.rotation * Quaternion.identity) as GameObject;
                            particle.SetActive(true);
                            
                            Item Item = item.AddComponent<Item>();

                            Item.ID = child.ID;
                            Item.Name = child.Name;
                            Item.Amount = child.Amount;
                            Item.Chance = child.Chance;
                            Item.Price = child.Price;
                            Item.Type = (Item._Type) child.Type;

                            item.name = child.Name;
                            item.transform.parent = ItemSpawnParent.transform;
                            particle.transform.parent = item.transform;

                            Rigidbody rb = item.AddComponent<Rigidbody>();
                            rb.useGravity = true;
                            BoxCollider bc = item.AddComponent<BoxCollider>();

                            item.transform.DOScale(0f, 0f);
                            ItemsDroppedParticles.SetActive(true);
                            item.transform.DOScale(1f, JumpDuration / 2);
                            item.transform.DOJump(SelectedSpawnPoint.transform.position, JumpPower, 1, JumpDuration,
                                    false)
                                .SetEase(Ease.Linear).OnComplete(() => { ItemsDroppedParticles.SetActive(false); });
                            Items.Add(item);

                            Debug.Log($"[DEBUG] - Item \"{child.Name}\" successful dropped by \"{Enemy.name}\"");
                        }
                    }
                }
            }

            if(PlayerController.Player.Status != Player._Status.Die){
                virtualTween = DOVirtual.DelayedCall(JumpDuration+0.5f, () => { this.AddToInventory(); });
            }
        }
    }

    [Button("Add To Inventory", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void AddToInventory()
    {
        List<GameObject> WorkingTakePoints = new List<GameObject>(TakePoints);
        for (int i = 0; i < Items.Count; i++)
        {
            GameObject WorkingItem = Items[i];
            Item Item = WorkingItem.GetComponent<Item>();

            if (WorkingTakePoints.Count == 0)
            {
                WorkingTakePoints = new List<GameObject>(TakePoints);
            }

            int rnd = Random.Range(0, WorkingTakePoints.Count);
            GameObject SelectedTakePoint = WorkingTakePoints[rnd];
            WorkingTakePoints.RemoveAt(rnd);

            Debug.Log($"[DEBUG] Taking item \"{WorkingItem.name} - [{Item.ID}]\".");
            WorkingItem.transform.DORotate(new Vector3(2280f, 2280f, 2280f), 0.5f).SetEase(Ease.Linear);
            WorkingItem.transform
                .DOPath(new[]
                {
                    new Vector3(0f, 0.6801552f, 0.5986252f),
                    SelectedTakePoint.transform.position
                }, 0.5f, PathType.CatmullRom, PathMode.Full3D, 10, Color.red)
                .SetDelay(i * 0.2f)
                .SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    WorkingItem.transform.DOScale(0f, 0.6f).SetEase(Ease.Linear);
                })
                .OnComplete(() =>
                {
                    if (Item.Name.Contains("Coin"))
                    {
                        int rndcoins = Random.Range(-5, 5);
                        Coins.Value += (Item.Amount+rndcoins);
                        MenuController.UpdateInGameUI();
                        MessageController.ConsolePopup($"You got item \"{Item.Name}\" x {Item.Amount+rndcoins}");
                    }
                    else
                    {
                        if (Player.Inventory.Count > 0)
                        {
                            for(int j = 0; j < Player.Inventory.Count; j++)
                            {
                                if (Player.Inventory[j].ID == Item.ID)
                                {
                                    Player.Inventory[j].Amount += Item.Amount;
                                }
                                else
                                {
                                    Player.Inventory.Add(new Player.InventoryItem
                                        {ID = Item.ID, Name = Item.Name, Amount = Item.Amount});
                                }
                            }
                        }
                        else
                        {
                            Player.Inventory.Add(new Player.InventoryItem {ID = Item.ID, Name = Item.Name, Amount = Item.Amount});
                        }

                        MessageController.ConsolePopup($"You got item \"{Item.Name}\" x {Item.Amount}");
                    }
                    
                    
                    Debug.Log($"[DEBUG] - Item \"{Item.Name} - [{Item.ID}]\" added to players inventory.");
                    DestroyImmediate(WorkingItem.gameObject);
                    Items.Remove(WorkingItem);
                });
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

//    [Button("Getting Item Animation", ButtonSizes.Large), GUIColor(1, 1, 1)]
//    private void GettingItemAnimation()
//    {
//        List<GameObject> WorkingTakePoints = new List<GameObject>(TakePoints);
//        for (int i = 0; i < Items.Count; i++)
//        {
//            if (WorkingTakePoints.Count == 0)
//            {
//                WorkingTakePoints = new List<GameObject>(TakePoints);
//            }
//
//            int rnd = Random.Range(0, WorkingTakePoints.Count);
//            GameObject SelectedTakePoint = WorkingTakePoints[rnd];
//            WorkingTakePoints.RemoveAt(rnd);
//            Items[i].transform
//                .DOPath(new[]
//                {
//                    new Vector3(0f, 0.6801552f, 0.5986252f),
//                    SelectedTakePoint.transform.position
//                }, 0.5f, PathType.CatmullRom, PathMode.Full3D, 10, Color.red)
//                .SetEase(Ease.Linear)
//                .OnComplete(() =>
//                {
//                    DestroyImmediate(Items[i].gameObject);
//                });
//        }
//    }

    void Start()
    {
        ItemsDroppedParticles.SetActive(false);
        ItemsDroppedGlowParticles.SetActive(false);
    }

    void Update()
    {
    }
}