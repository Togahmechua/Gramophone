using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelCanvas : UICanvas
{
    [SerializeField] private SpawnLevel spawnLevel;
    //[SerializeField] private Button quitBtn;

    private bool flag;

    private void Awake()
    {
        StartCoroutine(Wait());
    }

    private void OnEnable()
    {
        if (!flag)
            return;
        spawnLevel.Check();
    }

    /*private void Start()
    {
        quitBtn.onClick.AddListener(() =>
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            Application.Quit();
        });
    }*/

    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        spawnLevel.Check();
        flag = true;
    }
}
