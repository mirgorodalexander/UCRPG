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
            published = false;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    [ContextMenu("Reload")]
    public bool Reload()
    {
        published = false;
        return true;
    }
}