using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    public int id;
    public Image img;
    public Sprite[] spr;
    public Text txt;
    public Button btn;

    [SerializeField] private Animator anim;

    private void Start()
    {
        btn.onClick.AddListener(LoadLevel);
    }

    private void LoadLevel()
    {
        //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
        UIManager.Ins.TransitionUI<ChangeUICanvas, ChooseLevelCanvas>(0.6f,
           () =>
           {
               LevelManager.Ins.LoadMapByID(id);
               UIManager.Ins.OpenUI<MainCanvas>();
           });
    }

    public void PlayAnim()
    {
        anim.Play(CacheString.TAG_LVBTN);
    }
}
