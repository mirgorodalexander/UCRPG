using System;
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

    private Tween tweenVirtual;

    public void AnimationAttackBegin()
    {
        this.GetComponent<Animator>().SetInteger("Motion", 2);
    }

    public void AnimationAttackPlayer()
    {
        HealthController.PlayerDamage(EnemyController.Enemy.ATK);
    }
    
    public void AnimationAttackEnemy()
    {
        HealthController.EnemyDamage(PlayerController.Player.ATK);
    }
    public void AnimationAttackEnemyEnd()
    {
        WeaponController.AttackLock = false;
        this.GetComponent<Animator>().SetInteger("Motion", 0);
    }

    public void AnimationEnd()
    {
        tweenVirtual.Kill();
        this.GetComponent<Animator>().SetInteger("Motion", 0);
    }

    public void AnimationAttackEnd()
    {
        tweenVirtual.Kill();
        this.GetComponent<Animator>().SetInteger("Motion", 0);
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