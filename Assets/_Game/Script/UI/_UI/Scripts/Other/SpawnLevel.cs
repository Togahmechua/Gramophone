using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevel : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;
    //[SerializeField] private int spawnCount;
    //[SerializeField] private LevelBtn levelPrefab;
    [SerializeField] private List<LevelBtn> levelBtnList = new List<LevelBtn>();


    private void Start()
    {
        //GenerateBtn();
        Check();
    }

    /*private void GenerateBtn()
    {
        foreach (Transform child in spawnPos)
        {
            Destroy(child.gameObject);
        }

        levelBtnList.Clear();

        for (int i = 0; i < spawnCount; i++)
        {
            LevelBtn levelBtn = Instantiate(levelPrefab, spawnPos);
            levelBtnList.Add(levelBtn);
            levelBtn.id = i;
        }
    }*/

    public void Check()
    {
        for (int i = 0; i < levelBtnList.Count; i++)
        {
            LevelBtn levelBtn = levelBtnList[i];
            if (levelBtn.id <= LevelManager.Ins.curMap)
            {
                levelBtn.img.sprite = levelBtn.spr[1];

                levelBtn.txt.text = levelBtn.id < 9
               ? "0" + (levelBtn.id + 1).ToString()
               : (levelBtn.id + 1).ToString();

                levelBtn.btn.interactable = true;
                levelBtn.PlayAnim();
            }
            else
            {
                levelBtn.img.sprite = levelBtn.spr[0];
                levelBtn.txt.text = "";
                levelBtn.btn.interactable = false;
            } 
        }
    }

    private void OnDrawGizmosSelected()
    {
        levelBtnList.Clear();

        for (int i = 0; i < spawnPos.childCount; i++)
        {
            LevelBtn lv = spawnPos.GetChild(i).GetComponent<LevelBtn>();
            if (lv != null)
            {
                levelBtnList.Add(lv);
                lv.id = i;  
            }
        }
    }
}
