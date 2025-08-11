using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int id;


    private void Start()
    {
        
    }

    private void Update()
    {
        if (!LevelManager.Ins.isWin) return;

        if (id == LevelManager.Ins.curMapID &&
            !LevelManager.Ins.mapSO.mapList[LevelManager.Ins.curMapID].isWon)
        {
            LevelManager.Ins.mapSO.mapList[LevelManager.Ins.curMapID].isWon = true;
            SaveWinState(LevelManager.Ins.curMapID);
            Debug.Log("Map " + LevelManager.Ins.curMapID + " is won.");
            LevelManager.Ins.curMap++;
        }

        SetCurMap();
    }

    private void SetCurMap()
    {
        PlayerPrefs.SetInt("CurrentMap", LevelManager.Ins.curMapID);
        PlayerPrefs.Save();
    }

    private void SaveWinState(int mapIndex)
    {
        string key = "MapWin_" + mapIndex;
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
        LevelManager.Ins.mapSO.LoadWinStates();
    }
}
