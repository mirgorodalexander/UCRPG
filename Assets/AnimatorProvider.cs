using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class AnimatorProvider : MonoBehaviour
{
    [Title("Controllers")]
    public EnemyController EnemyController;

    public PlayerController PlayerController;
    public HealthController HealthController;

    private Tween tweenVirtual;

    public void AnimationAttackBegin()
    {
        this.GetComponent<Animator>().SetInteger("Motion", 2);
    }

    public void AnimationAttackPlayer()
    {
        HealthController.PlayerDamage(EnemyController.Enemy.ATK);
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
        HealthController = this.transform.parent.transform.parent.GetComponent<ControllerProvider>().HealthController;
        PlayerController = this.transform.parent.transform.parent.GetComponent<ControllerProvider>().PlayerController;
        EnemyController = this.transform.parent.transform.parent.GetComponent<ControllerProvider>().EnemyController;
    }

    void Update()
    {
    }
}