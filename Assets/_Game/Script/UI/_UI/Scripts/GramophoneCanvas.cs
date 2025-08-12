using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GramophoneCanvas : UICanvas
{
    public void Next()
    {
        UIManager.Ins.TransitionUI<ChangeUICanvas, GramophoneCanvas>(0.5f,
               () =>
               {
                   UIManager.Ins.OpenUI<WinCanvas>();
               });
    }
}
