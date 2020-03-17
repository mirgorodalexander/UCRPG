using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MenuDatabase : MonoBehaviour
{
    [Header("Menu Setup")]
    [Title("Locations")]
    [TableList] public List<LocationSetupClass> Locations;
    [Title("Weapons")]
    [TableList] public List<WeaponSetupClass> Weapons;

    [Serializable]
    public class LocationSetupClass
    {
        [TableColumnWidth(50, Resizable = false)]
        public int ID;
        
        [TableColumnWidth(50, Resizable = false)]
        [PreviewField(50, ObjectFieldAlignment.Center)]
        public Sprite Icon;
        
        [VerticalGroup("Settings")]
        public string Title;
        
        [VerticalGroup("Settings")]
        public string Description;

        [VerticalGroup("Descriptions")] [LabelWidth(80)]
        public string LockedText;
        [VerticalGroup("Descriptions")] [LabelWidth(80)]
        public string OpenedText;
        [VerticalGroup("Descriptions")] [LabelWidth(80)]
        public string OwnedText;
        
        [VerticalGroup("Conditions")] [LabelWidth(40)]
        public int Level;
        [VerticalGroup("Conditions")] [LabelWidth(40)]
        public int Price;
        [VerticalGroup("Conditions")]
        public _Сurrency Сurrency;
        public enum _Сurrency { Coin, Diamond };

        [TableColumnWidth(50, Resizable = false)]
        public int REFID;
    }
    [Serializable]
    public class WeaponSetupClass
    {
        [TableColumnWidth(50, Resizable = false)]
        public int ID;
        
        [TableColumnWidth(50, Resizable = false)]
        [PreviewField(50, ObjectFieldAlignment.Center)]
        public GameObject Prefab;
        
        [VerticalGroup("Settings")]
        public string Title;
        
        [VerticalGroup("Settings")]
        public string Description;
        
        [VerticalGroup("Settings")]
        public _Status Status;
        public enum _Status { Locked, Opened, Owned };

        [VerticalGroup("Descriptions")] [LabelWidth(80)]
        public string LockedText;
        [VerticalGroup("Descriptions")] [LabelWidth(80)]
        public string OpenedText;
        [VerticalGroup("Descriptions")] [LabelWidth(80)]
        public string OwnedText;
        
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
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
