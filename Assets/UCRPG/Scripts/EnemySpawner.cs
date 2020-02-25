using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
//    [Title("Run")]
//    public bool _Spawn;
//    public bool _Live;
//    [Title("Controllers")]
//    public PlayerController PlayerController;
//    public EnemyController EnemyController;
//    public HealthController HealthController;
//    
//    [Title("Configurations")]
//    public EnemyDatabase EnemyDatabase;
//
//    public GameObject EnemySpawnParent;
//    public Transform EnemySpawnPoint;
//
//    private GameObject enemy;
//    private Tween virtualTween;
//
//    [Title("Buttons")]
//    [Button("Spawn Enemy", ButtonSizes.Large), GUIColor(1, 1, 1)]
//    public void Spawn()
//    {
//        int rnd = Random.Range(0, EnemyDatabase.Enemies.Count);
//        
//        enemy = Instantiate(
//            EnemyDatabase.Enemies[rnd].Prefab,
//            EnemySpawnPoint.transform.position,
//            EnemySpawnPoint.transform.rotation *
//            Quaternion.Euler(0f, 90f, 0f)
//        ) as GameObject;
//        
//        Enemy Enemy = enemy.AddComponent<Enemy>();
//        
//        Enemy.LVL = EnemyDatabase.Enemies[rnd].LVL;
//        Enemy.BEXP = EnemyDatabase.Enemies[rnd].BEXP;
//        Enemy.JEXP = EnemyDatabase.Enemies[rnd].JEXP;
//        Enemy.HP = EnemyDatabase.Enemies[rnd].HP;
//        Enemy.MP = EnemyDatabase.Enemies[rnd].MP;
//        Enemy.ATK = EnemyDatabase.Enemies[rnd].ATK;
//        Enemy.ATKD = EnemyDatabase.Enemies[rnd].ATKD;
//        Enemy.DEF = EnemyDatabase.Enemies[rnd].DEF;
//        Enemy.MOD = (Enemy._MOD) EnemyDatabase.Enemies[rnd].MOD;
//        
//        enemy.name = EnemyDatabase.Enemies[rnd].Name;
//        enemy.transform.parent = EnemySpawnParent.transform;
//
//        EnemyController.Enemy = Enemy;
//        HealthController.Enemy = Enemy;
//        
//        Enemy.Status = Enemy._Status.Init;
//        enemy.transform.localPosition = new Vector3(0, 0, 0);
//        
//        Enemy.transform.DORotate(new Vector3(Enemy.transform.rotation.eulerAngles.x, Enemy.transform.rotation.eulerAngles.y, 0f), 0.01f);
//        Enemy.transform.DOLocalRotate(new Vector3(Enemy.transform.rotation.eulerAngles.x, Enemy.transform.rotation.eulerAngles.y, 0f), 0.01f);
//        //EnemyWrapper.GetComponent<Rigidbody>().DORotate(new Vector3(0f, 0f, 0f), 0.01f);
//        
//        Debug.Log($"[DEBUG] - Spawn enemy \"{enemy.name}\".");
//        
//        if(_Live){
//            Debug.Log($"[DEBUG] - Position \"{EnemySpawnPoint.transform.position}\".");
//            virtualTween = DOVirtual.DelayedCall(0.1f, () =>
//            {
//                EnemyController.Live();
//            });
//        }
//    }
//    
//    [Button("Remove Enemy", ButtonSizes.Large), GUIColor(1, 1, 1)]
//    public void Remove()
//    {
//        Debug.Log($"[DEBUG] - Removing enemy \"{enemy.name}\".");
//        DestroyImmediate(enemy.gameObject);
//    }
//
//    void Start()
//    {
//        if (_Spawn)
//        {
//            this.Spawn();
//        }
//    }
//
//    void Update()
//    {
//    }
}