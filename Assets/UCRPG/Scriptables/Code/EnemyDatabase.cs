﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "UCRPG/Database/Enemy", order = 0)]
public class EnemyDatabase : ScriptableObject
{
    [FormerlySerializedAs("Enemies")] [Header("Enemy Setup")] [TableList] public List<EnemySetupClass> Items;

    [Serializable]
    public class EnemySetupClass
    {
        [TableColumnWidth(50, Resizable = false)]
        public int ID;

        [TableColumnWidth(50, Resizable = false)] [PreviewField(50, ObjectFieldAlignment.Center)]
        public GameObject Prefab;

        [VerticalGroup("Preferences")] public int LVL, BEXP, JEXP, HP, MP, ATK;
        [VerticalGroup("Preferences")] public float ATKD;
        [VerticalGroup("Preferences")] public int DEF;

        [VerticalGroup("Settings")] public string Name;

        [VerticalGroup("Settings")] public _MOD MOD;

        public enum _MOD
        {
            Neutral,
            Aggresive
        };

        [VerticalGroup("Settings")] public _MOVE MOVE;

        public enum _MOVE
        {
            Walking,
            Flying,
            Static
        };

        [VerticalGroup("Settings")] public int SLID;

        [VerticalGroup("Loot List")] [ListDrawerSettings(DraggableItems = false, Expanded = true)]
        //public List<DroppedClass> Dropped;
        public List<int> ItemID;
    }

    [Serializable]
    public class DroppedClass
    {
        [PreviewField(50, ObjectFieldAlignment.Left)]
        public GameObject Prefab;

        public float Chance;
    }
}