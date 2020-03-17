using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LocationDatabase : MonoBehaviour
{
    [Title("Location Setup")]
    [TableList] public List<LocationSetupClass> Locations;

    [Serializable]
    public class LocationSetupClass
    {
        [TableColumnWidth(50, Resizable = false)]
        public int ID;
        
        [TableColumnWidth(50, Resizable = false)]
        [PreviewField(50, ObjectFieldAlignment.Center)]
        public GameObject Prefab;
        
        [VerticalGroup("Settings")]
        public string Name;
        
        [VerticalGroup("Settings")]
        public _Status Status;
        public enum _Status { Locked, Opened, Owned };
        
        [VerticalGroup("Enemy List")]
        [ListDrawerSettings(DraggableItems = false, NumberOfItemsPerPage = 500)]
        public List<int> EnemyID;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
