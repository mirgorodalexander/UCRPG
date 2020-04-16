// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using UnityEngine.Scripting;
//
// public class GarbageCollectionManager : MonoBehaviour
// {
//     [SerializeField] private float maxTimeBetweenGarbageCollections = 60f;
//     private float _timeSinceLastGarbageCollection;
//
//     private void Start()
//     {
// #if !UNITY_EDITOR
//     GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
// #endif
//         Resources.UnloadUnusedAssets();
//     }
//
//     private void Update()
//     {
//         _timeSinceLastGarbageCollection += Time.unscaledDeltaTime;
//         if (_timeSinceLastGarbageCollection > maxTimeBetweenGarbageCollections)
//         {
//             Resources.UnloadUnusedAssets();
//             CollectGarbage();
//         }
//     }
//
//     
//     [Button("Collect Garbage", ButtonSizes.Large), GUIColor(1, 1, 1)]
//     public void CollectGarbage()
//     {
//         _timeSinceLastGarbageCollection = 0f;
//         Debug.Log("Collecting garbage"); // talking about garbage... 
// #if !UNITY_EDITOR
//     // Not supported on the editor
//     GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
//     GC.Collect();
//     GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
// #endif
//     }
//     static void ListenForGCModeChange()
//     {
//         // Listen on garbage collector mode changes.
//         GarbageCollector.GCModeChanged += (GarbageCollector.Mode mode) =>
//         {
//             Debug.Log("GCModeChanged: " + mode);
//         };
//     }
//
//     static void LogMode()
//     {
//         Debug.Log("GCMode: " + GarbageCollector.GCMode);
//     }
//
//     [Button("Enable Garbage", ButtonSizes.Large), GUIColor(1, 1, 1)]
//     public void EnableGC()
//     {
//         Debug.Log("Enable Garbage");
//         GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
//         // Trigger a collection to free memory.
//         GC.Collect();
//     }
//     
//     [Button("Disable Garbage", ButtonSizes.Large), GUIColor(1, 1, 1)]
//     public void DisableGC()
//     {
//         Debug.Log("Disable Garbage");
//         GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
//     }
// }