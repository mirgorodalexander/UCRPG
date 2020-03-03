﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Title("Run")]
    public bool _Spawn;

    public bool _Live;

    [Title("Configurations")]
    public GameObject EnemyWrapper;

    public EnemyDatabase EnemyDatabase;
    public GameObject EnemySpawnParent;
    public Transform EnemySpawnPoint;

    [Title("Controllers")]
    public LocationController LocationController;
    public HealthController HealthController;
    public ItemController ItemController;
    public AnimatorProvider AnimatorProvider;

    public float CompleteMoveInSeconds;
    public bool IsTrigger = false;

    [Title("Moving Points")]
    public Transform[] Points;

    private GameObject enemy;
    private GameObject item;
    private Tween tween;
    private Tween virtualTween;
    private bool paused = false;

    private Vector3 enemyWrapperDefaultPosition;
    private Vector3 enemyWrapperDefaultRotation;

    [Title("Current Enemy")]
    public Enemy Enemy;

    [Title("Buttons")]
    [Button("Spawn Enemy", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Spawn()
    {
        int rnd = Random.Range(0, EnemyDatabase.Enemies.Count);

        enemy = Instantiate(
            EnemyDatabase.Enemies[rnd].Prefab,
            EnemySpawnPoint.transform.position,
            EnemyDatabase.Enemies[rnd].Prefab.transform.rotation
        ) as GameObject;

        Enemy Enemy = enemy.AddComponent<Enemy>();

        Enemy.LVL = EnemyDatabase.Enemies[rnd].LVL;
        Enemy.BEXP = EnemyDatabase.Enemies[rnd].BEXP;
        Enemy.JEXP = EnemyDatabase.Enemies[rnd].JEXP;
        Enemy.HP = EnemyDatabase.Enemies[rnd].HP;
        Enemy.MP = EnemyDatabase.Enemies[rnd].MP;
        Enemy.ATK = EnemyDatabase.Enemies[rnd].ATK;
        Enemy.ATKD = EnemyDatabase.Enemies[rnd].ATKD;
        Enemy.DEF = EnemyDatabase.Enemies[rnd].DEF;
        Enemy.MOD = (Enemy._MOD) EnemyDatabase.Enemies[rnd].MOD;
        Enemy.ItemID = EnemyDatabase.Enemies[rnd].ItemID;
        

        enemy.name = EnemyDatabase.Enemies[rnd].Name;
        enemy.transform.parent = EnemySpawnParent.transform;

        this.Enemy = Enemy;
        HealthController.Enemy = Enemy;
        ItemController.Enemy = Enemy;
        AnimatorProvider = Enemy.GetComponent<AnimatorProvider>();

        Enemy.Status = Enemy._Status.Init;
        enemy.transform.localPosition = new Vector3(0, 0, 0);

        Enemy.transform.DORotate(
            new Vector3(Enemy.transform.rotation.eulerAngles.x, Enemy.transform.rotation.eulerAngles.y, 0f), 0.01f);
        Enemy.transform.DOLocalRotate(
            new Vector3(Enemy.transform.rotation.eulerAngles.x, Enemy.transform.rotation.eulerAngles.y, 0f), 0.01f);
        //EnemyWrapper.GetComponent<Rigidbody>().DORotate(new Vector3(0f, 0f, 0f), 0.01f);

        Debug.Log($"[DEBUG] - Spawn enemy \"{enemy.name}\".");
        
        if(Enemy.GetComponent<Animator>() != null){
            Enemy.GetComponent<Animator>().SetInteger("Motion", 0);
        }

        if (_Live)
        {
            Debug.Log($"[DEBUG] - Position \"{EnemySpawnPoint.transform.position}\".");
            virtualTween = DOVirtual.DelayedCall(0f, () => { this.Live(); });
        }
    }

    [Button("Remove Enemy", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Remove()
    {
        Debug.Log($"[DEBUG] - Removing enemy \"{enemy.name}\".");
        DestroyImmediate(enemy.gameObject);
    }

    [Button("Enemy Live", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Live()
    {
        if(Enemy.GetComponent<Animator>() != null){
            Enemy.GetComponent<Animator>().SetInteger("Motion", 0);
        }
        Debug.Log($"[DEBUG] - Enemy \"{Enemy.gameObject.name}\" begin live.");
        //LocationController.Move();
        virtualTween = DOVirtual.DelayedCall(0f, () => { this.WalkIn(); });
    }

    [Button("Walk In", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void WalkIn()
    {
        Enemy.transform.DORotate(
            new Vector3(Enemy.transform.rotation.eulerAngles.x, Enemy.transform.rotation.eulerAngles.y, 0f), 0.01f);
        Enemy.transform.DOLocalRotate(
            new Vector3(Enemy.transform.rotation.eulerAngles.x, Enemy.transform.rotation.eulerAngles.y, 0f), 0.01f);
        EnemyWrapper.GetComponent<Rigidbody>().DORotate(new Vector3(0f, 0f, 0f), 0.01f);
        tween = EnemyWrapper.transform
            .DOPath(new[]
                {
                    Points[0].position,
                    Points[1].position
                },
                CompleteMoveInSeconds,
                PathType.Linear)
            .SetDelay(0)
            .SetEase(Ease.Linear)
            .SetOptions(false)
            .OnWaypointChange((int point) => { })
            .OnStart(() =>
            {
                Debug.Log($"[DEBUG] - Enemy \"{Enemy.gameObject.name}\" walk in begin.");
                Enemy.Status = Enemy._Status.Moving;
            })
            .OnPlay(() =>
            {
                //EnemyWrapper.GetComponent<Animator>().SetInteger("Motion", 1);
                if(Enemy.GetComponent<Animator>() != null){
                    Enemy.GetComponent<Animator>().SetInteger("Motion", 1);
                }
            })
            .OnComplete(() =>
            {
                //EnemyWrapper.GetComponent<Animator>().SetInteger("Motion", 0);
                if(Enemy.GetComponent<Animator>() != null){
                    Enemy.GetComponent<Animator>().SetInteger("Motion", 0);
                }
                
                Debug.Log($"[DEBUG] - Enemy \"{Enemy.gameObject.name}\" walk in end.");
                Enemy.Status = Enemy._Status.Waiting;

                if (Enemy.MOD == global::Enemy._MOD.Aggresive)
                {
                    Debug.Log($"[DEBUG] - Enemy \"{Enemy.gameObject.name}\" is aggresive");

                    virtualTween = DOVirtual.DelayedCall(1, () =>
                    {
                        Enemy.Status = Enemy._Status.Fighting;

                        this.Attack();
                    });
                }

                if (Enemy.MOD == global::Enemy._MOD.Neutral)
                {
                    int rnd = Random.Range(5, 10);

                    Debug.Log($"[DEBUG] - Enemy \"{Enemy.gameObject.name}\" is neutral.");
                    Debug.Log($"[DEBUG] - Enemy \"{Enemy.gameObject.name}\" begin waiting {rnd} sec.");

                    virtualTween = DOVirtual.DelayedCall(rnd, () =>
                    {
                        Debug.Log($"[DEBUG] - Enemy \"{Enemy.gameObject.name}\" waiting end.");
                        this.WalkOut();
                    });
                }
            });
    }

    [Button("Walk Out", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void WalkOut()
    {
        if (Enemy.Status != Enemy._Status.Fighting)
        {
            EnemyWrapper.GetComponent<Rigidbody>().DORotate(new Vector3(0f, -180f, 0f), 1f);
            Enemy.Status = Enemy._Status.Moving;
            
            if(Enemy.GetComponent<Animator>() != null){
                Enemy.GetComponent<Animator>().SetInteger("Motion", 1);
            }
            
            tween = EnemyWrapper.transform
                .DOPath(new[]
                    {
                        Points[1].position,
                        Points[0].position
                    },
                    CompleteMoveInSeconds,
                    PathType.Linear)
                .SetDelay(1)
                .SetEase(Ease.Linear)
                .SetOptions(false)
                .OnWaypointChange((int point) => { })
                .OnStart(() =>
                {
                    Debug.Log($"[DEBUG] - Enemy \"{Enemy.gameObject.name}\" walk out begin.");
                })
                .OnPlay(() =>
                {
                    //EnemyWrapper.GetComponent<Animator>().SetInteger("Motion", 1);
                    if(Enemy.GetComponent<Animator>() != null){
                        Enemy.GetComponent<Animator>().SetInteger("Motion", 1);
                    }
                })
                .OnComplete(() =>
                {
                    EnemyWrapper.GetComponent<Animator>().SetInteger("Motion", 0);
                    if(Enemy.GetComponent<Animator>() != null){
                        Enemy.GetComponent<Animator>().SetInteger("Motion", 0);
                    }
                    
                    Debug.Log($"[DEBUG] - Enemy \"{Enemy.gameObject.name}\" walk out end.");
                    EnemyWrapper.GetComponent<Rigidbody>().DORotate(new Vector3(0f, 0f, 0f), 0.1f).OnComplete(() =>
                    {
                        Enemy.Status = Enemy._Status.Waiting;
                        this.Remove();

                        //int rnd = Random.Range(5, 10);

                        //Debug.Log($"[DEBUG] - Enemy controller is begin waiting {rnd} sec.");

                        virtualTween = DOVirtual.DelayedCall(0.1f, () =>
                        {
                            Debug.Log($"[DEBUG] - Enemy controller waiting is end.");
                            this.Spawn();
                        });
                    });
                });
        }
    }

    [Button("Attack", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Attack()
    {
        Debug.Log($"[DEBUG] - Enemy \"{Enemy.gameObject.name}\" is attack you!");
        //EnemyWrapper.GetComponent<Animator>().SetInteger("Motion", 2);
        if(Enemy.GetComponent<Animator>() != null){
            Enemy.GetComponent<Animator>().SetInteger("Motion", 2);
        }

        foreach (var child in EnableOnAttackBegin)
        {
            child.SetActive(true);
        }

        foreach (var child in DisableOnAttackBegin)
        {
            child.SetActive(false);
        }
    }

    [Button("Stop Attack", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void StopAttack()
    {
        Debug.Log($"[DEBUG] - Enemy \"{Enemy.gameObject.name}\" stops attack.");
        Enemy.Status = Enemy._Status.Dying;
        AnimatorProvider.AnimationAttackEnd();
        AnimatorProvider.JumpAnimationEnd();
        virtualTween.Kill();
        tween.Kill();
        //EnemyWrapper.GetComponent<Animator>().SetInteger("Motion", 0);
        if(Enemy.GetComponent<Animator>() != null){
            Enemy.GetComponent<Animator>().SetInteger("Motion", 9);
        }

    }

    [Title("Attack Event")]
    public List<GameObject> EnableOnAttackBegin;
    public List<GameObject> EnableOnAttackEnd;
    public List<GameObject> DisableOnAttackBegin;
    public List<GameObject> DisableOnAttackEnd;

    [Button("Die", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Die()
    {
        Debug.Log($"[DEBUG] - Enemy \"{Enemy.gameObject.name}\" is died.");

        AnimatorProvider.AnimationEnd();
        virtualTween.Kill();
        tween.Kill();

        foreach (var child in EnableOnAttackEnd)
        {
            child.SetActive(true);
        }

        foreach (var child in DisableOnAttackEnd)
        {
            child.SetActive(false);
        }

        DestroyImmediate(Enemy.gameObject);

        EnemyWrapper.transform.localPosition = enemyWrapperDefaultPosition;
        EnemyWrapper.transform.localRotation = Quaternion.Euler(enemyWrapperDefaultRotation);

        virtualTween = DOVirtual.DelayedCall(0f, () => { this.Spawn(); });
    }

    [Button("Wait Time", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void WaitTime()
    {
        StartCoroutine(waitTime(1f));
    }

    private IEnumerator waitTime(float TimeToWait)
    {
        Debug.Log($"[DEBUG] - Wait Time - \"{TimeToWait}\" sec.");
        yield return new WaitForSeconds(TimeToWait);
        Debug.Log($"[DEBUG] - Wait Time - End.");
    }

    [Button("Pause", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Pause()
    {
        string text = paused ? "Unpaused" : "Paused";
        Debug.Log($"[DEBUG] {text} - \"" + EnemyWrapper.name + "\"");
        if (!paused)
        {
            //EnemyWrapper.GetComponent<Animator>().SetInteger("Motion", 0);
            EnemyWrapper.transform.DOPause();
            paused = true;
        }
        else
        {
            //EnemyWrapper.GetComponent<Animator>().SetInteger("Motion", 1);
            EnemyWrapper.transform.DOPlay();
            paused = false;
        }
    }

    void Start()
    {
        enemyWrapperDefaultPosition = EnemyWrapper.transform.localPosition;
        enemyWrapperDefaultRotation = EnemyWrapper.transform.localRotation.eulerAngles;
        gameObject.AddComponent<BoxCollider>();
        if (IsTrigger)
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        if (_Spawn)
        {
            this.Spawn();
        }
    }

    void Update()
    {
    }
}