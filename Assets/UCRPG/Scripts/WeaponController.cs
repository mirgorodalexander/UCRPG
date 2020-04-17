using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [Title("Preferences")]
    public Button AttackButton;
    
    [Title("Database")]
    public WeaponDatabase WeaponDatabase;
    
    [Title("Controllers")]
    public EnemyController EnemyController;
    public PlayerController PlayerController;
    public HealthController HealthController;

    [FormerlySerializedAs("Weapon")] [Title("Configurations")]
    public Weapon Weapon;
    private GameObject weapon;
    [FormerlySerializedAs("WeaponWrapper")]
    public GameObject WeaponParent;

    public Animator WeaponAnimator;
    public Animator PlayerAvatarAnimator;
    
    [Title("Debug")]
    public bool AttackLock = false;
    public bool Taked = false;

    private Vector3 defaultRotation;


    [Title("Buttons")]
    [Button("Equip", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Equip(int WeaponID)
    {
        if (weapon != null)
        {
            DestroyImmediate(weapon);
        }
        
        weapon = Instantiate(WeaponDatabase.Items[WeaponID].Prefab, WeaponDatabase.Items[WeaponID].Prefab.transform.position, WeaponDatabase.Items[WeaponID].Prefab.transform.rotation) as GameObject;
        
        Weapon = weapon.AddComponent<Weapon>();

        Weapon.ATK = WeaponDatabase.Items[WeaponID].ATK;
        Weapon.SPD = WeaponDatabase.Items[WeaponID].SPD;
        
        weapon.name = WeaponDatabase.Items[WeaponID].Name;
        weapon.transform.SetParent(WeaponParent.transform);
        
        weapon.transform.position = new Vector3(0, 0, 0);
        weapon.transform.localPosition = new Vector3(0, 0, 0);
        weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 345, 0));
        weapon.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        PlayerController.Player.WID = WeaponID;
    }
    
    [Title("Buttons")]
    [Button("Attack", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Attack()
    {
        if (EnemyController.Enemy != null)
        {
            if ((EnemyController.Enemy.Status == Enemy._Status.Waiting ||
                EnemyController.Enemy.Status == Enemy._Status.Fighting) &&
                PlayerController.Player.Status != Player._Status.Menu)
            {
                if (!AttackLock)
                {
                    AttackLock = true;
                    //WeaponAnimator.SetInteger("Motion", 1);
                    PlayerAvatarAnimator.SetInteger("Motion", 2);
                    //WeaponAnimator.SetFloat("Speed", Weapon.SPD*0.03f+0.5f);
                    PlayerAvatarAnimator.SetFloat("Speed", Weapon.SPD*0.03f+0.5f);

                    PlayerController.Player.Status = Player._Status.Fighting;

                    DOVirtual.DelayedCall(0.4f, () =>
                    {
                        // If enemy is neutral but you are attacking
                        if (EnemyController.Enemy.Status == Enemy._Status.Waiting)
                        {
                            EnemyController.Enemy.Status = Enemy._Status.Fighting;
                            EnemyController.Attack();
                            PlayerController.MenuController.ShowMenuButton.interactable = PlayerController.Player.Status != Player._Status.Fighting;
                        }
                    });

                    Debug.Log($"[DEBUG] - ATTACKED!");
                }
            }
        }
    }

    [Button("TakeOn", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void TakeOn()
    {
        Taked = true;
        WeaponParent.SetActive(false);
        Debug.Log($"[DEBUG] - Player taking on weapon.");
        WeaponAnimator.SetInteger("Motion", 2);
        WeaponParent.SetActive(true);
        DOVirtual.DelayedCall(0.28f, () =>
        {
            AttackLock = false;
            WeaponAnimator.SetInteger("Motion", 0);
        });
    }

    [Button("TakeOff", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void TakeOff()
    {
        Taked = false;
        //WeaponParent.SetActive(true);
        Debug.Log($"[DEBUG] - Player taking off weapon.");
        WeaponAnimator.SetInteger("Motion", 3);
        DOVirtual.DelayedCall(0.28f, () =>
        {
            AttackLock = false;
            WeaponAnimator.SetInteger("Motion", 0);
            WeaponParent.SetActive(false);
        });
    }
    
    [Button("Run", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Run()
    {
        WeaponParent.SetActive(true);
        Debug.Log($"[DEBUG] - Player is runnig with weapon.");
        WeaponAnimator.SetInteger("Motion", 4);
        DOVirtual.DelayedCall(0.6f, () =>
        {
            AttackLock = false;
            WeaponAnimator.SetInteger("Motion", 0);
            //Weapon.SetActive(false);
        });
    }
    
    [Button("Idle", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Idle()
    {
        WeaponParent.SetActive(true);
        Debug.Log($"[DEBUG] - Player is idle with weapon.");
        WeaponAnimator.SetInteger("Motion", 0);
    }

    public bool attacking = false;

    void Start()
    {
        defaultRotation = WeaponParent.transform.rotation.eulerAngles;
        AttackLock = false;
        Taked = false;
        WeaponParent.SetActive(false);
        
        EventTrigger trigger = AttackButton.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => attacking = true);
        trigger.triggers.Add(pointerDown);
        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => attacking = false);
        trigger.triggers.Add(pointerUp);
        
        Equip(PlayerController.Player.WID);
        attacking = true;
        TakeOn();
    }

    void Update()
    {
        attacking = true;
        if (attacking)
        {
            if (PlayerController.Player.Status != Player._Status.Die)
            {
                this.Attack();
            }
        }
    }
}