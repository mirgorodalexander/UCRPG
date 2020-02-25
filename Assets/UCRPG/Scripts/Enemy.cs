using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Title("Preferences")]
    public _MOD MOD;
    public enum _MOD { Neutral, Aggresive };

    [Title("Status")]
    public _Status Status;
    public enum _Status { Init, Waiting, Moving, Fighting, Dying };
    [Title("Settings")]
    public int LVL;
    public int BEXP;
    public int JEXP;
    public int HP;
    public int MP;
    public int ATK;
    public float ATKD;
    public int DEF;
    
    void Start()
    {
        Debug.Log($"[DEBUG] - Enemy \"{gameObject.name}\" spawned.");
    }

    void Update()
    {
        
    }
}
