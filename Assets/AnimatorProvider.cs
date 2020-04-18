﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class AnimatorProvider : MonoBehaviour
{
    [Title("Controllers")]
    public HealthController HealthController;
    public PlayerController PlayerController;
    public WeaponController WeaponController;
    public EnemyController EnemyController;

    [Title("FX")]
    public GameObject ComboParticles;

    public Tween tweenVirtual;

    public void ComboFX()
    {
        ComboParticles.SetActive(true);
        DOVirtual.DelayedCall(0.8f, () =>
        {
            ComboParticles.SetActive(false);
        });
    }
    public void AnimationAttackBegin()
    {
        //this.GetComponent<Animator>().SetFloat("speed", 5.0f);
        // this.GetComponent<Animator>().SetInteger("Motion", 2);
    }

    public void AnimationAttackPlayer()
    {
        HealthController.PlayerWasAttacked();
    }
    
    public void AnimationAttackEnemy()
    {
        HealthController.EnemyWasAttacked();
    }
    public void AnimationAttackEnemyEnd()
    {
        if(PlayerController.Player.Status != Player._Status.Die){
            this.GetComponent<Animator>().SetInteger("Motion", 0);
            DOVirtual.DelayedCall(0.4f-WeaponController.Weapon.SPD*0.01f, () =>
            {
                if(PlayerController.Player.Status != Player._Status.Die){
                    this.GetComponent<Animator>().SetInteger("Motion", 0);
                }
                WeaponController.AttackLock = false;
            });
        }
    }

    public void AnimationEnd()
    {
        if (PlayerController.Player.Status != Player._Status.Die)
        {
            tweenVirtual.Kill();
            this.GetComponent<Animator>().SetInteger("Motion", 0);
        }
    }

    public void AnimationAttackEnd()
    {
        tweenVirtual.Kill();
        if (PlayerController.Player.Status != Player._Status.Die)
        {
            this.GetComponent<Animator>().SetInteger("Motion", 0);
        }

        if (EnemyController.Enemy != null)
        {
            Debug.Log(
                $"[DEBUG] - Enemy \"{EnemyController.Enemy.gameObject.name}\" waiting \"ATKD\" for {EnemyController.Enemy.ATKD} sec begin.");
            if (EnemyController.Enemy != null)
            {
                if (EnemyController.Enemy.Status == Enemy._Status.Fighting)
                {
                    tweenVirtual = DOVirtual.DelayedCall(EnemyController.Enemy.ATKD, () =>
                    {
                        Debug.Log($"[DEBUG] - Enemy \"{EnemyController.Enemy.gameObject.name}\" waiting \"ATKD\" end.");
                        EnemyController.Attack();
                    });
                }
            }
        }
    }

    public void JumpAnimationStart()
    {
        this.GetComponent<Animator>().SetInteger("Motion", 1);
    }

    public void JumpAnimationEnd()
    {
        this.GetComponent<Animator>().SetInteger("Motion", 0);
    }

    public void DOTogglePause()
    {
        //Debug.Log("[DEBUG] - Tween paused.");
        this.transform.DOTogglePause();
        this.transform.parent.transform.parent.transform.DOTogglePause();
    }

    void Start()
    {
        if(HealthController == null){
            HealthController = this.transform.parent.transform.parent.GetComponent<ControllerProvider>().HealthController;
        }
        if (PlayerController == null){
            PlayerController = this.transform.parent.transform.parent.GetComponent<ControllerProvider>().PlayerController;
        }
        if (WeaponController == null){
            WeaponController = this.transform.parent.transform.parent.GetComponent<ControllerProvider>().WeaponController;
        }
        if (EnemyController == null){
            EnemyController = this.transform.parent.transform.parent.GetComponent<ControllerProvider>().EnemyController;
        }
    }

    void Update()
    {
    }
}