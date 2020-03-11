using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Title("Configurations")]
    public Player Player;
    public ExperienceDatabase ExperienceDatabase;
    
    [Title("Controllers")]
    public MessageController MessageController;
    
    [Title("Sliders")]
    public Slider PlayerExperience;
    
    [Title("FX")]
    public GameObject ExperiencePopupCanvas;
    public GameObject PlayerExperiencePopupPrefab;
    
    [Title("Buttons")]
    [Button("Gain EXP", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void GainEXP(int exp)
    {    
        if (Player.EXP < ExperienceDatabase.Experience[Player.LVL])
        {
            //ExperiencePopup(exp, PlayerExperiencePopupPrefab);
            MessageController.ExperiencePopup(exp);
            //MessageController.ConsolePopup($"You got \"{exp}\" experience");
            Player.EXP += exp;
            if (Player.EXP >= ExperienceDatabase.Experience[Player.LVL])
            {
                int addToNextLvlEXP = Player.EXP - ExperienceDatabase.Experience[Player.LVL];
                Player.EXP = 0;
                Player.EXP += addToNextLvlEXP;
                this.LevelUp();
            }
        }
        PlayerExperience.value = (1f / ExperienceDatabase.Experience[Player.LVL]) * Player.EXP;
        PlayerExperience.gameObject.transform.GetChild(0).gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = $"{Player.EXP} / {ExperienceDatabase.Experience[Player.LVL]}";

    }
    [Button("Level Up", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void LevelUp()
    {
        Player.LVL += 1;
        MessageController.ConsolePopup($"You got new level!");
    }
//    public void ExperiencePopup(int experience, GameObject prefab)
//    {
//        if(prefab == null)
//        {
//            prefab = PlayerExperiencePopupPrefab;
//        }
//        
//        GameObject Damage =
//            Instantiate(prefab, ExperiencePopupCanvas.transform.position, Quaternion.identity) as GameObject;
//        Damage.transform.SetParent(ExperiencePopupCanvas.transform);
//        Damage.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = $"+ {experience}";
//        Damage.name = $"Experience + {experience}";
//        DOVirtual.DelayedCall(1f, () => { DestroyImmediate(Damage.gameObject); });
//    }
    void Start()
    {
        PlayerExperience.value = (1f / ExperienceDatabase.Experience[Player.LVL]) * Player.EXP;
        PlayerExperience.gameObject.transform.GetChild(0).transform.Find("Value").GetComponent<TextMeshProUGUI>().text = $"{Player.EXP} / {ExperienceDatabase.Experience[Player.LVL]}";
    }

    void Update()
    {
        
    }
}
