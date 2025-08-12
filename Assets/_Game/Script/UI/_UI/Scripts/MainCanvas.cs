using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : UICanvas
{
    [SerializeField] private Button pauseBtn;
    //[SerializeField] private Button retryBtn;
    [SerializeField] private Text levelTxt;
    private void OnEnable()
    {
        levelTxt.text = "LEVEL " + LoadText();
    }

    private void Start()
    {
        pauseBtn.onClick.AddListener(() =>
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            UIManager.Ins.OpenUI<PauseCanvas>();
            UIManager.Ins.CloseUI<MainCanvas>();
        });

        /*retryBtn.onClick.AddListener(() =>
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            LevelManager.Ins.LoadMapByID(LevelManager.Ins.curMapID);
        });

        upBtn.onClick.AddListener(() =>
        {
            LevelManager.Ins.level.player.OnMoveButton(Vector2.up);
            AudioManager.Ins.PlaySFX(AudioManager.Ins.clickMoveBtn);
        });

        leftBtn.onClick.AddListener(() =>
        {
            LevelManager.Ins.level.player.OnMoveButton(Vector2.left);
            AudioManager.Ins.PlaySFX(AudioManager.Ins.clickMoveBtn);
        });

        rightBtn.onClick.AddListener(() =>
        {
            LevelManager.Ins.level.player.OnMoveButton(Vector2.right);
            AudioManager.Ins.PlaySFX(AudioManager.Ins.clickMoveBtn);
        });*/
    }

    private string LoadText()
    {
        string txt = "";
        int num = LevelManager.Ins.curMapID + 1;
        //Debug.Log(num);

        if (num <= 9)
        {
            txt = "0" + num.ToString();
        }
        else
        {
            txt = num.ToString();
        }

        return txt;
    }
}
