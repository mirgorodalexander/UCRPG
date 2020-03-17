using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "UCRPG/Menu/Element", order = 1)]
public class MenuElement : ScriptableObject
{
    [Title("Menu Element")]
    [TableList] public List<ElementSetupClass> Elements;
    [Serializable]
    public class ElementSetupClass
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
        [VerticalGroup("Settings")]
        public _Status Status;
        public enum _Status { Locked, Opened, Owned, Rechargeable };

        [VerticalGroup("Descriptions")] [LabelWidth(80)] [TextArea(2,2)]
        public string LockedText;
        [VerticalGroup("Descriptions")] [LabelWidth(80)] [TextArea(2,2)]
        public string OpenedText;
        [VerticalGroup("Descriptions")] [LabelWidth(80)] [TextArea(2,2)]
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

}