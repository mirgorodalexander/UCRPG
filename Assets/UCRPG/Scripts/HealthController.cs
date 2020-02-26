﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [Title("Controllers")]
    public EnemyController EnemyController;
    public PlayerController PlayerController;
    
    [Title("Configurations")]
    public Player Player;
    public Enemy Enemy;
    
    [Title("Sliders")]
    public Slider PlayerHealth;
    public Slider EnemyHealth;
    
    [Title("FX")]
    public GameObject EnemyDeathParticles;
    public GameObject EnemyAttackedLight;
    public GameObject EnemyAttackedParticles;
    public GameObject PlayerAttackedParticles;
    public GameObject PlayerDamagePopupPrefab;
    public GameObject EnemyDamagePopupPrefab;
    public GameObject DamagePopupCanvas;
    public DOTweenAnimation CameraPunchEffect;
    
    [Title("Debug")]
    public int playerHealthDefault;
    public int enemyHealthDefault;
    
    [Title("Buttons")]
    [Button("Player Damage", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void PlayerDamage(int damage)
    {
        Debug.Log($"[DEBUG] - Player has damage \"{damage}\"");
        if (Player.HP > 0)
        {
            Player.HP -= damage;
            if (Player.HP < 0)
            {
                Player.HP = 0;
            }
        }

        PlayerHealth.value = (1f / playerHealthDefault) * Player.HP;
        PlayerHealth.gameObject.transform.Find("Value").GetComponent<Text>().text = $"{Player.HP} / {playerHealthDefault}";
        
        DamagePopup(damage, PlayerDamagePopupPrefab);

        CameraPunchEffect.DORestart();
        
        PlayerAttackedParticles.SetActive(true);

        DOVirtual.DelayedCall(0.2f, () =>
        {
            PlayerAttackedParticles.SetActive(false);
        });
    }
    [Button("Enemy Damage", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void EnemyDamage(int damage)
    {
        Debug.Log($"[DEBUG] - Enemy has damage \"{damage}\"");
        if (Enemy.HP > 0)
        {
            Enemy.HP -= damage;
            if (Enemy.HP <= 0)
            {
                Enemy.HP = 0;
                enemyHealthDefault = 0;
                DOVirtual.DelayedCall(0.3f, () =>
                {  
                    PlayerController.GainEXP(Enemy.BEXP);
                    EnemyController.Die();
                    EnemyDeathParticles.SetActive(true);
                    DOVirtual.DelayedCall(2f, () => { EnemyDeathParticles.SetActive(false); });
                });
            }
        }
        
        EnemyHealth.value = (1f/ enemyHealthDefault) * Enemy.HP;
        EnemyHealth.gameObject.transform.Find("Value").GetComponent<Text>().text = $"{Enemy.HP} / {enemyHealthDefault}";

        DamagePopup(damage, EnemyDamagePopupPrefab);
        
        EnemyAttackedParticles.SetActive(true);

        EnemyAttackedLight.SetActive(true);
        DOVirtual.DelayedCall(0.06f, () =>
        {
            Enemy.gameObject.transform.DOPunchPosition(new Vector3(0f, 0.05f, 0.05f), 0.3f, 10).SetEase(Ease.OutQuad);
            EnemyAttackedLight.SetActive(false);
            DOVirtual.DelayedCall(0.06f, () =>
            {
                Enemy.gameObject.transform.DOPunchPosition(new Vector3(0f, 0.05f, 0.05f), 0.3f, 10).SetEase(Ease.OutQuad);
                EnemyAttackedLight.SetActive(true);
                DOVirtual.DelayedCall(0.06f, () =>
                {
                    EnemyAttackedLight.SetActive(false);
                    DOVirtual.DelayedCall(0.06f, () =>
                    {
                        EnemyAttackedLight.SetActive(true);
                        DOVirtual.DelayedCall(0.06f, () =>
                        {
                            EnemyAttackedLight.SetActive(false);
                        });
                    });
                });
            });
        });
        DOVirtual.DelayedCall(0.2f, () =>
        {
            EnemyAttackedParticles.SetActive(false);
        });
    }
    [Button("Damage Popup", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void DamagePopup(int damage, GameObject prefab)
    {
        if(prefab == null)
        {
            prefab = EnemyDamagePopupPrefab;
        }
        
        GameObject Damage =
            Instantiate(prefab, DamagePopupCanvas.transform.position, Quaternion.identity) as GameObject;
        Damage.transform.SetParent(DamagePopupCanvas.transform);
        Damage.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = $"{damage}";
        Damage.name = $"Damage - {damage}";
        DOVirtual.DelayedCall(1f, () => { DestroyImmediate(Damage.gameObject); });
    }
    
    void Start()
    {
        EnemyAttackedLight.SetActive(false);
        EnemyAttackedParticles.SetActive(false);
        PlayerAttackedParticles.SetActive(false);
        EnemyDeathParticles.SetActive(false);
    }

    void Update()
    {
        if(Player != null && playerHealthDefault == 0){
            playerHealthDefault = Player.HP;
            PlayerHealth.gameObject.transform.Find("Value").GetComponent<Text>().text = $"{Player.HP} / {playerHealthDefault}";
        }
        if(Enemy != null && enemyHealthDefault == 0){
            enemyHealthDefault = Enemy.HP;
            EnemyHealth.gameObject.transform.Find("Value").GetComponent<Text>().text = $"{Enemy.HP} / {enemyHealthDefault}";
        }
    }
}