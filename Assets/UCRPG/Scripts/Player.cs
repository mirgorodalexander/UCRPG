using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Title("Status")]
    public _Status Status;
    public enum _Status { Waiting, Moving, Fighting, Dying, Menu };
    [Title("Settings")]
    public int LVL;
    public int EXP;
    public int EXPNEED;
    public int HP;
    public int MP;
    public int ATK;
    public float ATKD;
    public int DEF;
    public int LID;

    [Title("Inventory")]
    public List<int> Inventory;
    
    void Start()
    {
//        LVL = ES3.Load<int>("LVL");
//        EXP = ES3.Load<int>("EXP");
//        EXPNEED = ES3.Load<int>("EXPNEED");
//        HP = ES3.Load<int>("HP");
//        MP = ES3.Load<int>("MP");
//        ATK = ES3.Load<int>("ATK");
//        ATKD = ES3.Load<float>("ATKD");
//        DEF = ES3.Load<int>("DEF");
    }
    
    void OnApplicationQuit()
    {
//        ES3.Save<int>("LVL", LVL);
//        ES3.Save<int>("EXP", EXP);
//        ES3.Save<int>("EXPNEED", EXPNEED);
//        ES3.Save<int>("HP", HP);
//        ES3.Save<int>("MP", MP);
//        ES3.Save<int>("ATK", ATK);
//        ES3.Save<float>("ATKD", ATKD);
//        ES3.Save<int>("DEF", DEF);
    }

    void Update()
    {
    }
}
