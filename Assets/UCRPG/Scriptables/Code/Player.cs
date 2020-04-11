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
    public enum _Status { Waiting, Moving, Fighting, Die, Menu };
    [Title("Settings")]
    public int LVL;
    public int EXP;
    public int HP;
    public int ATK;
    public int LUK;
    public int DEF;
    public float ASPD;
    public int WID;
    public int LID;

    [Title("Inventory")]
    //public List<int> Inventory;
    [TableList] public List<InventoryItem> Inventory;
    
    [Serializable]
    public class InventoryItem
    {
        public int ID;
        public string Name;
        public int Amount;
    }
}