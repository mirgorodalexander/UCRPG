using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Player", menuName = "UCRPG/Global/Player", order = 0)]
public class Player : ScriptableObject
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
}