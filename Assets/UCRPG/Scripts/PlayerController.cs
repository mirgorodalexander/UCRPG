using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Title("Configurations")]
    public Player Player;
    public ExperienceDatabase ExperienceDatabase;
    
    [Title("Sliders")]
    public Slider PlayerExperience;
    
    [Title("Buttons")]
    [Button("Gain EXP", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void GainEXP(int exp)
    {    
        if (Player.EXP < ExperienceDatabase.Experience[Player.LVL])
        {
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
        PlayerExperience.gameObject.transform.Find("Value").GetComponent<Text>().text = $"{Player.EXP} / {ExperienceDatabase.Experience[Player.LVL]}";

    }
    [Button("Level Up", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void LevelUp()
    {
        Player.LVL += 1;
    }
    
    void Start()
    {
        PlayerExperience.value = (1f / ExperienceDatabase.Experience[Player.LVL]) * Player.EXP;
        PlayerExperience.gameObject.transform.Find("Value").GetComponent<Text>().text = $"{Player.EXP} / {ExperienceDatabase.Experience[Player.LVL]}";
    }

    void Update()
    {
        
    }
}
