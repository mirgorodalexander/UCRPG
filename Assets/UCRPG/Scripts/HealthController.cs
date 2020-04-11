using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [Title("Controllers")]
    public MessageController MessageController;
    public WeaponController WeaponController;
    public PlayerController PlayerController;
    public EnemyController EnemyController;
    public ItemController ItemController;

    [Title("Configurations")]
    public Player Player;

    public Enemy Enemy;
    private int FlashNumsCount;
    public int FlashNums;

    [Title("Sliders")]
    public Slider PlayerHealth;
    public Slider EnemyHealth;

    [Title("FX")]
    public GameObject EnemyDeathParticles;

    public GameObject EnemyAttackedLight;
    public GameObject CriticalAttackedLight;
    public GameObject EnemyAttackedParticles;
    public GameObject CriticalAttack;
    public GameObject PlayerAttackedParticles;
    public GameObject PlayerDamagePopupPrefab;
    public GameObject EnemyDamagePopupPrefab;
    public GameObject DamagePopupCanvas;
    public DOTweenAnimation CameraPunchEffect;

    [Title("Debug")]
    public int playerHealthDefault;
    public int playerHealthDefault1;

    public int enemyHealthDefault;

    [Title("Buttons")]
    public void PlayerWasAttacked()
    {
        int damage = EnemyController.Enemy.ATK;
        PlayerDamage(damage);
    }
    public void EnemyWasAttacked()
    {
        int damage = PlayerController.Player.ATK + WeaponController.Weapon.ATK;
        
        float chance = Player.LUK;
        float rndchance = Random.Range(0f, 100f);
        
        if (rndchance >= 0f && rndchance <= chance)
        {
            Debug.Log($"[DEBUG] - Player critical attack!");
            CriticalAttack.SetActive(true);
            DOVirtual.DelayedCall(1f, () => { CriticalAttack.SetActive(false); });
            CriticalDamage(damage * 2);
        }
        else
        {
            EnemyDamage(damage);
        }
    }
    
    [Button("Player Damage", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void PlayerDamage(int damage)
    {
        damage = damage+(Random.Range(-4, 4));
        Debug.Log($"[DEBUG] - Player has damage \"{damage}\"");
        if (Player.HP > 0)
        {
            Player.HP -= damage;
            if (Player.HP <= 0)
            {
                Player.HP = 0;
                PlayerController.Die();
            }
        }

        PlayerHealth.value = (1f / playerHealthDefault) * Player.HP;
        PlayerHealth.gameObject.transform.Find("Viewport").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text =
            $"{Player.HP} / {playerHealthDefault}";
        
        MessageController.PlayerDamagePopup(damage);

        CameraPunchEffect.DORestart();

        PlayerAttackedParticles.SetActive(true);

        DOVirtual.DelayedCall(0.2f, () => { PlayerAttackedParticles.SetActive(false); });
    }

    private void EnemyDyingAnimation()
    {
        EnemyAttackedLight.SetActive(true);
        DOVirtual.DelayedCall(0.06f, () =>
        {
            EnemyAttackedLight.SetActive(false);
            DOVirtual.DelayedCall(0.06f, () =>
            {
                //Enemy.gameObject.transform.DOPunchPosition(new Vector3(0f, 0.05f, 0.05f), 0.3f, 10)
                //    .SetEase(Ease.OutQuad);
                if (FlashNumsCount < FlashNums)
                {
                    EnemyDyingAnimation();
                    FlashNumsCount++;
                }
                else
                {
                    FlashNumsCount = 0;
                    Enemy.gameObject.SetActive(false);
                    PlayerController.GainEXP(Enemy.BEXP);
                    EnemyAttackedLight.SetActive(false);
                    EnemyDeathParticles.SetActive(true);
                    if(PlayerController.Player.Status != Player._Status.Die){
                        PlayerController.Player.Status = Player._Status.Waiting;
                    }
                    
                    ItemController.Drop();
                    enemyHealthDefault = 0;
                    EnemyController.Die();
                    
                    DOVirtual.DelayedCall(2f, () => { 
                        EnemyDeathParticles.SetActive(false); 
                        EnemyHealth.value = 1f;
                    });
                }
            });
        });
    }

    [Button("Critical Damage", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void CriticalDamage(int damage)
    {
        damage = damage+(Random.Range(-4, 4));
        if (Player != null && playerHealthDefault == 0)
        {
            playerHealthDefault = Player.HP;
            PlayerHealth.gameObject.transform.Find("Viewport").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text =
                $"{Player.HP} / {playerHealthDefault}";
        }

        if (Enemy != null && enemyHealthDefault == 0)
        {
            enemyHealthDefault = Enemy.HP;
            EnemyHealth.gameObject.transform.Find("Viewport").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text =
                $"{Enemy.HP} / {enemyHealthDefault}";
        }
        Debug.Log($"[DEBUG] - Enemy has damage \"{damage}\"");
        if (Enemy.HP > 0)
        {
            Enemy.HP -= damage;
            if (Enemy.HP <= 0)
            {
                Enemy.HP = 0;
                EnemyController.StopAttack();
                EnemyDyingAnimation();
                DOVirtual.DelayedCall(0.3f, () => { 
                    WeaponController.TakeOff();
                });
            }
        }

        EnemyHealth.value = (1f / enemyHealthDefault) * Enemy.HP;
        EnemyHealth.gameObject.transform.Find("Viewport").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = $"{Enemy.HP} / {enemyHealthDefault}";

        MessageController.CriticalDamagePopup(damage);

        EnemyAttackedParticles.SetActive(true);

        DOVirtual.DelayedCall(0.06f, () =>
        {
            Enemy.gameObject.transform.DOPunchPosition(new Vector3(0f, 0.05f, 0.05f), 0.6f, 20, 0f).SetEase(Ease.OutQuad);
            EnemyAttackedLight.SetActive(false);
            DOVirtual.DelayedCall(0.06f, () =>
            {
                EnemyAttackedLight.SetActive(true);
                DOVirtual.DelayedCall(0.06f, () =>
                {
                    EnemyAttackedLight.SetActive(false);
                    DOVirtual.DelayedCall(0.06f, () =>
                    {
                        EnemyAttackedLight.SetActive(true);
                        DOVirtual.DelayedCall(0.06f, () => { EnemyAttackedLight.SetActive(false); });
                    });
                });
            });
        });
        DOVirtual.DelayedCall(0.2f, () => { EnemyAttackedParticles.SetActive(false); });
    }

    [Button("Enemy Damage", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void EnemyDamage(int damage)
    {
        damage = damage+(Random.Range(-4, 4));
        if (Player != null && playerHealthDefault == 0)
        {
            playerHealthDefault = Player.HP;
            PlayerHealth.gameObject.transform.Find("Viewport").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text =
                $"{Player.HP} / {playerHealthDefault}";
        }

        if (Enemy != null && enemyHealthDefault == 0)
        {
            enemyHealthDefault = Enemy.HP;
            EnemyHealth.gameObject.transform.Find("Viewport").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text =
                $"{Enemy.HP} / {enemyHealthDefault}";
        }
        Debug.Log($"[DEBUG] - Enemy has damage \"{damage}\"");
        if (Enemy.HP > 0)
        {
            Enemy.HP -= damage;
            if (Enemy.HP <= 0)
            {
                Enemy.HP = 0;
                EnemyController.StopAttack();
                EnemyDyingAnimation();
                DOVirtual.DelayedCall(0.3f, () => { 
                    WeaponController.TakeOff();
                });
            }
        }

        EnemyHealth.value = (1f / enemyHealthDefault) * Enemy.HP;
        EnemyHealth.gameObject.transform.Find("Viewport").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = $"{Enemy.HP} / {enemyHealthDefault}";

        MessageController.EnemyDamagePopup(damage);

        EnemyAttackedParticles.SetActive(true);

        EnemyAttackedLight.SetActive(true);
        DOVirtual.DelayedCall(0.06f, () =>
        {
            Enemy.gameObject.transform.DOPunchPosition(new Vector3(0f, 0.05f, 0.05f), 0.6f, 20, 0f).SetEase(Ease.OutQuad);
            EnemyAttackedLight.SetActive(false);
            DOVirtual.DelayedCall(0.06f, () =>
            {
                EnemyAttackedLight.SetActive(true);
                DOVirtual.DelayedCall(0.06f, () =>
                {
                    EnemyAttackedLight.SetActive(false);
                    DOVirtual.DelayedCall(0.06f, () =>
                    {
                        EnemyAttackedLight.SetActive(true);
                        DOVirtual.DelayedCall(0.06f, () => { EnemyAttackedLight.SetActive(false); });
                    });
                });
            });
        });
        DOVirtual.DelayedCall(0.2f, () => { EnemyAttackedParticles.SetActive(false); });
    }

    void Start()
    {
        CriticalAttack.SetActive(false);
        EnemyAttackedLight.SetActive(false);
        EnemyDeathParticles.SetActive(false);
        CriticalAttackedLight.SetActive(false);
        EnemyAttackedParticles.SetActive(false);
        PlayerAttackedParticles.SetActive(false);
        
        FlashNumsCount = 0;

        playerHealthDefault = PlayerController.HealthDatabase.Items[Player.LVL];
        
        PlayerHealth.value = (1f / playerHealthDefault) * Player.HP;
        PlayerHealth.gameObject.transform.Find("Viewport").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text =
            $"{Player.HP} / {playerHealthDefault}";
    }

    void Update()
    {
    }
}