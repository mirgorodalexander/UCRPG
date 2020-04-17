using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Title("Configurations")]
    public Player Player;
    public Animator PlayerAvatar;
    public AnimatorProvider AnimatorProvider;
    public ExperienceDatabase ExperienceDatabase;
    public HealthDatabase HealthDatabase;
    
    [Title("Controllers")]
    public LocationController LocationController;
    public MessageController MessageController;
    public RenderController RenderController;
    public HealthController HealthController;
    public WeaponController WeaponController;
    public EnemyController EnemyController;
    public MenuController MenuController;
    public ItemController ItemController;
    
    [Title("Sliders")]
    public Slider PlayerExperience;
    
    [Title("FX")]
    public GameObject ExperiencePopupCanvas;
    public GameObject PlayerExperiencePopupPrefab;
    
    private Tween virtualTween;
    
    [Title("Buttons")]
    [Button("Gain EXP", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void GainEXP(int exp)
    {   
        exp = exp+(Random.Range(-4, 4));
        if (Player.EXP < ExperienceDatabase.Items[Player.LVL])
        {
            //ExperiencePopup(exp, PlayerExperiencePopupPrefab);
            MessageController.ExperiencePopup(exp);
            //MessageController.ConsolePopup($"You got \"{exp}\" experience");
            Player.EXP += exp;
            if (Player.EXP >= ExperienceDatabase.Items[Player.LVL])
            {
                int addToNextLvlEXP = Player.EXP - ExperienceDatabase.Items[Player.LVL];
                Player.EXP = 0;
                Player.EXP += addToNextLvlEXP;
                this.LevelUp();
            }
        }

        PlayerExperience.GetComponent<SliderProvider>().PreFill.DOFillAmount((1f / ExperienceDatabase.Items[Player.LVL]) * Player.EXP, 1f).OnComplete(
            () => { PlayerExperience.value = (1f / ExperienceDatabase.Items[Player.LVL]) * Player.EXP; });
        PlayerExperience.gameObject.transform.Find("Line").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = $"{Player.EXP} / {ExperienceDatabase.Items[Player.LVL]}";

    }
    [Button("Level Up", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void LevelUp()
    {
        Player.LVL += 1;
        Player.HP = HealthDatabase.Items[Player.LVL];
        HealthController.playerHealthDefault = HealthDatabase.Items[Player.LVL];
        
        HealthController.PlayerHealth.value = (1f / HealthController.playerHealthDefault) * Player.HP;
        HealthController.PlayerHealth.gameObject.transform.Find("Viewport").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text =
            $"{Player.HP} / {HealthController.playerHealthDefault}";
        
        
        MenuController.UpdateInGameUI();
        
        MenuController.ShowLevelUp();
        
        MessageController.ConsolePopup($"You got new level!");
    }
    [Button("Die", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Die()
    {
        virtualTween.Kill();
        PlayerAvatar.SetInteger("Motion", 9);
        
        EnemyController.StopAttack();
        EnemyController.WalkOut();
        
        Player.Status = Player._Status.Die;
        
        int playerexp = Player.EXP;
        int loseexp = Random.Range(200, 400);
        int restexp = Player.EXP - loseexp;
        if (restexp <= 0)
        {
            Player.EXP = 0;
            Debug.Log($"[DEBUG] - Player is died and lose \"{playerexp}\" expirience.");
            MessageController.ConsolePopup($"You die and lose \"{playerexp}\" experience.");
            MenuController.ShowDefeated(playerexp);
        }
        else
        {
            Player.EXP -= loseexp;
            Debug.Log($"[DEBUG] - Player is died and lose {loseexp} expirience.");
            MessageController.ConsolePopup($"You die and lose \"{loseexp}\" experience.");
            MenuController.ShowDefeated(loseexp);
        }
        
        PlayerExperience.gameObject.transform.Find("Line").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = $"{Player.EXP} / {ExperienceDatabase.Items[Player.LVL]}";
        PlayerExperience.DOValue((1f / ExperienceDatabase.Items[Player.LVL]) * Player.EXP, 1f).OnComplete(
            () => {
                Player.Status = Player._Status.Die;
                PlayerAvatar.SetInteger("Motion", 9);
                PlayerExperience.GetComponent<SliderProvider>().PreFill.fillAmount = (1f / ExperienceDatabase.Items[Player.LVL]) * Player.EXP;
                MenuController.UpdateInGameUI();
            });
    }
    [Button("Stay", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Stay()
    {
        virtualTween.Kill();
        Debug.Log($"[DEBUG] - Player is staying.");
        WeaponController.AttackLock = false;
        if(Player.Status != Player._Status.Menu){
            PlayerAvatar.SetInteger("Motion", 0);
            Player.Status = Player._Status.Waiting;
            if (EnemyController.Enemy == null)
            {
                virtualTween = DOVirtual.DelayedCall(1f, () => { LocationController.Move(); });
            }
        }
    }
    [Button("Sit", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Sit()
    {
        Player.Status = Player._Status.Sitting;
        PlayerAvatar.SetInteger("Motion", 4);
        Debug.Log($"[DEBUG] - Player is sitting.");

        if (Player.HP < HealthDatabase.Items[Player.LVL] / 1.5f)
        {
            Debug.Log($"[DEBUG] - Next sitting health in {50f/Player.VIT} sec.");
            virtualTween = DOVirtual.DelayedCall(50f/Player.VIT, () =>
            {
                if(Player.Status == Player._Status.Sitting && Player.Status != Player._Status.Die){
                    Player.HP += Player.LVL * Player.VIT;

                    HealthController.PlayerHealth.DOValue((1f / HealthController.playerHealthDefault) * Player.HP, 1f).SetEase(Ease.Linear);
                    HealthController.PlayerHealth.gameObject.transform.Find("Viewport").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text =
                        $"{Player.HP} / {HealthController.playerHealthDefault}";
                    Sit();
                }
            });
        }
        else
        {
            Stay();
        }
    }
    [Button("Stop Attack", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void StopAttack()
    {
        Debug.Log($"[DEBUG] - Player stops attack.");
        AnimatorProvider.AnimationAttackEnd();
        AnimatorProvider.AnimationAttackEnemyEnd();
        if (PlayerAvatar.GetComponent<Animator>() != null)
        {
            PlayerAvatar.GetComponent<Animator>().SetInteger("Motion", 0);
        }
    }
    [Button("Respawn", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Respawn()
    {
        PlayerAvatar.SetInteger("Motion", 0);
        
        PlayerAvatar.Play("wait", -1, 0f);
        if (EnemyController.Enemy != null)
        {
            EnemyController.Remove();
        }
        
        Debug.Log($"[DEBUG] - Player respawning.");
        Player.Status = Player._Status.Menu;
        
        Player.HP = HealthDatabase.Items[Player.LVL];
        HealthController.playerHealthDefault = HealthDatabase.Items[Player.LVL];
        
        MenuController.UpdateInGameUI();
        
        int tempLID = Player.LID;
        Player.LID = -1;
        HealthController.PlayerHealth.gameObject.SetActive(false);
        HealthController.EnemyHealth.gameObject.SetActive(false);
        if(!RenderController.ThirdPersonView){
            WeaponController.WeaponParentFirstPerson.SetActive(false);
        }
        WeaponController.TakeOn();
        MenuController.Hide();
        ItemController.Remove();
        LocationController.Spawn(tempLID);
        EnemyController.Spawn();
    }
    void Start()
    {
        Player.Status = Player._Status.Menu;
        
        PlayerExperience.value = (1f / ExperienceDatabase.Items[Player.LVL]) * Player.EXP;
        PlayerExperience.GetComponent<SliderProvider>().PreFill.fillAmount = (1f / ExperienceDatabase.Items[Player.LVL]) * Player.EXP;
        PlayerExperience.gameObject.transform.Find("Line").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = $"{Player.EXP} / {ExperienceDatabase.Items[Player.LVL]}";
        
        if (Player.HP == 0)
        {
            Player.HP = HealthDatabase.Items[Player.LVL];
        }
        
        if(RenderController.ThirdPersonView){
            foreach (Transform child in PlayerAvatar.gameObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform child in PlayerAvatar.gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        //PlayerExperience.gameObject.transform.GetChild(0).transform.Find("Value").GetComponent<TextMeshProUGUI>().text = $"{Player.EXP} / {ExperienceDatabase.Experience[Player.LVL]}";
    }

    void Update()
    {
        
    }
}
