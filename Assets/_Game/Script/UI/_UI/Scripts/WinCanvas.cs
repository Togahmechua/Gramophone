using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCanvas : UICanvas
{
    [Header("===Effect===")]
    [SerializeField] private Image roateImg;
    [SerializeField] float speed = 90f;

    [Header("---Other Button---")]
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button menuBtn;

    private bool isClick;

    private void OnEnable()
    {
        //AudioManager.Ins.PlaySFX(AudioManager.Ins.win2);
    }

    private void Start()
    {
        nextBtn.onClick.AddListener(() =>
        {
            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            LevelManager.Ins.curMapID++;

            if (LevelManager.Ins.curMapID < LevelManager.Ins.mapSO.mapList.Count)
            {
                // Load the next level
                LevelManager.Ins.LoadMapByID(LevelManager.Ins.curMapID);
                UIManager.Ins.CloseUI<WinCanvas>();
                UIManager.Ins.OpenUI<MainCanvas>();
            }
            else
            {
                // Reached the last level
                Debug.Log("All levels completed!");
                LevelManager.Ins.DespawnMap();
                UIManager.Ins.CloseUI<WinCanvas>();
                UIManager.Ins.CloseUI<MainCanvas>();
                UIManager.Ins.OpenUI<ChooseLevelCanvas>();
            }
        });

        menuBtn.onClick.AddListener(() =>
        {
            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);

            UIManager.Ins.TransitionUI<ChangeUICanvas, WinCanvas>(0.6f,
                () =>
                {
                    LevelManager.Ins.DespawnMap();
                    UIManager.Ins.OpenUI<ChooseLevelCanvas>();
                });


        });
    }
    private void Update()
    {
        roateImg.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}