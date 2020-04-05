

using System.Collections;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "UCRPG/Global/Event", order = 1)]
public class GlobalEvent : ScriptableObject
{
    public bool published;
    [ContextMenu("Publish")]
    public bool Publish()
    {
        published = true;
        return true;
    }
    
    public bool isPublished()
    {
        if (published)
        {
            // DOVirtual.DelayedCall(Time.deltaTime,  () =>
            // {
            // });
            
            published = false;
            return true;
        }
        else
        {
            return false;
        }
    }
//System.Action<bool> callback
    [ContextMenu("Reload")]
    public bool Reload()
    {
        published = false;
        return true;
    }
}