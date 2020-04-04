using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "UCRPG/Database/Weapon", order = 1)]
public class WeaponDatabase : ScriptableObject
{
    public string LockedText;
    public string OpenedText;
    public string OwnedText;
    [Header("Item Setup")]
    [TableList] public List<ItemSetupClass> Items;

    [Serializable]
    public class ItemSetupClass
    {
        [TableColumnWidth(50, Resizable = false)]
        public int ID;
        
        [TableColumnWidth(50, Resizable = false)]
        [PreviewField(50, ObjectFieldAlignment.Center)]
        public GameObject Prefab;
        
        [VerticalGroup("Settings")]
        public string Name;
        [VerticalGroup("Settings")]
        public string Description;
        [VerticalGroup("Settings")]
        public _Status Status;
        public enum _Status { Locked, Opened, Owned, Rechargeable };

        [FormerlySerializedAs("Power")] [VerticalGroup("Preferences")]
        public int ATK, SPD;
        
        [VerticalGroup("Conditions")] [LabelWidth(40)]
        public int Level;
        [VerticalGroup("Conditions")] [LabelWidth(40)]
        public int Price;
        [VerticalGroup("Conditions")]
        public _Сurrency Сurrency;
        public enum _Сurrency { Coin, Diamond };
    }

    [Serializable]
    public class DroppedClass
    {
        [PreviewField(50, ObjectFieldAlignment.Left)]
        public GameObject Prefab;
        public float Chance;
    }
}
