using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "UCRPG/Menu/Descriptions", order = 1)]
public class MenuElementDescriptions : ScriptableObject
{
    [Title("Menu Element Descriptions")]
    [VerticalGroup("Descriptions")] [LabelWidth(80)]
    public string Locked;
    [VerticalGroup("Descriptions")] [LabelWidth(80)]
    public string Opened;
    [VerticalGroup("Descriptions")] [LabelWidth(80)]
    public string Owned;

}