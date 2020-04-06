using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "UCRPG/Database/Experience", order = 1)]
public class ExperienceDatabase : ScriptableObject
{
    [Title("Configurations")]
    [ListDrawerSettings(NumberOfItemsPerPage = 500, ShowIndexLabels = true)]
    public List<int> Items;
}

