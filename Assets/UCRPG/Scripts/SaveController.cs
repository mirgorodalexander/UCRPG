using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

public class SaveController : MonoBehaviour
{
    [Title("Configurations")]
    public bool SaveOnUpdate;

    public List<Object> SavedItems;
    
    [Title("Buttons")]
    [Button("Save", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Save()
    {
        foreach (Object child in SavedItems)
        {
            Debug.Log(JsonUtility.ToJson(child));
            Debug.Log("Writing GameProgress to file system");
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + $"/{child.name}", false);
            writer.WriteLine(JsonUtility.ToJson(child));
            writer.Close();
        }
        Debug.Log("APPLICATION DATA PATH: " + Application.persistentDataPath);
    }
    
    [Button("Load", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Load() {
        Debug.Log("Loading game progress");
        foreach (Object child in SavedItems)
        {
            try {
                StreamReader reader = new StreamReader(Application.persistentDataPath + $"/{child.name}");
                string line;
                JsonUtility.FromJsonOverwrite(reader.ReadLine(), child);
                reader.Close();
            } catch (Exception e) {
                Debug.Log("GameProgress doesent exist, creating file");
                StreamWriter writer = new StreamWriter(Application.persistentDataPath + $"/{child.name}", false);
                writer.Close();
            }
        }
    }
    void Awake()
    {
        Load();
    }
    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        Save();
    }
    void Start()
    {
        Debug.Log("APPLICATION DATA PATH: " + Application.persistentDataPath);
    }

    void Update()
    {
        if (SaveOnUpdate)
        {
        }
    }
}