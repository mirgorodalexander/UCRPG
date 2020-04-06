using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "UCRPG/Database/Location", order = 0)]
public class LocationDatabase : ScriptableObject
{
    [Title("Location Setup")]
    [TableList] public List<LocationSetupClass> Items;

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
        
        [VerticalGroup("Enemy List")]
        [ListDrawerSettings(DraggableItems = false, NumberOfItemsPerPage = 500)]
        public List<int> EnemyID;
    }
}