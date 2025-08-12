using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int id;

    public bool isTouchStartPoint;

    //[SerializeField] private float size = 5f;
    [SerializeField] private Canvas cv;
    [SerializeField] private List<Match> matches;

    private void Start()
    {
        Camera cam = Camera.main;

        cv.renderMode = RenderMode.ScreenSpaceCamera;
        cv.worldCamera = cam;
        //cam.orthographicSize = size;

        LineManager.Instance.SetMatches(matches);
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
        PlayerPrefs.SetInt("CurrentMap", LevelManager.Ins.curMap);
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
