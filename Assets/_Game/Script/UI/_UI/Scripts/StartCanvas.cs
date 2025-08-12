using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : UICanvas
{
    [SerializeField] private Button startBtn;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        startBtn.onClick.AddListener(() =>
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.click);

            anim.Play(CacheString.TAG_Clicked_StartCanvas);
        });
    }

    public void Play()
    {
        UIManager.Ins.TransitionUI<ChangeUICanvas, StartCanvas>(0.5f,
               () =>
               {
                   UIManager.Ins.OpenUI<ChooseLevelCanvas>();
                   //LevelManager.Ins.SpawnLevel();
               });
    }
}
