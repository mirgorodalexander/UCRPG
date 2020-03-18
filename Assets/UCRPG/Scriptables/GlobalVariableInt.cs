using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "UCRPG/Global/Int", order = 1)]
public class GlobalVariableInt : ScriptableObject
{
    public int Value;
}