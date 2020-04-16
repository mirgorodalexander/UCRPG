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
    public ExperienceDatabase ExperienceDatabase;
    public HealthDatabase HealthDatabase;
    
    [Title("Controllers")]
    public LocationController LocationController;
    public MessageController MessageController;
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
                PlayerExperience.GetComponent<SliderProvider>().PreFill.fillAmount = (1f / ExperienceDatabase.Items[Player.LVL]) * Player.EXP;
                MenuController.UpdateInGameUI();
            });
    }
    [Button("Respawn", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Respawn()
    {
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
        WeaponController.WeaponParent.SetActive(false);
        WeaponController.TakeOff();
        MenuController.Show();
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
        
        //PlayerExperience.gameObject.transform.GetChild(0).transform.Find("Value").GetComponent<TextMeshProUGUI>().text = $"{Player.EXP} / {ExperienceDatabase.Experience[Player.LVL]}";
    }

    void Update()
    {
        
    }
}
