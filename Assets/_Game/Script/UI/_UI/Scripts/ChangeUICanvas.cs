using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUICanvas : UICanvas
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
        UIManager.Ins.changeUICanvas = this;
        StartCoroutine(IEClose());
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    private IEnumerator IEClose()
    {
        yield return new WaitForSecondsRealtime(2f);
        UIManager.Ins.CloseUI<ChangeUICanvas>();
    }
}
