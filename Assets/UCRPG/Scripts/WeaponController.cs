using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Title("Controllers")]
    public EnemyController EnemyController;

    public PlayerController PlayerController;
    public HealthController HealthController;

    [Title("Configurations")]
    public GameObject Weapon;

    public GameObject WeaponWrapper;

    public Animator WeaponAnimator;

//    public GameObject EnemyAttackedLight;
//    public GameObject EnemyAttackedParticles;
//    public GameObject WeaponAttackParticles;
//    public GameObject DamagePopupPrefab;
//    public GameObject DamagePopupCanvas;
    public float AttackSpeed;

    [Title("Points")]
    public Transform[] AttackPoints;

    [Title("Debug")]
    public bool AttackLock = false;
    public bool Taked = false;

    private Vector3 defaultRotation;

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
                    WeaponAnimator.SetInteger("Motion", 1);

                    PlayerController.Player.Status = Player._Status.Fighting;

                    DOVirtual.DelayedCall(0.4f, () =>
                    {
                        // If enemy is neutral but you are attacking
                        if (EnemyController.Enemy.Status == Enemy._Status.Waiting)
                        {
                            EnemyController.Enemy.Status = Enemy._Status.Fighting;
                            EnemyController.Attack();
                        }
                    });

                    Debug.Log($"[DEBUG] - ATTACKED!");

//                    Vector3 DefaultPosition = Weapon.transform.position;
//                    Vector3[] waypoints = new Vector3[AttackPoints.Length];
//
//                    for (int i = 0; i < AttackPoints.Length; i++)
//                    {
//                        waypoints[i] = AttackPoints[i].position;
//                    }

//                    Weapon.transform.DORotate(new Vector3(-60.0f, 0.0f, -10.0f), AttackSpeed / 2).OnComplete(() =>
//                    {
//                        Weapon.transform.DORotate(new Vector3(110f, 0.0f, 45.0f), AttackSpeed / 4).OnComplete(() =>
//                        {
//                            Weapon.transform.DORotate(new Vector3(30f, 0.0f, 45.0f), AttackSpeed / 2)
//                                .OnComplete(() =>
//                                {
//                                    Weapon.transform.DORotate(defaultRotation, AttackSpeed / 2)
//                                        .OnComplete(() => { });
//                                });
//                        });
//
//                        Weapon.transform
//                            .DOPath(waypoints, AttackSpeed, PathType.Linear)
//                            .SetDelay(0)
//                            //.SetEase(Ease.Linear)
//                            .SetOptions(true)
//                            .OnWaypointChange((int point) =>
//                            {
//                                Debug.Log($"[DEBUG] - {point}!");
//                                if (point == ((waypoints.Length / 2) + 1))
//                                {
//                                    // If enemy is neutral but you are attacking
//                                    if (EnemyController.Enemy.Status == Enemy._Status.Waiting)
//                                    {
//                                        EnemyController.Enemy.Status = Enemy._Status.Fighting;
//                                        EnemyController.Attack();
//                                    }
//                                    
//                                    Debug.Log($"[DEBUG] - ATTACKED!");
//
////                                    DamagePopup(Random.Range(5, 20));
////                                    HealthController.EnemyDamage(PlayerController.Player.ATK);
////                                    EnemyAttackedLight.SetActive(true);
////                                    WeaponAttackParticles.SetActive(true);
////                                    EnemyAttackedParticles.SetActive(true);
////
////                                    DOVirtual.DelayedCall(0.06f, () => { EnemyAttackedLight.SetActive(false); });
////                                    DOVirtual.DelayedCall(0.2f, () =>
////                                    {
////                                        WeaponAttackParticles.SetActive(false);
////                                        EnemyAttackedParticles.SetActive(false);
////                                    });
//                                }
//                            })
//                            .OnStart(() => Debug.Log($"[DEBUG] - Weapon controller attack begin."))
//                            .OnPlay(() => { })
//                            .OnComplete(() =>
//                            {
//                                Debug.Log($"[DEBUG] - Weapon controller attack end.");
//                                AttackLock = false;
//                            });
//                    });
                }
            }
        }
    }

    [Button("TakeOn", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void TakeOn()
    {
        Taked = true;
        Weapon.SetActive(false);
        Debug.Log($"[DEBUG] - Player taking on weapon.");
        WeaponAnimator.SetInteger("Motion", 2);
        Weapon.SetActive(true);
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
        Weapon.SetActive(true);
        Debug.Log($"[DEBUG] - Player taking off weapon.");
        WeaponAnimator.SetInteger("Motion", 3);
        DOVirtual.DelayedCall(0.28f, () =>
        {
            AttackLock = false;
            WeaponAnimator.SetInteger("Motion", 0);
            Weapon.SetActive(false);
        });
    }
    [Button("Run", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Run()
    {
        Weapon.SetActive(true);
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
        Weapon.SetActive(true);
        Debug.Log($"[DEBUG] - Player is idle with weapon.");
        WeaponAnimator.SetInteger("Motion", 0);
    }

//    [Button("Damage Popup", ButtonSizes.Large), GUIColor(1, 1, 1)]
//    public void DamagePopup(int damage)
//    {
//        GameObject Damage =
//            Instantiate(DamagePopupPrefab, DamagePopupCanvas.transform.position, Quaternion.identity) as GameObject;
//        Damage.transform.parent = DamagePopupCanvas.transform;
//        Damage.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = $"{damage}";
//        Damage.name = $"Damage - {damage}";
//        DOVirtual.DelayedCall(1f, () => { DestroyImmediate(Damage.gameObject); });
//    }

    void Start()
    {
        defaultRotation = Weapon.transform.rotation.eulerAngles;
        AttackLock = false;
        Taked = false;
        Weapon.SetActive(false);
//        EnemyAttackedLight.SetActive(false);
//        WeaponAttackParticles.SetActive(false);
//        EnemyAttackedParticles.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            this.Attack();
        }
    }
}