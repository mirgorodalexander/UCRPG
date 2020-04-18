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
    public RenderController RenderController;
    public PlayerController PlayerController;
    public HealthController HealthController;
    public MenuController MenuController;

    [FormerlySerializedAs("Weapon")] [Title("Configurations")]
    public Weapon Weapon;
    private GameObject weapon;
    [FormerlySerializedAs("WeaponWrapper")]
    public GameObject WeaponParentFirstPerson;
    public GameObject WeaponParentThirdPerson;

    public Animator WeaponAnimator;
    public Animator PlayerAvatarAnimator;
    
    [Title("Debug")]
    public int StatePosition = 0;
    public int[] AnimationStates = new[] {2, 20, 21};
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
        if (RenderController.ThirdPersonView)
        {
            weapon.transform.SetParent(WeaponParentThirdPerson.transform);
        }
        else
        {
            weapon.transform.SetParent(WeaponParentFirstPerson.transform);
        }
        
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
                (PlayerController.Player.Status != Player._Status.Menu &&
                PlayerController.Player.Status != Player._Status.Sitting))
            {
                if (!AttackLock)
                {
                    AttackLock = true;
                    if (RenderController.ThirdPersonView)
                    {
                        float chance = PlayerController.Player.LUK/2;
                        var rndchance = Random.Range(0f, 100f);
                        if (rndchance >= 0f && rndchance <= chance)
                        {
                            Debug.Log($"[DEBUG] - Player combo attack!");
                            PlayerAvatarAnimator.SetInteger("Motion", 29);
                        }
                        else
                        {
                            PlayerAvatarAnimator.SetInteger("Motion", AnimationStates[StatePosition]);
                        }
                        
                        
                        PlayerAvatarAnimator.SetFloat("Speed", Weapon.SPD*0.03f+0.5f);
                        if (StatePosition < AnimationStates.Length-1)
                        {
                            StatePosition++;
                        }
                        else
                        {
                            StatePosition = 0;
                        }
                    }
                    else
                    {
                        WeaponAnimator.SetInteger("Motion", 1);
                        WeaponAnimator.SetFloat("Speed", Weapon.SPD*0.03f+0.5f);
                    }

                    PlayerController.Player.Status = Player._Status.Fighting;

                    DOVirtual.DelayedCall(0f, () =>
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
        Debug.Log($"[DEBUG] - Player taking on weapon.");
        WeaponAnimator.SetInteger("Motion", 2);
        WeaponParentFirstPerson.SetActive(!RenderController.ThirdPersonView);
        WeaponParentThirdPerson.SetActive(RenderController.ThirdPersonView);
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
            WeaponParentFirstPerson.SetActive(false);
        });
    }
    
    [Button("Run", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Run()
    {
        WeaponParentFirstPerson.SetActive(true);
        WeaponParentThirdPerson.SetActive(true);
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
        WeaponParentFirstPerson.SetActive(true);
        WeaponParentFirstPerson.SetActive(true);
        Debug.Log($"[DEBUG] - Player is idle with weapon.");
        WeaponAnimator.SetInteger("Motion", 0);
    }

    public bool attacking = false;

    void Start()
    {
        if (RenderController.ThirdPersonView)
        {
            defaultRotation = WeaponParentThirdPerson.transform.rotation.eulerAngles;
            WeaponParentThirdPerson.SetActive(false);
        }
        else
        {
            defaultRotation = WeaponParentFirstPerson.transform.rotation.eulerAngles;
            WeaponParentFirstPerson.SetActive(false);
        }
        
        AttackLock = false;
        Taked = false;
        
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
        
        AttackButton.gameObject.SetActive(!RenderController.ThirdPersonView);
        
        attacking = RenderController.ThirdPersonView;

        TakeOn();
    }

    void Update()
    {
        if (RenderController.ThirdPersonView)
        {
            attacking = true;
        }

        if (attacking)
        {
            if (PlayerController.Player.Status != Player._Status.Die && PlayerController.Player.Status != Player._Status.Sitting && PlayerController.Player.Status != Player._Status.Menu)
            {
                HealthController.PlayerCheck();
                this.Attack();
            }
        }
    }
}