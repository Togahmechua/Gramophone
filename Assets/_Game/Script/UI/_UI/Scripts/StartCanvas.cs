using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : UICanvas
{
    [SerializeField] private Button startBtn;

    private void Start()
    {
        startBtn.onClick.AddListener(() =>
        {
            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);

            UIManager.Ins.TransitionUI<ChangeUICanvas, StartCanvas>(0.5f,
                () =>
                {
                    UIManager.Ins.OpenUI<ChooseLevelCanvas>();
                });
        });
    }
}
