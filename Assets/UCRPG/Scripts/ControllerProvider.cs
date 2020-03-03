using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControllerProvider : MonoBehaviour
{
    [Title("Controllers")]
    public LocationController LocationController;
    public HealthController HealthController;
    public PlayerController PlayerController;
    public EnemyController EnemyController;
    public ItemController ItemController;
}
