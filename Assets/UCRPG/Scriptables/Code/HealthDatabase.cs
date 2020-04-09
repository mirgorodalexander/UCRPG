using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Health Database", menuName = "UCRPG/Database/Health", order = 1)]
public class HealthDatabase : ScriptableObject
{
    [Title("Configurations")]
    [ListDrawerSettings(NumberOfItemsPerPage = 500, ShowIndexLabels = true)]
    public List<int> Items;
}