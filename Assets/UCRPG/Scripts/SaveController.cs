using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    [Title("Configurations")]
    public bool SaveOnUpdate;

    public ES3AutoSaveMgr ES3AutoSaveMgr;

    void Start()
    {
        ES3AutoSaveMgr.Load();
    }

    void Update()
    {
        if (SaveOnUpdate)
        {
            ES3AutoSaveMgr.Save();
        }
    }
}